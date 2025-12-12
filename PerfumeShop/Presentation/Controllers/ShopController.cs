using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PerfumeShop.Domain.Models;
using PerfumeShop.Infrastructure.Data;

namespace PerfumeShop.Presentation.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ShopController(ApplicationDbContext db)
        {
            _db = db;
        }

        private void LoadCartCount()
        {
            if (!User.Identity.IsAuthenticated)
            {
                ViewBag.CartCount = 0;
                return;
            }
            int userId = int.Parse(User.FindFirst("userId").Value);

            ViewBag.CartCount = _db.CartItems
                .Where(i => i.Cart.UserId == userId)
                .Count();
        }

        public IActionResult Index()
        {
       
            LoadCartCount();
            var products = _db.Products.Include(c => c.Category).ToList();
            return View(products);
        }

        public IActionResult Details(int id)
        {
            LoadCartCount();

            var product = _db.Products
                .Include(x => x.Category)
                .Include(x => x.Reviews)
                .ThenInclude(r => r.User)   
                .FirstOrDefault(x => x.Id == id);

            if (product == null)
                return RedirectToAction("Index");

            return View(product);
        }

    }
}
