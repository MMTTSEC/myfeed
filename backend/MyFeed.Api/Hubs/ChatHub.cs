using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MyFeed.Api.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private const string MessageReceivedEvent = "messageReceived";

    public override async Task OnConnectedAsync()
    {
        var userId = GetCurrentUserId();
        // Join a group named after the user ID so we can target individuals
        await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetCurrentUserIdOrDefault();
        if (userId != null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(int receiverId, string content)
    {
        if (receiverId <= 0)
            throw new HubException("Receiver is required.");

        if (string.IsNullOrWhiteSpace(content))
            throw new HubException("Message content cannot be empty.");

        var senderId = GetCurrentUserId();
        var payload = new
        {
            id = Guid.NewGuid().ToString(),
            senderId,
            receiverId,
            content,
            createdAt = DateTime.UtcNow
        };

        // Broadcast to both sender and receiver groups so both sides stay in sync
        await Clients.Groups(senderId, receiverId.ToString()).SendAsync(MessageReceivedEvent, payload);
    }

    private string GetCurrentUserId()
    {
        var id = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(id))
            throw new HubException("Unauthorized: missing user id.");
        return id;
    }

    private string? GetCurrentUserIdOrDefault()
    {
        return Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}

