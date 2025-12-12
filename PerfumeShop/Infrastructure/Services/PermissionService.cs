using Microsoft.EntityFrameworkCore;
using PerfumeShop.Application.Services;
using PerfumeShop.Infrastructure.Data;

namespace PerfumeShop.Infrastructure.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly ApplicationDbContext _db;

        public PermissionService(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool HasPermission(int userId, string permissionCode)
        {
            var user = _db.Users
                .Include(u => u.UserPermissions)
                .ThenInclude(up => up.Permission)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
                return false;

  
            if (user.Role == "Admin")
                return true;

            return user.UserPermissions.Any(up => up.Permission.Code == permissionCode);
        }
    }
}
