using MediatR;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Application.Orders.User.Queries;
using PerfumeShop.Domain.Interfaces;

public class GetMyOrdersPagedHandler
    : IRequestHandler<GetMyOrdersPagedQuery, OrderListPagedVM>
{
    private readonly IOrderRepository _repo;

    public GetMyOrdersPagedHandler(IOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task<OrderListPagedVM> Handle(
        GetMyOrdersPagedQuery rq,
        CancellationToken ct)
    {
        return await _repo.GetMyOrdersPagedAsync(
            rq.UserId,
            rq.Page,
            rq.PageSize,
            rq.SearchCode,
            rq.Status
        );
    }
}
