namespace UserService.Models.Responses
{
    public class LoginResponse
    {
        public string? Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
