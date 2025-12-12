using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Application.Services;
using PerfumeShop.Domain.Models;
using PerfumeShop.Infrastructure.Data;

namespace PerfumeShop.Application.Orders.User.Commands
{
    public class CreateOrderHandler
        : IRequestHandler<CreateOrderCommand, int>
    {

        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IOrderCodeService _orderCodeService;
        public CreateOrderHandler(ApplicationDbContext db, IOrderCodeService orderCodeService, IMapper mapper)
        {
            _db = db;
            _orderCodeService = orderCodeService;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateOrderCommand request, CancellationToken ct)
        {
            var cart = await _db.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == request.UserId);
            if (cart == null || cart.Items.Count == 0)
                return -1;
            var dto = request.Dto;
            //
            var order = _mapper.Map<Order>(request.Dto);        
            order.UserId = request.UserId;
            order.TotalAmount = cart.Items.Sum(x => x.Product.Price * x.Qty);
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            //
            order.OrderCode = _orderCodeService.GenerateOrderCode(order.Id);
            await _db.SaveChangesAsync();
            //
            var orderItems = _mapper.Map<List<OrderItem>>(cart.Items);
            foreach (var item in orderItems)
                item.OrderId = order.Id;
            _db.OrderItems.AddRange(orderItems);
            await _db.SaveChangesAsync();
            //
            _db.CartItems.RemoveRange(cart.Items);
            _db.Carts.Remove(cart);
            await _db.SaveChangesAsync();

            return order.Id;
        }
    }
}
