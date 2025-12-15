using PerfumeShop.Application.DTOs;
using PerfumeShop.Domain.Models;

namespace PerfumeShop.Domain.Interfaces
{
    public interface IAdminOrderRepository
    {

        Task<List<AdminOrderListVM>> GetAllAsync();

        Task<AdminOrderPagedVM> GetPagedAsync(
            int page,
            int pageSize,
            string? searchCode,
            string? status
        );

        Task<AdminOrderDetailVM?> GetDetailAsync(int orderId);

     
        Task<Order?> GetByIdAsync(int orderId);

        Task<bool> UpdateStatusAsync(
            int orderId,
            string status
        );
    }
}
