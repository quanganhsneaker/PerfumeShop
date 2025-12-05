using MediatR;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Data;
using PerfumeShop.Helpers;
using PerfumeShop.Models;
// commands hay xử lý các created update các thứ còn Queries là hay mấy cái truy vấn get ý 

namespace PerfumeShop.Orders.User.Commands
{
    public class CreateOrderHandler
        : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly ApplicationDbContext _db;

        public CreateOrderHandler(ApplicationDbContext db)
        {
            _db = db;
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

            var order = new Order
            {
                UserId = request.UserId,
                FullName = dto.FullName,
                Phone = dto.Phone,
                Address = dto.Address,
                Status = "Pending",
                TotalAmount = cart.Items.Sum(x => x.Product.Price * x.Qty)
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            order.OrderCode = OrderCodeHelper.GenerateOrderCode(order.Id);
            await _db.SaveChangesAsync();

            foreach (var item in cart.Items)
            {
                _db.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Price = item.Product.Price,
                    Quantity = item.Qty
                });
            }

            await _db.SaveChangesAsync();

            _db.CartItems.RemoveRange(cart.Items);
            _db.Carts.Remove(cart);
            await _db.SaveChangesAsync();

            return order.Id;
        }
    }
}
