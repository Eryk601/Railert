namespace server.Models
{
    public class Verification
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int ReportId { get; set; }
        public Report Report { get; set; } = null!;

        public bool IsConfirmed { get; set; } // true = potwierdzenie, false = odrzucenie
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
