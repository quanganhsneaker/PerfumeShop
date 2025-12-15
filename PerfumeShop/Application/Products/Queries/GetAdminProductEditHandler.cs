using AutoMapper;
using MediatR;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Domain.Interfaces;

namespace PerfumeShop.Application.Products.Queries
{
    public class GetAdminProductEditHandler
        : IRequestHandler<GetAdminProductEditQuery, ProductUpdateDto?>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public GetAdminProductEditHandler(
            IProductRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ProductUpdateDto?> Handle(
            GetAdminProductEditQuery request,
            CancellationToken ct)
        {
            var product = await _repo.GetByIdAsync(request.Id);
            if (product == null) return null;

            return _mapper.Map<ProductUpdateDto>(product);
        }
    }
}
