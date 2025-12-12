using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PerfumeShop.Infrastructure.Data;
using PerfumeShop.Application.Orders.Admin.Commands;
using PerfumeShop.Application.Orders.Admin.Queries;
using PerfumeShop.Application.Orders.Admin.Queries.GetAdminOrders;
using System.Threading.Tasks;
using PerfumeShop.Infrastructure.Services;
using PerfumeShop.Application.Services;

namespace PerfumeShop.Presentation.Controllers.Admin
{
    [Authorize]
    public class AdminOrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IPermissionService _perm;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public AdminOrderController(ApplicationDbContext db, IPermissionService perm, IMapper mapper, IMediator mediator)
        {
            _db = db;
            _perm = perm;
            _mapper = mapper;
            _mediator = mediator;
        }


        public async Task<IActionResult> Index(
    int page = 1,
    int pageSize = 10,
    string searchCode = "",
    string status = "")
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            if (!_perm.HasPermission(userId, "order.manage"))
                return RedirectToAction("Denied", "Auth");

            var vm = await _mediator.Send(new GetAdminOrdersPagedQuery(
                page,
                pageSize,
                searchCode,
                status
            ));

            return View(vm);
        }



        public async Task<IActionResult> Detail(int id)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            if (!_perm.HasPermission(userId, "order.detail"))
                return RedirectToAction("Denied", "Auth");
            var vm = await _mediator.Send(new GetAdminOrderDetailQuery(id));


            if (vm == null) return NotFound();

            return View(vm);
        }

     
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);
            if (!_perm.HasPermission(userId, "order.updatestatus"))
                return RedirectToAction("Denied", "Auth");
            bool ok = await _mediator.Send(new UpdateAdminStatusOrderCommand(id, status));
            if (!ok) return NotFound();
            return RedirectToAction("Detail", new { id });
        }
    }
}
