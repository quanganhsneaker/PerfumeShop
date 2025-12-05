using MediatR;
using PerfumeShop.DTOs;

namespace PerfumeShop.Orders.Admin.Queries.GetAdminOrders
{
    public record GetAdminOrdersQuery()
        : IRequest<List<AdminOrderListVM>>;
}
