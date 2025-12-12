using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Infrastructure.Data;

namespace PerfumeShop.Application.Orders.Admin.Queries
{
    public class GetAdminOrdersPagedHandler
        : IRequestHandler<GetAdminOrdersPagedQuery, AdminOrderPagedVM>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public GetAdminOrdersPagedHandler(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<AdminOrderPagedVM> Handle(GetAdminOrdersPagedQuery rq, CancellationToken ct)
        {
            var query = _db.Orders
                .Include(o => o.User)
                .OrderByDescending(o => o.CreatedAt)
                .AsQueryable();


            if (!string.IsNullOrWhiteSpace(rq.SearchCode))
                query = query.Where(o => o.OrderCode.Contains(rq.SearchCode));

     
            if (!string.IsNullOrWhiteSpace(rq.Status))
                query = query.Where(o => o.Status == rq.Status);


            int totalItems = await query.CountAsync(ct);
            int totalPages = (int)Math.Ceiling((double)totalItems / rq.PageSize);

          
            var orders = await query
                .Skip((rq.Page - 1) * rq.PageSize)
                .Take(rq.PageSize)
                .ProjectTo<AdminOrderListVM>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);

            return new AdminOrderPagedVM
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
