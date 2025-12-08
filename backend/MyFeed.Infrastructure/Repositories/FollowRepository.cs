using Microsoft.EntityFrameworkCore;
using MyFeed.Domain.Entities;
using MyFeed.Domain.Interfaces;
using MyFeed.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFeed.Infrastructure.Repositories
{
    public class FollowRepository : IFollowRepository
    {
        private readonly AppDbContext _context;

        public FollowRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Follow follow)
        {
            await _context.Follows.AddAsync(follow);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int followerId, int followeeId)
        {
            var follow = await _context.Follows
                .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);

            if (follow != null)
            {
                _context.Follows.Remove(follow);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int followerId, int followeeId)
        {
            return await _context.Follows
                .AnyAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);
        }

        public async Task<IEnumerable<int>> GetFolloweeIdsAsync(int followerId)
        {
            return await _context.Follows
                .Where(f => f.FollowerId == followerId)
                .Select(f => f.FolloweeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<int>> GetFollowerIdsAsync(int followeeId)
        {
            return await _context.Follows
                .Where(f => f.FolloweeId == followeeId)
                .Select(f => f.FollowerId)
                .ToListAsync();
        }
    }
}