using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TurboTennisApp.Components.Backend.Models;

namespace TurboTennisApp.Components.Backend;

public partial class TournamentContext : DbContext
{
    public TournamentContext()
    {
    }

    public TournamentContext(DbContextOptions<TournamentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameStatus> GameStatuses { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Phase> Phases { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<PlayerGame> PlayerGames { get; set; }

    public virtual DbSet<PlayerSet> PlayerSets { get; set; }

    public virtual DbSet<Set> Sets { get; set; }

    public virtual DbSet<Tournament> Tournaments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=D:\\dev\\turbo-tennis\\tournamentV2.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.ToTable("Game");

            entity.Property(e => e.GroupId).HasColumnName("Group_id");
            entity.Property(e => e.PhaseId).HasColumnName("Phase_id");
            entity.Property(e => e.StatusId).HasColumnName("Status_id");
            entity.Property(e => e.TournamentId).HasColumnName("Tournament_id");

            entity.HasOne(d => d.Group).WithMany(p => p.Games).HasForeignKey(d => d.GroupId);

            entity.HasOne(d => d.Phase).WithMany(p => p.Games).HasForeignKey(d => d.PhaseId);

            entity.HasOne(d => d.Status).WithMany(p => p.Games).HasForeignKey(d => d.StatusId);

            entity.HasOne(d => d.Tournament).WithMany(p => p.Games).HasForeignKey(d => d.TournamentId);
        });

        modelBuilder.Entity<GameStatus>(entity =>
        {
            entity.ToTable("GameStatus");

            entity.Property(e => e.Id).HasColumnName("id");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.ToTable("Group");

            entity.Property(e => e.TournamentId).HasColumnName("Tournament_id");

            entity.HasOne(d => d.Tournament).WithMany(p => p.Groups).HasForeignKey(d => d.TournamentId);
        });

        modelBuilder.Entity<Phase>(entity =>
        {
            entity.ToTable("Phase");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.ToTable("Player");

            entity.HasMany(d => d.Groups).WithMany(p => p.Players)
                .UsingEntity<Dictionary<string, object>>(
                    "PlayerGroup",
                    r => r.HasOne<Group>().WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    l => l.HasOne<Player>().WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("PlayerId", "GroupId");
                        j.ToTable("PlayerGroup");
                        j.IndexerProperty<int>("PlayerId").HasColumnName("Player_id");
                        j.IndexerProperty<int>("GroupId").HasColumnName("Group_id");
                    });

            entity.HasMany(d => d.Tournaments).WithMany(p => p.Players)
                .UsingEntity<Dictionary<string, object>>(
                    "PlayerTournament",
                    r => r.HasOne<Tournament>().WithMany()
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    l => l.HasOne<Player>().WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("PlayerId", "TournamentId");
                        j.ToTable("PlayerTournament");
                        j.IndexerProperty<int>("PlayerId").HasColumnName("Player_id");
                        j.IndexerProperty<int>("TournamentId").HasColumnName("Tournament_id");
                    });
        });

        modelBuilder.Entity<PlayerGame>(entity =>
        {
            entity.ToTable("PlayerGame");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.PlayerId).HasColumnName("player_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");

            entity.HasOne(d => d.Game).WithMany(p => p.PlayerGames).HasForeignKey(d => d.GameId);

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerGames).HasForeignKey(d => d.PlayerId);

            entity.HasOne(d => d.Status).WithMany(p => p.PlayerGames).HasForeignKey(d => d.StatusId);
        });

        modelBuilder.Entity<PlayerSet>(entity =>
        {
            entity.HasKey(e => new { e.PlayerId, e.SetId });

            entity.ToTable("PlayerSet");

            entity.Property(e => e.PlayerId).HasColumnName("Player_id");
            entity.Property(e => e.SetId).HasColumnName("Set_id");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerSets)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Set).WithMany(p => p.PlayerSets)
                .HasForeignKey(d => d.SetId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Set>(entity =>
        {
            entity.ToTable("Set");

            entity.Property(e => e.GameId).HasColumnName("Game_id");

            entity.HasOne(d => d.Game).WithMany(p => p.Sets).HasForeignKey(d => d.GameId);
        });

        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.ToTable("Tournament");

            entity.Property(e => e.PhaseId).HasColumnName("Phase_id");

            entity.HasOne(d => d.Phase).WithMany(p => p.Tournaments).HasForeignKey(d => d.PhaseId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
