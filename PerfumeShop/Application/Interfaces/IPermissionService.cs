namespace PerfumeShop.Application.Services
{
    public interface IPermissionService
    {
        bool HasPermission(int userId, string permissionCode);
    }
}
