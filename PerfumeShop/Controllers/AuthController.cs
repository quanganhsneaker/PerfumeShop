using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PerfumeShop.Data;
using PerfumeShop.DTOs;
using PerfumeShop.Models;
using System.Security.Claims;

namespace PerfumeShop.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AuthController(ApplicationDbContext db)
        {
            _db = db;
        }

        // REGISTER
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(RegisterDto dto)
        {
            if (_db.Users.Any(x => x.Email == dto.Email))
            {
                ModelState.AddModelError("", "Email đã tồn tại");
                return View(dto);
            }

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "User"
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            return RedirectToAction("Login");
        }

        // LOGIN
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = _db.Users.SingleOrDefault(x => x.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Sai email hoặc mật khẩu");
                return View(dto);
            }

            // Claims
            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity)
            );
            if (user.Role =="Admin")
            { return RedirectToAction("Dashboard", "AdminHome"); }   
            return RedirectToAction("Index", "Shop");
        }

        // LOGOUT
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        // ACCESS DENIED
        public IActionResult Denied() => View();
    }
}
