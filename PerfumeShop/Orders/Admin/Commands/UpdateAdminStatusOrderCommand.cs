using MediatR;

namespace PerfumeShop.Orders.Admin.Commands
{
    public record UpdateAdminStatusOrderCommand(int OrderId, string Status)
        :IRequest<bool>;
 
}
