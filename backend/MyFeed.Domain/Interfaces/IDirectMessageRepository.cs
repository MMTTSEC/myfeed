using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyFeed.Domain.Entities;

namespace MyFeed.Domain.Interfaces
{
    public interface IDirectMessageRepository
    {
        Task AddAsync(DM dm);
        Task<IEnumerable<DM>> GetConversationAsync(int userAId, int userBId);
    }
}
