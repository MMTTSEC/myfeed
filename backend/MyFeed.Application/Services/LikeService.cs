using System;
using System.Threading.Tasks;
using MyFeed.Domain.Interfaces;
using MyFeed.Domain.Entities;

namespace MyFeed.Application.Services
{
    public class LikeService
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
    }
}

