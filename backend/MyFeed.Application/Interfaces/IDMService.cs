using System.Threading.Tasks;

namespace MyFeed.Application.Interfaces
{
    public interface IDMService
    {
        Task SendDMAsync(int senderId, int receiverId, string content);
    }
}