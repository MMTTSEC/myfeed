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
    public class DirectMessageRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly DirectMessageRepository _repository;

        public DirectMessageRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new DirectMessageRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddDirectMessageToDatabase()
        {
            // Arrange
            var dm = new DM(1, 2, "Hello there!");

            // Act
            await _repository.AddAsync(dm);

            // Assert
            var savedDM = await _context.DirectMessages.FirstOrDefaultAsync();
            Assert.NotNull(savedDM);
            Assert.Equal(1, savedDM.SenderUserId);
            Assert.Equal(2, savedDM.ReceiverUserId);
            Assert.Equal("Hello there!", savedDM.Message);
        }

        [Fact]
        public async Task AddAsync_ShouldSaveChangesToDatabase()
        {
            // Arrange
            var dm = new DM(1, 2, "Test message");

            // Act
            await _repository.AddAsync(dm);

            // Assert
            var count = await _context.DirectMessages.CountAsync();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task GetConversationAsync_ShouldReturnEmptyList_WhenNoMessagesExist()
        {
            // Act
            var result = await _repository.GetConversationAsync(1, 2);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetConversationAsync_ShouldReturnMessagesFromUserAToUserB()
        {
            // Arrange
            var dm1 = new DM(1, 2, "Message from 1 to 2");
            await _context.DirectMessages.AddAsync(dm1);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetConversationAsync(1, 2);

            // Assert
            Assert.Single(result);
            Assert.Contains(result, dm => dm.SenderUserId == 1 && dm.ReceiverUserId == 2);
        }

        [Fact]
        public async Task GetConversationAsync_ShouldReturnMessagesFromUserBToUserA()
        {
            // Arrange
            var dm1 = new DM(2, 1, "Message from 2 to 1");
            await _context.DirectMessages.AddAsync(dm1);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetConversationAsync(1, 2);

            // Assert
            Assert.Single(result);
            Assert.Contains(result, dm => dm.SenderUserId == 2 && dm.ReceiverUserId == 1);
        }

        [Fact]
        public async Task GetConversationAsync_ShouldReturnMessagesBothDirections()
        {
            // Arrange
            var dm1 = new DM(1, 2, "Hello from 1");
            var dm2 = new DM(2, 1, "Reply from 2");
            var dm3 = new DM(1, 2, "Another from 1");

            await _context.DirectMessages.AddRangeAsync(dm1, dm2, dm3);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetConversationAsync(1, 2);

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetConversationAsync_ShouldNotReturnMessagesFromOtherConversations()
        {
            // Arrange
            var dm1 = new DM(1, 2, "Between 1 and 2");
            var dm2 = new DM(3, 4, "Between 3 and 4");
            var dm3 = new DM(1, 3, "Between 1 and 3");

            await _context.DirectMessages.AddRangeAsync(dm1, dm2, dm3);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetConversationAsync(1, 2);

            // Assert
            Assert.Single(result);
            Assert.Contains(result, dm =>
                (dm.SenderUserId == 1 && dm.ReceiverUserId == 2) ||
                (dm.SenderUserId == 2 && dm.ReceiverUserId == 1));
        }

        [Fact]
        public async Task GetConversationAsync_ShouldReturnMessagesOrderedByCreatedAt()
        {
            // Arrange
            // Add messages with delays to ensure different timestamps
            var dm1 = new DM(1, 2, "First message");
            await _context.DirectMessages.AddAsync(dm1);
            await _context.SaveChangesAsync();
            await Task.Delay(10); // Small delay to ensure different timestamps

            var dm2 = new DM(2, 1, "Second message");
            await _context.DirectMessages.AddAsync(dm2);
            await _context.SaveChangesAsync();
            await Task.Delay(10);

            var dm3 = new DM(1, 2, "Third message");
            await _context.DirectMessages.AddAsync(dm3);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetConversationAsync(1, 2);

            // Assert
            var messages = result.ToList();
            Assert.Equal(3, messages.Count);

            // Verify chronological order
            for (int i = 0; i < messages.Count - 1; i++)
            {
                Assert.True(messages[i].CreatedAt <= messages[i + 1].CreatedAt);
            }

            Assert.Equal("First message", messages[0].Message);
            Assert.Equal("Second message", messages[1].Message);
            Assert.Equal("Third message", messages[2].Message);
        }

        [Fact]
        public async Task GetConversationAsync_ShouldWorkRegardlessOfParameterOrder()
        {
            // Arrange
            var dm1 = new DM(1, 2, "Message 1");
            var dm2 = new DM(2, 1, "Message 2");

            await _context.DirectMessages.AddRangeAsync(dm1, dm2);
            await _context.SaveChangesAsync();

            // Act
            var result1 = await _repository.GetConversationAsync(1, 2);
            var result2 = await _repository.GetConversationAsync(2, 1);

            // Assert
            Assert.Equal(result1.Count(), result2.Count());
            Assert.Equal(2, result1.Count());
            Assert.Equal(2, result2.Count());
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}