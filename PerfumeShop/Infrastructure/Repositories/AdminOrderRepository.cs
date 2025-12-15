using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Domain.Interfaces;
using PerfumeShop.Domain.Models;
using PerfumeShop.Infrastructure.Data;

namespace PerfumeShop.Infrastructure.Repositories
{
    public class AdminOrderRepository : IAdminOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public AdminOrderRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // ================= QUERY (ADMIN) =================

        public async Task<List<AdminOrderListVM>> GetAllAsync()
        {
            return await _db.Orders
                .Include(o => o.User)
                .OrderByDescending(o => o.CreatedAt)
                .ProjectTo<AdminOrderListVM>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<AdminOrderPagedVM> GetPagedAsync(
            int page,
            int pageSize,
            string? searchCode,
            string? status)
        {
            var query = _db.Orders
                .Include(o => o.User)
                .OrderByDescending(o => o.CreatedAt)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchCode))
                query = query.Where(o =>
                    o.OrderCode != null &&
                    o.OrderCode.Contains(searchCode));

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(o => o.Status == status);

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var orders = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<AdminOrderListVM>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new AdminOrderPagedVM
            {
                Orders = orders,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                SearchCode = searchCode,
                StatusFilter = status
            };
        }

        public async Task<AdminOrderDetailVM?> GetDetailAsync(int orderId)
        {
            var order = await _db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return null;

            return _mapper.Map<AdminOrderDetailVM>(order);
        }

        // ================= COMMAND (ADMIN) =================

        public async Task<Order?> GetByIdAsync(int orderId)
        {
            return await _db.Orders.FindAsync(orderId);
        }

        public async Task<bool> UpdateStatusAsync(int orderId, string status)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order == null) return false;

            order.Status = status;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
