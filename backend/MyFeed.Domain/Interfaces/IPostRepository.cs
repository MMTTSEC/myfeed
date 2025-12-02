using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyFeed.Domain.Entities;

namespace MyFeed.Domain.Interfaces
{
    public interface IPostRepository
    {
        Task<Post?> GetByIdAsync(int id);
        Task<IEnumerable<Post>> GetPostsByUserAsync(int userId);   
        Task<IEnumerable<Post>> GetFeedAsync(int userId);          
        Task AddAsync(Post post);
    }
}
