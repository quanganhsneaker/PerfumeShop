using MediatR;
using PerfumeShop.Application.DTOs;

namespace PerfumeShop.Application.Products.Commands
{
    public record CreateProductCommand(ProductCreateDto Dto, IFormFile? ImageFile)
        : IRequest<int>;
 
}
