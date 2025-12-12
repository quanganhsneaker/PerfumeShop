using MediatR;
using PerfumeShop.Application.DTOs;

namespace PerfumeShop.Application.Orders.User.Commands
{
    public record CreateOrderCommand(CheckoutDto Dto, int UserId)
      : IRequest<int>;
}
