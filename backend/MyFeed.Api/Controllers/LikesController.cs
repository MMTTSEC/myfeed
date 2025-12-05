using Microsoft.AspNetCore.Mvc;
using MyFeed.Application.Services;
using MyFeed.Application.Interfaces;

namespace MyFeed.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LikesController : ControllerBase
{
    private readonly ILikeService _likeService;

    public LikesController(ILikeService likeService)
    {
        _likeService = likeService;
    }

    [HttpPost]
    public async Task<IActionResult> LikePost([FromBody] LikePostRequest request)
    {
        try
        {
            await _likeService.LikePostAsync(request.UserId, request.PostId);
            return Ok(new { message = "Post liked successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while liking the post.");
        }
    }

    [HttpDelete]
    public async Task<IActionResult> UnlikePost([FromQuery] int userId, [FromQuery] int postId)
    {
        try
        {
            await _likeService.UnlikePostAsync(userId, postId);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while unliking the post.");
        }
    }

    [HttpGet("{postId}/count")]
    public async Task<IActionResult> GetLikeCount(int postId)
    {
        try
        {
            var count = await _likeService.GetLikeCountAsync(postId);
            return Ok(new { postId, likeCount = count });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving like count.");
        }
    }
}

public class LikePostRequest
{
    public int UserId { get; set; }
    public int PostId { get; set; }
}