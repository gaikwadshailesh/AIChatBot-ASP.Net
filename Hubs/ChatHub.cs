namespace AIChatbot.Hubs;

using Microsoft.AspNetCore.SignalR;
using AIChatbot.Models;

public class ChatHub : Hub
{
    public async Task SendMessage(ChatMessage message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
} 