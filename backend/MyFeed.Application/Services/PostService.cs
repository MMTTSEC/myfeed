using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFeed.Domain.Interfaces;
using MyFeed.Domain.Entities;
using MyFeed.Application.Interfaces;

namespace MyFeed.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepo;
        private readonly IUserRepository _userRepo;

        public PostService(IPostRepository postRepo, IUserRepository userRepo)
        {
            _postRepo = postRepo;
            _userRepo = userRepo;
        }

        public async Task CreatePostAsync(int authorId, string title, string body)
        {
            var author = await _userRepo.GetByIdAsync(authorId);
            if (author == null)
                throw new InvalidOperationException("Author not found.");

            // Domain entity enforces body/title rules
            var post = new Post(authorId, title, body);
            await _postRepo.AddAsync(post);
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            return await _postRepo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Post>> GetPostsByUserAsync(int userId)
        {
            return await _postRepo.GetPostsByUserAsync(userId);
        }

        public async Task<IEnumerable<Post>> GetFeedAsync(int userId)
        {
            return await _postRepo.GetFeedAsync(userId);
        }

    }
}
