using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class JourneySegment
    {
        public int Id { get; set; }

        [Required]
        public int JourneyId { get; set; }
        public Journey Journey { get; set; } = null!;

        [Required]
        public int RideId { get; set; }
        public Ride Ride { get; set; } = null!;

        public int Sequence { get; set; } // kolejność segmentów (1,2,3)
    }
}
