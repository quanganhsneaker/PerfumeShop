using MediatR;
using PerfumeShop.DTOs;

namespace PerfumeShop.Products.Queries
{
    public record GetAdminProductEditQuery(int Id)
        :IRequest<ProductUpdateDto>;
   
}
