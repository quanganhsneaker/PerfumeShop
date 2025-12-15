using MediatR;
using PerfumeShop.Domain.Interfaces;
using PerfumeShop.Domain.Models;

namespace PerfumeShop.Application.Reviews.Queries
{
    public class GetProductReviewsHandler
        : IRequestHandler<GetProductReviewsQuery, List<Review>>
    {
        private readonly IReviewRepository _repo;

        public GetProductReviewsHandler(IReviewRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Review>> Handle(
            GetProductReviewsQuery request,
            CancellationToken ct)
        {
            return await _repo.GetByProductAsync(request.ProductId);
        }
    }
}
