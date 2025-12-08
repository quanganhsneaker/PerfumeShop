using MediatR;
using PerfumeShop.DTOs;

namespace PerfumeShop.Orders.User.Queries
{
    public record GetMyOrdersPagedQuery(
        int UserId,
        int Page,
        int PageSize,
        string SearchCode,
        string Status
    ) : IRequest<OrderListPagedVM>;
}
