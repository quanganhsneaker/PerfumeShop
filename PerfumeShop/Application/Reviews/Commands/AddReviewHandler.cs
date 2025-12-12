using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Domain.Models;
using PerfumeShop.Infrastructure.Data;

namespace PerfumeShop.Application.Reviews.Commands
{
    public class AddReviewHandler : IRequestHandler<AddReviewCommand, bool>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public AddReviewHandler(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> Handle(AddReviewCommand request, CancellationToken ct)
        {
            var dto = request.Dto;
            int userId = request.UserId;

            bool bought = await _db.OrderItems
                .AnyAsync(x =>
                    x.ProductId == dto.ProductId &&
                    x.Order.UserId == userId &&
                    x.Order.Status == "Completed"
                );

            if (!bought)
                return false;

            var review = _mapper.Map<Review>(dto);
            review.UserId = userId;
            review.CreatedAt = DateTime.Now;

            await _db.Reviews.AddAsync(review, ct);
            await _db.SaveChangesAsync(ct);

            var avg = await _db.Reviews
                .Where(x => x.ProductId == dto.ProductId)
                .AverageAsync(x => x.Rating, ct);

            var product = await _db.Products.FindAsync(dto.ProductId);
            product.Rating = (float)avg;

            await _db.SaveChangesAsync(ct);

            return true;
        }
    }
}
