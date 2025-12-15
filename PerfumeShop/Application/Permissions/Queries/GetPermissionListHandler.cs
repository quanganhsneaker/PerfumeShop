using MediatR;
using PerfumeShop.Domain.Interfaces;
using PerfumeShop.Domain.Models;

namespace PerfumeShop.Application.Permissions.Queries
{
    public class GetPermissionListHandler
        : IRequestHandler<GetPermissionListQuery, List<Permission>>
    {
        private readonly IPermissionRepository _repo;

        public GetPermissionListHandler(IPermissionRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Permission>> Handle(
            GetPermissionListQuery request,
            CancellationToken ct)
        {
            return await _repo.GetAllPermissionsAsync();
        }
    }
}
