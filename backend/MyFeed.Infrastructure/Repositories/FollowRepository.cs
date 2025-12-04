using System.Collections.Generic;
using System.Threading.Tasks;
using MyFeed.Domain.Entities;
using MyFeed.Domain.Interfaces;
using MyFeed.Infrastructure.Data;

namespace MyFeed.Infrastructure.Repositories;

/// <summary>
/// Placeholder implementation. Replace with EF Core logic.
/// </summary>
public class FollowRepository : IFollowRepository
{
    private readonly AppDbContext _context;

    public FollowRepository(AppDbContext context)
    {
    }

    public Task AddAsync(Follow follow)
    {
    }

    public Task<bool> ExistsAsync(int followerId, int followeeId)
    {
    }

    public Task<IEnumerable<int>> GetFolloweeIdsAsync(int followerId)
    {
    }

    public Task RemoveAsync(int followerId, int followeeId)
    {
    }
}

