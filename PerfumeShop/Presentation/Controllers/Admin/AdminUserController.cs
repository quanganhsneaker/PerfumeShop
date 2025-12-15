using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeShop.Domain.Interfaces;
using PerfumeShop.Infrastructure.Data;
using PerfumeShop.Infrastructure.Services;

namespace PerfumeShop.Presentation.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IPermissionService _perm;
        public AdminUserController(ApplicationDbContext db, IPermissionService perm)
        {

            _db = db;
            _perm = perm;
        }

   
        public IActionResult Index()
        {
            var users = _db.Users.ToList();
            return View(users);
        }
        // edit quyền get 
        public IActionResult Edit(int id)
        {
            var user = _db.Users.FirstOrDefault(x => x.Id == id);
            if (user == null) return NotFound();
         
            return View(user);


        }
        // edit quyen post 
        [HttpPost]
        public IActionResult Edit(int id, string role)
        {
            var user = _db.Users.FirstOrDefault(x => x.Id == id);
            if (user == null) return NotFound();
            user.Role = role;
            _db.SaveChanges();
            return RedirectToAction("Index", new {success = 1});
        }
        public IActionResult Delete(int id)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            var u = _db.Users.Find(id);
            if (u == null) return NotFound();

            _db.Users.Remove(u);
            _db.SaveChanges();

            return RedirectToAction("Index", new { deleted = 1 });
        }
    }
}
