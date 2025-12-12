using Microsoft.AspNetCore.Mvc;
using PerfumeShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

using PerfumeShop.Infrastructure.Data;
using PerfumeShop.Infrastructure.Services;
using PerfumeShop.Application.Services;

namespace PerfumeShop.Presentation.Controllers.Admin
{
    public class AdminPermissionController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IPermissionService _perm;
        public AdminPermissionController(ApplicationDbContext db , IPermissionService perm)
        {
            _db = db;
            _perm = perm;
        }

        public IActionResult Index()
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            if (!_perm.HasPermission(userId, "permission.view"))
                return RedirectToAction("Denied", "Auth");

            var staff = _db.Users.Where(u => u.Role == "Staff").ToList();
            return View(staff);
        }

        public IActionResult Edit(int id)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            if (!_perm.HasPermission(userId, "permission.edit"))
                return RedirectToAction("Denied", "Auth");
            var user = _db.Users
                .Include(u => u.UserPermissions)
                .FirstOrDefault(u => u.Id == id);

            ViewBag.Permissions = _db.Permissions.ToList();

            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(int id, List<int> permissionIds)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            if (!_perm.HasPermission(userId, "permission.edit"))
                return RedirectToAction("Denied", "Auth");
            permissionIds = (permissionIds ?? new List<int>()).Distinct().ToList();
            var user = _db.Users
                .Include(u => u.UserPermissions)
                .First(n => n.Id == id);
            if (user == null) return NotFound();
            var current = user.UserPermissions.Select(up => up.PermissionId).ToList();

            var toAdd = permissionIds.Except(current).ToList();
            var toRemove = current.Except(permissionIds).ToList();

            if (toRemove.Any())
            {
           
                var removals = user.UserPermissions
                    .Where(up => toRemove.Contains(up.PermissionId))
                    .ToList();

               
                foreach (var up in removals) // duyet tung dong
                {
                    _db.UserPermissions.Remove(up);
                }
            }

         // them moi và check xem co trung khong
            foreach (var pid in toAdd)
            {
                bool exists = _db.UserPermissions.Any(up => up.UserId == id && up.PermissionId == pid);
                if (!exists)
                {
                    _db.UserPermissions.Add(new UserPermission
                    {
                        UserId = id,
                        PermissionId = pid
                    });
                }
            }
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }

}
