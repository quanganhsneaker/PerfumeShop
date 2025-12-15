using PerfumeShop.Domain.Models;

namespace PerfumeShop.Domain.Interfaces
{
    public interface IProductRepository
    {
    
        Task<List<Product>> GetAllWithCategoryAsync();
        Task<Product?> GetByIdAsync(int id);


        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
    }
}
