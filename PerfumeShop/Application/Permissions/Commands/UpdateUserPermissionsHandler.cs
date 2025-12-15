using MediatR;
using PerfumeShop.Domain.Interfaces;

namespace PerfumeShop.Application.Permissions.Commands
{
    public class UpdateUserPermissionsHandler
        : IRequestHandler<UpdateUserPermissionsCommand, bool>
    {
        private readonly IPermissionRepository _repo;

        public UpdateUserPermissionsHandler(IPermissionRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(
            UpdateUserPermissionsCommand request,
            CancellationToken ct)
        {
            return await _repo.UpdateUserPermissionsAsync(
                request.Dto.UserId,
                request.Dto.PermissionIds
            );
        }
    }
}
