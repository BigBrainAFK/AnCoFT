namespace AnCoFT.Database
{
    using AnCoFT.Database.Models;
    using Microsoft.EntityFrameworkCore;

    public class DatabaseContext : DbContext
    {
        public DbSet<Account> Account { get; set; }

        public DbSet<Challenge> Challenge { get; set; }

        public DbSet<ChallengeProgress> ChallengeProgress { get; set; }

        public DbSet<Tutorial> Tutorial { get; set; }

        public DbSet<TutorialProgress> TutorialProgress { get; set; }

        public DbSet<Character> Character { get; set; }

        public DbSet<GameServer> GameServer { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"Host=localhost;Port=5439;Database=AnCoFT;Username=postgres;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TutorialProgress>().HasKey(t => new { t.CharacterId, t.TutorialId });
            modelBuilder.Entity<ChallengeProgress>().HasKey(c => new { c.CharacterId, c.ChallengeId });
        }
    }
}