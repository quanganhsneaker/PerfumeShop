using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PerfumeShop.Application.Services;
using PerfumeShop.Infrastructure.Data;
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
                options.UseSqlServer
                (config.GetConnectionString("DefaultConnection")));

            services.AddHttpClient();

         
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IPayOSService, PayOSService>();
            services.AddScoped<IPermissionService, PermissionService>();

            return services;
        }
    }
}
