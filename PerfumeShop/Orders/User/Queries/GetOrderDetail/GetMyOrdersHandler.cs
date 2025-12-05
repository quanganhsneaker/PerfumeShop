using MediatR;
using PerfumeShop.DTOs;
using AutoMapper;

using Microsoft.EntityFrameworkCore;
using PerfumeShop.Data;
// handlder là hàm xửa lý cái yêu cầu của hàm query
namespace PerfumeShop.Orders.User.Queries.GetOrderDetail
{
    public class GetMyOrdersHandler
       : IRequestHandler<GetMyOrdersQuery, List<OrderListVM>>
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;
        public GetMyOrdersHandler(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<List<OrderListVM>> Handle (GetMyOrdersQuery request, CancellationToken ct)
        {
            var orders = await _db.Orders.Where(o => o.UserId == request.UserId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return _mapper.Map<List<OrderListVM>>(orders);
           
            // var return = orders.Select( o => new OrderListVM 
            //{ Id = o.Id,
            // OrderCode = o.OrderCode,
            // Status = o.Status,
            // CreatedAt = o.CreatedAt,
            // TotalAmount = o.TotalAmount
            // .ToList();
        }
    }
   
}
