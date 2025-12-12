using System.Threading.Tasks;

namespace PerfumeShop.Application.Services
{
    public interface IChatService
    {
        Task<string> ProcessQuestion(string question, int userId);
    }
}
