using FluentResults;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Data.Entities;
using UserService.Helpers;
using UserService.Models.DTOs;
using UserService.Models.Responses;
using UserService.Options;
using UserService.Repositories;

namespace UserService.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtOptions? _jwtOptions;
        public AuthService(IUserRepository userRepository, IOptions<JwtOptions> jwtOptions)
        {
            _userRepository = userRepository;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<Result<TokenResponse?>> GenerateToken(LoginDto loginDto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userRepository.GetByEmailAsync(loginDto.Email, cancellationToken);
                var user = result.Value;

                if (user is null || !PasswordHasher.VerifyPassword(loginDto.Password, user.Password))
                    return null!;

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(ClaimConstants.UserId, user.Id.ToString()),
                    new Claim(ClaimConstants.Name, user.Username),
                    new Claim(ClaimConstants.ClientId, user.ClientId),
                    new Claim(ClaimConstants.Mail, user.Email),
                };

                var roles = user.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimConstants.Roles, role));
                }

                var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresInMinutes);

                var token = new JwtSecurityToken(
                    issuer: _jwtOptions.Issuer,
                    audience: _jwtOptions.Audience,
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds);

                return new TokenResponse
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    ExpiresAt = expires
                };
            }
            catch(Exception ex)
            {
                return Result.Fail<TokenResponse?>(ex.Message);
            }
        }

        public async Task<Result<LoginResponse?>> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetByEmailAsync(loginDto.Email, cancellationToken);

            if (result.IsFailed)
            {
                return Result.Fail<LoginResponse?>("Database error");
            }

            if (result.Value == null) return Result.Fail<LoginResponse?>("User not found.");

            var user = result.Value;

            if (!PasswordHasher.VerifyPassword(loginDto.Password, user.Password))
                return Result.Fail<LoginResponse?>("Invalid credentials.");

            var generatedToken = await GenerateToken(new LoginDto
            {
                Email = loginDto.Email,
                Password = loginDto.Password
            }, cancellationToken);

            return new LoginResponse
            {
                Token = generatedToken.Value!.Token,
                ExpiresAt = generatedToken.Value!.ExpiresAt
            };
        }

        public async Task<Result<RegistrationResponse?>> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default)
        {
            var existingUserMail = await _userRepository.GetByEmailAsync(registerDto.Email, cancellationToken);

            if (existingUserMail.IsSuccess && existingUserMail.Value != null)
            {
                return Result.Fail<RegistrationResponse?>("Email already exists.");
            }
            else if (existingUserMail!.IsFailed)
            {
                return Result.Fail<RegistrationResponse?>("Database error");
            }

            var existingUserUsername = await _userRepository.GetByUsernameAsync(registerDto.Username, cancellationToken);
            if (existingUserUsername.IsSuccess && existingUserUsername.Value != null)
            {
                return Result.Fail<RegistrationResponse?>("Username already exists.");
            }
            else if (existingUserUsername!.IsFailed)
            {
                return Result.Fail<RegistrationResponse?>("Database error");
            }

            var clientId = Guid.NewGuid().ToString();
            var user = new User
            {
                Username = registerDto.Username,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                Password = PasswordHasher.HashPassword(registerDto.Password),
                Roles = registerDto.Roles,
                ClientId = clientId
            };

            await _userRepository.Create(user, cancellationToken);
            return new RegistrationResponse 
            { 
                ClientId = user.ClientId,
                Username = user.Username,
                Email = user.Email,
                Roles = user.Roles
            };
        }

        public async Task<Result<UserInfo?>> ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return Result.Fail("Invalid token");

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtOptions.Key);
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = _jwtOptions.Issuer,
                    ValidAudience = _jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                var resp = new UserInfo
                {
                    UserId = int.Parse(principal.FindFirst(ClaimConstants.UserId)?.Value),
                    Username = principal.FindFirst(ClaimConstants.Name)?.Value,
                    ClientId = principal.FindFirst(ClaimConstants.ClientId)?.Value,
                    Email = principal.FindFirst(ClaimConstants.Mail)?.Value,
                };

                return Result.Ok(resp);
            }
            catch (SecurityTokenExpiredException)
            {
                return Result.Fail("Invalid token");
            }
            catch (Exception ex)
            {
                return Result.Fail("Invalid token");
            }
        }

    }
}
