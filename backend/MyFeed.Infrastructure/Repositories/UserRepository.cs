using System.Threading.Tasks;
using MyFeed.Domain.Entities;
using MyFeed.Domain.Interfaces;
using MyFeed.Infrastructure.Data;

namespace MyFeed.Infrastructure.Repositories;

/// <summary>
/// Placeholder implementation. Replace with EF Core logic.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(User user)
    {
    }

    public Task<bool> ExistsAsync(int id)
    {
    }

    public Task<User?> GetByIdAsync(int id)
    {
    }

    public Task<User?> GetByUsernameAsync(string username)
    {
    }
}

