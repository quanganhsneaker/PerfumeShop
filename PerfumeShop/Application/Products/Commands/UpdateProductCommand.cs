using MediatR;
using PerfumeShop.Application.DTOs;

namespace PerfumeShop.Application.Products.Commands
{
    public record UpdateProductCommand(ProductUpdateDto Dto, IFormFile? ImageFile)
   :IRequest<bool>;
}
