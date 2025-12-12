using System.Collections.Generic;

namespace PerfumeShop.Application.DTOs
{
    public class AdminOrderDetailVM
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<OrderItemVM> Items { get; set; } = new();
    }
}
