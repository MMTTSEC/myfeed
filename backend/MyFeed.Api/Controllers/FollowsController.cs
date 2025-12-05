using Microsoft.AspNetCore.Mvc;
using MyFeed.Application.Interfaces;
using MyFeed.Application.Services;

namespace MyFeed.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FollowsController : ControllerBase
{
    private readonly IFollowService _followService;

    public FollowsController(IFollowService followService)
    {
        _followService = followService;
    }

    [HttpPost]
    public async Task<IActionResult> FollowUser([FromBody] FollowUserRequest request)
    {
        try
        {
            await _followService.FollowUserAsync(request.FollowerId, request.FolloweeId);
            return Ok(new { message = "User followed successfully" });
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
            return StatusCode(500, "An error occurred while following the user.");
        }
    }

    [HttpDelete]
    public async Task<IActionResult> UnfollowUser([FromQuery] int followerId, [FromQuery] int followeeId)
    {
        try
        {
            await _followService.UnfollowUserAsync(followerId, followeeId);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while unfollowing the user.");
        }
    }

    [HttpGet("{userId}/following")]
    public async Task<IActionResult> GetFollowing(int userId)
    {
        try
        {
            var followeeIds = await _followService.GetFollowingAsync(userId);
            return Ok(followeeIds);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving following list.");
        }
    }
}

public class FollowUserRequest
{
    public int FollowerId { get; set; }
    public int FolloweeId { get; set; }
}