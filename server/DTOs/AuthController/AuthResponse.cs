namespace server.DTOs
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public string? Role { get; set; }
        public int ReputationPoints { get; set; }
    }
}
