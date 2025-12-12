using Net.payOS;
using Net.payOS.Types;
using PerfumeShop.Application.Services;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace PerfumeShop.Infrastructure.Services
{
    public class PayOSService : IPayOSService
    {
        private readonly PayOS _payOS;
        private readonly string _baseUrl;

        public PayOSService(IConfiguration config)
        {
            _payOS = new PayOS(
                config["PayOS:ClientId"],
                config["PayOS:ApiKey"],
                config["PayOS:ChecksumKey"]
            );

            _baseUrl = config["PayOS:BaseUrl"];
        }

        public async Task<CreatePaymentResult> CreatePaymentLink(int orderId, decimal amount)
        {
            var items = new List<ItemData>
            {
                new ItemData($"Đơn hàng #{orderId}", 1, (int)amount)
            };

            var paymentData = new PaymentData(
                orderCode: orderId,
                amount: (int)amount,
                description: $"Thanh toán đơn hàng #{orderId}",
                items: items,
                returnUrl: $"{_baseUrl}/Order/PaymentSuccess?orderId={orderId}",
                cancelUrl: $"{_baseUrl}/Order/PaymentCancel?orderId={orderId}"
            );

            return await _payOS.createPaymentLink(paymentData);
        }

        public string ConvertQRToBase64(string rawQr)
        {
            QRCodeGenerator gen = new QRCodeGenerator();
            QRCodeData data = gen.CreateQrCode(rawQr, QRCodeGenerator.ECCLevel.Q);
            QRCode qr = new QRCode(data);

            using (Bitmap img = qr.GetGraphic(20))
            using (var ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                return $"data:image/png;base64,{Convert.ToBase64String(ms.ToArray())}";
            }
        }
    }
}
