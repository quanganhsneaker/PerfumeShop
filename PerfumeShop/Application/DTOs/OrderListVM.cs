using PerfumeShop.Domain.Models;
using System;

namespace PerfumeShop.Application.DTOs
{
    public class OrderListVM
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }


        public string CreatedDateString => CreatedAt.ToString("dd/MM/yyyy");

        public string PaymentStatus { get; set; }

        public int UserId { get; set; } 
    }
}
