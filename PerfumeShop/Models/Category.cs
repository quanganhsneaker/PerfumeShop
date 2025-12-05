    using PerfumeShop.Core;

    namespace PerfumeShop.Models
    {
        public class Category : BaseEntity
        {
 
            public string Name { get; set; }
            public List<Product> Products { get; set; }
        }
    }
