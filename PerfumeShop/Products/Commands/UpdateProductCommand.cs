using MediatR;
using PerfumeShop.DTOs;

namespace PerfumeShop.Products.Commands
{
    public record UpdateProductCommand(ProductUpdateDto Dto, IFormFile? ImageFile)
   :IRequest<bool>;
}
