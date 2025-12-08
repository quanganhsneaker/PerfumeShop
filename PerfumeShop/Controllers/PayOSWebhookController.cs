using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PerfumeShop.Data;

namespace PerfumeShop.Controllers
{
    [Route("payos/webhook")]
    public class PayOSWebhookController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PayOSWebhookController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Callback()
        {
            using var reader = new StreamReader(Request.Body);
            string raw = await reader.ReadToEndAsync();

            Console.WriteLine("WEBHOOK ĐÃ GỌI VÀO SERVER");
            Console.WriteLine(" RAW JSON:");
            Console.WriteLine(raw);

            if (string.IsNullOrEmpty(raw))
                return BadRequest("Empty body");

            JObject json = JObject.Parse(raw);
            var data = json["data"];

            if (data == null)
                return BadRequest("Invalid body");

            string? code = data["code"]?.ToString();
            string? desc = data["desc"]?.ToString();     
            string? orderCode = data["orderCode"]?.ToString();

            Console.WriteLine($" code = {code}, desc = {desc}, order = {orderCode}");

            if (orderCode == null)
                return BadRequest("Missing orderCode");

            int orderId = int.Parse(orderCode);

           
            if (code == "00")
            {
                var order = _db.Orders.FirstOrDefault(o => o.Id == orderId);

                if (order != null)
                {
                    order.PaymentStatus = "Paid";
                   
                    _db.SaveChanges();

                    Console.WriteLine(" Đã cập nhật PaymentStatus = Paid");
                }
            }

            return Ok();
        }
    }
}
