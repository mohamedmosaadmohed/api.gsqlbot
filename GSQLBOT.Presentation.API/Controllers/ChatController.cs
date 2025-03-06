using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using GSQLBOT.Core.DTOs;
using GSQLBOT.Core.Model;
using GSQLBOT.Core.Repositories;

namespace GSQLBOT.Presentation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IUnitOfWork _unitOfWork;

        public ChatController(HttpClient httpClient,IUnitOfWork unitOfWork)
        {
            _httpClient = httpClient;
            _unitOfWork = unitOfWork;
        }
        [HttpPost("generate_sql")]
        public async Task<IActionResult> GenerateSql([FromBody] SqlRequestDTOs request)
        {
            if (request.Question is null)
            {
                return BadRequest(new { error = "Question is required" });
            }
            if (request.Schema is null)
            {
                return BadRequest(new { error = "Schema is required" });
            }
            if (!Request.Headers.TryGetValue("ApplicationUserId", out var applicationUserId))
            {
                return BadRequest(new { error = "ApplicationUserId header is required" });
            }

            // Check if ChatId is provided in the request (optional)
            Chat chat;
            if (request.ChatId.HasValue)
            {
                chat = await _unitOfWork.Chat.GetFirstorDefaultAsync(c => c.Id == request.ChatId && c.ApplicationUserId == applicationUserId);
                if (chat == null)
                {
                    return NotFound(new { error = "Chat not found for this user" });
                }
            }
            else
            {
                chat = new Chat
                {
                    ApplicationUserId = applicationUserId,
                    createdDate = DateTime.Now
                };
                await _unitOfWork.Chat.AddAsync(chat);
                await _unitOfWork.CompleteAsync();
            }

            var apiUrl = "https://b80a-34-87-50-98.ngrok-free.app/generate_sql/";
            var jsonRequest = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(apiUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, new { error = "Error calling external API", details = responseContent });
                }

                // Store User Question as a ChatMessage
                var userMessage = new ChatMessage
                {
                    ChatId = chat.Id,
                    Message = request.Question,
                    SenderType = SenderType.User,
                    createdDate = DateTime.Now
                };
                await _unitOfWork.ChatMessage.AddAsync(userMessage);
                await _unitOfWork.CompleteAsync();

                // Store Generated SQL as a ChatMessage
                var modelMessage = new ChatMessage
                {
                    ChatId = chat.Id,
                    Message = responseContent,
                    SenderType = SenderType.Mode,
                    createdDate = DateTime.Now
                };
                await _unitOfWork.ChatMessage.AddAsync(modelMessage);
                await _unitOfWork.CompleteAsync();

                return Ok(new
                {
                    chatId = chat.Id,
                    question = request.Question,
                    generatedSql = responseContent
                });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }
        [HttpGet("all_chats")]
        public async Task<IActionResult> GetAllChats()
        {
            if (!Request.Headers.TryGetValue("ApplicationUserId", out var applicationUserId))
            {
                return BadRequest(new { error = "ApplicationUserId header is required" });
            }
            var chats =  await _unitOfWork.Chat.GetAllAsync(X => X.ApplicationUserId == applicationUserId);
            return Ok(chats);
        }
        [HttpGet("chatbyid")]
        public async Task<IActionResult> GetChatbyId(int id)
        {
            if (!Request.Headers.TryGetValue("ApplicationUserId", out var applicationUserId))
            {
                return BadRequest(new { error = "ApplicationUserId header is required" });
            }
            var chat = await _unitOfWork.Chat.GetFirstorDefaultAsync(X => X.ApplicationUserId == applicationUserId && X.Id == id);
            var chatmessage = await _unitOfWork.ChatMessage.GetAllAsync(X => X.ChatId == chat.Id);
            return Ok(chatmessage);
        }
        [HttpDelete("delete_allchats")]
        public async Task<IActionResult> DeleteAllChats()
        {
            if (!Request.Headers.TryGetValue("ApplicationUserId", out var applicationUserId))
            {
                return BadRequest(new { error = "ApplicationUserId header is required" });
            }
            var chats = await _unitOfWork.Chat.GetAllAsync(x => x.ApplicationUserId == applicationUserId);
            if (!chats.Any())
            {
                return NotFound(new { message = "No chats found for this user" });
            }
            // Delete all chats
            await _unitOfWork.Chat.RemoveRangeAsync(chats);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = "All chats deleted successfully"});
        }
        [HttpDelete("delete_chatbyid")]
        public async Task<IActionResult> DeletechatbyId(int id)
        {
            if (!Request.Headers.TryGetValue("ApplicationUserId", out var applicationUserId))
            {
                return BadRequest(new { error = "ApplicationUserId header is required" });
            }
            var chat = await _unitOfWork.Chat.GetFirstorDefaultAsync(x => x.ApplicationUserId == applicationUserId && x.Id == id);
            if (chat is null)
            {
                return NotFound(new { message = "No chats found for this user" });
            }
            // Delete all chats
            await _unitOfWork.Chat.RemoveAsync(chat);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = "chat deleted successfully" });
        }

    }
}
