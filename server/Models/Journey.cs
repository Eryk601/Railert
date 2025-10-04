using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class Journey
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public bool HasWarning { get; set; } = false;
        public string? SuggestedAlternative { get; set; }

        public List<JourneySegment> Segments { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
