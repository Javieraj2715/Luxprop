using System.Data.Entity;
using AdvancedProgramming.Mvc.Models.Chat;

namespace AdvancedProgramming.Mvc.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("LuxpropConnection") { }

        public DbSet<ChatThread> ChatThreads { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}
