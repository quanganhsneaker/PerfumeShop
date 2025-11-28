using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Data;

namespace PerfumeShop.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminStaticsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminStaticsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // =============================
        // 1) DASHBOARD VIEW
        // =============================
        public IActionResult Index()
        {
            // tổng doanh thu
            ViewBag.TotalRevenue = _db.Orders
                .Where(x => x.Status == "Completed")
                .Sum(x => (decimal?)x.TotalAmount) ?? 0;

            // tổng đơn hàng
            ViewBag.TotalOrders = _db.Orders.Count();

            // tổng người dùng
            ViewBag.TotalUsers = _db.Users.Count();

            return View();
        }

        // =============================
        // 2) DOANH THU THEO THÁNG (JSON)
        // =============================
        public IActionResult RevenueMonthly()
        {
            var data = _db.Orders
                .Where(x => x.Status == "Completed")
                .GroupBy(x => new { x.CreatedAt.Year, x.CreatedAt.Month })
                .Select(g => new
                {
                    month = g.Key.Month,
                    year = g.Key.Year,
                    revenue = g.Sum(x => x.TotalAmount)
                })
                .OrderBy(o => o.year)
                .ThenBy(o => o.month)
                .ToList();

            return Json(data);
        }

        // =============================
        // 3) ĐƠN HÀNG THEO TRẠNG THÁI (JSON)
        // =============================
        public IActionResult OrdersByStatus()
        {
            var data = _db.Orders
                .GroupBy(x => x.Status)
                .Select(g => new
                {
                    status = g.Key,
                    count = g.Count()
                })
                .ToList();

            return Json(data);
        }

        // =============================
        // 4) SẢN PHẨM BÁN CHẠY (JSON)
        // =============================
        public IActionResult BestSeller()
        {
            var data = _db.OrderItems
                .GroupBy(i => new { i.ProductId, i.Product.Name, i.Product.ImageUrl })
                .Select(g => new
                {
                    productId = g.Key.ProductId,
                    productName = g.Key.Name,
                    image = g.Key.ImageUrl,
                    totalSold = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.totalSold)
                .Take(5)
                .ToList();

            return Json(data);
        }

        // =============================
        // 5) SẢN PHẨM ĐÁNH GIÁ CAO NHẤT (JSON)
        // =============================
        public IActionResult TopRated()
        {
            var data = _db.Products
                .OrderByDescending(x => x.Rating)
                .Take(5)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.ImageUrl,
                    x.Rating
                })
                .ToList();

            return Json(data);
        }
    }
}
