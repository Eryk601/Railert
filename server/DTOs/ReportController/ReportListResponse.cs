namespace server.DTOs
{
    public class ReportListResponse
    {
        public int Id { get; set; }
        public string TransportTypeName { get; set; } = string.Empty;
        public string LineNumber { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int ConfirmationsCount { get; set; }
        public string UserDisplayName { get; set; } = string.Empty;
        public DateTime? ScheduledArrival { get; set; }  // np. żeby pokazać na mapie, że kurs trwa
    }
}
