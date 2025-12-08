using System.Collections.Generic;
using System.Threading.Tasks;
using MyFeed.Domain.Entities;

namespace MyFeed.Application.Interfaces
{
    public interface IDMService
    {
        Task SendDMAsync(int senderId, int receiverId, string content);
        Task<IEnumerable<DM>> GetConversationAsync(int userId, int otherUserId);
    }
}