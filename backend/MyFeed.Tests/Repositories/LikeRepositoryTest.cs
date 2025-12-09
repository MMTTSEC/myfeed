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
    public class LikeRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly LikeRepository _repository;

        public LikeRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new LikeRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddLikeToDatabase()
        {
            // Arrange
            var like = new Like(1, 100);

            // Act
            await _repository.AddAsync(like);

            // Assert
            var savedLike = await _context.Likes.FirstOrDefaultAsync();
            Assert.NotNull(savedLike);
            Assert.Equal(1, savedLike.UserId);
            Assert.Equal(100, savedLike.PostId);
        }

        [Fact]
        public async Task AddAsync_ShouldSaveChangesToDatabase()
        {
            // Arrange
            var like = new Like(1, 100);

            // Act
            await _repository.AddAsync(like);

            // Assert
            var count = await _context.Likes.CountAsync();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task RemoveAsync_ShouldRemoveLikeFromDatabase()
        {
            // Arrange
            var like = new Like(1, 100);
            await _context.Likes.AddAsync(like);
            await _context.SaveChangesAsync();

            // Act
            await _repository.RemoveAsync(1, 100);

            // Assert
            var count = await _context.Likes.CountAsync();
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task RemoveAsync_ShouldDoNothing_WhenLikeDoesNotExist()
        {
            // Arrange
            var like = new Like(1, 100);
            await _context.Likes.AddAsync(like);
            await _context.SaveChangesAsync();

            // Act
            await _repository.RemoveAsync(2, 200); // Non-existent like

            // Assert
            var count = await _context.Likes.CountAsync();
            Assert.Equal(1, count); // Original like still exists
        }

        [Fact]
        public async Task RemoveAsync_ShouldOnlyRemoveSpecificLike()
        {
            // Arrange
            var like1 = new Like(1, 100);
            var like2 = new Like(1, 200);
            var like3 = new Like(2, 100);
            await _context.Likes.AddRangeAsync(like1, like2, like3);
            await _context.SaveChangesAsync();

            // Act
            await _repository.RemoveAsync(1, 100);

            // Assert
            var remainingLikes = await _context.Likes.ToListAsync();
            Assert.Equal(2, remainingLikes.Count);
            Assert.DoesNotContain(remainingLikes, l => l.UserId == 1 && l.PostId == 100);
            Assert.Contains(remainingLikes, l => l.UserId == 1 && l.PostId == 200);
            Assert.Contains(remainingLikes, l => l.UserId == 2 && l.PostId == 100);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnTrue_WhenLikeExists()
        {
            // Arrange
            var like = new Like(1, 100);
            await _context.Likes.AddAsync(like);
            await _context.SaveChangesAsync();

            // Act
            var exists = await _repository.ExistsAsync(1, 100);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnFalse_WhenLikeDoesNotExist()
        {
            // Act
            var exists = await _repository.ExistsAsync(1, 100);

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnFalse_WhenOnlyUserIdMatches()
        {
            // Arrange
            var like = new Like(1, 100);
            await _context.Likes.AddAsync(like);
            await _context.SaveChangesAsync();

            // Act
            var exists = await _repository.ExistsAsync(1, 200);

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnFalse_WhenOnlyPostIdMatches()
        {
            // Arrange
            var like = new Like(1, 100);
            await _context.Likes.AddAsync(like);
            await _context.SaveChangesAsync();

            // Act
            var exists = await _repository.ExistsAsync(2, 100); // Different userId

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task CountForPostAsync_ShouldReturnZero_WhenPostHasNoLikes()
        {
            // Act
            var count = await _repository.CountForPostAsync(100);

            // Assert
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CountForPostAsync_ShouldReturnCorrectCount_WhenPostHasOneLike()
        {
            // Arrange
            var like = new Like(1, 100);
            await _context.Likes.AddAsync(like);
            await _context.SaveChangesAsync();

            // Act
            var count = await _repository.CountForPostAsync(100);

            // Assert
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CountForPostAsync_ShouldReturnCorrectCount_WhenPostHasMultipleLikes()
        {
            // Arrange
            var like1 = new Like(1, 100);
            var like2 = new Like(2, 100);
            var like3 = new Like(3, 100);
            await _context.Likes.AddRangeAsync(like1, like2, like3);
            await _context.SaveChangesAsync();

            // Act
            var count = await _repository.CountForPostAsync(100);

            // Assert
            Assert.Equal(3, count);
        }

        [Fact]
        public async Task CountForPostAsync_ShouldOnlyCountSpecificPost()
        {
            // Arrange
            var like1 = new Like(1, 100);
            var like2 = new Like(2, 100);
            var like3 = new Like(3, 200);
            var like4 = new Like(4, 200);
            await _context.Likes.AddRangeAsync(like1, like2, like3, like4);
            await _context.SaveChangesAsync();

            // Act
            var count = await _repository.CountForPostAsync(100);

            // Assert
            Assert.Equal(2, count);
        }

        [Fact]
        public async Task CountForPostAsync_ShouldReturnZero_ForNonExistentPost()
        {
            // Arrange
            var like1 = new Like(1, 100);
            await _context.Likes.AddAsync(like1);
            await _context.SaveChangesAsync();

            // Act
            var count = await _repository.CountForPostAsync(999);

            // Assert
            Assert.Equal(0, count);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}