using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Data;
using PerfumeShop.DTOs;

namespace PerfumeShop.Orders.Admin.Queries.GetAdminOrders
{
    public class GetAdminOrdersHandler
         : IRequestHandler<GetAdminOrdersQuery, List<AdminOrderListVM>>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public GetAdminOrdersHandler(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<List<AdminOrderListVM>> Handle(GetAdminOrdersQuery request, CancellationToken ct)
        {
            var orders = await _db.Orders
                .Include(o => o.User)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
            return _mapper.Map<List<AdminOrderListVM>>(orders);
        }
    }
}
