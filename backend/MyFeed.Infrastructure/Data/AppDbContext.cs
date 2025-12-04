using Microsoft.EntityFrameworkCore;
using MyFeed.Domain.Entities;

namespace MyFeed.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<DM> DirectMessages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Local SQLite file database for development
                optionsBuilder.UseSqlite("Data Source=Database/myfeed.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.HasIndex(u => u.Username).IsUnique();
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Title).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Body).IsRequired();
                entity.Property(p => p.AuthorUserId).IsRequired();
            });

            modelBuilder.Entity<Follow>(entity =>
            {
                entity.HasKey(f => f.Id);
                entity.Property(f => f.FollowerId).IsRequired();
                entity.Property(f => f.FolloweeId).IsRequired();
                entity.HasIndex(f => new { f.FollowerId, f.FolloweeId }).IsUnique();
            });

            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasKey(l => l.Id);
                entity.Property(l => l.UserId).IsRequired();
                entity.Property(l => l.PostId).IsRequired();
                entity.HasIndex(l => new { l.UserId, l.PostId }).IsUnique();
            });

            modelBuilder.Entity<DM>(entity =>
            {
                entity.HasKey(dm => dm.Id);
                entity.Property(dm => dm.SenderUserId).IsRequired();
                entity.Property(dm => dm.ReceiverUserId).IsRequired();
                entity.Property(dm => dm.Message).IsRequired().HasMaxLength(1000);
            });
        }
    }
}
