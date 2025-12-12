using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeShop.Application.Categories.Commands;
using PerfumeShop.Application.Categories.Queries;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Application.Services;

namespace PerfumeShop.Presentation.Controllers.Admin
{
    [Authorize]
    public class CategoryAdminController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IPermissionService _perm;

        public CategoryAdminController(IMediator mediator, IPermissionService perm)
        {
            _mediator = mediator;
            _perm = perm;
        }

        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            if (!_perm.HasPermission(userId, "category.manage"))
                return RedirectToAction("Denied", "Auth");

            var list = await _mediator.Send(new GetCategoriesQuery());
            return View(list);
        }

        public async Task<IActionResult> Edit(int id)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            if (!_perm.HasPermission(userId, "category.edit"))
                return RedirectToAction("Denied", "Auth");

            var category = await _mediator.Send(new GetCategoryByIdQuery(id));
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryUpdateDto dto)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            if (!_perm.HasPermission(userId, "category.edit"))
                return RedirectToAction("Denied", "Auth");

            await _mediator.Send(new UpdateCategoryCommand(dto));
            return RedirectToAction("Index", new { edited = 1 });
        }

        public IActionResult Create()
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            if (!_perm.HasPermission(userId, "category.create"))
                return RedirectToAction("Denied", "Auth");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto dto)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            if (!_perm.HasPermission(userId, "category.create"))
                return RedirectToAction("Denied", "Auth");

            await _mediator.Send(new CreateCategoryCommand(dto));
            return RedirectToAction("Index", new { success = 1 });
        }

        public async Task<IActionResult> Delete(int id)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            if (!_perm.HasPermission(userId, "category.delete"))
                return RedirectToAction("Denied", "Auth");

            await _mediator.Send(new DeleteCategoryCommand(id));
            return RedirectToAction("Index", new { deleted = 1 });
        }
    }
}
