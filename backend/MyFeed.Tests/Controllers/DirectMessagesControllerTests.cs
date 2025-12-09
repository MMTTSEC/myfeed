using Moq;
using Microsoft.AspNetCore.Mvc;
using MyFeed.Api.Controllers;
using MyFeed.Application.Interfaces;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MyFeed.Api.Hubs;
using Microsoft.AspNetCore.SignalR;
using MyFeed.Domain.Entities;

namespace MyFeed.Tests.Controllers
{
    public class DirectMessagesControllerTests
    {
        private static DirectMessagesController CreateController(Mock<IDMService> dmService, Mock<IUserService>? userService = null, Mock<IHubContext<ChatHub>>? hubContext = null, int userId = 1, IClientProxy? clientProxy = null)
        {
            var hub = hubContext ?? new Mock<IHubContext<ChatHub>>();
            var clients = new Mock<IHubClients>();
            var proxy = clientProxy ?? new Mock<IClientProxy>().Object;
            clients.Setup(c => c.Groups(It.IsAny<string[]>())).Returns(proxy);
            hub.Setup(h => h.Clients).Returns(clients.Object);

            var controller = new DirectMessagesController(dmService.Object, (userService ?? new Mock<IUserService>()).Object, hub.Object);
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
        public async Task SendDM_ValidData_ReturnsOk()
        {
            // Arrange
            var mockDMService = new Mock<IDMService>();
            var mockUserService = new Mock<IUserService>();
            var mockHub = new Mock<IHubContext<ChatHub>>();
            var clients = new Mock<IHubClients>();
            var proxy = new Mock<IClientProxy>();
            clients.Setup(c => c.Groups(It.IsAny<string[]>())).Returns(proxy.Object);
            mockHub.Setup(h => h.Clients).Returns(clients.Object);

            mockDMService.Setup(s => s.SendDMAsync(1, 2, "Hello!"))
                .ReturnsAsync(new DM(1, 2, "Hello!") { Id = 10 });
            mockUserService.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(new User("sender", "hash") { Id = 1 });
            mockUserService.Setup(s => s.GetUserByIdAsync(2)).ReturnsAsync(new User("receiver", "hash") { Id = 2 });

            var controller = CreateController(mockDMService, mockUserService, mockHub, userId: 1, clientProxy: proxy.Object);
            var request = new SendDMRequest
            {
                ReceiverId = 2,
                Message = "Hello!"
            };

            // Act
            var result = await controller.SendDM(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            mockDMService.Verify(s => s.SendDMAsync(1, 2, "Hello!"), Times.Once);
        }

        [Fact]
        public async Task SendDM_SenderNotFound_ReturnsBadRequest()
        {
            // Arrange
            var mockDMService = new Mock<IDMService>();
            mockDMService
                .Setup(s => s.SendDMAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new InvalidOperationException("Sender not found."));

            var controller = CreateController(mockDMService);
            var request = new SendDMRequest
            {
                ReceiverId = 2,
                Message = "Hello!"
            };

            // Act
            var result = await controller.SendDM(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
            Assert.Equal("Sender not found.", badRequest.Value);
        }

        [Fact]
        public async Task SendDM_ReceiverNotFound_ReturnsBadRequest()
        {
            // Arrange
            var mockDMService = new Mock<IDMService>();
            mockDMService
                .Setup(s => s.SendDMAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new InvalidOperationException("Receiver not found."));

            var controller = CreateController(mockDMService);
            var request = new SendDMRequest
            {
                ReceiverId = 999,
                Message = "Hello!"
            };

            // Act
            var result = await controller.SendDM(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task SendDM_SameSenderAndReceiver_ReturnsBadRequest()
        {
            // Arrange
            var mockDMService = new Mock<IDMService>();
            mockDMService
                .Setup(s => s.SendDMAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException("A user cannot send a DM to themselves."));

            var controller = CreateController(mockDMService);
            var request = new SendDMRequest
            {
                ReceiverId = 1,
                Message = "Hello!"
            };

            // Act
            var result = await controller.SendDM(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task SendDM_EmptyMessage_ReturnsBadRequest()
        {
            // Arrange
            var mockDMService = new Mock<IDMService>();
            mockDMService
                .Setup(s => s.SendDMAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException("DM message cannot be empty."));

            var controller = CreateController(mockDMService);
            var request = new SendDMRequest
            {
                ReceiverId = 2,
                Message = ""
            };

            // Act
            var result = await controller.SendDM(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task SendDM_MessageTooLong_ReturnsBadRequest()
        {
            // Arrange
            var mockDMService = new Mock<IDMService>();
            mockDMService
                .Setup(s => s.SendDMAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException("DM message cannot be longer than 1000 characters."));

            var controller = CreateController(mockDMService);
            var request = new SendDMRequest
            {
                ReceiverId = 2,
                Message = new string('a', 1001)
            };

            // Act
            var result = await controller.SendDM(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }

        [Fact]
        public async Task SendDM_UnexpectedError_ReturnsServerError()
        {
            // Arrange
            var mockDMService = new Mock<IDMService>();
            mockDMService
                .Setup(s => s.SendDMAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            var controller = CreateController(mockDMService);
            var request = new SendDMRequest
            {
                ReceiverId = 2,
                Message = "Hello!"
            };

            // Act
            var result = await controller.SendDM(request);

            // Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }
    }
}