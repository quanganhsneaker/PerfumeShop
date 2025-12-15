using MediatR;
using System.Reflection;
using FluentValidation;
using PerfumeShop.Application.Common.Behaviors;
using PerfumeShop.Domain.Interfaces;
namespace PerfumeShop.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {         
            services.AddAutoMapper(Assembly.GetExecutingAssembly());     
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped<IOrderCodeService, OrderCodeService>();
            return services;
        }
    }
}
