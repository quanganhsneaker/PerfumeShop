using MediatR;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Infrastructure.Data;
using PerfumeShop.Domain.Models;

namespace PerfumeShop.Application.Permissions.Queries
{
    public class GetPermissionListHandler : IRequestHandler<GetPermissionListQuery, List<Permission>>
    {
        private readonly ApplicationDbContext _db;

        public GetPermissionListHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Permission>> Handle(GetPermissionListQuery request, CancellationToken ct)
        {
            return await _db.Permissions.ToListAsync(ct);
        }
    }
}
