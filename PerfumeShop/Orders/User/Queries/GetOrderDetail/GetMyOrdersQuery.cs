using MediatR;
using PerfumeShop.DTOs;
// hàm yêu cầu 
namespace PerfumeShop.Orders.User.Queries.GetOrderDetail
{
    public record GetMyOrdersQuery(int UserId)
        : IRequest<List<OrderListVM>>;
    

}
