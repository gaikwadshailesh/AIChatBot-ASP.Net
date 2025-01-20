using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using AIChatbot.Data;
using AIChatbot.Models;
using AIChatbot.Services;
using AIChatbot.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Basic services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database configuration
builder.Services.AddDbContext<ChatDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// SignalR
builder.Services.AddSignalR();

// Configure OpenAI service
builder.Services.AddScoped<IAiService, OpenAiService>();
builder.Services.Configure<ChatSettings>(
    builder.Configuration.GetSection("ChatSettings"));

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithOrigins("http://localhost:5173"));
});

var app = builder.Build();

// Middleware pipeline
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("CorsPolicy");
app.UseAuthorization();

// Map endpoints
app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.Run();
