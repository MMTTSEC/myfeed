using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyFeed.Domain.Entities;

namespace MyFeed.Domain.Interfaces
{
    public interface IFollowRepository
    {
        Task AddAsync(Follow follow);
        Task RemoveAsync(int followerId, int followeeId);
        Task<bool> ExistsAsync(int followerId, int followeeId);
        Task<IEnumerable<int>> GetFolloweeIdsAsync(int followerId);
        Task<IEnumerable<int>> GetFollowerIdsAsync(int followeeId);
    }
}
