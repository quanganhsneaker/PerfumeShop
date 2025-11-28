using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeShop.Data;
using System.Linq;

namespace PerfumeShop.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminHomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminHomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Dashboard()
        {
            // Stat numbers
            ViewBag.TotalRevenue = _db.Orders
                .Where(o => o.Status == "Completed")
                .Sum(o => (decimal?)o.TotalAmount) ?? 0;

            ViewBag.TotalOrders = _db.Orders.Count();
            ViewBag.TotalUsers = _db.Users.Count();

            // Monthly chart
            var monthly = _db.Orders
                .Where(o => o.Status == "Completed")
                .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
                .AsEnumerable()
                .Select(g => new
                {
                    Month = $"{g.Key.Month}/{g.Key.Year}",
                    Revenue = g.Sum(x => x.TotalAmount)
                })
                .OrderBy(x => x.Month)
                .ToList();

            ViewBag.Months = Newtonsoft.Json.JsonConvert.SerializeObject(monthly.Select(x => x.Month));
            ViewBag.Revenues = Newtonsoft.Json.JsonConvert.SerializeObject(monthly.Select(x => x.Revenue));

            return View();
        }
    }
}
