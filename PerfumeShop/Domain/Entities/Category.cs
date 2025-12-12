using PerfumeShop.Domain.Core;

namespace PerfumeShop.Domain.Models
    {
        public class Category : BaseEntity
        {
 
            public string Name { get; set; }
            public List<Product> Products { get; set; }
        }
    }
