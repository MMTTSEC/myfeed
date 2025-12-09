using MyFeed.Domain.Interfaces;
using MyFeed.Domain.Entities;
using MyFeed.Application.Interfaces;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace MyFeed.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly PasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
            _passwordHasher = new PasswordHasher();
        }

        public async Task RegisterUserAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            if (password.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters long.", nameof(password));

            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                throw new ArgumentException("Password must contain at least one special character.", nameof(password));

            // Domain entity will validate username (empty, too long, etc.)
            // But we check for duplicates first
            var existingUser = await _userRepo.GetByUsernameAsync(username);
            if (existingUser != null)
                throw new InvalidOperationException("Username already taken.");

            // Hash the password before storing
            var passwordHash = _passwordHasher.HashPassword(password);

            // Domain entity enforces username/password rules
            var user = new User(username, passwordHash);
            await _userRepo.AddAsync(user);
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await _userRepo.GetByUsernameAsync(username);
            if (user == null)
                return null;

            if (!_passwordHasher.VerifyPassword(password, user.PasswordHash))
                return null;

            return user;
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
