using AutoMapper;
using MediatR;
using PerfumeShop.Domain.Interfaces;
using PerfumeShop.Domain.Models;

namespace PerfumeShop.Application.Products.Commands
{
    public class CreateProductHandler
        : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IProductRepository _repo;
        private readonly IFileStorageService _file;
        private readonly IMapper _mapper;

        public CreateProductHandler(
            IProductRepository repo,
            IFileStorageService file,
            IMapper mapper)
        {
            _repo = repo;
            _file = file;
            _mapper = mapper;
        }

        public async Task<int> Handle(
            CreateProductCommand request,
            CancellationToken ct)
        {
            var product = _mapper.Map<Product>(request.Dto);

            if (request.ImageFile != null)
            {
                product.ImageUrl =
                    await _file.SaveProductImageAsync(request.ImageFile);
            }

            await _repo.AddAsync(product);
            return product.Id;
        }
    }
}
