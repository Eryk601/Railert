namespace server.DTOs
{
    public class AdminUserResponse
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Nip { get; set; }
        public int Discount { get; set; }
        public DateTime? LastActivity { get; set; }
        public int OrdersCount { get; set; }
    }
}
