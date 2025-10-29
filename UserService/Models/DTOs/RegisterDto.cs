namespace UserService.Models.DTOs
{
    public class RegisterDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public required string Roles { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
