using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeShop.Data;

namespace PerfumeShop.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminUserController(ApplicationDbContext db)
        {
            _db = db;
        }

        // LIST USERS
        public IActionResult Index()
        {
            var users = _db.Users.ToList();
            return View(users);
        }
    }
}
