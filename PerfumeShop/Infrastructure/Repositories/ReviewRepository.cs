using Microsoft.EntityFrameworkCore;
using PerfumeShop.Domain.Interfaces;
using PerfumeShop.Domain.Models;
using PerfumeShop.Infrastructure.Data;

namespace PerfumeShop.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _db;

        public ReviewRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Review>> GetByProductAsync(int productId)
        {
            return await _db.Reviews
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> HasUserBoughtProductAsync(int userId, int productId)
        {
            return await _db.OrderItems
                .AnyAsync(x =>
                    x.ProductId == productId &&
                    x.Order.UserId == userId &&
                    x.Order.Status == "Completed"
                );
        }

        public async Task AddAsync(Review review)
        {
            await _db.Reviews.AddAsync(review);
            await _db.SaveChangesAsync();
        }

        public async Task<float> UpdateProductRatingAsync(int productId)
        {
            var avg = await _db.Reviews
                .Where(x => x.ProductId == productId)
                .AverageAsync(x => x.Rating);

            var product = await _db.Products.FindAsync(productId);
            if (product != null)
            {
                product.Rating = (float)avg;
                await _db.SaveChangesAsync();
            }

            return (float)avg;
        }
    }
}
