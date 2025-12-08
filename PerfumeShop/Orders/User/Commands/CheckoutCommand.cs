using MediatR;
using PerfumeShop.DTOs;

namespace PerfumeShop.Orders.User.Commands
{
    public record CheckoutCommand(CheckoutDto Dto, int UserId) : IRequest<CheckoutResult>;
}
