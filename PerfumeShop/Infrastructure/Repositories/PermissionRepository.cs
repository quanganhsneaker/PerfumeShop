using Microsoft.EntityFrameworkCore;
using PerfumeShop.Domain.Interfaces;
using PerfumeShop.Domain.Models;
using PerfumeShop.Infrastructure.Data;

namespace PerfumeShop.Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _db;

        public PermissionRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<User?> GetUserWithPermissionsAsync(int userId)
        {
            return await _db.Users
                .Include(u => u.UserPermissions)
                .ThenInclude(up => up.Permission)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<User>> GetStaffUsersAsync()
        {
            return await _db.Users
                .Where(u => u.Role == "Staff")
                .ToListAsync();
        }

        public async Task<List<Permission>> GetAllPermissionsAsync()
        {
            return await _db.Permissions.ToListAsync();
        }

        public async Task<bool> UpdateUserPermissionsAsync(
            int userId,
            List<int> permissionIds)
        {
            var user = await _db.Users
                .Include(u => u.UserPermissions)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return false;

            var newList = permissionIds.Distinct().ToList();
            var oldList = user.UserPermissions
                .Select(x => x.PermissionId)
                .ToList();

            var toAdd = newList.Except(oldList).ToList();
            var toRemove = oldList.Except(newList).ToList();

            foreach (var pid in toRemove)
            {
                var item = user.UserPermissions
                    .First(x => x.PermissionId == pid);
                _db.UserPermissions.Remove(item);
            }

            foreach (var pid in toAdd)
            {
                _db.UserPermissions.Add(new UserPermission
                {
                    UserId = userId,
                    PermissionId = pid
                });
            }

          
            return true;
        }
    }
}
