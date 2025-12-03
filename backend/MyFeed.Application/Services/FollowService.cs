using System;
using System.Threading.Tasks;
using MyFeed.Domain.Interfaces;
using MyFeed.Domain.Entities;

namespace MyFeed.Application.Services
{
    public class FollowService
    {
        private readonly IFollowRepository _followRepo;
        private readonly IUserRepository _userRepo;

        public FollowService(IFollowRepository followRepo, IUserRepository userRepo)
        {
            _followRepo = followRepo;
            _userRepo = userRepo;
        }
    }
}

