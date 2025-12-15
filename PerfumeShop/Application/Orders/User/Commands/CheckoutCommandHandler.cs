using MediatR;
using PerfumeShop.Domain.Interfaces;

namespace PerfumeShop.Application.Orders.User.Commands
{
    public class CheckoutCommandHandler
        : IRequestHandler<CheckoutCommand, CheckoutResult>
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IPaymentService _paymentService;
        private readonly IMediator _mediator;

        public CheckoutCommandHandler(
            IOrderRepository orderRepo,
            IPaymentService paymentService,
            IMediator mediator)
        {
            _orderRepo = orderRepo;
            _paymentService = paymentService;
            _mediator = mediator;
        }

        public async Task<CheckoutResult> Handle(
            CheckoutCommand request,
            CancellationToken ct)
        {
    
            int orderId = await _mediator.Send(
                new CreateOrderCommand(
                    request.Dto,
                    request.UserId
                ),
                ct
            );

            if (orderId <= 0)
            {
                return new CheckoutResult
                {
                    OrderId = -1
                };
            }

            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null)
            {
                return new CheckoutResult
                {
                    OrderId = -1
                };
            }

            if (request.Dto.PaymentMethod == "ONLINE")
            {
                var payment = await _paymentService
                    .CreatePaymentAsync(
                        orderId,
                        order.TotalAmount
                    );

                return new CheckoutResult
                {
                    OrderId = orderId,
                    IsOnline = true,
                    Amount = order.TotalAmount,
                    QrCode = payment.QrCode,
                    RedirectUrl = payment.CheckoutUrl
                };
            }

            return new CheckoutResult
            {
                OrderId = orderId,
                IsOnline = false
            };
        }
    }
}
