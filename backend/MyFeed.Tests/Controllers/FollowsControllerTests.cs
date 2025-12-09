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
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MyFeed.Domain.Entities;

namespace MyFeed.Tests.Controllers
{
    public class FollowsControllerTests
    {
        private static FollowsController CreateController(Mock<IFollowService> followService, Mock<IUserService>? userService = null, int userId = 1)
        {
            var controller = new FollowsController(followService.Object, (userService ?? new Mock<IUserService>()).Object);
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
        public async Task FollowUser_ValidData_ReturnsOk()
        {
            // Arrange
            var mockFollowService = new Mock<IFollowService>();
            var controller = CreateController(mockFollowService);
            var request = new FollowUserRequest
            {
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

            var controller = CreateController(mockFollowService);
            var request = new FollowUserRequest
            {
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

            var controller = CreateController(mockFollowService);
            var request = new FollowUserRequest
            {
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

            var controller = CreateController(mockFollowService);
            var request = new FollowUserRequest
            {
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

            var controller = CreateController(mockFollowService);
            var request = new FollowUserRequest
            {
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

            var controller = CreateController(mockFollowService);
            var request = new FollowUserRequest
            {
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
            var controller = CreateController(mockFollowService);

            // Act
            var result = await controller.UnfollowUser(followeeId: 2);

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

            var controller = CreateController(mockFollowService);

            // Act
            var result = await controller.UnfollowUser(followeeId: 2);

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

            var controller = CreateController(mockFollowService);

            // Act
            var result = await controller.UnfollowUser(followeeId: 2);

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

            var controller = CreateController(mockFollowService);

            // Act
            var result = await controller.UnfollowUser(followeeId: 2);

            // Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public async Task GetFollowing_ValidUser_ReturnsOk()
        {
            // Arrange
            var mockFollowService = new Mock<IFollowService>();
            var mockUserService = new Mock<IUserService>();
            var followeeIds = new List<int> { 2, 3, 4 };
            mockFollowService
                .Setup(s => s.GetFollowingAsync(1))
                .ReturnsAsync(followeeIds);

            mockUserService.Setup(s => s.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => new User($"user{id}", "hash") { Id = id });

            var controller = CreateController(mockFollowService, mockUserService, userId: 1);

            // Act
            var result = await controller.GetFollowing(userId: 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var returnedUsers = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);
            Assert.Equal(3, returnedUsers.Count());
        }

        [Fact]
        public async Task GetFollowing_UserNotFound_ReturnsBadRequest()
        {
            // Arrange
            var mockFollowService = new Mock<IFollowService>();
            mockFollowService
                .Setup(s => s.GetFollowingAsync(It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException("User not found."));

            var controller = CreateController(mockFollowService);

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

            var controller = CreateController(mockFollowService);

            // Act
            var result = await controller.GetFollowing(userId: 1);

            // Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }
    }
}