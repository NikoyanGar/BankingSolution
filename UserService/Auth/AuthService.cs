using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Helpers;
using UserService.Models;
using UserService.Options;
using UserService.Repositories;

namespace UserService.Auth
{
    public class AuthService: IAuthService
    {
        private readonly IUserRepository? _userRepository;
        private readonly JwtOptions? _jwtOptions;
        public AuthService(IUserRepository userRepository, IOptions<JwtOptions> jwtOptions)
        {
            _userRepository = userRepository;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<TokenResponse?> GenerateToken(string email, string password)
        {
            var userByEmail = await _userRepository.GetByEmailAsync(email);
            if (userByEmail == null) return null;

            if (!PasswordHasher.VerifyPassword(password, userByEmail.Password))
                return null;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("username", userByEmail.Username),
                new Claim(ClaimTypes.Name, userByEmail.Username),
                new Claim("clientId", userByEmail.ClientId),
                new Claim(ClaimTypes.Email, userByEmail.Email),
            };

            var roles = userByEmail.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
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

        public async Task<User?> LoginAsync(LoginDto loginDto)
        {
            var userByMail = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (userByMail == null) return null;

            if (!PasswordHasher.VerifyPassword(loginDto.Password, userByMail.Password))
                return null;

            //return GenerateToken(userByMail);
            return userByMail;
        }

        public async Task<User?> RegisterAsync(RegisterDto registerDto)
        {
            var existingUserMail = await _userRepository.GetByEmailAsync(registerDto.Email);
            var existingUserUsername = await _userRepository.GetByUsernameAsync(registerDto.Username);
            if (existingUserMail != null || existingUserUsername != null) return null;

            var clientId = await _userRepository.GetNextClientIdAsync();
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

            await _userRepository.Create(user);
            //GenerateToken(user);
            return user;
        }

        public async Task<string> ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtOptions.Key);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = _jwtOptions.Issuer,
                    ValidAudience = _jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                return principal.FindFirstValue(ClaimTypes.Name);
            }
            catch (SecurityTokenExpiredException)
            {
                // token is expired
                return null;
            }
            catch (Exception)
            {
                // token invalid
                return null;
            }
        }
    }
}
