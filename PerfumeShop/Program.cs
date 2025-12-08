using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Data;
using PerfumeShop.Helpers;
using PerfumeShop.Mappings;
using PerfumeShop.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<PayOSService>();


builder.Services.AddScoped<PermissionService>();
//AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

// 1) MVC
builder.Services.AddControllersWithViews();


// 2) SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 3) Cookie Authentication (MVC thuần)
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", opt =>
    {
        opt.LoginPath = "/Auth/Login";
        opt.AccessDeniedPath = "/Auth/Denied";
        opt.ExpireTimeSpan = TimeSpan.FromHours(24);
    });
// 4) Authorization (Roles)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// 5) Session (giỏ hàng)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// 6) Error handler
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Shop/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// 7) Auth
app.UseAuthentication();
app.UseAuthorization();

// 8) Session
app.UseSession();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Shop}/{action=Index}/{id?}"
);
app.Run();
