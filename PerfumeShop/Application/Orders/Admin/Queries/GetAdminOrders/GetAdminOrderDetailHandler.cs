using MediatR;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Domain.Interfaces;

namespace PerfumeShop.Application.Orders.Admin.Queries.GetAdminOrders
{
    public class GetAdminOrderDetailHandler
        : IRequestHandler<GetAdminOrderDetailQuery, AdminOrderDetailVM?>
    {
        private readonly IAdminOrderRepository _repo;

        public GetAdminOrderDetailHandler(IAdminOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task<AdminOrderDetailVM?> Handle(
            GetAdminOrderDetailQuery request,
            CancellationToken ct)
        {
            return await _repo.GetDetailAsync(request.OrderId);
        }
    }
}
