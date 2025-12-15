using MediatR;
using PerfumeShop.Domain.Interfaces;
namespace PerfumeShop.Application.Orders.Admin.Commands
{
    public class UpdateAdminStatusOrderHandler
        : IRequestHandler<UpdateAdminStatusOrderCommand, bool>
    {
        private readonly IAdminOrderRepository _repo;

        public UpdateAdminStatusOrderHandler(IAdminOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(
            UpdateAdminStatusOrderCommand request,
            CancellationToken ct)
        {
            return await _repo.UpdateStatusAsync(
                request.OrderId,
                request.Status
            );
        }
    }
}
