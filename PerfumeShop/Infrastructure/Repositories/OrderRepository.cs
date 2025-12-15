using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Domain.Interfaces;
using PerfumeShop.Domain.Models;
using PerfumeShop.Infrastructure.Data;

namespace PerfumeShop.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public OrderRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // ========= QUERY =========

        public async Task<List<OrderListVM>> GetMyOrdersAsync(int userId)
        {
            return await _db.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ProjectTo<OrderListVM>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<OrderListPagedVM> GetMyOrdersPagedAsync(
            int userId,
            int page,
            int pageSize,
            string? searchCode,
            string? status)
        {
            var query = _db.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchCode))
                query = query.Where(o => o.OrderCode != null &&
                                         o.OrderCode.Contains(searchCode));

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(o => o.Status == status);

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var orders = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<OrderListVM>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new OrderListPagedVM
            {
                Orders = orders,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                SearchCode = searchCode,
                StatusFilter = status
            };
        }

        public async Task<OrderDetailVM?> GetOrderDetailAsync(int orderId, int userId)
        {
            var order = await _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null) return null;
            return _mapper.Map<OrderDetailVM>(order);
        }

        // ========= COMMAND =========

        public async Task AddAsync(Order order)
        {
            _db.Orders.Add(order);
            await Task.CompletedTask;
        }

        public async Task AddItemsAsync(List<OrderItem> items)
        {
            _db.OrderItems.AddRange(items);
            await Task.CompletedTask;
        }

        public async Task<Order?> GetByIdAsync(int orderId)
        {
            return await _db.Orders.FindAsync(orderId);
        }

        // ========= CART =========

        public async Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return await _db.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task RemoveCartAsync(Cart cart)
        {
            _db.CartItems.RemoveRange(cart.Items);
            _db.Carts.Remove(cart);
            await Task.CompletedTask;
        }
    }
}
