using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using PerfumeShop.Application;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Infrastructure;
using PerfumeShop.Infrastructure.Data;
using PerfumeShop.Presentation.Filters;
using System.Security.Claims;
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "Presentation/wwwroot"
});

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ValidationExceptionFilter>();
})
.AddRazorOptions(options =>
{
    options.ViewLocationFormats.Clear();
    options.ViewLocationFormats.Add("/Presentation/Views/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Presentation/Views/Shared/{0}.cshtml");
});
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.Configure<GroqSettings>(
builder.Configuration.GetSection("GroqAI"));

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(
//    builder.Configuration.GetConnectionString("DefaultConnection")
//    )
//);
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

var presentationRoot = Path.Combine(builder.Environment.ContentRootPath, "Presentation", "wwwroot");
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(presentationRoot),
    RequestPath = ""
});
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Shop}/{action=Index}/{id?}"
);
app.Run();
