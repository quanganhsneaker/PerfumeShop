using PerfumeShop.Domain.Models;

namespace PerfumeShop.Application.Auth.Commands.Login
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public User User { get; set; }
        public bool RememberMe { get; set; }
    }
}
