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

        // === PODSTAWOWE INFORMACJE ===
        public TransportType TransportType { get; set; }
        public IncidentType IncidentType { get; set; }
        public string LineNumber { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // === LOKALIZACJA ===
        public double Latitude { get; set; } = 50.0676;
        public double Longitude { get; set; } = 19.9864;
        public string LocationName { get; set; } = "Tauron Arena Kraków";

        // === CZAS ===
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // === GŁOSY SPOŁECZNOŚCI ===
        public int ConfirmationsCount { get; set; } = 0;
        public int RejectionsCount { get; set; } = 0;

        // === POWIĄZANIA ===
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int? RideId { get; set; }     // raport dotyczy konkretnego przejazdu
        public Ride? Ride { get; set; }

        public List<Verification> Verifications { get; set; } = new();

        // === LOGIKA ===
        public void UpdateActiveStatus()
        {
            // automatyczne wygaszanie po zakończeniu kursu
            if (Ride != null && Ride.ScheduledArrival <= DateTime.UtcNow)
            {
                IsActive = false;
                return;
            }

            // dezaktywacja po zbyt wielu negacjach
            IsActive = (ConfirmationsCount - RejectionsCount) >= -2;
        }
    }
}
