using Moq;
using MyFeed.Application.Services;
using MyFeed.Domain.Entities;
using MyFeed.Domain.Interfaces;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MyFeed.Tests.Application
{
    public class LikeServiceTests
    {
        [Fact]
        public async Task LikePost_WithValidData_CallsRepositoryAdd()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new User("user", "hash"));

            var postRepo = new Mock<IPostRepository>();
            postRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Post(1, "Title", "Body"));

            var likeRepo = new Mock<ILikeRepository>();
            likeRepo.Setup(x => x.ExistsAsync(1, 1)).ReturnsAsync(false);

            var svc = new LikeService(likeRepo.Object, userRepo.Object, postRepo.Object);

            await svc.LikePostAsync(1, 1);

            likeRepo.Verify(x => x.AddAsync(It.IsAny<Like>()), Times.Once);
        }
    }
}

