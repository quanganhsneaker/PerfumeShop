namespace PerfumeShop.Domain.Interfaces
{
    public interface IOrderCodeService
    {
        string GenerateOrderCode(int orderId);
    }
}
