using MediatR;
using PerfumeShop.DTOs;

namespace PerfumeShop.Orders.User.Commands
{
    public record CreateOrderCommand(CheckoutDto Dto, int UserId)
      : IRequest<int>;
}
