using UserService.Data.Entities;

namespace UserService.Models.Responses
{
    public class RegistrationResponse
    {
        public User? User { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
