using MediatR;
using PerfumeShop.Application.DTOs;

namespace PerfumeShop.Application.Orders.User.Queries.GetOrderDetail
{
   
        public record GetOrderDetailQuery(int OrderId, int UserId)
            : IRequest<OrderDetailVM>;
    
}
