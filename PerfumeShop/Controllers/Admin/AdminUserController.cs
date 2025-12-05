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
    }
}
