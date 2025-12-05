namespace PerfumeShop.Auth.Commands.Login
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public PerfumeShop.Models.User User { get; set; }
        public bool RememberMe { get; set; }
    }
}
