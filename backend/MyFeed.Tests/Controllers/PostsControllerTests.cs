using Moq;
using Microsoft.AspNetCore.Mvc;
using MyFeed.Application.Interfaces;
using MyFeed.Api.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MyFeed.Tests.Controllers
{
    public class PostsControllerTests
    {
        [Fact]
        public async Task CreatePost_ValidData_ReturnsCreated()
        {
            // Arrange
            var mockPostService = new Mock<IPostService>();
            var controller = new PostsController(mockPostService.Object);
            var request = new CreatePostRequest
            {
                AuthorId = 1,
                Title = "Hello",
                Body = "World"
            };

            // Act
            var result = await controller.CreatePost(request);

            // Assert
            var created = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, created.StatusCode);
            mockPostService.Verify(s => s.CreatePostAsync(1, "Hello", "World"), Times.Once);
        }

        [Fact]
        public async Task CreatePost_AuthorMissing_ReturnsBadRequest()
        {
            // Arrange
            var mockPostService = new Mock<IPostService>();
            mockPostService
                .Setup(s => s.CreatePostAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new InvalidOperationException("Author not found."));

            var controller = new PostsController(mockPostService.Object);
            var request = new CreatePostRequest
            {
                AuthorId = 123,
                Title = "Hello",
                Body = "World"
            };

            // Act
            var result = await controller.CreatePost(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task CreatePost_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            var mockPostService = new Mock<IPostService>();
            mockPostService
                .Setup(s => s.CreatePostAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException("Title cannot be empty."));

            var controller = new PostsController(mockPostService.Object);
            var request = new CreatePostRequest
            {
                AuthorId = 1,
                Title = "",
                Body = "Body"
            };

            // Act
            var result = await controller.CreatePost(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task CreatePost_UnexpectedError_ReturnsServerError()
        {
            // Arrange
            var mockPostService = new Mock<IPostService>();
            mockPostService
                .Setup(s => s.CreatePostAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Unexpected"));

            var controller = new PostsController(mockPostService.Object);
            var request = new CreatePostRequest
            {
                AuthorId = 1,
                Title = "Title",
                Body = "Body"
            };

            // Act
            var result = await controller.CreatePost(request);

            // Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public async Task UpdatePost_Valid_ReturnsNoContent()
        {
            // Arrange
            var mockPostService = new Mock<IPostService>();
            var controller = new PostsController(mockPostService.Object);
            var request = new UpdatePostRequest
            {
                AuthorId = 1,
                Title = "New",
                Body = "Body"
            };

            // Act
            var result = await controller.UpdatePost(10, request);

            // Assert
            Assert.IsType<NoContentResult>(result);
            mockPostService.Verify(s => s.UpdatePostAsync(10, 1, "New", "Body"), Times.Once);
        }

        [Fact]
        public async Task UpdatePost_NotFound_ReturnsNotFound()
        {
            // Arrange
            var mockPostService = new Mock<IPostService>();
            mockPostService
                .Setup(s => s.UpdatePostAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new KeyNotFoundException());

            var controller = new PostsController(mockPostService.Object);
            var request = new UpdatePostRequest
            {
                AuthorId = 1,
                Title = "New",
                Body = "Body"
            };

            // Act
            var result = await controller.UpdatePost(10, request);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdatePost_InvalidOperation_ReturnsBadRequest()
        {
            // Arrange
            var mockPostService = new Mock<IPostService>();
            mockPostService
                .Setup(s => s.UpdatePostAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new InvalidOperationException("Only the author can edit this post."));

            var controller = new PostsController(mockPostService.Object);
            var request = new UpdatePostRequest
            {
                AuthorId = 2,
                Title = "New",
                Body = "Body"
            };

            // Act
            var result = await controller.UpdatePost(10, request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task DeletePost_Valid_ReturnsNoContent()
        {
            // Arrange
            var mockPostService = new Mock<IPostService>();
            var controller = new PostsController(mockPostService.Object);

            // Act
            var result = await controller.DeletePost(10, authorId: 1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            mockPostService.Verify(s => s.DeletePostAsync(10, 1), Times.Once);
        }

        [Fact]
        public async Task DeletePost_NotFound_ReturnsNotFound()
        {
            // Arrange
            var mockPostService = new Mock<IPostService>();
            mockPostService
                .Setup(s => s.DeletePostAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new KeyNotFoundException());

            var controller = new PostsController(mockPostService.Object);

            // Act
            var result = await controller.DeletePost(10, authorId: 1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeletePost_InvalidOperation_ReturnsBadRequest()
        {
            // Arrange
            var mockPostService = new Mock<IPostService>();
            mockPostService
                .Setup(s => s.DeletePostAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException("Only the author can delete this post."));

            var controller = new PostsController(mockPostService.Object);

            // Act
            var result = await controller.DeletePost(10, authorId: 2);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }
    }
}

