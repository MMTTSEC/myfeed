using System.Collections.Generic;
using System.Threading.Tasks;
using MyFeed.Domain.Entities;
using MyFeed.Domain.Interfaces;
using MyFeed.Infrastructure.Data;

namespace MyFeed.Infrastructure.Repositories;

/// <summary>
/// Placeholder implementation. Replace with EF Core logic.
/// </summary>
public class DirectMessageRepository : IDirectMessageRepository
{
    private readonly AppDbContext _context;

    public DirectMessageRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(DM dm)
    {
    }

    public Task<IEnumerable<DM>> GetConversationAsync(int userAId, int userBId)
    {
    }
}

