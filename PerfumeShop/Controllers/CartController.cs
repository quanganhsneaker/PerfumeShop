using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PerfumeShop.Data;
using PerfumeShop.Helpers;
using PerfumeShop.Models;

namespace PerfumeShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }
     

        // Lấy hoặc tạo giỏ hàng của user
        private Cart GetOrCreateCart()
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            var cart = _db.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _db.Carts.Add(cart);
                _db.SaveChanges();
            }

            return cart;
        }
        // đếm số lượng item ở trong giỏ 
        private int GetCartCount()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return 0;
            }
            int userId = int.Parse(User.FindFirst("userId").Value);
            return _db.CartItems
                .Where(i => i.Cart.UserId == userId)
                .Select(i => i.ProductId)
        .Distinct()
                .Count();
        }
        // ==========================
        // Hiển thị giỏ hàng
        // ==========================
        public IActionResult Index()
        {
         
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");
            ViewBag.CartCount = GetCartCount();
            var cart = GetOrCreateCart();
            return View(cart);
        }

        // ==========================
        // Thêm vào giỏ hàng
        // ==========================
        public IActionResult Add(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var cart = GetOrCreateCart();

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item == null)
            {
                cart.Items.Add(new CartItem { ProductId = id, Qty = 1 });
            }
            else
            {
                item.Qty++;
            }

            _db.SaveChanges();

            int count = GetCartCount();  // đếm số product khác nhau

            return Json(new { cartCount = count });
        }


        // ==========================
        // Update số lượng
        // ==========================
        [HttpPost]
        public IActionResult UpdateQty(int productId, int qty)
        {
            var cart = GetOrCreateCart();

            if (cart == null) return RedirectToAction("Index");
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null) return RedirectToAction("Index");
            if (qty <= 0)
                _db.CartItems.Remove(item);
            else
                item.Qty = qty;

            _db.SaveChanges();
            ViewBag.CartCount = GetCartCount();
            return RedirectToAction("Index");
        }

        // ==========================
        // Xóa item khỏi giỏ hàng
        // ==========================
        public IActionResult Remove(int productId)
        {
            var cart = GetOrCreateCart();
            if (cart == null) return RedirectToAction("Index");
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                _db.CartItems.Remove(item);
                _db.SaveChanges();
                TempData["DeleteSuccess"] = "Sản phẩm đã được xoá khỏi giỏ hàng!";
            }
            ViewBag.CartCount = GetCartCount();

            return RedirectToAction("Index");
        }
        // =======================
        // Tăng số lượng
        // =======================
        [HttpPost]
        public IActionResult Increase(int productId)
        {
            var cart = GetOrCreateCart();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                item.Qty++;
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // =======================
        // Giảm số lượng
        // =======================
        [HttpPost]
        public IActionResult Decrease(int productId)
        {
            var cart = GetOrCreateCart();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                item.Qty--;

                if (item.Qty <= 0)
                    _db.CartItems.Remove(item);

                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}
