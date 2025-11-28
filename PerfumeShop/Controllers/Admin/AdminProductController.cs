using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Data;
using PerfumeShop.Helpers;
using PerfumeShop.Models;

namespace PerfumeShop.Controllers.Admin
{
    [Authorize]  // Chỉ cần đăng nhập, không khóa Role nữa
    public class ProductAdminController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _db;
        private readonly PermissionService _perm;

        public ProductAdminController(ApplicationDbContext db, IWebHostEnvironment env, PermissionService perm)
        {
            _db = db;
            _env = env;
            _perm = perm;
        }

        // ============================
        // VIEW LIST
        // ============================
        public IActionResult Index()
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            // Ai có quyền product.view mới được xem
            if (!_perm.HasPermission(userId, "product.view"))
                return RedirectToAction("Denied", "Auth");

            var list = _db.Products.Include(c => c.Category).ToList();
            return View(list);
        }

        // ============================
        // CREATE PRODUCT
        // ============================
        public IActionResult Create()
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            if (!_perm.HasPermission(userId, "product.create"))
                return RedirectToAction("Denied", "Auth");

            ViewBag.Categories = _db.Categories.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product, IFormFile ImageFile)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            if (!_perm.HasPermission(userId, "product.create"))
                return RedirectToAction("Denied", "Auth");

            if (ImageFile != null)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                string folder = Path.Combine(_env.WebRootPath, "images/products");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                using (var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                product.ImageUrl = "/images/products/" + fileName;
            }

            _db.Products.Add(product);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // ============================
        // EDIT PRODUCT
        // ============================
        public IActionResult Edit(int id)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            if (!_perm.HasPermission(userId, "product.edit"))
                return RedirectToAction("Denied", "Auth");

            var p = _db.Products.Find(id);
            if (p == null) return NotFound();

            ViewBag.Categories = _db.Categories.ToList();
            return View(p);
        }

        [HttpPost]
        public IActionResult Edit(Product product, IFormFile ImageFile)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            if (!_perm.HasPermission(userId, "product.edit"))
                return RedirectToAction("Denied", "Auth");

            var p = _db.Products.Find(product.Id);
            if (p == null) return NotFound();

            p.Name = product.Name;
            p.Description = product.Description;
            p.Price = product.Price;
            p.CategoryId = product.CategoryId;

            // Update image
            if (ImageFile != null)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                string folder = Path.Combine(_env.WebRootPath, "images/products");

                using (var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                p.ImageUrl = "/images/products/" + fileName;
            }

            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // ============================
        // DELETE PRODUCT
        // ============================
        public IActionResult Delete(int id)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            if (!_perm.HasPermission(userId, "product.delete"))
                return RedirectToAction("Denied", "Auth");

            var p = _db.Products.Find(id);
            if (p == null) return NotFound();

            _db.Products.Remove(p);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
