using MediatR;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Domain.Models;
using PerfumeShop.Infrastructure.Data;

namespace PerfumeShop.Application.Permissions.Queries
{
    public class GetUserPermissionsHandler : IRequestHandler<GetUserPermissionsQuery, User>
    {
        private readonly ApplicationDbContext _db;

        public GetUserPermissionsHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<User> Handle(GetUserPermissionsQuery request, CancellationToken ct)
        {
            return await _db.Users
                .Include(u => u.UserPermissions)
                .FirstOrDefaultAsync(u => u.Id == request.UserId, ct);
        }
    }
}
