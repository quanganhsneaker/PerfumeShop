using MediatR;
using PerfumeShop.Domain.Models;

namespace PerfumeShop.Application.Reviews.Queries
{
    public record GetProductReviewsQuery(int ProductId) : IRequest<List<Review>>;
}
