namespace PerfumeShop.Domain.Interfaces
{
    public interface IPermissionService
    {
        bool HasPermission(int userId, string permissionCode);
    }
}
