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

            modelBuilder.Entity<User>()
                .HasMany(u => u.Verifications)
                .WithOne(v => v.User)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // === Report ===
            modelBuilder.Entity<Report>()
                .HasIndex(r => new { r.TransportType, r.LineNumber });
            // pozwala szybciej wyszukiwać raporty po typie transportu i linii

            modelBuilder.Entity<Report>()
                .HasMany(r => r.Verifications)
                .WithOne(v => v.Report)
                .HasForeignKey(v => v.ReportId)
                .OnDelete(DeleteBehavior.Cascade);

            // === Verification ===
            modelBuilder.Entity<Verification>()
                .HasIndex(v => new { v.UserId, v.ReportId })
                .IsUnique();
            // jeden użytkownik może potwierdzić/odrzucić dane zgłoszenie tylko raz
        }
    }
}
