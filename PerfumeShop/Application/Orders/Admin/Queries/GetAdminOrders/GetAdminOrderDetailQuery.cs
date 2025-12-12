using MediatR;
using PerfumeShop.Application.DTOs;

namespace PerfumeShop.Application.Orders.Admin.Queries.GetAdminOrders
{
    public record GetAdminOrderDetailQuery(int OrderId)
        :IRequest<AdminOrderDetailVM>;
    
}
