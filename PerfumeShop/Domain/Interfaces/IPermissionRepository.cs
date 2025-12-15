using PerfumeShop.Domain.Models;

namespace PerfumeShop.Domain.Interfaces
{
    public interface IPermissionRepository
    {
        Task<User?> GetUserWithPermissionsAsync(int userId);

        Task<List<User>> GetStaffUsersAsync();

        Task<List<Permission>> GetAllPermissionsAsync();

        Task<bool> UpdateUserPermissionsAsync(
            int userId,
            List<int> permissionIds
        );
    }
}
