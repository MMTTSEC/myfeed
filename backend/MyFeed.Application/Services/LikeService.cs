using System;
using System.Threading.Tasks;
using MyFeed.Domain.Interfaces;
using MyFeed.Domain.Entities;
using MyFeed.Application.Interfaces;

namespace MyFeed.Application.Services
{
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _likeRepo;
        private readonly IUserRepository _userRepo;
        private readonly IPostRepository _postRepo;

        public LikeService(ILikeRepository likeRepo, IUserRepository userRepo, IPostRepository postRepo)
        {
            _likeRepo = likeRepo;
            _userRepo = userRepo;
            _postRepo = postRepo;
        }

        public async Task LikePostAsync(int userId, int postId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            var post = await _postRepo.GetByIdAsync(postId);
            if (post == null)
                throw new InvalidOperationException("Post not found.");

            var alreadyLiked = await _likeRepo.ExistsAsync(userId, postId);
            if (alreadyLiked)
                throw new InvalidOperationException("Post already liked.");

            var like = new Like(userId, postId);
            await _likeRepo.AddAsync(like);
        }

        public async Task UnlikePostAsync(int userId, int postId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            var post = await _postRepo.GetByIdAsync(postId);
            if (post == null)
                throw new InvalidOperationException("Post not found.");

            var exists = await _likeRepo.ExistsAsync(userId, postId);
            if (!exists)
                throw new InvalidOperationException("Like does not exist.");

            await _likeRepo.RemoveAsync(userId, postId);
        }

        public async Task<int> GetLikeCountAsync(int postId)
        {
            var post = await _postRepo.GetByIdAsync(postId);
            if (post == null)
                throw new InvalidOperationException("Post not found.");

            return await _likeRepo.CountForPostAsync(postId);
        }

        public async Task<bool> HasUserLikedPostAsync(int userId, int postId)
        {
            return await _likeRepo.ExistsAsync(userId, postId);
        }
    }
}

