using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public enum UserRole
    {
        Passenger,
        Moderator,
        Admin
    }

    public class User
    {
        public int Id { get; set; }

        // === LOGOWANIE ===
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // === PROFIL ===
        public string? DisplayName { get; set; }
        public string? City { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // === ROLE I WERYFIKACJA ===
        public UserRole Role { get; set; } = UserRole.Passenger;
        public bool IsEmailConfirmed { get; set; } = false;
        public string? EmailConfirmationToken { get; set; }

        // === REPUTACJA ===
        public int ReputationPoints { get; set; } = 0;
        public int ReportsCount { get; set; } = 0;

        // === RESET HASŁA ===
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpires { get; set; }

        // === RELACJE ===
        public List<Report> Reports { get; set; } = new();
        public List<Verification> Verifications { get; set; } = new();
    }
}
