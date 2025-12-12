using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Infrastructure.Data;

namespace PerfumeShop.Application.Orders.Admin.Queries.GetAdminOrders
{
    public class GetAdminOrderDetailHandler
        :IRequestHandler<GetAdminOrderDetailQuery , AdminOrderDetailVM>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
         
        public GetAdminOrderDetailHandler(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<AdminOrderDetailVM> Handle( GetAdminOrderDetailQuery request, CancellationToken ct)
        {
            var order = await _db.Orders
                .Include(o => o.User)
                .Include ( o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId);
            return _mapper.Map<AdminOrderDetailVM>(order);
        }

    }
}
