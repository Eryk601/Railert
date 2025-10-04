using System.ComponentModel.DataAnnotations;
using server.Models;

namespace server.DTOs
{
    public class CreateReportRequest
    {
        [Required]
        public TransportType TransportType { get; set; }

        [Required]
        public IncidentType IncidentType { get; set; }

        [Required]
        [StringLength(20)]
        public string LineNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        // powiązanie z przejazdem
        public int? RideId { get; set; }

        // liczba minut opóźnienia (dotyczy tylko IncidentType.Delay)
        [Range(0, 300, ErrorMessage = "Opóźnienie musi być w zakresie 0–300 minut.")]
        public int? DelayMinutes { get; set; }
    }
}
