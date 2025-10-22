namespace UserService.Models
{
    public class AuthResponseDto
    {
        public string? Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
