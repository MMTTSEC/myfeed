using Microsoft.AspNetCore.Mvc;
using MyFeed.Application.Interfaces;
using MyFeed.Domain.Entities;

namespace MyFeed.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;

    public UsersController(IUserService userService, IJwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
    {
        try
        {
            await _userService.RegisterUserAsync(request.Username, request.Password);
            var createdUser = await _userService.GetUserByUsernameAsync(request.Username);
            if (createdUser == null)
            {
                return StatusCode(500, "User was created but could not be retrieved.");
            }
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Username and password are required.");
        }

        var user = await _userService.LoginAsync(request.Username, request.Password);
        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        // Generate JWT token
        var token = _jwtService.GenerateToken(user.Id, user.Username);

        // Return token along with user info
        return Ok(new { 
            token = token,
            id = user.Id, 
            username = user.Username,
            message = "Login successful" 
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetUserByUsername([FromQuery] string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return BadRequest("Username parameter is required.");
        }

        var user = await _userService.GetUserByUsernameAsync(username);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
}

public class RegisterUserRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}


