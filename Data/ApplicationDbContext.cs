using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Level> Levels { get; set; } = default!;
        public DbSet<UserTask> UserTasks { get; set; } = default!;
        public DbSet<TaskAssignment> TaskAssignments { get; set; } = default!;
        public DbSet<DifficultyVote> DifficultyVotes { get; set; } = default!;
        public DbSet<CompletionFeedback> CompletionFeedbacks { get; set; } = default!;
        public DbSet<UserXpHistory> UserXpHistories { get; set; } = default!;
        public DbSet<Badge> Badges { get; set; } = default!;
        public DbSet<UserBadge> UserBadges { get; set; } = default!;
        public DbSet<LeaderboardEntry> LeaderboardEntries { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Identity tables

            // --- Levels ---
            modelBuilder.Entity<Level>(entity =>
            {
                entity.ToTable("Levels");
                entity.HasKey(l => l.LevelId);
                entity.Property(l => l.LevelName).IsRequired().HasMaxLength(100);
                entity.Property(l => l.MinXp).IsRequired();
                entity.Property(l => l.MaxXp).IsRequired();
            });

            // --- ApplicationUser ---
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("AspNetUsers"); // Identity table
                entity.HasOne(u => u.Level)
                      .WithMany(l => l.Users)
                      .HasForeignKey(u => u.LevelId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(u => u.TaskAssignments)
                      .WithOne(t => t.User)
                      .HasForeignKey(t => t.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.XpHistories)
                      .WithOne(x => x.User)
                      .HasForeignKey(x => x.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.Badges)
                      .WithOne(ub => ub.User)
                      .HasForeignKey(ub => ub.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.LeaderboardEntries)
                      .WithOne(l => l.User)
                      .HasForeignKey(l => l.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- UserTask ---
            modelBuilder.Entity<UserTask>(entity =>
            {
                entity.ToTable("UserTasks");
                entity.HasKey(t => t.TaskId);
                entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
                entity.Property(t => t.Description).HasMaxLength(1000);
                entity.Property(t => t.Priority).IsRequired();
                entity.Property(t => t.IsActive).IsRequired();

                entity.HasMany(t => t.Assignments)
                      .WithOne(a => a.Task)
                      .HasForeignKey(a => a.TaskId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- TaskAssignment ---
            modelBuilder.Entity<TaskAssignment>(entity =>
            {
                entity.ToTable("TaskAssignments");
                entity.HasKey(a => a.TaskAssignmentId);

                entity.HasOne(a => a.Task)
                      .WithMany(t => t.Assignments)
                      .HasForeignKey(a => a.TaskId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.User)
                      .WithMany(u => u.TaskAssignments)
                      .HasForeignKey(a => a.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(a => a.DifficultyVotes)
                      .WithOne(d => d.TaskAssignment)
                      .HasForeignKey(d => d.TaskAssignmentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(a => a.CompletionFeedbacks)
                      .WithOne(c => c.TaskAssignment)
                      .HasForeignKey(c => c.TaskAssignmentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- DifficultyVote ---
            modelBuilder.Entity<DifficultyVote>(entity =>
            {
                entity.ToTable("DifficultyVotes");
                entity.HasKey(d => d.DifficultyVoteId);

                entity.HasOne(d => d.TaskAssignment)
                      .WithMany(t => t.DifficultyVotes)
                      .HasForeignKey(d => d.TaskAssignmentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- CompletionFeedback ---
            modelBuilder.Entity<CompletionFeedback>(entity =>
            {
                entity.ToTable("CompletionFeedbacks");
                entity.HasKey(c => c.CompletionFeedbackId);

                entity.HasOne(c => c.TaskAssignment)
                      .WithMany(t => t.CompletionFeedbacks)
                      .HasForeignKey(c => c.TaskAssignmentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- UserXpHistory ---
            modelBuilder.Entity<UserXpHistory>(entity =>
            {
                entity.ToTable("UserXpHistories");
                entity.HasKey(u => u.UserXpHistoryId);

                entity.HasOne(u => u.User)
                      .WithMany(usr => usr.XpHistories)
                      .HasForeignKey(u => u.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- Badge ---
            modelBuilder.Entity<Badge>(entity =>
            {
                entity.ToTable("Badges");
                entity.HasKey(b => b.BadgeId);
                entity.Property(b => b.Name).IsRequired().HasMaxLength(100);
                entity.Property(b => b.Description).HasMaxLength(500);

                entity.HasMany(b => b.UserBadges)
                      .WithOne(ub => ub.Badge)
                      .HasForeignKey(ub => ub.BadgeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- UserBadge ---
            modelBuilder.Entity<UserBadge>(entity =>
            {
                entity.ToTable("UserBadges");
                entity.HasKey(ub => ub.UserBadgeId);

                entity.HasOne(ub => ub.User)
                      .WithMany(u => u.Badges)
                      .HasForeignKey(ub => ub.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ub => ub.Badge)
                      .WithMany(b => b.UserBadges)
                      .HasForeignKey(ub => ub.BadgeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- LeaderboardEntry ---
            modelBuilder.Entity<LeaderboardEntry>(entity =>
            {
                entity.ToTable("LeaderboardEntries");
                entity.HasKey(l => l.LeaderboardEntryId);

                entity.HasOne(l => l.User)
                      .WithMany(u => u.LeaderboardEntries)
                      .HasForeignKey(l => l.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
