using MediatR;
using PerfumeShop.Application.DTOs;

namespace PerfumeShop.Application.Products.Queries
{
    public record GetAdminProductListQuery
    :IRequest<List<ProductListVM>>;
    
}
