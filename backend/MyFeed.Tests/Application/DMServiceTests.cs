using Moq;
using MyFeed.Application.Services;
using MyFeed.Domain.Entities;
using MyFeed.Domain.Interfaces;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MyFeed.Tests.Application
{
    public class DMServiceTests
    {
        [Fact]
        public async Task SendDM_WithValidData_CallsRepositoryAdd()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new User("sender", "hash"));
            userRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(new User("receiver", "hash"));

            var dmRepo = new Mock<IDirectMessageRepository>();
            var svc = new DMService(dmRepo.Object, userRepo.Object);

            await svc.SendDMAsync(1, 2, "Hello!");

            dmRepo.Verify(x => x.AddAsync(It.IsAny<DM>()), Times.Once);
        }

        [Fact]
        public async Task SendDM_SenderDoesNotExist_ThrowsException()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((User?)null);
            userRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(new User("receiver", "hash"));

            var dmRepo = new Mock<IDirectMessageRepository>();
            var svc = new DMService(dmRepo.Object, userRepo.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                svc.SendDMAsync(1, 2, "Hello!")
            );

            dmRepo.Verify(x => x.AddAsync(It.IsAny<DM>()), Times.Never);
        }

        [Fact]
        public async Task SendDM_ReceiverDoesNotExist_ThrowsException()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new User("sender", "hash"));
            userRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync((User?)null);

            var dmRepo = new Mock<IDirectMessageRepository>();
            var svc = new DMService(dmRepo.Object, userRepo.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                svc.SendDMAsync(1, 2, "Hello!")
            );

            dmRepo.Verify(x => x.AddAsync(It.IsAny<DM>()), Times.Never);
        }

        [Fact]
        public async Task SendDM_SameSenderAndReceiver_ThrowsException()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new User("user", "hash"));

            var dmRepo = new Mock<IDirectMessageRepository>();
            var svc = new DMService(dmRepo.Object, userRepo.Object);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                svc.SendDMAsync(1, 1, "Hello!")
            );

            dmRepo.Verify(x => x.AddAsync(It.IsAny<DM>()), Times.Never);
        }

        [Fact]
        public async Task SendDM_EmptyMessage_ThrowsException()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new User("sender", "hash"));
            userRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(new User("receiver", "hash"));

            var dmRepo = new Mock<IDirectMessageRepository>();
            var svc = new DMService(dmRepo.Object, userRepo.Object);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                svc.SendDMAsync(1, 2, "")
            );

            dmRepo.Verify(x => x.AddAsync(It.IsAny<DM>()), Times.Never);
        }

        [Fact]
        public async Task SendDM_MessageTooLong_ThrowsException()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new User("sender", "hash"));
            userRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(new User("receiver", "hash"));

            var dmRepo = new Mock<IDirectMessageRepository>();
            var svc = new DMService(dmRepo.Object, userRepo.Object);

            string longMessage = new string('a', 1001); // 1001 characters

            await Assert.ThrowsAsync<ArgumentException>(() =>
                svc.SendDMAsync(1, 2, longMessage)
            );

            dmRepo.Verify(x => x.AddAsync(It.IsAny<DM>()), Times.Never);
        }
    }
}

