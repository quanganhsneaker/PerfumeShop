using PerfumeShop.Orders.User.Queries.GetOrderDetail;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeShop.DTOs;
using PerfumeShop.Orders.User.Commands;

[Authorize]
public class OrderController : Controller
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
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

        int newOrderId = await _mediator.Send(new CreateOrderCommand(dto, userId));

        if (newOrderId < 0)
        {
            TempData["error"] = "Giỏ hàng trống!";
            return RedirectToAction("Index", "Cart");
        }

        return RedirectToAction("Detail", new { id = newOrderId });
    }
    public async Task<IActionResult> Detail(int id)
    {
        int userId = int.Parse(User.FindFirst("userId").Value);

        var result = await _mediator.Send(new GetOrderDetailQuery(id, userId));

        if (result == null)
            return NotFound();

        return View(result);
    }
    public async Task<IActionResult> MyOrders()
    {
        int userId = int.Parse(User.FindFirst("userId").Value);

        var list = await _mediator.Send(new GetMyOrdersQuery(userId));

        return View(list);
    }
}
