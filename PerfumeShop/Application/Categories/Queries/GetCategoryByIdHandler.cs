using MediatR;
using PerfumeShop.Infrastructure.Data;
using PerfumeShop.Domain.Models;

namespace PerfumeShop.Application.Categories.Queries
{
    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Category>
    {
        private readonly ApplicationDbContext _db;

        public GetCategoryByIdHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Category> Handle(GetCategoryByIdQuery request, CancellationToken ct)
        {
            return await _db.Categories.FindAsync(request.Id);
        }
    }
}
