namespace PerfumeShop.Application.Services
{
    public class OrderCodeService : IOrderCodeService
    {
        public string GenerateOrderCode(int orderId)
        {
            return $"DH{orderId.ToString().PadLeft(5, '0')}";
        }
    }
}
