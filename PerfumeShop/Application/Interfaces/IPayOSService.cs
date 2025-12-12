using Net.payOS.Types;
using System.Threading.Tasks;

namespace PerfumeShop.Application.Services
{
    public interface IPayOSService
    {
        Task<CreatePaymentResult> CreatePaymentLink(int orderId, decimal amount);
        string ConvertQRToBase64(string rawQr);
    }
}
