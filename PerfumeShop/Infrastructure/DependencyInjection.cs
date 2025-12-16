using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PerfumeShop.Domain.Interfaces;
using PerfumeShop.Infrastructure.Data;
using PerfumeShop.Infrastructure.Persistence;
using PerfumeShop.Infrastructure.Persistence.Repositories;
using PerfumeShop.Infrastructure.Repositories;
using PerfumeShop.Infrastructure.Services;

namespace PerfumeShop.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    config.GetConnectionString("DefaultConnection")));

            services.AddHttpClient();
            
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IOrderRepository, OrderRepository>();          
            services.AddScoped<IAdminOrderRepository, AdminOrderRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IOrderCodeService, OrderCodeService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPayOSService, PayOSService>();
            services.AddScoped<PayOSService>();

            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IPermissionService, PermissionService>();

            return services;
        }
    }
}
