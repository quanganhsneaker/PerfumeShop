using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PerfumeShop.Auth.Commands.Login;
using PerfumeShop.Auth.Commands.Register;
using PerfumeShop.Data;
using PerfumeShop.DTOs;
using PerfumeShop.Models;
using System.Security.Claims;

namespace PerfumeShop.Controllers
{
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);
            var result = await _mediator.Send(new RegisterCommand(dto));
            //dto.Email = dto.Email.Trim().ToLower();

            if (!result)
            {
                TempData["toast"] = "Email đã tồn tại!";
                TempData["toastType"] = "error";
                return View(dto);
            }
            //var user = new User
            //{
            //    FullName = dto.FullName,
            //    Email = dto.Email,
            //    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            //    Role = "User"
            //};

            //_db.Users.Add(user);
            //await _db.SaveChangesAsync();
            TempData["toast"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            TempData["toastType"] = "success";

            return RedirectToAction("Login");
        }


    
        public IActionResult Login() => View();



        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);
            var result = await _mediator.Send(new LoginCommand(dto));

            //dto.Email = dto.Email.Trim().ToLower();

            //string key = $"login_fail_{dto.Email}";
            if (!result.Success)
            {
                TempData["toast"] = result.Error;
                TempData["toastType"] = "error";
                return View(dto);
            }

            //var user = await _db.Users.SingleOrDefaultAsync(x => x.Email == dto.Email);

            //if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            //{
            //    failCount++;
            //    _cache.Set(key, failCount, TimeSpan.FromMinutes(5)); // tăng số lần sai
            //    ModelState.AddModelError("", "Sai email hoặc mật khẩu");
            //    return View(dto);
            //}


            //_cache.Remove(key);


            var claims = new List<Claim>
            {
                new Claim("userId", result.User.Id.ToString()),
                new Claim(ClaimTypes.Name, result.User.FullName),
                new Claim(ClaimTypes.Role, result.User.Role)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var authProps = new AuthenticationProperties
            {
                IsPersistent = dto.RememberMe,
                ExpiresUtc = DateTime.UtcNow.AddDays(30)
            };


            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                authProps
            );


            TempData["toast"] = "Đăng nhập thành công!";
            TempData["toastType"] = "success";
            if (result.User.Role == "Admin" || result.User.Role == "Staff")
                return RedirectToAction("Dashboard", "AdminHome");
            
         
            return RedirectToAction("Index", "Shop");
        }



        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            TempData["toast"] = "Đăng xuất thành công!";
            TempData["toastType"] = "success";
            return RedirectToAction("Login");
        }

        public IActionResult Denied() => View();
    }
}
