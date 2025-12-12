using MediatR;
using PerfumeShop.Application.DTOs;

namespace PerfumeShop.Application.Orders.User.Commands
{
    public record CheckoutCommand(CheckoutDto Dto, int UserId) : IRequest<CheckoutResult>;
}
