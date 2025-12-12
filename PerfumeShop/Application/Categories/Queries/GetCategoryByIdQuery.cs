using MediatR;
using PerfumeShop.Domain.Models;

namespace PerfumeShop.Application.Categories.Queries
{
    public record GetCategoryByIdQuery(int Id) : IRequest<Category>;
}
