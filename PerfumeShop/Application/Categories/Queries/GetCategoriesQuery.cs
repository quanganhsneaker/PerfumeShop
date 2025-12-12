using MediatR;
using PerfumeShop.Domain.Models;

namespace PerfumeShop.Application.Categories.Queries
{
    public record GetCategoriesQuery() : IRequest<List<Category>>;
}
