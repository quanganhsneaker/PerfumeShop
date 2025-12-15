using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeShop.Domain.Interfaces;
using PerfumeShop.Infrastructure.Data;
using PerfumeShop.Infrastructure.Services;
using System.Linq;

namespace PerfumeShop.Presentation.Controllers.Admin
{
    [Authorize]
    public class AdminHomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IPermissionService _perm;

        public AdminHomeController(ApplicationDbContext db, IPermissionService perm)
        {
            _db = db;
            _perm = perm;
        }

        public IActionResult Dashboard()
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            if (!_perm.HasPermission(userId, "admin.dashboard"))
            return RedirectToAction("Denied", "Auth");
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
