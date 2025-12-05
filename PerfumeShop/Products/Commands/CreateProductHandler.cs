using MediatR;
using AutoMapper;
using PerfumeShop.Data;
using PerfumeShop.Models;

namespace PerfumeShop.Products.Commands.CreateProduct
{
    public class CreateProductHandler
        : IRequestHandler<CreateProductCommand, int>
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public CreateProductHandler(ApplicationDbContext db, IWebHostEnvironment env, IMapper mapper)
        {
            _db = db;
            _env = env;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken ct)
        {
    
            var product = _mapper.Map<Product>(request.Dto);

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

            _db.Products.Add(product);
            await _db.SaveChangesAsync(ct);

            return product.Id;
        }
    }
}
