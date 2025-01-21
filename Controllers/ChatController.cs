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
using Microsoft.Extensions.Logging;

namespace AIChatbot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ChatDbContext _context;
        private readonly IAiService _aiService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ILogger<ChatController> _logger;

        public ChatController(
            ChatDbContext context,
            IAiService aiService,
            IHubContext<ChatHub> hubContext,
            ILogger<ChatController> logger)
        {
            _context = context;
            _aiService = aiService;
            _hubContext = hubContext;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Test()
        {
            _logger.LogInformation("Chat controller test endpoint accessed");
            return Ok("Chat Controller is working!");
        }

        [HttpPost]
        public async Task<ActionResult<ChatMessage>> SendMessage(ChatMessage message)
        {
            try
            {
                if (string.IsNullOrEmpty(message.Content))
                {
                    _logger.LogWarning("Empty message content received");
                    return BadRequest("Message content cannot be empty");
                }

                _logger.LogInformation("Processing message from user: {UserId}", message.UserId);

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing chat message");
                return StatusCode(500, "An error occurred while processing your message");
            }
        }

        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<ChatMessage>>> GetHistory([FromQuery] int limit = 50)
        {
            try
            {
                _logger.LogInformation("Retrieving chat history. Limit: {limit}", limit);
                return await _context.Messages
                    .OrderByDescending(m => m.Timestamp)
                    .Take(Math.Min(limit, 100))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving chat history");
                return StatusCode(500, "An error occurred while retrieving chat history");
            }
        }
    }
} 