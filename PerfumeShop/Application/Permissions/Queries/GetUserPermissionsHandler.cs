using MediatR;
using PerfumeShop.Domain.Interfaces;
using PerfumeShop.Domain.Models;

namespace PerfumeShop.Application.Permissions.Queries
{
    public class GetUserPermissionsHandler
        : IRequestHandler<GetUserPermissionsQuery, User?>
    {
        private readonly IPermissionRepository _repo;

        public GetUserPermissionsHandler(IPermissionRepository repo)
        {
            _repo = repo;
        }

        public async Task<User?> Handle(
            GetUserPermissionsQuery request,
            CancellationToken ct)
        {
            return await _repo.GetUserWithPermissionsAsync(request.UserId);
        }
    }
}
