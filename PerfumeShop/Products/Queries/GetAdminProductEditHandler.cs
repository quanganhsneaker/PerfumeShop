using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Data;
using PerfumeShop.DTOs;

namespace PerfumeShop.Products.Queries
{
    public class GetAdminProductEditHandler 
        :IRequestHandler<GetAdminProductEditQuery, ProductUpdateDto>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public GetAdminProductEditHandler(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<ProductUpdateDto> Handle(GetAdminProductEditQuery request, CancellationToken ct)
        {
            var product = await _db.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

            if (product == null) return null;

            return _mapper.Map<ProductUpdateDto>(product);
        }
    }
}
