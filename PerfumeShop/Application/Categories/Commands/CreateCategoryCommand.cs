using MediatR;
using PerfumeShop.Application.DTOs;

namespace PerfumeShop.Application.Categories.Commands
{
    public record CreateCategoryCommand(CategoryDto Dto) : IRequest<bool>;
}
