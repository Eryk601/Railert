using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public enum TransportType
    {
        Bus,
        Tram,
        Train,
        Metro,
        Other
    }

    public enum ReportStatus
    {
        Pending,
        Verified,
        Rejected,
        Resolved
    }

    public class Report
    {
        public int Id { get; set; }

        // === PODSTAWOWE INFORMACJE ===
        [Required]
        public TransportType TransportType { get; set; }
        [Required]
        public string LineNumber { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        // === LOKALIZACJA ===
        public double? Latitude { get; set; }     // współrzędne GPS
        public double? Longitude { get; set; }
        public string? LocationName { get; set; } // np. "Dworzec Główny", "ul. Piłsudskiego"

        // === INFORMACJE CZASOWE ===
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; } // kiedy zgłoszenie zakończono

        // === STATUS I WERYFIKACJA ===
        public ReportStatus Status { get; set; } = ReportStatus.Pending;
        public int ConfirmationsCount { get; set; } = 0;   // ile osób potwierdziło zgłoszenie
        public int RejectionsCount { get; set; } = 0;      // ile osób uznało je za nieprawdziwe

        // === POWIĄZANIA ===
        public int UserId { get; set; }
        public User User { get; set; } = null!;             // użytkownik, który dodał zgłoszenie

        public List<Verification> Verifications { get; set; } = new(); // głosy innych użytkowników

        // === DODATKOWE DANE ===
        public string? Source { get; set; } // np. "Manual", "API:ZTM", "API:PKP"
        public string? ImageUrl { get; set; } // zdjęcie / screen zgłoszenia
    }
}
