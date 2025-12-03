using MyFeed.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFeed.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;

        }
        public async Task RegisterUserAsync(string username, string passwordHash)
        {
            var existingUser = await _userRepo.GetByUsernameAsync(username);
            if (existingUser != null)
                throw new InvalidOperationException("Username already taken.");
            var user = new Domain.Entities.User(username, passwordHash);
            await _userRepo.AddAsync(user);
        }
    }
}
