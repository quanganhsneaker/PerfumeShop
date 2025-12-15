using AutoMapper;
using MediatR;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Domain.Interfaces;

namespace PerfumeShop.Application.Products.Queries
{
    public class GetAdminProductListHandler
        : IRequestHandler<GetAdminProductListQuery, List<ProductListVM>>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public GetAdminProductListHandler(
            IProductRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<ProductListVM>> Handle(
            GetAdminProductListQuery request,
            CancellationToken ct)
        {
            var products = await _repo.GetAllWithCategoryAsync();
            return _mapper.Map<List<ProductListVM>>(products);
        }
    }
}
