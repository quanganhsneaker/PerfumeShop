using MediatR;
using PerfumeShop.DTOs;


namespace PerfumeShop.Orders.User.Queries.GetOrderDetail
{
   
        public record GetOrderDetailQuery(int OrderId, int UserId)
            : IRequest<OrderDetailVM>;
    
}
