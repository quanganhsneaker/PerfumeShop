namespace PerfumeShop.Helpers
{
    public class OrderCodeHelper
    {
        public static string GenerateOrderCode(int orderId)
        {
            return $"DH{orderId.ToString().PadLeft(5,'0')}";
        }
    }
}
