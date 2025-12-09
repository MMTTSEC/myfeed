using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyFeed.Domain.Interfaces;
using MyFeed.Domain.Entities;
using MyFeed.Application.Interfaces;

namespace MyFeed.Application.Services
{
    public class DMService : IDMService
    {
        private readonly IDirectMessageRepository _dmRepo;
        private readonly IUserRepository _userRepo;

        public DMService(IDirectMessageRepository dmRepo, IUserRepository userRepo)
        {
            _dmRepo = dmRepo;
            _userRepo = userRepo;
        }
        public async Task<DM> SendDMAsync(int senderId, int receiverId, string content)
        {
            var sender = await _userRepo.GetByIdAsync(senderId);
            if (sender == null)
                throw new InvalidOperationException("Sender not found.");
            var receiver = await _userRepo.GetByIdAsync(receiverId);
            if (receiver == null)
                throw new InvalidOperationException("Receiver not found.");
            // Domain entity enforces content rules
            var dm = new DM(senderId, receiverId, content);
            await _dmRepo.AddAsync(dm);
            return dm;
        }

        public async Task<IEnumerable<DM>> GetConversationAsync(int userId, int otherUserId)
        {
            return await _dmRepo.GetConversationAsync(userId, otherUserId);
        }
    }
}
