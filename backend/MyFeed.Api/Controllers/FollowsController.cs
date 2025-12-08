using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFeed.Application.Interfaces;
using MyFeed.Application.Services;
using MyFeed.Api.Extensions;

namespace MyFeed.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FollowsController : ControllerBase
{
    private readonly IFollowService _followService;
    private readonly IUserService _userService;

    public FollowsController(IFollowService followService, IUserService userService)
    {
        _followService = followService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> FollowUser([FromBody] FollowUserRequest request)
    {
        try
        {
            var followerId = HttpContext.GetCurrentUserIdRequired();
            await _followService.FollowUserAsync(followerId, request.FolloweeId);
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
    public async Task<IActionResult> UnfollowUser([FromQuery] int followeeId)
    {
        try
        {
            var followerId = HttpContext.GetCurrentUserIdRequired();
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

    [HttpGet("following")]
    public async Task<IActionResult> GetFollowing()
    {
        try
        {
            var userId = HttpContext.GetCurrentUserIdRequired();
            var followeeIds = await _followService.GetFollowingAsync(userId);
            
            // Get user info for each followee ID
            var followees = new List<object>();
            foreach (var followeeId in followeeIds)
            {
                var user = await _userService.GetUserByIdAsync(followeeId);
                if (user != null)
                {
                    followees.Add(new
                    {
                        id = user.Id,
                        username = user.Username
                    });
                }
            }
            
            return Ok(followees);
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

    [HttpGet("check/{followeeId}")]
    public async Task<IActionResult> CheckIfFollowing(int followeeId)
    {
        try
        {
            var followerId = HttpContext.GetCurrentUserIdRequired();
            var isFollowing = await _followService.IsFollowingAsync(followerId, followeeId);
            return Ok(new { followeeId, isFollowing });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while checking follow status.");
        }
    }
}

public class FollowUserRequest
{
    public int FolloweeId { get; set; }
}