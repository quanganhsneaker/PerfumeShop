using System.Collections.Generic;

namespace PerfumeShop.DTOs
{
    public class OrderDetailVM
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }

        public List<OrderItemVM> Items { get; set; } = new();
    }
}
