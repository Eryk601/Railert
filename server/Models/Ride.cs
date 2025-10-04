using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class Ride
    {
        public int Id { get; set; }

        [Required]
        public TransportType TransportType { get; set; }

        [Required]
        public string LineNumber { get; set; } = string.Empty;

        [Required]
        public string StartStation { get; set; } = string.Empty;

        [Required]
        public string EndStation { get; set; } = string.Empty;

        [Range(0, 10000)]
        public double DistanceKm { get; set; }

        [Required]
        public DateTime ScheduledDeparture { get; set; }

        [Required]
        public DateTime ScheduledArrival { get; set; }

        public int DelayMinutes { get; set; } = 0;
        public bool IsCancelled { get; set; } = false;

        // Zgłoszenia powiązane z przejazdem
        public List<Report> Reports { get; set; } = new();

        // Pomocnicza metoda sprawdzająca, czy przejazd się zakończył
        public bool IsCompleted => ScheduledArrival <= DateTime.UtcNow;
    }
}
