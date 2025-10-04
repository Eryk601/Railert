using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // === DbSety (tabele) ===
        public DbSet<User> Users { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Verification> Verifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // === User ===
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Reports)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // === Report ===
            modelBuilder.Entity<Report>()
                .HasIndex(r => new { r.TransportType, r.LineNumber });

            // === Verification ===
            modelBuilder.Entity<Verification>()
                .HasIndex(v => new { v.UserId, v.ReportId })
                .IsUnique(); // użytkownik może głosować tylko raz na dane zgłoszenie

            modelBuilder.Entity<Verification>()
                .HasOne(v => v.Report)
                .WithMany()
                .HasForeignKey(v => v.ReportId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
