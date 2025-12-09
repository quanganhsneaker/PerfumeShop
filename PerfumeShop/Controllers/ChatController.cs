using Microsoft.AspNetCore.Mvc;
using PerfumeShop.Services;

namespace PerfumeShop.Controllers
{
    public class ChatController : Controller
    {
        private readonly ChatService _chat;

        public ChatController(ChatService chat)
        {
            _chat = chat;
        }

        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] ChatRequest req)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");

                string answer = await _chat.ProcessQuestion(req.Question, userId);

                return Json(new { answer });
            }
            catch (Exception ex)
            {
                return Json(new { answer = "⚠ Lỗi server: " + ex.Message });
            }
        }
    }

    public class ChatRequest
    {
        public string Question { get; set; }
    }
}
