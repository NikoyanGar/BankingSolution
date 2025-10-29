namespace UserService.Models.Responses
{
    public class TokenResponse
    {
        public string? Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
