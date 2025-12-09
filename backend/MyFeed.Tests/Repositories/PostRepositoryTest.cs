using Microsoft.EntityFrameworkCore;
using MyFeed.Domain.Entities;
using MyFeed.Infrastructure.Data;
using MyFeed.Infrastructure.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MyFeed.Tests.Infrastructure.Repositories
{
    public class PostRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly PostRepository _repository;

        public PostRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new PostRepository(_context);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPost_WhenPostExists()
        {
            // Arrange
            var post = new Post(1, "Test Title", "Test Body");
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            var postId = post.Id;

            // Act
            var result = await _repository.GetByIdAsync(postId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(postId, result.Id);
            Assert.Equal("Test Title", result.Title);
            Assert.Equal("Test Body", result.Body);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenPostDoesNotExist()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoPostsExist()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllPosts()
        {
            // Arrange
            var post1 = new Post(1, "Title 1", "Body 1");
            var post2 = new Post(2, "Title 2", "Body 2");
            var post3 = new Post(3, "Title 3", "Body 3");
            await _context.Posts.AddRangeAsync(post1, post2, post3);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            var posts = result.ToList();
            Assert.Equal(3, posts.Count);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnPostsOrderedByCreatedAtDescending()
        {
            // Arrange
            var post1 = new Post(1, "First", "Body 1");
            await _context.Posts.AddAsync(post1);
            await _context.SaveChangesAsync();
            await Task.Delay(10);

            var post2 = new Post(2, "Second", "Body 2");
            await _context.Posts.AddAsync(post2);
            await _context.SaveChangesAsync();
            await Task.Delay(10);

            var post3 = new Post(3, "Third", "Body 3");
            await _context.Posts.AddAsync(post3);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            var posts = result.ToList();
            Assert.Equal("Third", posts[0].Title);
            Assert.Equal("Second", posts[1].Title);
            Assert.Equal("First", posts[2].Title);
        }

        [Fact]
        public async Task GetPostsByUserAsync_ShouldReturnEmptyList_WhenUserHasNoPosts()
        {
            // Act
            var result = await _repository.GetPostsByUserAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetPostsByUserAsync_ShouldReturnOnlyUsersPosts()
        {
            // Arrange
            var post1 = new Post(1, "User 1 Post 1", "Body");
            var post2 = new Post(1, "User 1 Post 2", "Body");
            var post3 = new Post(2, "User 2 Post", "Body");
            await _context.Posts.AddRangeAsync(post1, post2, post3);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetPostsByUserAsync(1);

            // Assert
            var posts = result.ToList();
            Assert.Equal(2, posts.Count);
            Assert.All(posts, p => Assert.Equal(1, p.AuthorUserId));
        }

        [Fact]
        public async Task GetPostsByUserAsync_ShouldReturnPostsOrderedByCreatedAtDescending()
        {
            // Arrange
            var post1 = new Post(1, "First", "Body");
            await _context.Posts.AddAsync(post1);
            await _context.SaveChangesAsync();
            await Task.Delay(10);

            var post2 = new Post(1, "Second", "Body");
            await _context.Posts.AddAsync(post2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetPostsByUserAsync(1);

            // Assert
            var posts = result.ToList();
            Assert.Equal("Second", posts[0].Title);
            Assert.Equal("First", posts[1].Title);
        }

        [Fact]
        public async Task GetFeedAsync_ShouldReturnEmptyList_WhenUserFollowsNoOne()
        {
            // Act
            var result = await _repository.GetFeedAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetFeedAsync_ShouldReturnOnlyPostsFromFollowedUsers()
        {
            // Arrange
            // User 1 follows User 2 and User 3
            var follow1 = new Follow(1, 2);
            var follow2 = new Follow(1, 3);
            await _context.Follows.AddRangeAsync(follow1, follow2);

            // Posts from different users
            var post1 = new Post(2, "From User 2", "Body");
            var post2 = new Post(3, "From User 3", "Body");
            var post3 = new Post(4, "From User 4", "Body"); // Not followed
            await _context.Posts.AddRangeAsync(post1, post2, post3);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetFeedAsync(1);

            // Assert
            var posts = result.ToList();
            Assert.Equal(2, posts.Count);
            Assert.Contains(posts, p => p.AuthorUserId == 2);
            Assert.Contains(posts, p => p.AuthorUserId == 3);
            Assert.DoesNotContain(posts, p => p.AuthorUserId == 4);
        }

        [Fact]
        public async Task GetFeedAsync_ShouldReturnPostsOrderedByCreatedAtDescending()
        {
            // Arrange
            var follow = new Follow(1, 2);
            await _context.Follows.AddAsync(follow);

            var post1 = new Post(2, "First", "Body");
            await _context.Posts.AddAsync(post1);
            await _context.SaveChangesAsync();
            await Task.Delay(10);

            var post2 = new Post(2, "Second", "Body");
            await _context.Posts.AddAsync(post2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetFeedAsync(1);

            // Assert
            var posts = result.ToList();
            Assert.Equal("Second", posts[0].Title);
            Assert.Equal("First", posts[1].Title);
        }

        [Fact]
        public async Task GetFeedAsync_ShouldNotIncludeUsersOwnPosts_UnlessTheyFollowThemselves()
        {
            // Arrange
            var follow = new Follow(1, 2);
            await _context.Follows.AddAsync(follow);

            var post1 = new Post(1, "Own Post", "Body");
            var post2 = new Post(2, "Followed User Post", "Body");
            await _context.Posts.AddRangeAsync(post1, post2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetFeedAsync(1);

            // Assert
            var posts = result.ToList();
            Assert.Single(posts);
            Assert.Equal(2, posts[0].AuthorUserId);
        }

        [Fact]
        public async Task AddAsync_ShouldAddPostToDatabase()
        {
            // Arrange
            var post = new Post(1, "Test Title", "Test Body");

            // Act
            await _repository.AddAsync(post);

            // Assert
            var savedPost = await _context.Posts.FirstOrDefaultAsync();
            Assert.NotNull(savedPost);
            Assert.Equal("Test Title", savedPost.Title);
            Assert.Equal("Test Body", savedPost.Body);
        }

        [Fact]
        public async Task AddAsync_ShouldSaveChangesToDatabase()
        {
            // Arrange
            var post = new Post(1, "Test Title", "Test Body");

            // Act
            await _repository.AddAsync(post);

            // Assert
            var count = await _context.Posts.CountAsync();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdatePostInDatabase()
        {
            // Arrange
            var post = new Post(1, "Original Title", "Original Body");
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            // Act
            post.Update("Updated Title", "Updated Body");
            await _repository.UpdateAsync(post);

            // Assert
            var updatedPost = await _context.Posts.FindAsync(post.Id);
            Assert.NotNull(updatedPost);
            Assert.Equal("Updated Title", updatedPost.Title);
            Assert.Equal("Updated Body", updatedPost.Body);
        }

        [Fact]
        public async Task UpdateAsync_ShouldSaveChangesToDatabase()
        {
            // Arrange
            var post = new Post(1, "Original Title", "Original Body");
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            // Act
            post.Update("Updated Title", "Updated Body");
            await _repository.UpdateAsync(post);

            // Assert
            // Detach and re-fetch to ensure changes are persisted
            _context.Entry(post).State = EntityState.Detached;
            var updatedPost = await _context.Posts.FindAsync(post.Id);
            Assert.Equal("Updated Title", updatedPost.Title);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemovePostFromDatabase()
        {
            // Arrange
            var post = new Post(1, "Test Title", "Test Body");
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(post);

            // Assert
            var count = await _context.Posts.CountAsync();
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task DeleteAsync_ShouldOnlyRemoveSpecificPost()
        {
            // Arrange
            var post1 = new Post(1, "Post 1", "Body 1");
            var post2 = new Post(1, "Post 2", "Body 2");
            await _context.Posts.AddRangeAsync(post1, post2);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(post1);

            // Assert
            var remainingPosts = await _context.Posts.ToListAsync();
            Assert.Single(remainingPosts);
            Assert.Equal("Post 2", remainingPosts[0].Title);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}