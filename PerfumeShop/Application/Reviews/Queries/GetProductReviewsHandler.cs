using MediatR;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Infrastructure.Data;
using PerfumeShop.Domain.Models;

namespace PerfumeShop.Application.Reviews.Queries
{
    public class GetProductReviewsHandler
        : IRequestHandler<GetProductReviewsQuery, List<Review>>
    {
        private readonly ApplicationDbContext _db;

        public GetProductReviewsHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Review>> Handle(GetProductReviewsQuery request, CancellationToken ct)
        {
            return await _db.Reviews
                .Where(r => r.ProductId == request.ProductId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync(ct);
        }
    }
}
