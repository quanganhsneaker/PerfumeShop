using MediatR;

namespace PerfumeShop.Application.Categories.Commands
{
    public record DeleteCategoryCommand(int Id) : IRequest<bool>;
}
