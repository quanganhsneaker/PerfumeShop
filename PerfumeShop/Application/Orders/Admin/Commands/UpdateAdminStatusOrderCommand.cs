using MediatR;

namespace PerfumeShop.Application.Orders.Admin.Commands
{
    public record UpdateAdminStatusOrderCommand(int OrderId, string Status)
        :IRequest<bool>;
 
}
