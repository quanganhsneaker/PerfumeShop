using MediatR;
using Microsoft.Extensions.Caching.Memory;
using PerfumeShop.Domain.Interfaces;

namespace PerfumeShop.Application.Auth.Commands.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResult>
    {
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMemoryCache _cache;

        public LoginHandler(
            IUserRepository userRepo,
            IUnitOfWork uow,
            IMemoryCache cache)
        {
            _userRepo = userRepo;
            _uow = uow;
            _cache = cache;
        }

        public async Task<LoginResult> Handle(LoginCommand request, CancellationToken ct)
        {
            var dto = request.Dto;
            dto.Email = dto.Email.Trim().ToLower();

            string key = $"login_failed_{dto.Email}";

            if (_cache.TryGetValue(key, out int failCount) && failCount >= 5)
            {
                return new LoginResult
                {
                    Success = false,
                    Error = "Tài khoản bị khóa 5 phút do đăng nhập sai nhiều lần."
                };
            }

            var user = await _userRepo.GetByEmailAsync(dto.Email);

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
