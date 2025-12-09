using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFeed.Application.Interfaces;
using MyFeed.Api.Extensions;
using System;
using System.Collections.Generic;

namespace MyFeed.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IUserService _userService;

    public PostsController(IPostService postService, IUserService userService)
    {
        _postService = postService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
    {
        try
        {
            var userId = HttpContext.GetCurrentUserIdRequired();
            var title = string.IsNullOrWhiteSpace(request.Title) ? "test" : request.Title;
            await _postService.CreatePostAsync(userId, title, request.Body);
            return Created(string.Empty, null);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while creating the post.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(int id, [FromBody] UpdatePostRequest request)
    {
        try
        {
            var userId = HttpContext.GetCurrentUserIdRequired();
            await _postService.UpdatePostAsync(id, userId, request.Title, request.Body);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while updating the post.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        try
        {
            var userId = HttpContext.GetCurrentUserIdRequired();
            await _postService.DeletePostAsync(id, userId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while deleting the post.");
        }
    }

    // Specific routes must come before parameterized routes
    [HttpGet("all")]
    public async Task<IActionResult> GetAllPosts()
    {
        try
        {
            var posts = await _postService.GetAllPostsAsync();
            var postDtos = new List<object>();

            foreach (var post in posts)
            {
                var author = await _userService.GetUserByIdAsync(post.AuthorUserId);
                var authorUsername = author?.Username ?? "Unknown";

                postDtos.Add(new
                {
                    id = post.Id,
                    authorId = post.AuthorUserId,
                    author = authorUsername,
                    title = post.Title,
                    body = post.Body,
                    createdAt = post.CreatedAt
                });
            }

            return Ok(postDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while retrieving posts: {ex.Message}");
        }
    }

    [HttpGet("feed")]
    public async Task<IActionResult> GetFeed()
    {
        try
        {
            var userId = HttpContext.GetCurrentUserIdRequired();
            var posts = await _postService.GetFeedAsync(userId);
            var postDtos = new List<object>();

            foreach (var post in posts)
            {
                var author = await _userService.GetUserByIdAsync(post.AuthorUserId);
                var authorUsername = author?.Username ?? "Unknown";

                postDtos.Add(new
                {
                    id = post.Id,
                    authorId = post.AuthorUserId,
                    author = authorUsername,
                    title = post.Title,
                    body = post.Body,
                    createdAt = post.CreatedAt
                });
            }

            return Ok(postDtos);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving the feed.");
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetPostsByUser(int userId)
    {
        try
        {
            var posts = await _postService.GetPostsByUserAsync(userId);
            var postDtos = new List<object>();

            foreach (var post in posts)
            {
                var author = await _userService.GetUserByIdAsync(post.AuthorUserId);
                var authorUsername = author?.Username ?? "Unknown";

                postDtos.Add(new
                {
                    id = post.Id,
                    authorId = post.AuthorUserId,
                    author = authorUsername,
                    title = post.Title,
                    body = post.Body,
                    createdAt = post.CreatedAt
                });
            }

            return Ok(postDtos);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving posts.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById(int id)
    {
        try
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var author = await _userService.GetUserByIdAsync(post.AuthorUserId);
            var authorUsername = author?.Username ?? "Unknown";

            return Ok(new
            {
                id = post.Id,
                authorId = post.AuthorUserId,
                author = authorUsername,
                title = post.Title,
                body = post.Body,
                createdAt = post.CreatedAt
            });
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving the post.");
        }
    }
}

public class CreatePostRequest
{
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}

public class UpdatePostRequest
{
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
