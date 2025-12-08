using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeShop.Data;
using PerfumeShop.DTOs;
using PerfumeShop.Orders.User.Commands;
using PerfumeShop.Orders.User.Queries;
using PerfumeShop.Orders.User.Queries.GetOrderDetail;
using PerfumeShop.Services;

[Authorize]
public class OrderController : Controller
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _db;

    public OrderController(IMediator mediator, ApplicationDbContext db)
    {
        _db = db;
        _mediator = mediator;
    }
    [HttpGet]
    public IActionResult Checkout()
    {
        return View(new CheckoutDto());  
    }
    [HttpPost]
    public async Task<IActionResult> Checkout(CheckoutDto dto)
    {
        int userId = int.Parse(User.FindFirst("userId").Value);

        var result = await _mediator.Send(new CheckoutCommand(dto, userId));

        if (result.OrderId < 0)
        {
            TempData["toast"] = "Giỏ hàng trống!";
            TempData["toastType"] = "error";
            return RedirectToAction("Index", "Cart");
        }
        if (result.IsOnline)
        {
            return View("PayWithQR", result);
        }
        return RedirectToAction("Detail", new { id = result.OrderId });
    }
    [HttpGet]
    public async Task<IActionResult> CheckPaid(int orderId)
    {
        var order = await _db.Orders.FindAsync(orderId);
        return Json(new { paid = order?.PaymentStatus == "Paid" });
    }



    public async Task<IActionResult> Detail(int id)
    {
        int userId = int.Parse(User.FindFirst("userId").Value);

        var result = await _mediator.Send(new GetOrderDetailQuery(id, userId));

        if (result == null)
            return NotFound();

        return View(result);
    }
    public async Task<IActionResult> MyOrders(
        int page = 1,
        int pageSize = 5,
        string searchCode = "",
        string status = "")
    {
        int userId = int.Parse(User.FindFirst("userId").Value);

        var vm = await _mediator.Send(new GetMyOrdersPagedQuery(
            userId,
            page,
            pageSize,
            searchCode,
            status
        ));

        return View(vm);
    }
}
