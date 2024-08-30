using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using TurboTennisApp.Components.Backend.Models;

namespace TurboTennisApp.Components.Backend
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base() { }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
        {
        }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Models.MatchType> MatchTypes { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<PlayerScore> PlayerScores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=D:\dev\turbo-tennis\tournament.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tournament - Group Beziehung
            modelBuilder.Entity<Group>()
                .HasOne(g => g.Tournament)
                .WithMany(t => t.Groups)
                .HasForeignKey(g => g.TournamentId);  

            // Tournament - Match Beziehung
            modelBuilder.Entity<Match>()
                .HasOne(m => m.Tournament)
                .WithMany(t => t.Matches)
                .HasForeignKey(m => m.TournamentId);  

            // Match - MatchType Beziehung
            modelBuilder.Entity<Match>()
                .HasOne(m => m.Type)
                .WithMany(mt => mt.Matches)
                .HasForeignKey(m => m.MatchTypeId);  

            // Match - Set Beziehung
            modelBuilder.Entity<Set>()
                .HasOne(s => s.Match)
                .WithMany(m => m.Sets)
                .HasForeignKey(s => s.MatchId);  

            // Set - PlayerScore Beziehung
            modelBuilder.Entity<PlayerScore>()
                .HasOne(ps => ps.Set)
                .WithMany(s => s.PlayerScores)
                .HasForeignKey(ps => ps.SetId);  

            modelBuilder.Entity<PlayerScore>()
                .HasOne(ps => ps.Player)
                .WithMany()
                .HasForeignKey(ps => ps.PlayerId);  

            // Optional: Group - Player Beziehung (Falls nicht über eine separate Tabelle gehandhabt)
            modelBuilder.Entity<Player>()
                .HasMany(p => p.Groups)
                .WithMany(g => g.Players)
                .UsingEntity<Dictionary<string, object>>(
                    "GroupPlayer",
                    j => j
                        .HasOne<Group>()
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j
                        .HasOne<Player>()
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Restrict));

            // Tournament - Player Beziehung (Many-to-Many)
            modelBuilder.Entity<Player>()
                .HasMany(p => p.Tournaments)
                .WithMany(t => t.Players)
                .UsingEntity<Dictionary<string, object>>(
                    "TournamentPlayer",
                    j => j
                        .HasOne<Tournament>()
                        .WithMany()
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j
                        .HasOne<Player>()
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Restrict));
        }

    }
}
