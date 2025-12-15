namespace PerfumeShop.Domain.Interfaces
{
    public interface IFileStorageService
    {
        Task<string?> SaveProductImageAsync(IFormFile file);
    }
}
