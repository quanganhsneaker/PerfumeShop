using MediatR;
using PerfumeShop.DTOs;

namespace PerfumeShop.Orders.Admin.Queries.GetAdminOrders
{
    public record GetAdminOrderDetailQuery(int OrderId)
        :IRequest<AdminOrderDetailVM>;
    
}
