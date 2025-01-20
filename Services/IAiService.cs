namespace AIChatbot.Services;

public interface IAiService
{
    Task<string> GetAiResponseAsync(string message, string conversationMode);
} 