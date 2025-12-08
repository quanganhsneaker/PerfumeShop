namespace PerfumeShop.Orders.User.Commands
{
    public class CheckoutResult
    {
        public int OrderId { get; set; }
        public bool IsOnline { get; set; }

        public string? RedirectUrl { get; set; } 
        public string? QrCode { get; set; }     
        public decimal Amount { get; set; }
    }

}
