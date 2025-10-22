using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Options;

namespace UserService.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware>? _logger;
        private readonly JwtOptions? _jwtOptions;
        public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware>? logger, IConfiguration config)
        {
            _next = next;
            _logger = logger;
            _jwtOptions = config.GetSection("Jwt").Get<JwtOptions>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(_jwtOptions!.Key);

                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = _jwtOptions.Issuer,
                        ValidAudience = _jwtOptions.Audience,
                        ClockSkew = TimeSpan.Zero
                    }, out var validatedToken);

                    context.User = ((JwtSecurityToken)validatedToken).Claims.Aggregate(new ClaimsPrincipal(), (principal, claim) =>
                    {
                        var identity = principal.Identity as ClaimsIdentity ?? new ClaimsIdentity();
                        identity.AddClaim(claim);
                        principal.AddIdentity(identity);
                        return principal;
                    });
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning("Token validation failed: {Message}", ex.Message);
                }
            }
            await _next(context);
        }

    }
}
