using Microsoft.EntityFrameworkCore;
using MyFeed.Domain.Entities;
using MyFeed.Infrastructure.Data;
using MyFeed.Infrastructure.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MyFeed.Tests.Infrastructure.Repositories
{
    public class FollowRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly FollowRepository _repository;

        public FollowRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new FollowRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddFollowToDatabase()
        {
            // Arrange
            var follow = new Follow(1, 2);

            // Act
            await _repository.AddAsync(follow);

            // Assert
            var savedFollow = await _context.Follows.FirstOrDefaultAsync();
            Assert.NotNull(savedFollow);
            Assert.Equal(1, savedFollow.FollowerId);
            Assert.Equal(2, savedFollow.FolloweeId);
        }

        [Fact]
        public async Task AddAsync_ShouldSaveChangesToDatabase()
        {
            // Arrange
            var follow = new Follow(1, 2);

            // Act
            await _repository.AddAsync(follow);

            // Assert
            var count = await _context.Follows.CountAsync();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task RemoveAsync_ShouldRemoveFollowFromDatabase()
        {
            // Arrange
            var follow = new Follow(1, 2);
            await _context.Follows.AddAsync(follow);
            await _context.SaveChangesAsync();

            // Act
            await _repository.RemoveAsync(1, 2);

            // Assert
            var count = await _context.Follows.CountAsync();
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task RemoveAsync_ShouldDoNothing_WhenFollowDoesNotExist()
        {
            // Arrange
            var follow = new Follow(1, 2);
            await _context.Follows.AddAsync(follow);
            await _context.SaveChangesAsync();

            // Act
            await _repository.RemoveAsync(3, 4);

            // Assert
            var count = await _context.Follows.CountAsync();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task RemoveAsync_ShouldOnlyRemoveSpecificFollow()
        {
            // Arrange
            var follow1 = new Follow(1, 2);
            var follow2 = new Follow(1, 3);
            var follow3 = new Follow(2, 3);
            await _context.Follows.AddRangeAsync(follow1, follow2, follow3);
            await _context.SaveChangesAsync();

            // Act
            await _repository.RemoveAsync(1, 2);

            // Assert
            var remainingFollows = await _context.Follows.ToListAsync();
            Assert.Equal(2, remainingFollows.Count);
            Assert.DoesNotContain(remainingFollows, f => f.FollowerId == 1 && f.FolloweeId == 2);
            Assert.Contains(remainingFollows, f => f.FollowerId == 1 && f.FolloweeId == 3);
            Assert.Contains(remainingFollows, f => f.FollowerId == 2 && f.FolloweeId == 3);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnTrue_WhenFollowExists()
        {
            // Arrange
            var follow = new Follow(1, 2);
            await _context.Follows.AddAsync(follow);
            await _context.SaveChangesAsync();

            // Act
            var exists = await _repository.ExistsAsync(1, 2);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnFalse_WhenFollowDoesNotExist()
        {
            // Act
            var exists = await _repository.ExistsAsync(1, 2);

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnFalse_WhenReverseFollowExists()
        {
            // Arrange
            var follow = new Follow(1, 2);
            await _context.Follows.AddAsync(follow);
            await _context.SaveChangesAsync();

            // Act
            var exists = await _repository.ExistsAsync(2, 1); // Reverse direction

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task GetFolloweeIdsAsync_ShouldReturnEmptyList_WhenUserFollowsNoOne()
        {
            // Act
            var followeeIds = await _repository.GetFolloweeIdsAsync(1);

            // Assert
            Assert.NotNull(followeeIds);
            Assert.Empty(followeeIds);
        }

        [Fact]
        public async Task GetFolloweeIdsAsync_ShouldReturnFolloweeIds()
        {
            // Arrange
            var follow1 = new Follow(1, 2);
            var follow2 = new Follow(1, 3);
            var follow3 = new Follow(1, 4);
            await _context.Follows.AddRangeAsync(follow1, follow2, follow3);
            await _context.SaveChangesAsync();

            // Act
            var followeeIds = await _repository.GetFolloweeIdsAsync(1);

            // Assert
            var idList = followeeIds.ToList();
            Assert.Equal(3, idList.Count);
            Assert.Contains(2, idList);
            Assert.Contains(3, idList);
            Assert.Contains(4, idList);
        }

        [Fact]
        public async Task GetFolloweeIdsAsync_ShouldOnlyReturnSpecificUsersFollowees()
        {
            // Arrange
            var follow1 = new Follow(1, 2);
            var follow2 = new Follow(1, 3);
            var follow3 = new Follow(5, 4); // Different follower
            await _context.Follows.AddRangeAsync(follow1, follow2, follow3);
            await _context.SaveChangesAsync();

            // Act
            var followeeIds = await _repository.GetFolloweeIdsAsync(1);

            // Assert
            var idList = followeeIds.ToList();
            Assert.Equal(2, idList.Count);
            Assert.DoesNotContain(4, idList);
        }

        [Fact]
        public async Task GetFollowerIdsAsync_ShouldReturnEmptyList_WhenUserHasNoFollowers()
        {
            // Act
            var followerIds = await _repository.GetFollowerIdsAsync(1);

            // Assert
            Assert.NotNull(followerIds);
            Assert.Empty(followerIds);
        }

        [Fact]
        public async Task GetFollowerIdsAsync_ShouldReturnFollowerIds()
        {
            // Arrange
            var follow1 = new Follow(2, 1);
            var follow2 = new Follow(3, 1);
            var follow3 = new Follow(4, 1);
            await _context.Follows.AddRangeAsync(follow1, follow2, follow3);
            await _context.SaveChangesAsync();

            // Act
            var followerIds = await _repository.GetFollowerIdsAsync(1);

            // Assert
            var idList = followerIds.ToList();
            Assert.Equal(3, idList.Count);
            Assert.Contains(2, idList);
            Assert.Contains(3, idList);
            Assert.Contains(4, idList);
        }

        [Fact]
        public async Task GetFollowerIdsAsync_ShouldOnlyReturnSpecificUsersFollowers()
        {
            // Arrange
            var follow1 = new Follow(2, 1);
            var follow2 = new Follow(3, 1);
            var follow3 = new Follow(4, 5); // Different followee
            await _context.Follows.AddRangeAsync(follow1, follow2, follow3);
            await _context.SaveChangesAsync();

            // Act
            var followerIds = await _repository.GetFollowerIdsAsync(1);

            // Assert
            var idList = followerIds.ToList();
            Assert.Equal(2, idList.Count);
            Assert.DoesNotContain(4, idList);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}