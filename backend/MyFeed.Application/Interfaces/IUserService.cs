using MyFeed.Domain.Entities;
using System.Threading.Tasks;

namespace MyFeed.Application.Interfaces
{
    public interface IUserService
    {
        Task RegisterUserAsync(string username, string password);
        Task<User?> LoginAsync(string username, string password);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByUsernameAsync(string username);
    }
}

