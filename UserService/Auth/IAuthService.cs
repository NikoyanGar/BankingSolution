using UserService.Models;

namespace UserService.Auth
{
    public interface IAuthService
    {
        public Task<string> ValidateToken(string token);
        public Task<TokenResponse?> GenerateToken(string email, string password);
        public Task<User?> RegisterAsync(RegisterDto registerDto);
        public Task<User?> LoginAsync(LoginDto loginDto);
    }
}
