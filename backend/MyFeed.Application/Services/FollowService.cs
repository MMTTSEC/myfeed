using System;
using System.Threading.Tasks;
using MyFeed.Domain.Interfaces;
using MyFeed.Domain.Entities;
using MyFeed.Application.Interfaces;

namespace MyFeed.Application.Services
{
    public class FollowService : IFollowService
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

        public async Task UnfollowUserAsync(int followerId, int followeeId)
        {
            var follower = await _userRepo.GetByIdAsync(followerId);
            if (follower == null)
                throw new InvalidOperationException("Follower not found.");

            var followee = await _userRepo.GetByIdAsync(followeeId);
            if (followee == null)
                throw new InvalidOperationException("Followee not found.");

            var exists = await _followRepo.ExistsAsync(followerId, followeeId);
            if (!exists)
                throw new InvalidOperationException("Not following this user.");

            await _followRepo.RemoveAsync(followerId, followeeId);
        }

        public async Task<IEnumerable<int>> GetFollowingAsync(int userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            return await _followRepo.GetFolloweeIdsAsync(userId);
        }

        public async Task<bool> IsFollowingAsync(int followerId, int followeeId)
        {
            return await _followRepo.ExistsAsync(followerId, followeeId);
        }
    }
}

