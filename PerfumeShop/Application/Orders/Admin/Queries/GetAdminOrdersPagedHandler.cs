using MediatR;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Domain.Interfaces;

namespace PerfumeShop.Application.Orders.Admin.Queries
{
    public class GetAdminOrdersPagedHandler
        : IRequestHandler<GetAdminOrdersPagedQuery, AdminOrderPagedVM>
    {
        private readonly IAdminOrderRepository _repo;

        public GetAdminOrdersPagedHandler(IAdminOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task<AdminOrderPagedVM> Handle(
            GetAdminOrdersPagedQuery rq,
            CancellationToken ct)
        {
            return await _repo.GetPagedAsync(
                rq.Page,
                rq.PageSize,
                rq.SearchCode,
                rq.Status
            );
        }
    }
}
