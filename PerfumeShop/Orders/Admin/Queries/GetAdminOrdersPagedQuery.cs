using MediatR;
using PerfumeShop.DTOs;

namespace PerfumeShop.Orders.Admin.Queries
{
    public record GetAdminOrdersPagedQuery
    (int Page,
        int PageSize,
        string SearchCode,
        string Status
    ) : IRequest<AdminOrderPagedVM>;
}
       

