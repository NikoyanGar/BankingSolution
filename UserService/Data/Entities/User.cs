namespace UserService.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string ClientId { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Password { get; set; }
        public required string Roles { get; set; }
    }
}
