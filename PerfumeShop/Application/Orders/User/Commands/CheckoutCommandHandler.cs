using MediatR;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Application.Orders.User.Commands;
using PerfumeShop.Application.Services;
using PerfumeShop.Infrastructure.Data;
using PerfumeShop.Infrastructure.Services;
using PerfumeShop.Application.Services;
namespace PerfumeShop.Application.Orders.User.Commands
{
    public class CheckoutCommandHandler : IRequestHandler<CheckoutCommand, CheckoutResult>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMediator _mediator;
        private readonly IPayOSService _payOS;
        public CheckoutCommandHandler(ApplicationDbContext db, IMediator mediator, IPayOSService payOS)
        {
            _db = db;
            _mediator = mediator;
            _payOS = payOS;
        }
        public async Task<CheckoutResult> Handle(CheckoutCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var userId = request.UserId;
            int newOrderId = await _mediator.Send(new CreateOrderCommand(dto, userId));
            if (newOrderId < 0)
                return new CheckoutResult { OrderId = -1 };
            var order = await _db.Orders.FirstAsync(x => x.Id == newOrderId);
            if (dto.PaymentMethod == "ONLINE")
            {
                var payData = await _payOS.CreatePaymentLink(newOrderId, order.TotalAmount);

                return new CheckoutResult
                {
                    OrderId = newOrderId,
                    IsOnline = true,
                    Amount = order.TotalAmount,
                    QrCode = _payOS.ConvertQRToBase64(payData.qrCode),
                    //RedirectUrl = payData.checkoutUrl
                };
            }
            return new CheckoutResult
            {
                OrderId = newOrderId,
                IsOnline = false
            };
        }


    }
}
