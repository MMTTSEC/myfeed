using Microsoft.EntityFrameworkCore;
using MyFeed.Domain.Entities;
using MyFeed.Infrastructure.Data;
using MyFeed.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MyFeed.Tests.Infrastructure.Repositories
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new UserRepository(_context);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = new User("testuser", "hash123");
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            var userId = user.Id;

            // Act
            var result = await _repository.GetByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("testuser", result.Username);
            Assert.Equal("hash123", result.PasswordHash);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByUsernameAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = new User("testuser", "hash123");
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByUsernameAsync("testuser");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("testuser", result.Username);
            Assert.Equal("hash123", result.PasswordHash);
        }

        [Fact]
        public async Task GetByUsernameAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Act
            var result = await _repository.GetByUsernameAsync("nonexistent");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByUsernameAsync_ShouldBeCaseSensitive()
        {
            // Arrange
            var user = new User("TestUser", "hash123");
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByUsernameAsync("testuser");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByUsernameAsync_ShouldReturnCorrectUser_WhenMultipleUsersExist()
        {
            // Arrange
            var user1 = new User("user1", "hash1");
            var user2 = new User("user2", "hash2");
            var user3 = new User("user3", "hash3");
            await _context.Users.AddRangeAsync(user1, user2, user3);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByUsernameAsync("user2");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("user2", result.Username);
            Assert.Equal("hash2", result.PasswordHash);
        }

        [Fact]
        public async Task AddAsync_ShouldAddUserToDatabase()
        {
            // Arrange
            var user = new User("newuser", "hash123");

            // Act
            await _repository.AddAsync(user);

            // Assert
            var savedUser = await _context.Users.FirstOrDefaultAsync();
            Assert.NotNull(savedUser);
            Assert.Equal("newuser", savedUser.Username);
            Assert.Equal("hash123", savedUser.PasswordHash);
        }

        [Fact]
        public async Task AddAsync_ShouldSaveChangesToDatabase()
        {
            // Arrange
            var user = new User("newuser", "hash123");

            // Act
            await _repository.AddAsync(user);

            // Assert
            var count = await _context.Users.CountAsync();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task AddAsync_ShouldAssignId()
        {
            // Arrange
            var user = new User("newuser", "hash123");

            // Act
            await _repository.AddAsync(user);

            // Assert
            Assert.NotEqual(0, user.Id);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnTrue_WhenUserExists()
        {
            // Arrange
            var user = new User("testuser", "hash123");
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            var userId = user.Id;

            // Act
            var exists = await _repository.ExistsAsync(userId);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Act
            var exists = await _repository.ExistsAsync(999);

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnFalse_ForDeletedUser()
        {
            // Arrange
            var user = new User("testuser", "hash123");
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            var userId = user.Id;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            // Act
            var exists = await _repository.ExistsAsync(userId);

            // Assert
            Assert.False(exists);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}