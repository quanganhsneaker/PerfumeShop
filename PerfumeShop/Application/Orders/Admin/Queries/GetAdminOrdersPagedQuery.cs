using MediatR;
using PerfumeShop.Application.DTOs;

namespace PerfumeShop.Application.Orders.Admin.Queries
{
    public record GetAdminOrdersPagedQuery
    (int Page,
        int PageSize,
        string SearchCode,
        string Status
    ) : IRequest<AdminOrderPagedVM>;
}
       

