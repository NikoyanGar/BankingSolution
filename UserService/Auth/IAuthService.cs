using FluentResults;
using UserService.Models.Requests;
using UserService.Models.Responses;

namespace UserService.Auth
{
    public interface IAuthService
    {
        public Task<Result<UserInfo?>> ValidateToken(string token);
        public Task<Result<TokenResponse?>> GenerateToken(LoginRequest loginRequest, CancellationToken cancellationToken);
        public Task<Result<RegistrationResponse?>> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken);
        public Task<Result<LoginResponse?>> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken);
    }
}
