using MediatR;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Domain.Interfaces;

namespace PerfumeShop.Application.Orders.Admin.Queries.GetAdminOrders
{
    public class GetAdminOrdersHandler
        : IRequestHandler<GetAdminOrdersQuery, List<AdminOrderListVM>>
    {
        private readonly IAdminOrderRepository _repo;

        public GetAdminOrdersHandler(IAdminOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<AdminOrderListVM>> Handle(
            GetAdminOrdersQuery request,
            CancellationToken ct)
        {
            return await _repo.GetAllAsync();
        }
    }
}
