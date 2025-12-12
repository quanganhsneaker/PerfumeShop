using MediatR;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Infrastructure.Data;
using PerfumeShop.Domain.Models;

namespace PerfumeShop.Application.Categories.Queries
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, List<Category>>
    {
        private readonly ApplicationDbContext _db;

        public GetCategoriesHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Category>> Handle(GetCategoriesQuery request, CancellationToken ct)
        {
            return await _db.Categories.ToListAsync(ct);
        }
    }
}
