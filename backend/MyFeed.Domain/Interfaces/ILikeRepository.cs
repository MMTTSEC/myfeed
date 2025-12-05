using System;
using System.Threading.Tasks;
using MyFeed.Domain.Entities;

namespace MyFeed.Domain.Interfaces
{
    public interface ILikeRepository
    {
        Task AddAsync(Like like);
        Task RemoveAsync(int userId, int postId);
        Task<bool> ExistsAsync(int userId, int postId);
        Task<int> CountForPostAsync(int postId);
    }
}
