using MediatR;
using PerfumeShop.DTOs;


namespace PerfumeShop.Products.Queries
{
    public record GetAdminProductListQuery
    :IRequest<List<ProductListVM>>;
    
}
