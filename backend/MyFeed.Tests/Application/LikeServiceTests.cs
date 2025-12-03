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

        [Fact]
        public async Task LikePost_UserDoesNotExist_ThrowsException()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((User?)null);

            var postRepo = new Mock<IPostRepository>();
            var likeRepo = new Mock<ILikeRepository>();
            var svc = new LikeService(likeRepo.Object, userRepo.Object, postRepo.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                svc.LikePostAsync(1, 1)
            );

            likeRepo.Verify(x => x.AddAsync(It.IsAny<Like>()), Times.Never);
        }

        [Fact]
        public async Task LikePost_PostDoesNotExist_ThrowsException()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new User("user", "hash"));

            var postRepo = new Mock<IPostRepository>();
            postRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((Post?)null);

            var likeRepo = new Mock<ILikeRepository>();
            var svc = new LikeService(likeRepo.Object, userRepo.Object, postRepo.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                svc.LikePostAsync(1, 1)
            );

            likeRepo.Verify(x => x.AddAsync(It.IsAny<Like>()), Times.Never);
        }

        [Fact]
        public async Task LikePost_AlreadyLiked_ThrowsException()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new User("user", "hash"));

            var postRepo = new Mock<IPostRepository>();
            postRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Post(1, "Title", "Body"));

            var likeRepo = new Mock<ILikeRepository>();
            likeRepo.Setup(x => x.ExistsAsync(1, 1)).ReturnsAsync(true);

            var svc = new LikeService(likeRepo.Object, userRepo.Object, postRepo.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                svc.LikePostAsync(1, 1)
            );

            likeRepo.Verify(x => x.AddAsync(It.IsAny<Like>()), Times.Never);
        }

        [Fact]
        public async Task UnlikePost_WithValidData_CallsRepositoryRemove()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new User("user", "hash"));

            var postRepo = new Mock<IPostRepository>();
            postRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Post(1, "Title", "Body"));

            var likeRepo = new Mock<ILikeRepository>();
            likeRepo.Setup(x => x.ExistsAsync(1, 1)).ReturnsAsync(true);

            var svc = new LikeService(likeRepo.Object, userRepo.Object, postRepo.Object);

            await svc.UnlikePostAsync(1, 1);

            likeRepo.Verify(x => x.RemoveAsync(1, 1), Times.Once);
        }

        [Fact]
        public async Task UnlikePost_LikeDoesNotExist_ThrowsException()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new User("user", "hash"));

            var postRepo = new Mock<IPostRepository>();
            postRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Post(1, "Title", "Body"));

            var likeRepo = new Mock<ILikeRepository>();
            likeRepo.Setup(x => x.ExistsAsync(1, 1)).ReturnsAsync(false);

            var svc = new LikeService(likeRepo.Object, userRepo.Object, postRepo.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                svc.UnlikePostAsync(1, 1)
            );

            likeRepo.Verify(x => x.RemoveAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetLikeCount_WithValidPost_ReturnsCount()
        {
            var postRepo = new Mock<IPostRepository>();
            postRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Post(1, "Title", "Body"));

            var likeRepo = new Mock<ILikeRepository>();
            likeRepo.Setup(x => x.CountForPostAsync(1)).ReturnsAsync(5);

            var userRepo = new Mock<IUserRepository>();
            var svc = new LikeService(likeRepo.Object, userRepo.Object, postRepo.Object);

            var count = await svc.GetLikeCountAsync(1);

            Assert.Equal(5, count);
        }

        [Fact]
        public async Task GetLikeCount_PostDoesNotExist_ThrowsException()
        {
            var postRepo = new Mock<IPostRepository>();
            postRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((Post?)null);

            var likeRepo = new Mock<ILikeRepository>();
            var userRepo = new Mock<IUserRepository>();
            var svc = new LikeService(likeRepo.Object, userRepo.Object, postRepo.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                svc.GetLikeCountAsync(1)
            );
        }
    }
}

