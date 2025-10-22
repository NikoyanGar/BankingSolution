namespace UserService.Models
{
    public class TokenPayload
    {
        public int Id { get; set; }
        public string? Role { get; set; }
        public DateTime Expiry { get; set; }
        public User? Username { get; set; }
    }
}
