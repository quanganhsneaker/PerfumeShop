using PerfumeShop.Domain.Models;

namespace PerfumeShop.Domain.Interfaces
{
    public interface IReviewRepository
    {
   
        Task<List<Review>> GetByProductAsync(int productId);

   
        Task<bool> HasUserBoughtProductAsync(int userId, int productId);

        Task AddAsync(Review review);

        Task<float> UpdateProductRatingAsync(int productId);
    }
}
