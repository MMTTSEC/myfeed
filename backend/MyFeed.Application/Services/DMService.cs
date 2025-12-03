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
    }
}
