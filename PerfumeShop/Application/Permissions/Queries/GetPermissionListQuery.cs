using MediatR;
using PerfumeShop.Domain.Models;

namespace PerfumeShop.Application.Permissions.Queries
{
    public record GetPermissionListQuery() : IRequest<List<Permission>>;
}
