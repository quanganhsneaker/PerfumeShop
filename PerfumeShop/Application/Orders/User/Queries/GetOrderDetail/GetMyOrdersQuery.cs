using MediatR;
using PerfumeShop.Application.DTOs;

// hàm yêu cầu 
namespace PerfumeShop.Application.Orders.User.Queries.GetOrderDetail
{
    public record GetMyOrdersQuery(int UserId)
        : IRequest<List<OrderListVM>>;
    

}
