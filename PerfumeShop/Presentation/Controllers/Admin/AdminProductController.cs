using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Domain.Models;
using PerfumeShop.Application.Products.Commands;
using PerfumeShop.Application.Products.Queries;
using System.Threading.Tasks;
using PerfumeShop.Application.DTOs;

using PerfumeShop.Infrastructure.Data;
using PerfumeShop.Infrastructure.Services;
using PerfumeShop.Domain.Interfaces;

namespace PerfumeShop.Presentation.Controllers.Admin
{
    [Authorize] 
    public class ProductAdminController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _db;
        private readonly IPermissionService _perm;
        private readonly IMediator _mediator;

        public ProductAdminController(ApplicationDbContext db, IWebHostEnvironment env, IPermissionService perm, IMediator mediator)

        {
            _db = db;
            _env = env;
            _perm = perm;
            _mediator = mediator;
        }

       
        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

         
            if (!_perm.HasPermission(userId, "product.view"))
                return RedirectToAction("Denied", "Auth");

            var vm = await _mediator.Send(new GetAdminProductListQuery());
            return View(vm);
        }

        
        public IActionResult Create()
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            if (!_perm.HasPermission(userId, "product.create"))
                return RedirectToAction("Denied", "Auth");

            ViewBag.Categories = _db.Categories.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDto dto, IFormFile imageFile)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            if (!_perm.HasPermission(userId, "product.create"))
                return RedirectToAction("Denied", "Auth");

            //if (ImageFile != null)
            //{
            //    string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
            //    string folder = Path.Combine(_env.WebRootPath, "images/products");

            //    if (!Directory.Exists(folder))
            //        Directory.CreateDirectory(folder);

            //    using (var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create))
            //    {
            //        ImageFile.CopyTo(stream); // de luu vào wwwroot
            //    }

            //    product.ImageUrl = "/images/products/" + fileName;
            //}

            //_db.Products.Add(product);
            //_db.SaveChanges();
            int id = await _mediator.Send(new CreateProductCommand(dto, imageFile));
            return RedirectToAction("Index", new {success = 1});
        }

       
        public async Task<IActionResult> Edit(int id)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            if (!_perm.HasPermission(userId, "product.edit"))
                return RedirectToAction("Denied", "Auth");

            var dto = await _mediator.Send(new GetAdminProductEditQuery(id));

            if (dto == null) return NotFound();

            ViewBag.Categories = _db.Categories.ToList();
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductUpdateDto dto, IFormFile imageFile)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            if (!_perm.HasPermission(userId, "product.edit"))
                return RedirectToAction("Denied", "Auth");
            bool result = await _mediator.Send(new UpdateProductCommand(dto, imageFile));
            if (!result) return NotFound();
            
            //var p = _db.Products.Find(product.Id);
            //if (p == null) return NotFound();

            //p.Name = product.Name;
            //p.Description = product.Description;
            //p.Price = product.Price;
            //p.CategoryId = product.CategoryId;

            
            //if (ImageFile != null)
            //{
            //    string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
            //    string folder = Path.Combine(_env.WebRootPath, "images/products");

            //    using (var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create))
            //    {
            //        ImageFile.CopyTo(stream);
            //    }

            //    p.ImageUrl = "/images/products/" + fileName;
            //}

            //_db.SaveChanges();
            return RedirectToAction("Index", new {edited = 1});
        }

     
        public IActionResult Delete(int id)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            if (!_perm.HasPermission(userId, "product.delete"))
                return RedirectToAction("Denied", "Auth");

            var p = _db.Products.Find(id);
            if (p == null) return NotFound();

            _db.Products.Remove(p);
            _db.SaveChanges();

            return RedirectToAction("Index", new {deleted = 1});
        }
    }
}
