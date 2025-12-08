namespace PerfumeShop.DTOs
{
    public class AdminOrderListVM
    {
    public int Id { get; set; }
        public string OrderCode { get; set; }
        public string CustomerName { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public string PaymentStatus { get; set; }
        public string CreatedDateString => CreatedAt.ToString("dd/MM/yyyy");


   
    }
}
