using BGSQLBOTOT.Core.Model;
using GSQLBOT.Core.DTOs;
using GSQLBOT.Core.Helpers;
using GSQLBOT.Core.Model;
using GSQLBOT.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GSQLBOT.Services.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;
        public AuthService(UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt, IEmailSender emailSender,IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _emailSender = emailSender;
            _env = env;
        }
        // Register
        public async Task<AuthDTOs> RegisterAsync(RegisterDTOs registerDT)
        {
            // Check if the email is already registered
            if (await _userManager.FindByEmailAsync(registerDT.Email) is not null)
                return new AuthDTOs { Message = "Email is already registered!", IsAuthenticated = false };

            // Check if the username is already taken
            if (await _userManager.FindByNameAsync(registerDT.Username) is not null)
                return new AuthDTOs { Message = "Username is already registered!", IsAuthenticated = false };

            var user = new ApplicationUser
            {
                UserName = registerDT.Username,
                Email = registerDT.Email,
                FirstName = registerDT.FirstName,
                LastName = registerDT.LastName
            };

            // Create user
            var result = await _userManager.CreateAsync(user, registerDT.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthDTOs { Message = errors, IsAuthenticated = false };
            }

            // Assign default role
            await _userManager.AddToRoleAsync(user, "User");

            // Send OTP for email verification
            var otpSent = await SendOtpAsync(user.Email);
            if (!otpSent)
                return new AuthDTOs { Message = "Failed to send OTP. Please try again.", IsAuthenticated = false };

            return new AuthDTOs
            {
                Email = user.Email,
                IsAuthenticated = false,
                Message = "OTP sent to your email. Please verify your email to continue."
            };
        }
        // Login
        public async Task<AuthDTOs> LoginAsync(LoginDTOs loginDTOs)
        {
            var authDTOs = new AuthDTOs();
            var user = await _userManager.FindByEmailAsync(loginDTOs.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, loginDTOs.Password))
            {
                authDTOs.Message = "Email or Password is incorrect!";
                return authDTOs;
            }
            if(user.EmailConfirmed != true)
            {
                await SendOtpAsync(loginDTOs.Email);
                return new AuthDTOs
                {
                    Email = loginDTOs.Email,
                    IsAuthenticated = false,
                    Message = "OTP sent to your email. Please verify your email to continue."
                };
            }
            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authDTOs.IsAuthenticated = true;
            authDTOs.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authDTOs.Email = user.Email;
            authDTOs.UserName = user.UserName;
            //authDTOs.ExpiresOn = jwtSecurityToken.ValidTo;
            authDTOs.Roles = rolesList.ToList();

            if(user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                authDTOs.RefreshToken = activeRefreshToken.Token;
                authDTOs.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authDTOs.RefreshToken = refreshToken.Token;
                authDTOs.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }
            return authDTOs;
        }
        // Send OTP
        public async Task<bool> SendOtpAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            var templatePath = Path.Combine(_env.WebRootPath, "EmailTemplete", "OTP.cshtml");
            if (!File.Exists(templatePath)) return false;

            var otp = OtpGenerator.GenerateOtp();
            user.OTP = otp;
            user.Expiredat = DateTime.UtcNow.AddMinutes(2);
            await _userManager.UpdateAsync(user);

            string emailBody;
            using (var reader = new StreamReader(templatePath))
            {
                emailBody = await reader.ReadToEndAsync();
            }
            emailBody = emailBody.Replace("{{otp}}", otp);
            await _emailSender.SendEmailAsync(email, "Your OTP Code", emailBody);

            return true;
        }
        // Verify OTP
        public async Task<AuthDTOs> VerifyOtpAsync(string email, string otp)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new AuthDTOs { IsAuthenticated = false, Message = "User not found." };
            }

            if (string.IsNullOrEmpty(user.OTP) || user.OTP != otp)
            {
                return new AuthDTOs { IsAuthenticated = false, Message = "Invalid OTP." };
            }

            if (user.Expiredat < DateTime.UtcNow)
            {
                return new AuthDTOs { IsAuthenticated = false, Message = "OTP expired." };
            }

            // Mark the email as confirmed
            user.EmailConfirmed = true;
            user.OTP = null;
            await _userManager.UpdateAsync(user);

            // Generate JWT token
            var jwtSecurityToken = await CreateJwtToken(user);
            return new AuthDTOs
            {
                Email = user.Email,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = user.UserName
            };
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
        private TbRefreshToken GenerateRefreshToken()
        {
            var randomBytes = new Byte[64]; // 512-bit
            using (var generate = RandomNumberGenerator.Create())
            {
                generate.GetBytes(randomBytes);
            }
            // Remove special characters to keep it URL-safe
            string token = Convert.ToBase64String(randomBytes)
            .Replace("+", "")
            .Replace("/", "")
            .Replace("=", "");
            return new TbRefreshToken
            {
                Token = token,
                ExpiresOn = DateTime.UtcNow.AddDays(3),
                CreateOn = DateTime.UtcNow,
            };
        }
    }
}
