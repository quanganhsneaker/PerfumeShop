using MediatR;
using PerfumeShop.Application.DTOs;

namespace PerfumeShop.Application.Categories.Commands
{
    public record UpdateCategoryCommand(CategoryUpdateDto Dto) : IRequest<bool>;
}
