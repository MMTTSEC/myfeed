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

        public async Task FollowUserAsync(int followerId, int followeeId)
        {
            var follower = await _userRepo.GetByIdAsync(followerId);
            if (follower == null)
                throw new InvalidOperationException("Follower not found.");

            var followee = await _userRepo.GetByIdAsync(followeeId);
            if (followee == null)
                throw new InvalidOperationException("Followee not found.");

            var alreadyFollowing = await _followRepo.ExistsAsync(followerId, followeeId);
            if (alreadyFollowing)
                throw new InvalidOperationException("Already following this user.");

            // Domain entity enforces can't follow yourself
            var follow = new Follow(followerId, followeeId);
            await _followRepo.AddAsync(follow);
        }
    }
}

