namespace AIChatbot.Models;

public class ChatMessage
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Content { get; set; }
    public bool IsAiResponse { get; set; }
    public DateTime Timestamp { get; set; }
    public string ConversationMode { get; set; }
} 