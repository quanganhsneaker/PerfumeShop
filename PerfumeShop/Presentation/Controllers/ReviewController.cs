using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Application.Reviews.Commands;
using PerfumeShop.Application.Reviews.Queries;

namespace PerfumeShop.Presentation.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IMediator _mediator;

        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddReview(ReviewDto dto)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            var success = await _mediator.Send(new AddReviewCommand(dto, userId));

            if (!success)
                return Unauthorized("Bạn chưa mua sản phẩm này hoặc đơn hàng chưa hoàn thành.");

            return Redirect("/Order/Detail/" + dto.OrderId);
        }

        public async Task<IActionResult> List(int productId)
        {
            var reviews = await _mediator.Send(new GetProductReviewsQuery(productId));
            return View(reviews);
        }
    }
}
