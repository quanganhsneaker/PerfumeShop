using MediatR;
using PerfumeShop.Application.DTOs;

namespace PerfumeShop.Application.Products.Queries
{
    public record GetAdminProductEditQuery(int Id)
        :IRequest<ProductUpdateDto>;
   
}
