using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Infrastructure.Data;

namespace PerfumeShop.Application.Orders.User.Queries
{
    public class GetMyOrdersPagedHandler
        : IRequestHandler<GetMyOrdersPagedQuery, OrderListPagedVM>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public GetMyOrdersPagedHandler(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<OrderListPagedVM> Handle(GetMyOrdersPagedQuery rq, CancellationToken ct)
        {
            var query = _db.Orders
                .Where(o => o.UserId == rq.UserId)
                .OrderByDescending(o => o.CreatedAt)
                .AsQueryable();

     
            if (!string.IsNullOrEmpty(rq.SearchCode))
                query = query.Where(o => o.OrderCode.Contains(rq.SearchCode));

           
            if (!string.IsNullOrEmpty(rq.Status))
                query = query.Where(o => o.Status == rq.Status);

            int totalItems = await query.CountAsync(ct);
            int totalPages = (int)Math.Ceiling((double)totalItems / rq.PageSize);

          
            var orders = await query
                .Skip((rq.Page - 1) * rq.PageSize)
                .Take(rq.PageSize)
                .ProjectTo<OrderListVM>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);

            return new OrderListPagedVM
            {
                Orders = orders,
                CurrentPage = rq.Page,
                TotalPages = totalPages,
                PageSize = rq.PageSize,
                SearchCode = rq.SearchCode,
                StatusFilter = rq.Status
            };
        }
    }
}
