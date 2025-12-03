using System;
using System.Threading.Tasks;
using MyFeed.Domain.Interfaces;
using MyFeed.Domain.Entities;

namespace MyFeed.Application.Services
{
    public class DMService
    {
        private readonly IDirectMessageRepository _dmRepo;
        private readonly IUserRepository _userRepo;

        public DMService(IDirectMessageRepository dmRepo, IUserRepository userRepo)
        {
            _dmRepo = dmRepo;
            _userRepo = userRepo;
        }
        public async Task SendDMAsync(int senderId, int receiverId, string content)
        {
            var sender = await _userRepo.GetByIdAsync(senderId);
            
            // Domain entity enforces content rules
            var dm = new DM(senderId, receiverId, content);
            await _dmRepo.AddAsync(dm);
        }
    }
}
