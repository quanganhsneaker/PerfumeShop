using PerfumeShop.Core;

namespace PerfumeShop.Models
{
    public class Product : BaseEntity
    {
    
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public float Rating { get; set; }

        public List<Review> Reviews { get; set; }
        public Category Category { get; set; }
    }
}
