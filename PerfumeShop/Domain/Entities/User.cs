using PerfumeShop.Domain.Core;

namespace PerfumeShop.Domain.Models
{
    public class User : BaseEntity
    {
     
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } 
      
        public List<UserPermission> UserPermissions { get; set; }

    }
}
