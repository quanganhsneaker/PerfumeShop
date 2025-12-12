namespace PerfumeShop.Application.Services
{
    public interface IOrderCodeService
    {
        string GenerateOrderCode(int orderId);
    }
}
