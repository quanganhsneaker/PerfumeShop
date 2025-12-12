namespace PerfumeShop.Application.DTOs
{
    public class AdminOrderPagedVM
    {
        public List<AdminOrderListVM> Orders { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        public string SearchCode { get; set; }
        public string StatusFilter { get; set; }
    }
}
