using MyFeed.Domain.Entities;
using System.Threading.Tasks;

namespace MyFeed.Application.Interfaces
{
    public interface IUserService
    {
        Task RegisterUserAsync(string username, string passwordHash);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByUsernameAsync(string username);
    }
}

