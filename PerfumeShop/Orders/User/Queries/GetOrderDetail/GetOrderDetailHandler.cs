using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Data;
using PerfumeShop.DTOs;
namespace PerfumeShop.Orders.User.Queries.GetOrderDetail
{
    public class GetOrderDetailHandler 
        : IRequestHandler<GetOrderDetailQuery, OrderDetailVM>
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;
        public GetOrderDetailHandler(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<OrderDetailVM> Handle ( GetOrderDetailQuery request, CancellationToken ct)
        {
            var order = await _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId
                && o.UserId == request.UserId);
            if (order == null) return null;
            return _mapper.Map<OrderDetailVM>(order);
        }
    }
}
