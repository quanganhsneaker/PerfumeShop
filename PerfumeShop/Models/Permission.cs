using PerfumeShop.Core;

namespace PerfumeShop.Models
{
    public class Permission : BaseEntity
    {
     
        public string Name { get; set; }    
        public string Code { get; set; }    

        public List<UserPermission> UserPermissions { get; set; }
    }

}
