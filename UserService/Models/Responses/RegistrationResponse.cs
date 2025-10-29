namespace UserService.Models.Responses
{
    public class RegistrationResponse
    {
        public string? ClientId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Roles { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
