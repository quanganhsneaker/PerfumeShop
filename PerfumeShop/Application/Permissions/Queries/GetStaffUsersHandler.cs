using MediatR;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Domain.Models;
using PerfumeShop.Infrastructure.Data;

namespace PerfumeShop.Application.Permissions.Queries
{
    public class GetStaffUsersHandler : IRequestHandler<GetStaffUsersQuery, List<User>>
    {
        private readonly ApplicationDbContext _db;

        public GetStaffUsersHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<User>> Handle(GetStaffUsersQuery request, CancellationToken ct)
        {
            return await _db.Users
                .Where(u => u.Role == "Staff")
                .ToListAsync(ct);
        }
    }
}
