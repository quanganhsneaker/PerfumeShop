namespace PerfumeShop.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }     // Ví dụ: Thêm sản phẩm
        public string Code { get; set; }     // Ví dụ: product.create

        public List<UserPermission> UserPermissions { get; set; }
    }

}
