using Moq;
using MyFeed.Api.Controllers;
using MyFeed.Application.Interfaces;
using MyFeed.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System;
using System.Threading.Tasks;

namespace MyFeed.Tests.Controllers
{
    public class UsersControllerTests
    {
        [Fact]
        public async Task RegisterUser_ValidData_ReturnsCreated()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var controller = new UsersController(mockUserService.Object);
            var request = new RegisterUserRequest { Username = "testuser", PasswordHash = "hashedpassword123" };

            // Act
            var result = await controller.RegisterUser(request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            mockUserService.Verify(s => s.RegisterUserAsync("testuser", "hashedpassword123"), Times.Once);
        }

        [Fact]
        public async Task RegisterUser_UsernameAlreadyTaken_ReturnsBadRequest()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.RegisterUserAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new InvalidOperationException("Username already taken."));
            var controller = new UsersController(mockUserService.Object);
            var request = new RegisterUserRequest { Username = "existinguser", PasswordHash = "hashedpassword123" };

            // Act
            var result = await controller.RegisterUser(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task GetUserById_UserExists_ReturnsOk()
        {
            // Arrange
            var user = new User("testuser", "hashedpassword123");
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(user);
            var controller = new UsersController(mockUserService.Object);

            // Act
            var result = await controller.GetUserById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var returnedUser = Assert.IsType<User>(okResult.Value);
            Assert.Equal("testuser", returnedUser.Username);
        }

        [Fact]
        public async Task GetUserById_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.GetUserByIdAsync(999)).ReturnsAsync((User?)null);
            var controller = new UsersController(mockUserService.Object);

            // Act
            var result = await controller.GetUserById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetUserByUsername_UserExists_ReturnsOk()
        {
            // Arrange
            var user = new User("testuser", "hashedpassword123");
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.GetUserByUsernameAsync("testuser")).ReturnsAsync(user);
            var controller = new UsersController(mockUserService.Object);

            // Act
            var result = await controller.GetUserByUsername("testuser");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var returnedUser = Assert.IsType<User>(okResult.Value);
            Assert.Equal("testuser", returnedUser.Username);
        }

        [Fact]
        public async Task GetUserByUsername_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.GetUserByUsernameAsync("nonexistent")).ReturnsAsync((User?)null);
            var controller = new UsersController(mockUserService.Object);

            // Act
            var result = await controller.GetUserByUsername("nonexistent");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}

