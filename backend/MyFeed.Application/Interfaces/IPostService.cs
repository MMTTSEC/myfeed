using MyFeed.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFeed.Application.Interfaces
{
    public interface IPostService
    {
        Task CreatePostAsync(int authorId, string title, string body);
        Task<Post?> GetPostByIdAsync(int id);
        Task<IEnumerable<Post>> GetPostsByUserAsync(int userId);
        Task<IEnumerable<Post>> GetFeedAsync(int userId);
    }
}

