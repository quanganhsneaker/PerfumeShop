using Microsoft.EntityFrameworkCore;
using PerfumeShop.Domain.Interfaces;
using PerfumeShop.Domain.Models;
using PerfumeShop.Infrastructure.Data;

namespace PerfumeShop.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<List<Product>> GetAllWithCategoryAsync()
        {
            return await _db.Products
                .Include(p => p.Category)
                .ToListAsync();
        }
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _db.Products.FindAsync(id);
        }
        public Task AddAsync(Product product)
        {
            _db.Products.Add(product);
            return Task.CompletedTask;
        }
        public Task UpdateAsync(Product product)
        {
            _db.Products.Update(product);
            return Task.CompletedTask;
        }
    }

}
