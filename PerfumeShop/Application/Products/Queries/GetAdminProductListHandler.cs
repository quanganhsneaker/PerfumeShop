using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Infrastructure.Data;

namespace PerfumeShop.Application.Products.Queries
{
    public class GetAdminProductListHandler : IRequestHandler<GetAdminProductListQuery, List<ProductListVM>>

    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public GetAdminProductListHandler(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<List<ProductListVM>> Handle(GetAdminProductListQuery request, CancellationToken ct)
        {
            var products = await _db.Products
                .Include(c => c.Category)
                .ToListAsync(ct);
            return _mapper.Map<List<ProductListVM>>(products);
        }
    }
}
