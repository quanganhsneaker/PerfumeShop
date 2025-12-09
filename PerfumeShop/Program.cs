using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Data;
using PerfumeShop.DTOs;
using PerfumeShop.Helpers;
using PerfumeShop.Mappings;
using PerfumeShop.Services;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<GroqSettings>(
    builder.Configuration.GetSection("GroqAI"));

builder.Services.AddScoped<PayOSService>();

builder.Services.AddHttpClient<ChatService>();
builder.Services.AddScoped<ChatService>();

builder.Services.AddScoped<PermissionService>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
 
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", opt =>
    {
        opt.LoginPath = "/Auth/Login";
        opt.AccessDeniedPath = "/Auth/Denied";
        opt.ExpireTimeSpan = TimeSpan.FromHours(24);
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});
 
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ClaimsIssuer = "PerfumeShop";

    options.Events.OnSigningIn = context =>
    {

        var identity = context.Principal.Identity as ClaimsIdentity;

        foreach (var c in identity.Claims)
            Console.WriteLine($"CLAIM: {c.Type} = {c.Value}");

        return Task.CompletedTask;
    };
});
////
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Shop/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Shop}/{action=Index}/{id?}"
);
app.Run();
