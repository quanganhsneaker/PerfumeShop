using System.Threading.Tasks;

namespace PerfumeShop.Domain.Interfaces
{
    public interface IChatService
    {
        Task<string> ProcessQuestion(string question, int userId);
    }
}
