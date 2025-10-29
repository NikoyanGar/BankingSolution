using FluentResults;
using UserService.Models.DTOs;
using UserService.Models.Responses;

namespace UserService.Auth
{
    public interface IAuthService
    {
        public Task<Result<UserInfo?>> ValidateToken(string token);
        public Task<Result<TokenResponse?>> GenerateToken(LoginDto loginDto, CancellationToken cancellationToken);
        public Task<Result<RegistrationResponse?>> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken);
        public Task<Result<LoginResponse?>> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
    }
}
