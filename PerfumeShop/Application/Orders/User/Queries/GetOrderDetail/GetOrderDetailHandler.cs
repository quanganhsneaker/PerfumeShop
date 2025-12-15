using MediatR;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Domain.Interfaces;

namespace PerfumeShop.Application.Orders.User.Queries.GetOrderDetail
{
    public class GetOrderDetailHandler
        : IRequestHandler<GetOrderDetailQuery, OrderDetailVM?>
    {
        private readonly IOrderRepository _repo;

        public GetOrderDetailHandler(IOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task<OrderDetailVM?> Handle(
     GetOrderDetailQuery request,
     CancellationToken ct)
        {
            return await _repo.GetOrderDetailAsync(
                request.OrderId,
                request.UserId
            );
        }

    }
}
