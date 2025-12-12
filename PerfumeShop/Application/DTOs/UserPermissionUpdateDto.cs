namespace PerfumeShop.Application.DTOs
{
    public class UserPermissionUpdateDto
    {
        public int UserId { get; set; }
        public List<int> PermissionIds { get; set; } = new();
    }
}
