using PerfumeShop.Application.DTOs;
using PerfumeShop.Domain.Models;

namespace PerfumeShop.Domain.Interfaces
{
    public interface IOrderRepository
    {

        Task<List<OrderListVM>> GetMyOrdersAsync(int userId);

        Task<OrderListPagedVM> GetMyOrdersPagedAsync(
            int userId,
            int page,
            int pageSize,
            string? searchCode,
            string? status
        );

        Task<OrderDetailVM?> GetOrderDetailAsync(int orderId, int userId);


        Task AddAsync(Order order);
        Task AddItemsAsync(List<OrderItem> items);
        Task<Order?> GetByIdAsync(int orderId);

   
        Task<Cart?> GetCartByUserIdAsync(int userId);
        Task RemoveCartAsync(Cart cart);

    }
}
