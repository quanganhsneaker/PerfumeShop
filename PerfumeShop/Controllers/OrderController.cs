using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Data;
using PerfumeShop.DTOs;
using PerfumeShop.Helpers;
using PerfumeShop.Models;
using System.Security.Claims;

namespace PerfumeShop.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrderController(ApplicationDbContext db)
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
        // ===========================
        // CHECKOUT FORM
        // ===========================
        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Checkout(CheckoutDto dto)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            var cart = _db.Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault();

            if (cart == null || cart.Items.Count == 0)
            {
                TempData["error"] = "Giỏ hàng trống!";
                return RedirectToAction("Index", "Cart");
            }

            // 1. Tạo order trước (OrderCode tạm = "")
            var order = new Order
            {
                UserId = userId,
                FullName = dto.FullName,
                Phone = dto.Phone,
                Address = dto.Address,
                Status = "Pending",
                OrderCode = "",  // Tránh NULL
                TotalAmount = cart.Items.Sum(x => x.Product.Price * x.Qty)
            };

            _db.Orders.Add(order);
            
            
            _db.SaveChanges();

            // 2. Gán mã đơn hàng dựa trên order.Id
            order.OrderCode = OrderCodeHelper.GenerateOrderCode(order.Id);
            _db.SaveChanges();

            // 3. Lưu Order Items
            foreach (var item in cart.Items)
            {
                _db.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Price = item.Product.Price,
                    Quantity = item.Qty
                });
            }

            _db.SaveChanges();

            // 4. Xóa giỏ hàng
            _db.CartItems.RemoveRange(cart.Items);
            _db.Carts.Remove(cart);
            _db.SaveChanges();

            TempData["success"] = "Đặt hàng thành công!";
            return RedirectToAction("Detail", new { id = order.Id });
        }



        // ===========================
        // ORDER DETAIL
        // ===========================
        public IActionResult Detail(int id)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            var order = _db.Orders
                .Where(o => o.Id == id && o.UserId == userId)
                .Select(o => new OrderDetailVM
                {
                    Id = o.Id,
                    OrderCode = o.OrderCode,
                    FullName = o.FullName,
                    Phone = o.Phone,
                    Address = o.Address,
                    Status = o.Status,
                    CreatedAt = o.CreatedAt,
                    UserId = userId,
                    Items = o.OrderItems.Select(i => new OrderItemVM
                    {
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name,
                        Price = i.Price,
                        Quantity = i.Quantity,
                        Image = i.Product.ImageUrl
                    }).ToList()
                })
                .FirstOrDefault();

            if (order == null) return NotFound();

            return View(order);
        }
        [Authorize]
        public IActionResult MyOrders()
        {
            LoadCartCount();
            int userId = int.Parse(User.FindFirst("userId").Value);

            var orders = _db.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

            return View(orders);
        }
    }
}
