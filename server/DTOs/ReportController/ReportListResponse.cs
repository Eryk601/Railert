namespace server.DTOs
{
    public class ReportListResponse
    {
        public int Id { get; set; }
        public string TransportTypeName { get; set; } = "";
        public string LineNumber { get; set; } = "";
        public string? Title { get; set; }
        public string? Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? LocationName { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }
        public int ConfirmationsCount { get; set; }
        public int RejectionsCount { get; set; }

        public string UserDisplayName { get; set; } = "Użytkownik anonimowy";
        public DateTime? ScheduledArrival { get; set; }
    }
}
