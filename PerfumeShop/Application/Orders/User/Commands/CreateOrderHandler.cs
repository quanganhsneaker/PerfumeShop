using AutoMapper;
using MediatR;
using PerfumeShop.Domain.Interfaces;
using PerfumeShop.Domain.Models;

namespace PerfumeShop.Application.Orders.User.Commands
{
    public class CreateOrderHandler
        : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IUnitOfWork _uow;
        private readonly IOrderCodeService _orderCodeService;
        private readonly IMapper _mapper;

        public CreateOrderHandler(
            IOrderRepository orderRepo,
            IUnitOfWork uow,
            IOrderCodeService orderCodeService,
            IMapper mapper)
        {
            _orderRepo = orderRepo;
            _uow = uow;
            _orderCodeService = orderCodeService;
            _mapper = mapper;
        }

        public async Task<int> Handle(
            CreateOrderCommand request,
            CancellationToken ct)
        {
         
            var cart = await _orderRepo.GetCartByUserIdAsync(request.UserId);
            if (cart == null || cart.Items.Count == 0)
                return -1;

            var order = _mapper.Map<Order>(request.Dto);
            order.UserId = request.UserId;
            order.TotalAmount = cart.Items.Sum(
                x => x.Product.Price * x.Qty
            );
            order.CreatedAt = DateTime.Now;
            order.Status = "Pending";

            await _orderRepo.AddAsync(order);
            await _uow.SaveChangesAsync(ct); 

            order.OrderCode =
                _orderCodeService.GenerateOrderCode(order.Id);

            await _uow.SaveChangesAsync(ct);

            var orderItems = cart.Items.Select(i => new OrderItem
            {
                OrderId = order.Id,
                ProductId = i.ProductId,
                Quantity = i.Qty,
                Price = i.Product.Price
            }).ToList();

            await _orderRepo.AddItemsAsync(orderItems);
            await _uow.SaveChangesAsync(ct);

            await _orderRepo.RemoveCartAsync(cart);
            await _uow.SaveChangesAsync(ct);

            return order.Id;
        }
    }
}
