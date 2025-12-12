using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PerfumeShop.Domain.Models;
using PerfumeShop.Infrastructure.Data;

namespace PerfumeShop.Application.Auth.Commands.Login
{
    public class LoginHandler :IRequestHandler<LoginCommand, LoginResult>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMemoryCache _cache;

        public LoginHandler (ApplicationDbContext db, IMemoryCache cache)
        { 
            _db = db;
            _cache = cache;
        }
        public async Task<LoginResult?>Handle(LoginCommand request, CancellationToken ct)
        {
            var dto = request.Dto;

            dto.Email = dto.Email.Trim().ToLower();
        
            string key = $"login_failed_{dto.Email}";
            if(_cache.TryGetValue(key, out int failCount) && failCount >= 5)
            {
                return new LoginResult
                {
                    Success = false,
                    Error = " Tài khoản bị khóa do đăng nhập sai quá nhiều lần. Vui lòng thử lại sau 5 phút."
                };
               

            }
            var user = await _db.Users.SingleOrDefaultAsync(x => x.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                failCount++;
                _cache.Set(key, failCount, TimeSpan.FromMinutes(5));

                return new LoginResult
                {
                    Success = false,
                    Error = "Sai email hoặc mật khẩu."
                };
            }

        
            _cache.Remove(key);

            return new LoginResult
            {
                Success = true,
                User = user,
                RememberMe = dto.RememberMe
            };
        }
    }
}
