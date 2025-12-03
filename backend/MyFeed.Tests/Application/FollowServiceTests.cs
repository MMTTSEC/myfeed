using Moq;
using MyFeed.Application.Services;
using MyFeed.Domain.Entities;
using MyFeed.Domain.Interfaces;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MyFeed.Tests.Application
{
    public class FollowServiceTests
    {
        [Fact]
        public async Task FollowUser_WithValidData_CallsRepositoryAdd()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new User("follower", "hash"));
            userRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(new User("followee", "hash"));

            var followRepo = new Mock<IFollowRepository>();
            followRepo.Setup(x => x.ExistsAsync(1, 2)).ReturnsAsync(false);

            var svc = new FollowService(followRepo.Object, userRepo.Object);

            await svc.FollowUserAsync(1, 2);

            followRepo.Verify(x => x.AddAsync(It.IsAny<Follow>()), Times.Once);
        }

        [Fact]
        public async Task FollowUser_FollowerDoesNotExist_ThrowsException()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((User?)null);
            userRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(new User("followee", "hash"));

            var followRepo = new Mock<IFollowRepository>();
            var svc = new FollowService(followRepo.Object, userRepo.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                svc.FollowUserAsync(1, 2)
            );

            followRepo.Verify(x => x.AddAsync(It.IsAny<Follow>()), Times.Never);
        }

        [Fact]
        public async Task FollowUser_FolloweeDoesNotExist_ThrowsException()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new User("follower", "hash"));
            userRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync((User?)null);

            var followRepo = new Mock<IFollowRepository>();
            var svc = new FollowService(followRepo.Object, userRepo.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                svc.FollowUserAsync(1, 2)
            );

            followRepo.Verify(x => x.AddAsync(It.IsAny<Follow>()), Times.Never);
        }

        [Fact]
        public async Task FollowUser_AlreadyFollowing_ThrowsException()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new User("follower", "hash"));
            userRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(new User("followee", "hash"));

            var followRepo = new Mock<IFollowRepository>();
            followRepo.Setup(x => x.ExistsAsync(1, 2)).ReturnsAsync(true);

            var svc = new FollowService(followRepo.Object, userRepo.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                svc.FollowUserAsync(1, 2)
            );

            followRepo.Verify(x => x.AddAsync(It.IsAny<Follow>()), Times.Never);
        }

        [Fact]
        public async Task FollowUser_CannotFollowYourself_ThrowsException()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new User("user", "hash"));

            var followRepo = new Mock<IFollowRepository>();
            followRepo.Setup(x => x.ExistsAsync(1, 1)).ReturnsAsync(false);

            var svc = new FollowService(followRepo.Object, userRepo.Object);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                svc.FollowUserAsync(1, 1)
            );

            followRepo.Verify(x => x.AddAsync(It.IsAny<Follow>()), Times.Never);
        }
    }
}

