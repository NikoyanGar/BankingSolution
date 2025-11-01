using Microsoft.EntityFrameworkCore;
using ScoringServiceAPI.Data.Entities;

namespace ScoringServiceAPI.Data
{
    public class ScoringDbContext: DbContext
    {
        public ScoringDbContext(DbContextOptions<ScoringDbContext> options): base(options) {}

        public DbSet<ClientEntity> Scores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure table
            modelBuilder.Entity<ClientEntity>().ToTable("ClientScores");

            base.OnModelCreating(modelBuilder);

            // Seed data
            modelBuilder.Entity<ClientEntity>().HasData(
                new ClientEntity { Id = 1, ClientId = "C001", Score = 750, UpdatedAt = DateTime.UtcNow },
                new ClientEntity { Id = 2, ClientId = "C002", Score = 620, UpdatedAt = DateTime.UtcNow },
                new ClientEntity { Id = 3, ClientId = "C003", Score = 450, UpdatedAt = DateTime.UtcNow }
            );
        }
    }
}
