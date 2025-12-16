using AutoMapper;
using MediatR;
using PerfumeShop.Domain.Interfaces;
using PerfumeShop.Domain.Models;

namespace PerfumeShop.Application.Reviews.Commands
{
    public class AddReviewHandler
        : IRequestHandler<AddReviewCommand, bool>
    {
        private readonly IReviewRepository _repo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public AddReviewHandler(IReviewRepository repo, IMapper mapper, IUnitOfWork uow)
        {
            _repo = repo;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<bool> Handle(
            AddReviewCommand request,
            CancellationToken ct)
        {
            var dto = request.Dto;
            int userId = request.UserId;
            bool bought = await _repo.HasUserBoughtProductAsync(
                userId,
                dto.ProductId
            );
            if (!bought)
                return false;
            var review = _mapper.Map<Review>(dto);
            review.UserId = userId;
            review.CreatedAt = DateTime.Now;
            await _repo.AddAsync(review);
            await _repo.UpdateProductRatingAsync(dto.ProductId);
            await _uow.SaveChangesAsync(ct);

            return true;
        }
    }
}
