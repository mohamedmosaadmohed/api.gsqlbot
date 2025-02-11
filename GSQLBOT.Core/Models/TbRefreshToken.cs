using Microsoft.EntityFrameworkCore;

namespace BGSQLBOTOT.Core.Model
{
    [Owned]
    public class TbRefreshToken
    {
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsExpired => DateTime.Now >= ExpiresOn;
        public DateTime CreateOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public bool IsActive => RevokedOn == null && !IsExpired;
    }
}
