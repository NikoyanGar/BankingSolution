using FluentResults;
using UserService.Models.DTOs;
using UserService.Models.Responses;

namespace UserService.Auth
{
    public interface IAuthService
    {
        public Task<Result<UserInfo>> ValidateToken(string token);
        public Task<TokenResponse?> GenerateToken(LoginDto loginDto, CancellationToken cancellationToken);
        public Task<RegistrationResponse?> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken);
        public Task<LoginResponse> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
    }
}
