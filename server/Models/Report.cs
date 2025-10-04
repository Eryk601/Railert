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

    public enum IncidentType
    {
        Delay,      // Opóźnienie
        Accident,   // Wypadek
        Breakdown,  // Awaria
        Blockage,   // Zablokowany przejazd
        Other
    }

    public class Report
    {
        public int Id { get; set; }

        public TransportType TransportType { get; set; }
        public IncidentType IncidentType { get; set; }
        public string LineNumber { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public double Latitude { get; set; } = 50.0676;
        public double Longitude { get; set; } = 19.9864;
        public string LocationName { get; set; } = "Tauron Arena Kraków";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int ConfirmationsCount { get; set; } = 0;
        public int RejectionsCount { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // 🔹 Automatyczna aktualizacja statusu
        public void UpdateActiveStatus()
        {
            IsActive = (ConfirmationsCount - RejectionsCount) >= -2;
        }
    }
}
