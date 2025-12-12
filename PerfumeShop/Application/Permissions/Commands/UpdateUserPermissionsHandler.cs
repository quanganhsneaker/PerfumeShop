using MediatR;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Domain.Models;
using PerfumeShop.Infrastructure.Data;

namespace PerfumeShop.Application.Permissions.Commands
{
    public class UpdateUserPermissionsHandler
        : IRequestHandler<UpdateUserPermissionsCommand, bool>
    {
        private readonly ApplicationDbContext _db;

        public UpdateUserPermissionsHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(UpdateUserPermissionsCommand request, CancellationToken ct)
        {
            var dto = request.Dto;

            var user = await _db.Users
                .Include(u => u.UserPermissions)
                .FirstOrDefaultAsync(u => u.Id == dto.UserId, ct);

            if (user == null) return false;

            var newList = dto.PermissionIds.Distinct().ToList();
            var oldList = user.UserPermissions.Select(up => up.PermissionId).ToList();

            var toAdd = newList.Except(oldList).ToList();
            var toRemove = oldList.Except(newList).ToList();
            foreach (var pid in toRemove)
            {
                var removeItem = user.UserPermissions.First(up => up.PermissionId == pid);
                _db.UserPermissions.Remove(removeItem);
            }
            foreach (var pid in toAdd)
            {
                _db.UserPermissions.Add(new UserPermission
                {
                    UserId = dto.UserId,
                    PermissionId = pid
                });
            }

            await _db.SaveChangesAsync(ct);

            return true;
        }
    }
}
