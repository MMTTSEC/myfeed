using Microsoft.EntityFrameworkCore;
using MyFeed.Domain.Entities;
using MyFeed.Domain.Interfaces;
using MyFeed.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFeed.Infrastructure.Repositories
{
    public class DirectMessageRepository : IDirectMessageRepository
    {
        private readonly AppDbContext _context;

        public DirectMessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DM dm)
        {
            await _context.DirectMessages.AddAsync(dm);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DM>> GetConversationAsync(int userAId, int userBId)
        {
            return await _context.DirectMessages
                .Where(dm =>
                    (dm.SenderUserId == userAId && dm.ReceiverUserId == userBId) ||
                    (dm.SenderUserId == userBId && dm.ReceiverUserId == userAId))
                .OrderBy(dm => dm.CreatedAt)
                .ToListAsync();
        }
    }
}