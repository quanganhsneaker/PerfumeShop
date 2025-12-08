using Net.payOS;
using Net.payOS.Types;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace PerfumeShop.Services
{
    public class PayOSService
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

            // ⭐ LẤY BASE URL TỪ appsettings.json
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

                // ⭐ DÙNG BASE URL TỪ CONFIG
                returnUrl: $"{_baseUrl}/Order/PaymentSuccess?orderId={orderId}",
                cancelUrl: $"{_baseUrl}/Order/PaymentCancel?orderId={orderId}"
            );

            var result = await _payOS.createPaymentLink(paymentData);

            Console.WriteLine("📌 JSON PayOS:");
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result));

            return result;
        }

        public string ConvertQRToBase64(string rawQr)
        {
            QRCodeGenerator qrGen = new QRCodeGenerator();
            QRCodeData data = qrGen.CreateQrCode(rawQr, QRCodeGenerator.ECCLevel.Q);
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
