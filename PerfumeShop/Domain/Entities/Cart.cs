using PerfumeShop.Domain.Core;

namespace PerfumeShop.Domain.Models
{
    public class Cart : BaseEntity
    {
   
        public int UserId { get; set; }
        public User User { get; set; }

        public List<CartItem> Items { get; set; } = new();
    } 
}
