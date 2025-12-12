using MediatR;
using PerfumeShop.Infrastructure.Data;

namespace PerfumeShop.Application.Orders.Admin.Commands
{
    public class UpdateAdminStatusOrderHandler
        :IRequestHandler<UpdateAdminStatusOrderCommand, bool>

    {
        private readonly ApplicationDbContext _db;
        public UpdateAdminStatusOrderHandler(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Handle(UpdateAdminStatusOrderCommand request, CancellationToken ct)
        {
            var order = await _db.Orders.FindAsync(request.OrderId);
            if (order == null) return false;
            order.Status = request.Status;
            await _db.SaveChangesAsync();
            return true;
                
            
        }

    }
}
