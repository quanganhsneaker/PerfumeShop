using MediatR;
using AutoMapper;
using PerfumeShop.Domain.Models;
using PerfumeShop.Infrastructure.Data;

namespace PerfumeShop.Application.Products.Commands
{
    public class UpdateProductHandler
        : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public UpdateProductHandler(ApplicationDbContext db, IWebHostEnvironment env, IMapper mapper)
        {
            _db = db;
            _env = env;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken ct)
        {
            var dto = request.Dto;

            var product = await _db.Products.FindAsync(dto.Id);
            if (product == null) return false;


            _mapper.Map(dto, product);


            if (request.ImageFile != null)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(request.ImageFile.FileName);
                string folder = Path.Combine(_env.WebRootPath, "images/products");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                using (var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream);
                }

                product.ImageUrl = "/images/products/" + fileName;
            }

            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
