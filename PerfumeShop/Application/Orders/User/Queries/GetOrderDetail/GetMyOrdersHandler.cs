using MediatR;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Domain.Interfaces;

namespace PerfumeShop.Application.Orders.User.Queries.GetOrderDetail
{
    public class GetMyOrdersHandler
        : IRequestHandler<GetMyOrdersQuery, List<OrderListVM>>
    {
        private readonly IOrderRepository _repo;

        public GetMyOrdersHandler(IOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<OrderListVM>> Handle(
      GetMyOrdersQuery request,
      CancellationToken ct)
        {
            return await _repo.GetMyOrdersAsync(request.UserId);
        }

    }
}
