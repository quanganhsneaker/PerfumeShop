using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Application.DTOs;
using PerfumeShop.Infrastructure.Data;

namespace PerfumeShop.Application.Categories.Commands
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, bool>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public UpdateCategoryHandler(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken ct)
        {
            var dto = request.Dto;

            var category = await _db.Categories.FindAsync(dto.Id);
            if (category == null) return false;

            _mapper.Map(dto, category);

            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
