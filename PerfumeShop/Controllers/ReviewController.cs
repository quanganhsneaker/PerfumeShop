using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeShop.Data;
using PerfumeShop.DTOs;
using PerfumeShop.Models;
using System.Security.Claims;

namespace PerfumeShop.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ReviewController(ApplicationDbContext db)
        {
            _db = db;
        }
        private bool UserBoughtProduct(int userId, int productId)
        {
            return _db.OrderItems
                .Any(x =>
                    x.ProductId == productId &&
                    x.Order.UserId == userId &&
                    x.Order.Status == "Completed"
                );
        }

        // ===========================
        // ADD REVIEW
        // ===========================
        [Authorize]
        [HttpPost]
        public IActionResult AddReview(ReviewDto dto)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            // kiểm tra user có đơn hàng Completed với sản phẩm chưa?
            if (!UserBoughtProduct(userId, dto.ProductId))
                return Unauthorized("Bạn chưa mua sản phẩm này hoặc đơn hàng chưa hoàn thành.");

            var review = new Review
            {
                UserId = userId,
                ProductId = dto.ProductId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.Now
            };

            _db.Reviews.Add(review);
            _db.SaveChanges();

            // Cập nhật điểm trung bình
            var avg = _db.Reviews
                .Where(x => x.ProductId == dto.ProductId)
                .Average(x => x.Rating);

            var product = _db.Products.Find(dto.ProductId);
            product.Rating = (float)avg;
            _db.SaveChanges();

            return Redirect("/Order/Detail/" + dto.OrderId);
        }


        // ===========================
        // SHOW REVIEWS FOR PRODUCT
        // ===========================
        public IActionResult List(int productId)
        {
            var list = _db.Reviews
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            return View(list);
        }
    }
}
