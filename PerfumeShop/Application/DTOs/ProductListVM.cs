namespace PerfumeShop.Application.DTOs
{
    public class ProductListVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public float Rating { get; set; }
    }
}
