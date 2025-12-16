using MediatR;
using PerfumeShop.Domain.Interfaces;

namespace PerfumeShop.Application.Permissions.Commands
{
    public class UpdateUserPermissionsHandler
        : IRequestHandler<UpdateUserPermissionsCommand, bool>
    {
        private readonly IPermissionRepository _repo;
        private readonly IUnitOfWork _uow;

        public UpdateUserPermissionsHandler(IPermissionRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<bool> Handle(
            UpdateUserPermissionsCommand request,
            CancellationToken ct)
        {
            var success = await _repo.UpdateUserPermissionsAsync(
                request.Dto.UserId,
                request.Dto.PermissionIds
            );

            if (!success) return false;
            await _uow.SaveChangesAsync(ct);

            return true;
        }
    }
}
