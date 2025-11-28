using Microsoft.AspNetCore.Mvc;
using PerfumeShop.Data;
using PerfumeShop.Models;
using Microsoft.EntityFrameworkCore;

namespace PerfumeShop.Controllers.Admin
{
    public class AdminPermissionController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminPermissionController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var staff = _db.Users.Where(u => u.Role == "Staff").ToList();
            return View(staff);
        }

        public IActionResult Edit(int id)
        {
            var user = _db.Users
                .Include(u => u.UserPermissions)
                .FirstOrDefault(u => u.Id == id);

            ViewBag.Permissions = _db.Permissions.ToList();

            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(int id, List<int> permissionIds)
        {
            var user = _db.Users
                .Include(u => u.UserPermissions)
                .First(n => n.Id == id);

            // Xóa quyền cũ
            _db.UserPermissions.RemoveRange(user.UserPermissions);

            // Thêm quyền mới
            foreach (var pid in permissionIds)
            {
                _db.UserPermissions.Add(new UserPermission
                {
                    UserId = id,
                    PermissionId = pid
                });
            }

            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }

}
