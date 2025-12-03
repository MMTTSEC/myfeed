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

        [Fact]
        public async Task RegisterUser_WithExistingUsername_ThrowsException()
        {
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByUsernameAsync("testuser")).ReturnsAsync(new User("testuser", "hash"));
            var svc = new UserService(userRepo.Object);
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                svc.RegisterUserAsync("testuser", "password123")
            );
            userRepo.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task RegisterUser_InvalidUsername_ThrowsArgumentException()
        {
            var userRepo = new Mock<IUserRepository>();
            var svc = new UserService(userRepo.Object);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                svc.RegisterUserAsync("", "password123")
            );

            userRepo.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task RegisterUser_UsernameTooLong_ThrowsArgumentException()
        {
            var userRepo = new Mock<IUserRepository>();
            var svc = new UserService(userRepo.Object);

            string longUsername = new string('a', 51); 

            await Assert.ThrowsAsync<ArgumentException>(() =>
                svc.RegisterUserAsync(longUsername, "password123")
            );

            userRepo.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never);
        }
        
       
    }
}

