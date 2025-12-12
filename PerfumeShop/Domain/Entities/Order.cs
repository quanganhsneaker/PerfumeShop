using PerfumeShop.Domain.Core;

namespace PerfumeShop.Domain.Models
{
    public class Order : BaseEntity
    {
   
        public string OrderCode { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }

        public string PaymentStatus { get; set; } = "Unpaid";

        // IMPORTANT: THIS IS WHAT YOU ARE MISSING
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    }
}
