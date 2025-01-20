namespace AIChatbot.Services;

using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Options;
using AIChatbot.Models;

public class OpenAiService : IAiService
{
    private readonly HttpClient _httpClient;
    private readonly ChatSettings _settings;

    public OpenAiService(IOptions<ChatSettings> settings)
    {
        _settings = settings.Value;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", _settings.ApiKey);
    }

    public async Task<string> GetAiResponseAsync(string message, string conversationMode)
    {
        try
        {
            // For testing, return a simple response
            return $"AI Response to: {message}";
            
            // Uncomment this when you have your OpenAI API key
            /*
            var request = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = $"You are a {conversationMode} assistant." },
                    new { role = "user", content = message }
                }
            };

            var response = await _httpClient.PostAsync(
                _settings.ApiEndpoint,
                new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            
            // Parse the OpenAI response here
            return responseContent;
            */
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
} 