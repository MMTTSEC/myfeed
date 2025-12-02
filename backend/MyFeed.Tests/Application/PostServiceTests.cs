using Moq;
using MyFeed.Domain.Entities;
using MyFeed.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MyFeed.Application.Services;

namespace MyFeed.Tests.Application
{
    public class PostServiceTests
    {
        [Fact]
        public async Task CreatePost_AuthorDoesNotExist_ThrowsException()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((User?)null);

            var postRepo = new Mock<IPostRepository>();
            var svc = new PostService(postRepo.Object, userRepo.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                svc.CreatePostAsync(1, "Title", "Body")
            );
        }

        [Fact]
        public async Task CreatePost_ValidData_CallsRepositoryAdd()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new User("u", "h"));

            var postRepo = new Mock<IPostRepository>();
            var svc = new PostService(postRepo.Object, userRepo.Object);

            await svc.CreatePostAsync(1, "Title", "Body");

            postRepo.Verify(x => x.AddAsync(It.IsAny<Post>()), Times.Once);
        }

        [Fact]
        public async Task CreatePost_InvalidBody_DoesNotCallRepositoryAdd()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new User("u", "h"));

            var postRepo = new Mock<IPostRepository>();
            var svc = new PostService(postRepo.Object, userRepo.Object);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                svc.CreatePostAsync(1, "Title", "") // empty body -> Post throws
            );

            postRepo.Verify(x => x.AddAsync(It.IsAny<Post>()), Times.Never);
        }



        [Fact]
        public async Task CreatePost_EmptyTitle_ThrowsArgumentException_AndDoesNotCallRepositoryAdd()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new User("u", "h"));

            var postRepo = new Mock<IPostRepository>();
            var svc = new PostService(postRepo.Object, userRepo.Object);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                svc.CreatePostAsync(1, "", "Body")
            );

            postRepo.Verify(x => x.AddAsync(It.IsAny<Post>()), Times.Never);
        }
    }
}