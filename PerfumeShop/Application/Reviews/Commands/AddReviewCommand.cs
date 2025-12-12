using MediatR;
using PerfumeShop.Application.DTOs;

namespace PerfumeShop.Application.Reviews.Commands
{
    public record AddReviewCommand(ReviewDto Dto, int UserId) : IRequest<bool>;
}
