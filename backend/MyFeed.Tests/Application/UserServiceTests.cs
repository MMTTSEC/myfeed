using Moq;
using MyFeed.Application.Services;
using MyFeed.Domain.Entities;
using MyFeed.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyFeed.Tests.Application
{
    public class UserServiceTests
    {
        [Fact]
        public async Task RegisterUser_WithValidData_CallsRepoitory()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByUsernameAsync("testuser")).ReturnsAsync((User?)null);
            userRepo.Setup(x => x.AddAsync(It.IsAny<User>()));

            var svc = new UserService(userRepo.Object);

            await svc.RegisterUserAsync("testuser", "password123");

            userRepo.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        }
    }
}
