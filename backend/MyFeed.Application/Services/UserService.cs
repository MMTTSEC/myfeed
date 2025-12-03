using MyFeed.Domain.Interfaces;
using MyFeed.Domain.Entities;
using System;
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
            // Domain entity will validate username (empty, too long, etc.)
            // But we check for duplicates first
            var existingUser = await _userRepo.GetByUsernameAsync(username);
            if (existingUser != null)
                throw new InvalidOperationException("Username already taken.");

            // Domain entity enforces username/password rules
            var user = new User(username, passwordHash);
            await _userRepo.AddAsync(user);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepo.GetByIdAsync(id);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userRepo.GetByUsernameAsync(username);
        }
    }
}
