using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PerfumeShop.Data;
using PerfumeShop.DTOs;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace PerfumeShop.Services
{
    public class ChatService
    {
        private readonly ApplicationDbContext _db;
        private readonly HttpClient _http;

        private readonly string _apiKey;

        public ChatService(ApplicationDbContext db, HttpClient http, IOptions<GroqSettings> settings)
        {
            _db = db;
            _http = http;
            _apiKey = settings.Value.ApiKey;
        }

    
        public async Task<string> ProcessQuestion(string question, int userId)
        {
            if (string.IsNullOrWhiteSpace(question))
                return "Bạn muốn hỏi gì nhỉ? 😊";

            question = question.ToLower();

         
            if (userId > 0 && (question.Contains("đơn hàng") || question.Contains("order")))
                return await HandleOrderQuery(question, userId);

           
            if (question.Contains("sản phẩm") || question.Contains("product"))
                return await HandleProductQuery(question);

          
            return await AskAI(question);
        }


        private async Task<string> HandleOrderQuery(string question, int userId)
        {
            string orderCode = ExtractOrderCode(question);

            if (string.IsNullOrEmpty(orderCode))
                return "Bạn vui lòng cho mình mã đơn dạng **DHxxxx** nhé!";

            var order = await _db.Orders
                .FirstOrDefaultAsync(o => o.OrderCode == orderCode && o.UserId == userId);

            if (order == null)
                return $" Không tìm thấy đơn hàng `{orderCode}` của bạn.";

            return
                $" **THÔNG TIN ĐƠN HÀNG {orderCode}**\n" +
                $"- Trạng thái: **{order.Status}**\n" +
                $"- Ngày tạo: {order.CreatedAt:dd/MM/yyyy HH:mm}\n" +
                $"- Tổng tiền: {order.TotalAmount:N0} đ\n" +
                $"- Thanh toán: {(order.PaymentStatus == "Paid" ? "Đã thanh toán" : "Chưa thanh toán")}";
        }


        private string ExtractOrderCode(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;

            var match = Regex.Match(text.ToUpper(), @"DH\d{3,10}");
            return match.Success ? match.Value : null;
        }


        private async Task<string> HandleProductQuery(string question)
        {
            question = question.ToLower();

            var products = await _db.Products
                .Select(p => new { p.Id, p.Name, p.Description, p.Price })
                .ToListAsync();

    
            var exactProduct = products.FirstOrDefault(p =>
                question.Contains(p.Name.ToLower()));

            if (exactProduct != null)
                return await BuildProductAnswer(exactProduct, question);

            var keywords = question.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                   .Where(k => k.Length >= 3)         
                                   .ToList();


            var matched = products
                .Select(p => new {
                    Product = p,
                    Score = keywords.Count(k => p.Name.ToLower().Contains(k))
                })
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)   
                .FirstOrDefault();

            if (matched == null)
                return "Bạn đang muốn hỏi sản phẩm nào? Bạn ghi rõ tên giúp mình nhé!";

     
            return await BuildProductAnswer(matched.Product, question);
        }
        private async Task<string> BuildProductAnswer(object productObj, string question)
        {
   
            var dict = productObj.GetType().GetProperties()
                                 .ToDictionary(p => p.Name, p => p.GetValue(productObj));

            int id = (int)dict["Id"];
            string name = dict["Name"]?.ToString();
            string description = dict["Description"]?.ToString();
            decimal price = (decimal)dict["Price"];


            var reviews = await _db.Reviews
                .Where(r => r.ProductId == id)
                .Select(r => r.Comment)
                .ToListAsync();

            string reviewText = reviews.Count == 0
                ? "Chưa có đánh giá nào."
                : string.Join("\n- ", reviews);

         
            string prompt =
                $"Thông tin sản phẩm:\n" +
                $"Tên: {name}\n" +
                $"Giá: {price:N0} đ\n" +
                $"Mô tả: {description ?? "Không có mô tả."}\n\n" +
                $"Đánh giá khách hàng:\n- {reviewText}\n\n" +
                $"Câu hỏi của khách: {question}\n" +
                $"Hãy trả lời toàn diện, thân thiện và dễ hiểu.";

            return await AskAI(prompt);
        }



        public async Task<string> AskAI(string prompt)
        {
            try
            {
  
                prompt = prompt.Replace("\r", " ").Replace("\n", " ");

                var systemRule =
                    "Bạn là chatbot của PerfumeShop. " +
                    "Bạn CHỈ ĐƯỢC PHÉP trả lời dựa trên dữ liệu tôi cung cấp. " +
                    "Bạn KHÔNG ĐƯỢC tự bịa sản phẩm, tự tạo danh sách, tự thêm thương hiệu hoặc thông tin không có trong database. " +
                    "Nếu câu trả lời yêu cầu thông tin không có trong dữ liệu, hãy trả lời: 'Thông tin này không có trong hệ thống PerfumeShop.'";

                var payload = new
                {
                    model = "llama-3.1-8b-instant",
                    messages = new[]
                    {
                new { role = "system", content = systemRule },
                new { role = "user", content = prompt }
            }
                };

                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                var res = await _http.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);
                var jsonText = await res.Content.ReadAsStringAsync();

                if (!res.IsSuccessStatusCode)
                    return $"⚠ AI Error: {jsonText}";

                var json = JsonDocument.Parse(jsonText);

                if (!json.RootElement.TryGetProperty("choices", out var choices))
                    return "⚠ AI không trả về dữ liệu hợp lệ.";

                return choices[0].GetProperty("message").GetProperty("content").GetString();
            }
            catch (Exception ex)
            {
                return "⚠ Lỗi AI: " + ex.Message;
            }
        }

    }
}
