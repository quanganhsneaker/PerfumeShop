using MediatR;
using PerfumeShop.DTOs;
// hàm yêu cầu 
namespace PerfumeShop.Orders.User.Commands
{
    public record CreateOrderCommand(CheckoutDto Dto, int UserId)
      : IRequest<int>;
}
