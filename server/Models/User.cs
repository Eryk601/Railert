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

        // === PODSTAWOWE DANE LOGOWANIA ===
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }

        // === DANE PROFILOWE ===
        public string? DisplayName { get; set; }
        public string? City { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // === SYSTEM ROLI I WERYFIKACJI ===
        public UserRole Role { get; set; } = UserRole.Passenger;
        public bool IsEmailConfirmed { get; set; } = false;
        public string? EmailConfirmationToken { get; set; }

        // === SYSTEM NAGRÓD I AKTYWNOŚCI ===
        public int ReputationPoints { get; set; } = 0;
        public int ReportsCount { get; set; } = 0;

        // === RESETOWANIE HASŁA ===
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpires { get; set; }

        // === RELACJE ===
        public List<Report> Reports { get; set; } = new();     // lista zgłoszeń użytkownika
        public List<Verification> Verifications { get; set; } = new(); // np. głosy potwierdzające wiarygodność zgłoszeń
    }
}
