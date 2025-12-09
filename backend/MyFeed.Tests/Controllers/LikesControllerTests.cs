using Moq;
using Microsoft.AspNetCore.Mvc;
using MyFeed.Api.Controllers;
using MyFeed.Application.Services;
using MyFeed.Application.Interfaces;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MyFeed.Tests.Controllers
{
    public class LikesControllerTests
    {
        private static LikesController CreateController(Mock<ILikeService> likeService, int userId = 1)
        {
            var controller = new LikesController(likeService.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) }))
                }
            };
            return controller;
        }

        [Fact]
        public async Task LikePost_ValidData_ReturnsOk()
        {
            // Arrange
            var mockLikeService = new Mock<ILikeService>();
            var controller = CreateController(mockLikeService);
            var request = new LikePostRequest
            {
                PostId = 2
            };

            // Act
            var result = await controller.LikePost(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            mockLikeService.Verify(s => s.LikePostAsync(1, 2), Times.Once);
        }

        [Fact]
        public async Task LikePost_UserNotFound_ReturnsBadRequest()
        {
            // Arrange
            var mockLikeService = new Mock<ILikeService>();
            mockLikeService
                .Setup(s => s.LikePostAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException("User not found."));

            var controller = CreateController(mockLikeService);
            var request = new LikePostRequest
            {
                PostId = 2
            };

            // Act
            var result = await controller.LikePost(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task LikePost_PostNotFound_ReturnsBadRequest()
        {
            // Arrange
            var mockLikeService = new Mock<ILikeService>();
            mockLikeService
                .Setup(s => s.LikePostAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException("Post not found."));

            var controller = CreateController(mockLikeService);
            var request = new LikePostRequest
            {
                PostId = 999
            };

            // Act
            var result = await controller.LikePost(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task LikePost_AlreadyLiked_ReturnsBadRequest()
        {
            // Arrange
            var mockLikeService = new Mock<ILikeService>();
            mockLikeService
                .Setup(s => s.LikePostAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException("Post already liked."));

            var controller = CreateController(mockLikeService);
            var request = new LikePostRequest
            {
                PostId = 2
            };

            // Act
            var result = await controller.LikePost(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task LikePost_UnexpectedError_ReturnsServerError()
        {
            // Arrange
            var mockLikeService = new Mock<ILikeService>();
            mockLikeService
                .Setup(s => s.LikePostAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            var controller = CreateController(mockLikeService);
            var request = new LikePostRequest
            {
                PostId = 2
            };

            // Act
            var result = await controller.LikePost(request);

            // Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public async Task UnlikePost_ValidData_ReturnsNoContent()
        {
            // Arrange
            var mockLikeService = new Mock<ILikeService>();
            var controller = CreateController(mockLikeService);

            // Act
            var result = await controller.UnlikePost(postId: 2);

            // Assert
            Assert.IsType<NoContentResult>(result);
            mockLikeService.Verify(s => s.UnlikePostAsync(1, 2), Times.Once);
        }

        [Fact]
        public async Task UnlikePost_UserNotFound_ReturnsBadRequest()
        {
            // Arrange
            var mockLikeService = new Mock<ILikeService>();
            mockLikeService
                .Setup(s => s.UnlikePostAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException("User not found."));

            var controller = CreateController(mockLikeService);

            // Act
            var result = await controller.UnlikePost(postId: 2);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task UnlikePost_LikeDoesNotExist_ReturnsBadRequest()
        {
            // Arrange
            var mockLikeService = new Mock<ILikeService>();
            mockLikeService
                .Setup(s => s.UnlikePostAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException("Like does not exist."));

            var controller = CreateController(mockLikeService);

            // Act
            var result = await controller.UnlikePost(postId: 2);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task UnlikePost_UnexpectedError_ReturnsServerError()
        {
            // Arrange
            var mockLikeService = new Mock<ILikeService>();
            mockLikeService
                .Setup(s => s.UnlikePostAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            var controller = CreateController(mockLikeService);

            // Act
            var result = await controller.UnlikePost(postId: 2);

            // Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public async Task GetLikeCount_ValidPost_ReturnsOk()
        {
            // Arrange
            var mockLikeService = new Mock<ILikeService>();
            mockLikeService
                .Setup(s => s.GetLikeCountAsync(1))
                .ReturnsAsync(5);

            var controller = new LikesController(mockLikeService.Object);

            // Act
            var result = await controller.GetLikeCount(postId: 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            mockLikeService.Verify(s => s.GetLikeCountAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetLikeCount_PostNotFound_ReturnsBadRequest()
        {
            // Arrange
            var mockLikeService = new Mock<ILikeService>();
            mockLikeService
                .Setup(s => s.GetLikeCountAsync(It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException("Post not found."));

            var controller = new LikesController(mockLikeService.Object);

            // Act
            var result = await controller.GetLikeCount(postId: 999);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task GetLikeCount_UnexpectedError_ReturnsServerError()
        {
            // Arrange
            var mockLikeService = new Mock<ILikeService>();
            mockLikeService
                .Setup(s => s.GetLikeCountAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            var controller = new LikesController(mockLikeService.Object);

            // Act
            var result = await controller.GetLikeCount(postId: 1);

            // Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }
    }
}