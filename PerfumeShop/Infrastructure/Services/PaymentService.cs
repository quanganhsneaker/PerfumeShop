using PerfumeShop.Application.DTOs;
using PerfumeShop.Domain.Interfaces;

namespace PerfumeShop.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PayOSService _payOS;

        public PaymentService(PayOSService payOS)
        {
            _payOS = payOS;
        }

        public async Task<PaymentResult> CreatePaymentAsync(int orderId, decimal amount)
        {
            var result = await _payOS.CreatePaymentLink(orderId, amount);

            return new PaymentResult
            {
                QrCode = _payOS.ConvertQRToBase64(result.qrCode),
                CheckoutUrl = result.checkoutUrl
            };
        }
    }
}
