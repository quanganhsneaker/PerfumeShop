using MediatR;
using PerfumeShop.Domain.Interfaces;
using PerfumeShop.Domain.Models;

namespace PerfumeShop.Application.Permissions.Queries
{
    public class GetStaffUsersHandler
        : IRequestHandler<GetStaffUsersQuery, List<User>>
    {
        private readonly IPermissionRepository _repo;

        public GetStaffUsersHandler(IPermissionRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<User>> Handle(
            GetStaffUsersQuery request,
            CancellationToken ct)
        {
            return await _repo.GetStaffUsersAsync();
        }
    }
}
