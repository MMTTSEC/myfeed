using System.Threading.Tasks;

namespace MyFeed.Application.Interfaces
{
    public interface ILikeService
    {
        Task LikePostAsync(int userId, int postId);
        Task UnlikePostAsync(int userId, int postId);
        Task<int> GetLikeCountAsync(int postId);
        Task<bool> HasUserLikedPostAsync(int userId, int postId);
    }
}