using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Data;

namespace PerfumeShop.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminOrderController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminOrderController(ApplicationDbContext db)
        {
            _db = db;
        }

        // ============================
        // LIST ORDERS
        // ============================
        public IActionResult Index()
        {
            var orders = _db.Orders
                .Include(x => x.User)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            return View(orders);
        }

        // ============================
        // ORDER DETAIL
        // ============================
        public IActionResult Detail(int id)
        {
            var order = _db.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(x => x.Id == id);

            if (order == null) return NotFound();

            return View(order);
        }

        // ============================
        // UPDATE STATUS (POST)
        // ============================
        [HttpPost]
        public IActionResult UpdateStatus(int id, string status)
        {
            var order = _db.Orders.Find(id);
            if (order == null) return NotFound();

            order.Status = status;
            _db.SaveChanges();

            return RedirectToAction("Detail", new { id });
        }
    }
}
