namespace AIChatbot.Data;

using Microsoft.EntityFrameworkCore;
using AIChatbot.Models;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options)
        : base(options)
    {
    }

    public DbSet<ChatMessage> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatMessage>()
            .HasIndex(m => m.Timestamp);
    }
} 