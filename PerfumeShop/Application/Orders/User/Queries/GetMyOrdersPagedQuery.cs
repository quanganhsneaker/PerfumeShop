using MediatR;
using PerfumeShop.Application.DTOs;

namespace PerfumeShop.Application.Orders.User.Queries
{
    public record GetMyOrdersPagedQuery(
        int UserId,
        int Page,
        int PageSize,
        string SearchCode,
        string Status
    ) : IRequest<OrderListPagedVM>;
}
