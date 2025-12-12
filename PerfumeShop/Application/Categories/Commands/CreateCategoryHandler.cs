using AutoMapper;
using MediatR;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Domain.Models;
using PerfumeShop.Infrastructure.Data;

namespace PerfumeShop.Application.Categories.Commands
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, bool>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public CreateCategoryHandler(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateCategoryCommand request, CancellationToken ct)
        {
            var category = _mapper.Map<Category>(request.Dto);
            _db.Categories.Add(category);
            await _db.SaveChangesAsync(ct);

            return true;
        }
    }
}
