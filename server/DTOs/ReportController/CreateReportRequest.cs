using server.Models;

namespace server.DTOs
{
    public class CreateReportRequest
    {
        public TransportType TransportType { get; set; }
        public IncidentType IncidentType { get; set; }
        public string LineNumber { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
