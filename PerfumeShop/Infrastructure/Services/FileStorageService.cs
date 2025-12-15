using PerfumeShop.Domain.Interfaces;

namespace PerfumeShop.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;

        public FileStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string?> SaveProductImageAsync(IFormFile file)
        {
            if (file == null) return null;

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string folder = Path.Combine(_env.WebRootPath, "images/products");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var path = Path.Combine(folder, fileName);
            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return "/images/products/" + fileName;
        }
    }
}
