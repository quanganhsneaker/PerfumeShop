using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeShop.Application.Services;
using PerfumeShop.Domain.Models;
using PerfumeShop.Infrastructure.Data;
using PerfumeShop.Infrastructure.Services;

namespace PerfumeShop.Presentation.Controllers.Admin
{
    [Authorize]
    public class CategoryAdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IPermissionService _perm;
        public CategoryAdminController(ApplicationDbContext db, IPermissionService perm)
        {
            _db = db;
            _perm = perm;
        }

        public IActionResult Index()
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            if (!_perm.HasPermission(userId, "category.manage"))
                return RedirectToAction("Denied", "Auth");
            return View(_db.Categories.ToList());
        }

        public IActionResult Create()
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            if (!_perm.HasPermission(userId, "category.create"))
                return RedirectToAction("Denied", "Auth");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            if (!_perm.HasPermission(userId, "category.create"))
                return RedirectToAction("Denied", "Auth");
            _db.Categories.Add(category);
            _db.SaveChanges();
            return RedirectToAction("Index", new { success = 1 });
        }

        public IActionResult Edit(int id)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            if (!_perm.HasPermission(userId, "category.edit"))
                return RedirectToAction("Denied", "Auth");
            var category = _db.Categories.Find(id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            if (!_perm.HasPermission(userId, "category.edit"))
                return RedirectToAction("Denied", "Auth");
            _db.Categories.Update(category);
            _db.SaveChanges();
            return RedirectToAction("Index", new { edited = 1 });
        }

        public IActionResult Delete(int id)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            if (!_perm.HasPermission(userId, "category.delete"))
                return RedirectToAction("Denied", "Auth");
            var category = _db.Categories.Find(id);
            _db.Categories.Remove(category);
            _db.SaveChanges();
            return RedirectToAction("Index", new { deleted = 1 });
        }
    }
}

