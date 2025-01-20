using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AIChatbot.Data;
using AIChatbot.Models;
using AIChatbot.Services;
using AIChatbot.Hubs;

namespace AIChatbot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ChatDbContext _context;
        private readonly IAiService _aiService;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(
            ChatDbContext context,
            IAiService aiService,
            IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _aiService = aiService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public IActionResult Test()
        {
            return Ok("Chat Controller is working!");
        }

        [HttpPost]
        public async Task<ActionResult<ChatMessage>> SendMessage(ChatMessage message)
        {
            message.Timestamp = DateTime.UtcNow;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            // Get AI response
            var aiResponse = await _aiService.GetAiResponseAsync(
                message.Content, 
                message.ConversationMode);

            var aiMessage = new ChatMessage
            {
                Content = aiResponse,
                IsAiResponse = true,
                Timestamp = DateTime.UtcNow,
                UserId = "AI",
                ConversationMode = message.ConversationMode
            };

            _context.Messages.Add(aiMessage);
            await _context.SaveChangesAsync();

            // Broadcast the AI response to all clients
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", aiMessage);

            return Ok(new { userMessage = message, aiMessage });
        }

        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<ChatMessage>>> GetHistory()
        {
            return await _context.Messages
                .OrderByDescending(m => m.Timestamp)
                .Take(50)
                .ToListAsync();
        }
    }
} 