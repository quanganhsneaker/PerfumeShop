using MediatR;
using PerfumeShop.Domain.Interfaces;
namespace PerfumeShop.Application.Orders.Admin.Commands
{
    public class UpdateAdminStatusOrderHandler
        : IRequestHandler<UpdateAdminStatusOrderCommand, bool>
    {
        private readonly IAdminOrderRepository _repo;
        private readonly IUnitOfWork _uow;
        public UpdateAdminStatusOrderHandler(IAdminOrderRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<bool> Handle(
            UpdateAdminStatusOrderCommand request,
            CancellationToken ct)
        {
            var success = await _repo.UpdateStatusAsync(
                request.OrderId,
                request.Status
            );
            if (!success) return false;
            await _uow.SaveChangesAsync(ct);

            return true;
            
        }
    }
}
