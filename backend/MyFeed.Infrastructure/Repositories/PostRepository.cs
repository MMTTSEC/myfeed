using System.Collections.Generic;
using System.Threading.Tasks;
using MyFeed.Domain.Entities;
using MyFeed.Domain.Interfaces;
using MyFeed.Infrastructure.Data;

namespace MyFeed.Infrastructure.Repositories;

/// <summary>
/// Placeholder implementation. Replace with EF Core logic.
/// </summary>
public class PostRepository : IPostRepository
{
    private readonly AppDbContext _context;

    public PostRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(Post post)
    {
    }

    public Task<Post?> GetByIdAsync(int id)
    {
    }

    public Task<IEnumerable<Post>> GetFeedAsync(int userId)
    {
    }

    public Task<IEnumerable<Post>> GetPostsByUserAsync(int userId)
    {
    }
}

