using Microsoft.AspNetCore.Mvc;
using PerfumeShop.Application.Services;


namespace PerfumeShop.Presentation.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chat;

        public ChatController(IChatService chat)
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
