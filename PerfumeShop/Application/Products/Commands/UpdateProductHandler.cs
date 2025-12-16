using AutoMapper;
using MediatR;
using PerfumeShop.Domain.Interfaces;

namespace PerfumeShop.Application.Products.Commands
{
    public class UpdateProductHandler
        : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _repo;
        private readonly IFileStorageService _file;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        public UpdateProductHandler(
            IProductRepository repo,
            IFileStorageService file,
            IMapper mapper,
            IUnitOfWork uow)
        {
            _repo = repo;
            _file = file;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<bool> Handle(
            UpdateProductCommand request,
            CancellationToken ct)
        {
            var product = await _repo.GetByIdAsync(request.Dto.Id);
            if (product == null) return false;

            _mapper.Map(request.Dto, product);

            if (request.ImageFile != null)
            {
                product.ImageUrl =
                    await _file.SaveProductImageAsync(request.ImageFile);
            }

            await _repo.UpdateAsync(product);
            await _uow.SaveChangesAsync(ct);
            return true;
        }
    }
}
