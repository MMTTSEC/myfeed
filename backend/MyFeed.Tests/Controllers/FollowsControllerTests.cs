using Moq;
using Microsoft.AspNetCore.Mvc;
using MyFeed.Api.Controllers;
using MyFeed.Application.Services;
using MyFeed.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MyFeed.Tests.Controllers
{
    public class FollowsControllerTests
    {
        [Fact]
        public async Task FollowUser_ValidData_ReturnsOk()
        {
            // Arrange
            var mockFollowService = new Mock<IFollowService>();
            var controller = new FollowsController(mockFollowService.Object);
            var request = new FollowUserRequest
            {
                FollowerId = 1,
                FolloweeId = 2
            };

            // Act
            var result = await controller.FollowUser(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            mockFollowService.Verify(s => s.FollowUserAsync(1, 2), Times.Once);
        }

        [Fact]
        public async Task FollowUser_FollowerNotFound_ReturnsBadRequest()
        {
            // Arrange
            var mockFollowService = new Mock<IFollowService>();
            mockFollowService
                .Setup(s => s.FollowUserAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException("Follower not found."));

            var controller = new FollowsController(mockFollowService.Object);
            var request = new FollowUserRequest
            {
                FollowerId = 999,
                FolloweeId = 2
            };

            // Act
            var result = await controller.FollowUser(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task FollowUser_FolloweeNotFound_ReturnsBadRequest()
        {
            // Arrange
            var mockFollowService = new Mock<IFollowService>();
            mockFollowService
                .Setup(s => s.FollowUserAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException("Followee not found."));

            var controller = new FollowsController(mockFollowService.Object);
            var request = new FollowUserRequest
            {
                FollowerId = 1,
                FolloweeId = 999
            };

            // Act
            var result = await controller.FollowUser(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task FollowUser_AlreadyFollowing_ReturnsBadRequest()
        {
            // Arrange
            var mockFollowService = new Mock<IFollowService>();
            mockFollowService
                .Setup(s => s.FollowUserAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException("Already following this user."));

            var controller = new FollowsController(mockFollowService.Object);
            var request = new FollowUserRequest
            {
                FollowerId = 1,
                FolloweeId = 2
            };

            // Act
            var result = await controller.FollowUser(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task FollowUser_CannotFollowYourself_ReturnsBadRequest()
        {
            // Arrange
            var mockFollowService = new Mock<IFollowService>();
            mockFollowService
                .Setup(s => s.FollowUserAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new ArgumentException("A user cannot follow themselves."));

            var controller = new FollowsController(mockFollowService.Object);
            var request = new FollowUserRequest
            {
                FollowerId = 1,
                FolloweeId = 1
            };

            // Act
            var result = await controller.FollowUser(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task FollowUser_UnexpectedError_ReturnsServerError()
        {
            // Arrange
            var mockFollowService = new Mock<IFollowService>();
            mockFollowService
                .Setup(s => s.FollowUserAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            var controller = new FollowsController(mockFollowService.Object);
            var request = new FollowUserRequest
            {
                FollowerId = 1,
                FolloweeId = 2
            };

            // Act
            var result = await controller.FollowUser(request);

            // Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public async Task UnfollowUser_ValidData_ReturnsNoContent()
        {
            // Arrange
            var mockFollowService = new Mock<IFollowService>();
            var controller = new FollowsController(mockFollowService.Object);

            // Act
            var result = await controller.UnfollowUser(followerId: 1, followeeId: 2);

            // Assert
            Assert.IsType<NoContentResult>(result);
            mockFollowService.Verify(s => s.UnfollowUserAsync(1, 2), Times.Once);
        }

        [Fact]
        public async Task UnfollowUser_FollowerNotFound_ReturnsBadRequest()
        {
            // Arrange
            var mockFollowService = new Mock<IFollowService>();
            mockFollowService
                .Setup(s => s.UnfollowUserAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException("Follower not found."));

            var controller = new FollowsController(mockFollowService.Object);

            // Act
            var result = await controller.UnfollowUser(followerId: 999, followeeId: 2);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task UnfollowUser_NotFollowing_ReturnsBadRequest()
        {
            // Arrange
            var mockFollowService = new Mock<IFollowService>();
            mockFollowService
                .Setup(s => s.UnfollowUserAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException("Not following this user."));

            var controller = new FollowsController(mockFollowService.Object);

            // Act
            var result = await controller.UnfollowUser(followerId: 1, followeeId: 2);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task UnfollowUser_UnexpectedError_ReturnsServerError()
        {
            // Arrange
            var mockFollowService = new Mock<IFollowService>();
            mockFollowService
                .Setup(s => s.UnfollowUserAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            var controller = new FollowsController(mockFollowService.Object);

            // Act
            var result = await controller.UnfollowUser(followerId: 1, followeeId: 2);

            // Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public async Task GetFollowing_ValidUser_ReturnsOk()
        {
            // Arrange
            var mockFollowService = new Mock<IFollowService>();
            var followeeIds = new List<int> { 2, 3, 4 };
            mockFollowService
                .Setup(s => s.GetFollowingAsync(1))
                .ReturnsAsync(followeeIds);

            var controller = new FollowsController(mockFollowService.Object);

            // Act
            var result = await controller.GetFollowing(userId: 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var returnedIds = Assert.IsAssignableFrom<IEnumerable<int>>(okResult.Value);
            Assert.Equal(3, returnedIds.Count());
            Assert.Contains(2, returnedIds);
            Assert.Contains(3, returnedIds);
            Assert.Contains(4, returnedIds);
        }

        [Fact]
        public async Task GetFollowing_UserNotFound_ReturnsBadRequest()
        {
            // Arrange
            var mockFollowService = new Mock<IFollowService>();
            mockFollowService
                .Setup(s => s.GetFollowingAsync(It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException("User not found."));

            var controller = new FollowsController(mockFollowService.Object);

            // Act
            var result = await controller.GetFollowing(userId: 999);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task GetFollowing_UnexpectedError_ReturnsServerError()
        {
            // Arrange
            var mockFollowService = new Mock<IFollowService>();
            mockFollowService
                .Setup(s => s.GetFollowingAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            var controller = new FollowsController(mockFollowService.Object);

            // Act
            var result = await controller.GetFollowing(userId: 1);

            // Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }
    }
}