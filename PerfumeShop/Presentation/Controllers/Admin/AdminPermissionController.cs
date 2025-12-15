using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Application.Permissions.Commands;
using PerfumeShop.Application.Permissions.Queries;
using PerfumeShop.Domain.Interfaces;

namespace PerfumeShop.Presentation.Controllers.Admin
{
    [Authorize]
    public class AdminPermissionController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IPermissionService _perm;

        public AdminPermissionController(IMediator mediator, IPermissionService perm)
        {
            _mediator = mediator;
            _perm = perm;
        }

        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            if (!_perm.HasPermission(userId, "permission.view"))
                return RedirectToAction("Denied", "Auth");

            var staff = await _mediator.Send(new GetStaffUsersQuery());
            return View(staff);
        }

        public async Task<IActionResult> Edit(int id)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            if (!_perm.HasPermission(userId, "permission.edit"))
                return RedirectToAction("Denied", "Auth");

            var user = await _mediator.Send(new GetUserPermissionsQuery(id));
            var permissions = await _mediator.Send(new GetPermissionListQuery());

            ViewBag.Permissions = permissions;

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, List<int> permissionIds)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            if (!_perm.HasPermission(userId, "permission.edit"))
                return RedirectToAction("Denied", "Auth");

            var dto = new UserPermissionUpdateDto
            {
                UserId = id,
                PermissionIds = permissionIds ?? new List<int>()
            };

            await _mediator.Send(new UpdateUserPermissionsCommand(dto));

            return RedirectToAction("Index", new { edited = 1 });
        }
    }
}
