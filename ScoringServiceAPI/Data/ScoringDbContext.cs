using Microsoft.EntityFrameworkCore;
using ScoringServiceAPI.Models;

namespace ScoringServiceAPI.Data
{
    public class ScoringDbContext: DbContext
    {
        public ScoringDbContext(DbContextOptions<ScoringDbContext> options): base(options) {}

        public DbSet<ClientScore> Scores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure table
            modelBuilder.Entity<ClientScore>().ToTable("ClientScores");

            base.OnModelCreating(modelBuilder);

            // Seed data
            modelBuilder.Entity<ClientScore>().HasData(
                new ClientScore { Id = 1, ClientId = "C001", Score = 750, UpdatedAt = DateTime.UtcNow },
                new ClientScore { Id = 2, ClientId = "C002", Score = 620, UpdatedAt = DateTime.UtcNow },
                new ClientScore { Id = 3, ClientId = "C003", Score = 450, UpdatedAt = DateTime.UtcNow }
            );
        }
    }
}
