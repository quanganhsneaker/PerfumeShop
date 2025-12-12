using MediatR;
using PerfumeShop.Application.DTOs;

namespace PerfumeShop.Application.Permissions.Commands
{
    public record UpdateUserPermissionsCommand(UserPermissionUpdateDto Dto) : IRequest<bool>;
}
