using System.Threading.Tasks;
using MyFeed.Domain.Entities;
using MyFeed.Domain.Interfaces;
using MyFeed.Infrastructure.Data;

namespace MyFeed.Infrastructure.Repositories;

/// <summary>
/// Placeholder implementation. Replace with EF Core logic.
/// </summary>
public class LikeRepository : ILikeRepository
{
    private readonly AppDbContext _context;

    public LikeRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(Like like)
    {
    }

    public Task<int> CountForPostAsync(int postId)
    {
    }

    public Task<bool> ExistsAsync(int userId, int postId)
    {
    }

    public Task RemoveAsync(int userId, int postId)
    {
    }
}

