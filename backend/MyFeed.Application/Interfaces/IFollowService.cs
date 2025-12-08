using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFeed.Application.Interfaces
{
    public interface IFollowService
    {
        Task FollowUserAsync(int followerId, int followeeId);
        Task UnfollowUserAsync(int followerId, int followeeId);
        Task<IEnumerable<int>> GetFollowingAsync(int userId);
        Task<bool> IsFollowingAsync(int followerId, int followeeId);
    }
}