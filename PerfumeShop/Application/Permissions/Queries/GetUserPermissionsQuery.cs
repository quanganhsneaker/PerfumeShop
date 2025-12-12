using MediatR;
using PerfumeShop.Domain.Models;

namespace PerfumeShop.Application.Permissions.Queries
{
    public record GetUserPermissionsQuery(int UserId) : IRequest<User>;
}
