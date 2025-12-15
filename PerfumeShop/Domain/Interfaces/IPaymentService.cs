using PerfumeShop.Application.DTOs;

namespace PerfumeShop.Domain.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResult> CreatePaymentAsync(int orderId, decimal amount);
    }
}
