using MyFeed.Application.Interfaces;
using MyFeed.Application.Services;
using MyFeed.Domain.Interfaces;
using MyFeed.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace MyFeed.Api.Data
{
    public class DatabaseSeeder
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepo;
        private readonly IPostRepository _postRepo;
        private readonly ILikeRepository _likeRepo;
        private readonly IFollowRepository _followRepo;
        private readonly IDirectMessageRepository _dmRepo;
        private readonly PasswordHasher _passwordHasher;

        public DatabaseSeeder(
            AppDbContext context,
            IUserRepository userRepo,
            IPostRepository postRepo,
            ILikeRepository likeRepo,
            IFollowRepository followRepo,
            IDirectMessageRepository dmRepo)
        {
            _context = context;
            _userRepo = userRepo;
            _postRepo = postRepo;
            _likeRepo = likeRepo;
            _followRepo = followRepo;
            _dmRepo = dmRepo;
            _passwordHasher = new PasswordHasher();
        }

        private void SetCreatedAt(MyFeed.Domain.Entities.Entity entity, DateTime createdAt)
        {
            var property = typeof(MyFeed.Domain.Entities.Entity).GetProperty("CreatedAt", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (property != null)
            {
                property.SetValue(entity, createdAt);
            }
        }

        public async Task SeedAsync()
        {
            // Password that meets requirements: at least 8 chars and one special character
            const string commonPassword = "Password123!";

            // Create users
            var user1 = new MyFeed.Domain.Entities.User("Zenty", _passwordHasher.HashPassword(commonPassword));
            var user2 = new MyFeed.Domain.Entities.User("mycookie5", _passwordHasher.HashPassword(commonPassword));
            var user3 = new MyFeed.Domain.Entities.User("MMTTSEC", _passwordHasher.HashPassword(commonPassword));
            var user4 = new MyFeed.Domain.Entities.User("MSU98", _passwordHasher.HashPassword(commonPassword));

            await _userRepo.AddAsync(user1);
            await _userRepo.AddAsync(user2);
            await _userRepo.AddAsync(user3);
            await _userRepo.AddAsync(user4);

            // Get user IDs after save
            var zenty = await _userRepo.GetByUsernameAsync("Zenty") ?? throw new Exception("Failed to create Zenty user");
            var mycookie5 = await _userRepo.GetByUsernameAsync("mycookie5") ?? throw new Exception("Failed to create mycookie5 user");
            var mmttsec = await _userRepo.GetByUsernameAsync("MMTTSEC") ?? throw new Exception("Failed to create MMTTSEC user");
            var msu98 = await _userRepo.GetByUsernameAsync("MSU98") ?? throw new Exception("Failed to create MSU98 user");

            // Create posts with varied timestamps (spread over the last 7 days)
            var now = DateTime.UtcNow;
            var random = new Random(42); // Fixed seed for consistency

            // Zenty's posts
            var zentyPosts = new[]
            {
                ("test", "Just finished a great coding session! Feeling productive today. 🚀"),
                ("test", "Anyone else love the smell of coffee in the morning? ☕"),
                ("test", "Working on a new project. Can't wait to share it with everyone!"),
                ("test", "Weekend vibes are hitting different today. Time to relax! 😎"),
                ("test", "Learning new technologies is always exciting. What are you learning?")
            };

            foreach (var (title, body) in zentyPosts)
            {
                var post = new MyFeed.Domain.Entities.Post(zenty.Id, title, body);
                var createdAt = now.AddDays(-random.Next(0, 7)).AddHours(-random.Next(0, 24));
                SetCreatedAt(post, createdAt);
                await _postRepo.AddAsync(post);
            }

            // mycookie5's posts
            var mycookie5Posts = new[]
            {
                ("test", "Had an amazing day at the park today! Nature is beautiful. 🌳"),
                ("test", "Just discovered a new favorite song. Music makes everything better! 🎵"),
                ("test", "Working on some personal projects. Always good to keep learning."),
                ("test", "Coffee break time! Who else needs their daily caffeine fix? ☕"),
                ("test", "Weekend plans: coding, reading, and maybe some gaming. Perfect! 🎮")
            };

            foreach (var (title, body) in mycookie5Posts)
            {
                var post = new MyFeed.Domain.Entities.Post(mycookie5.Id, title, body);
                var createdAt = now.AddDays(-random.Next(0, 7)).AddHours(-random.Next(0, 24));
                SetCreatedAt(post, createdAt);
                await _postRepo.AddAsync(post);
            }

            // MMTTSEC's posts
            var mmttsecPosts = new[]
            {
                ("test", "Tech stack decisions are always interesting. What's your favorite? 💻"),
                ("test", "Morning run completed! Exercise keeps the mind sharp. 🏃"),
                ("test", "Reading about new design patterns. Always something new to learn!"),
                ("test", "Collaboration is key in software development. Teamwork makes the dream work!"),
                ("test", "Debugging can be frustrating, but solving problems is so satisfying! 🐛")
            };

            foreach (var (title, body) in mmttsecPosts)
            {
                var post = new MyFeed.Domain.Entities.Post(mmttsec.Id, title, body);
                var createdAt = now.AddDays(-random.Next(0, 7)).AddHours(-random.Next(0, 24));
                SetCreatedAt(post, createdAt);
                await _postRepo.AddAsync(post);
            }

            // MSU98's posts
            var msu98Posts = new[]
            {
                ("test", "New project idea brewing! Excited to start working on it. 💡"),
                ("test", "Sometimes the best code is the code you don't write. Keep it simple!"),
                ("test", "Weekend coding session with good music. Perfect combination! 🎧"),
                ("test", "Learning from mistakes is part of the journey. Growth mindset! 📈"),
                ("test", "Sharing knowledge with others is one of the best parts of development.")
            };

            foreach (var (title, body) in msu98Posts)
            {
                var post = new MyFeed.Domain.Entities.Post(msu98.Id, title, body);
                var createdAt = now.AddDays(-random.Next(0, 7)).AddHours(-random.Next(0, 24));
                SetCreatedAt(post, createdAt);
                await _postRepo.AddAsync(post);
            }

            // Get all posts after they're saved
            var allPosts = (await _postRepo.GetAllAsync()).ToList();

            // Create follows (various combinations)
            await _followRepo.AddAsync(new MyFeed.Domain.Entities.Follow(zenty.Id, mycookie5.Id));
            await _followRepo.AddAsync(new MyFeed.Domain.Entities.Follow(zenty.Id, mmttsec.Id));
            await _followRepo.AddAsync(new MyFeed.Domain.Entities.Follow(mycookie5.Id, zenty.Id));
            await _followRepo.AddAsync(new MyFeed.Domain.Entities.Follow(mycookie5.Id, msu98.Id));
            await _followRepo.AddAsync(new MyFeed.Domain.Entities.Follow(mmttsec.Id, zenty.Id));
            await _followRepo.AddAsync(new MyFeed.Domain.Entities.Follow(mmttsec.Id, mycookie5.Id));
            await _followRepo.AddAsync(new MyFeed.Domain.Entities.Follow(msu98.Id, mmttsec.Id));
            await _followRepo.AddAsync(new MyFeed.Domain.Entities.Follow(msu98.Id, zenty.Id));

            // Create likes (users like each other's posts)
            // Zenty likes some posts
            await _likeRepo.AddAsync(new MyFeed.Domain.Entities.Like(zenty.Id, allPosts.First(p => p.AuthorUserId == mycookie5.Id).Id));
            await _likeRepo.AddAsync(new MyFeed.Domain.Entities.Like(zenty.Id, allPosts.First(p => p.AuthorUserId == mmttsec.Id).Id));
            await _likeRepo.AddAsync(new MyFeed.Domain.Entities.Like(zenty.Id, allPosts.Where(p => p.AuthorUserId == msu98.Id).Skip(1).First().Id));

            // mycookie5 likes some posts
            await _likeRepo.AddAsync(new MyFeed.Domain.Entities.Like(mycookie5.Id, allPosts.First(p => p.AuthorUserId == zenty.Id).Id));
            await _likeRepo.AddAsync(new MyFeed.Domain.Entities.Like(mycookie5.Id, allPosts.First(p => p.AuthorUserId == msu98.Id).Id));
            await _likeRepo.AddAsync(new MyFeed.Domain.Entities.Like(mycookie5.Id, allPosts.Where(p => p.AuthorUserId == mmttsec.Id).Skip(1).First().Id));

            // MMTTSEC likes some posts
            await _likeRepo.AddAsync(new MyFeed.Domain.Entities.Like(mmttsec.Id, allPosts.Where(p => p.AuthorUserId == zenty.Id).Skip(1).First().Id));
            await _likeRepo.AddAsync(new MyFeed.Domain.Entities.Like(mmttsec.Id, allPosts.First(p => p.AuthorUserId == mycookie5.Id).Id));
            await _likeRepo.AddAsync(new MyFeed.Domain.Entities.Like(mmttsec.Id, allPosts.Where(p => p.AuthorUserId == msu98.Id).Skip(2).First().Id));

            // MSU98 likes some posts
            await _likeRepo.AddAsync(new MyFeed.Domain.Entities.Like(msu98.Id, allPosts.Where(p => p.AuthorUserId == zenty.Id).Skip(2).First().Id));
            await _likeRepo.AddAsync(new MyFeed.Domain.Entities.Like(msu98.Id, allPosts.Where(p => p.AuthorUserId == mycookie5.Id).Skip(1).First().Id));
            await _likeRepo.AddAsync(new MyFeed.Domain.Entities.Like(msu98.Id, allPosts.First(p => p.AuthorUserId == mmttsec.Id).Id));

            // Create direct messages with varied timestamps
            var messages = new[]
            {
                (zenty.Id, mycookie5.Id, "Hey! How's your project going?", -2, -5),
                (mycookie5.Id, zenty.Id, "It's going great! Thanks for asking. How about yours?", -2, -4),
                (zenty.Id, mycookie5.Id, "Mine is progressing well too. Want to collaborate?", -1, -3),
                (mmttsec.Id, zenty.Id, "Great post today! Really enjoyed reading it.", -3, -2),
                (zenty.Id, mmttsec.Id, "Thanks! I appreciate the feedback. 😊", -3, -1),
                (msu98.Id, mycookie5.Id, "Hey, can we discuss that idea we talked about?", -1, -4),
                (mycookie5.Id, msu98.Id, "Sure! Let's schedule a time to chat.", -1, -3),
                (mmttsec.Id, msu98.Id, "I saw your latest post. Very interesting points!", -4, -3),
                (msu98.Id, mmttsec.Id, "Thanks! I'd love to hear your thoughts on it.", -4, -2),
                (zenty.Id, msu98.Id, "Quick question about your project approach.", -5, -1)
            };

            foreach (var (senderId, receiverId, message, daysAgo, hoursAgo) in messages)
            {
                var dm = new MyFeed.Domain.Entities.DM(senderId, receiverId, message);
                var createdAt = now.AddDays(daysAgo).AddHours(hoursAgo);
                SetCreatedAt(dm, createdAt);
                await _dmRepo.AddAsync(dm);
            }

        }
    }
}

