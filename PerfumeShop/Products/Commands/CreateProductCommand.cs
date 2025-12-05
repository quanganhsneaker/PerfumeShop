using MediatR;
using PerfumeShop.DTOs;

namespace PerfumeShop.Products.Commands
{
    public record CreateProductCommand(ProductCreateDto Dto, IFormFile? ImageFile)
        : IRequest<int>;
 
}
