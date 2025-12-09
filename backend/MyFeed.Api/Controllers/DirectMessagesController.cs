using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFeed.Application.Interfaces;
using MyFeed.Api.Extensions;
using System;
using System.Collections.Generic;
using MyFeed.Api.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace MyFeed.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DirectMessagesController : ControllerBase
{
    private readonly IDMService _dmService;
    private readonly IUserService _userService;
    private readonly IHubContext<ChatHub> _hubContext;

    public DirectMessagesController(IDMService dmService, IUserService userService, IHubContext<ChatHub> hubContext)
    {
        _dmService = dmService;
        _userService = userService;
        _hubContext = hubContext;
    }

    [HttpPost]
    public async Task<IActionResult> SendDM([FromBody] SendDMRequest request)
    {
        try
        {
            var senderId = HttpContext.GetCurrentUserIdRequired();
            var dm = await _dmService.SendDMAsync(senderId, request.ReceiverId, request.Message);

            var sender = await _userService.GetUserByIdAsync(dm.SenderUserId);
            var receiver = await _userService.GetUserByIdAsync(dm.ReceiverUserId);

            var dto = new
            {
                id = dm.Id,
                senderId = dm.SenderUserId,
                sender = sender?.Username ?? "Unknown",
                receiverId = dm.ReceiverUserId,
                receiver = receiver?.Username ?? "Unknown",
                content = dm.Message,
                createdAt = dm.CreatedAt
            };

            await _hubContext.Clients.Groups(dm.SenderUserId.ToString(), dm.ReceiverUserId.ToString())
                .SendAsync("messageReceived", dto);

            return Ok(dto);
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

    [HttpGet("conversation/{otherUserId}")]
    public async Task<IActionResult> GetConversation(int otherUserId)
    {
        try
        {
            var userId = HttpContext.GetCurrentUserIdRequired();
            var messages = await _dmService.GetConversationAsync(userId, otherUserId);
            
            var messageDtos = new List<object>();
            foreach (var message in messages)
            {
                var sender = await _userService.GetUserByIdAsync(message.SenderUserId);
                var receiver = await _userService.GetUserByIdAsync(message.ReceiverUserId);
                
                messageDtos.Add(new
                {
                    id = message.Id,
                    senderId = message.SenderUserId,
                    sender = sender?.Username ?? "Unknown",
                    receiverId = message.ReceiverUserId,
                    receiver = receiver?.Username ?? "Unknown",
                    content = message.Message,
                    createdAt = message.CreatedAt
                });
            }
            
            return Ok(messageDtos);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while retrieving the conversation: {ex.Message}");
        }
    }
}

public class SendDMRequest
{
    public int ReceiverId { get; set; }
    public string Message { get; set; } = string.Empty;
}