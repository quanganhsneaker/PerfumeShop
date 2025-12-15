using Microsoft.EntityFrameworkCore;
using PerfumeShop.Infrastructure.Data;
using PerfumeShop.Domain.Models;

namespace PerfumeShop.Tests.Common
{
    public class TestDbContext : ApplicationDbContext
    {
        public TestDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

         
            builder.Entity<Order>()
                .Property(o => o.OrderCode)
                .IsRequired(false);

            builder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired(false);

            builder.Entity<Product>()
                .Property(p => p.Description)
                .IsRequired(false);
        }
    }
}
