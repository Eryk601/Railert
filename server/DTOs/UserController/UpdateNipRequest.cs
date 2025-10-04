namespace server.DTOs
{
    public class UserListResponse
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public string? Role { get; set; }
        public string? City { get; set; }
        public int ReputationPoints { get; set; }
        public int ReportsCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}