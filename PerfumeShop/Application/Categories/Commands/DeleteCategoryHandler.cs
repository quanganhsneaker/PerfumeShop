using MediatR;
using PerfumeShop.Infrastructure.Data;

namespace PerfumeShop.Application.Categories.Commands
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly ApplicationDbContext _db;

        public DeleteCategoryHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken ct)
        {
            var category = await _db.Categories.FindAsync(request.Id);
            if (category == null) return false;

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync(ct);

            return true;
        }
    }
}
