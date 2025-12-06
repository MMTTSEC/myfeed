using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFeed.Application.Interfaces;
using MyFeed.Application.Services;
using MyFeed.Api.Extensions;

namespace MyFeed.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DirectMessagesController : ControllerBase
{
    private readonly IDMService _dmService;

    public DirectMessagesController(IDMService dmService)
    {
        _dmService = dmService;
    }

    [HttpPost]
    public async Task<IActionResult> SendDM([FromBody] SendDMRequest request)
    {
        try
        {
            var senderId = HttpContext.GetCurrentUserIdRequired();
            await _dmService.SendDMAsync(senderId, request.ReceiverId, request.Message);
            return Ok(new { message = "DM sent successfully" });
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
            return StatusCode(500, "An error occurred while sending the message.");
        }
    }
}

public class SendDMRequest
{
    public int ReceiverId { get; set; }
    public string Message { get; set; } = string.Empty;
}