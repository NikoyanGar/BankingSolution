using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using UserService.Auth;
using UserService.Options;

namespace UserService.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware>? _logger;
        private readonly JwtOptions? _jwtOptions;
        public AuthenticationMiddleware(
            RequestDelegate next,
            ILogger<AuthenticationMiddleware>? logger,
            IConfiguration config)
        {
            _next = next;
            _logger = logger;
            _jwtOptions = config.GetSection("Jwt").Get<JwtOptions>();
        }

        public async Task InvokeAsync(HttpContext context, IAuthService authService)
        {
            var endpoint = context.GetEndpoint();

            // Check if endpoint has [Roles("admin,manager")] attribute
            var roleAttr = endpoint?.Metadata.GetMetadata<RolesAttribute>();
            var Authorize = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>();
            if (Authorize == null)
            {
                await _next(context);
                return;
            }

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                var userResult = await authService.ValidateToken(token);

                if (roleAttr != null)
                {
                    var userRoles = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

                    if (!userRoles.Intersect(roleAttr.Roles.Split(',')).Any())
                    {
                        _logger!.LogWarning("User lacks required roles: {Roles}", roleAttr.Roles);
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsJsonAsync(new { message = "Forbidden: insufficient role" });
                        return;
                    }
                }

                if (userResult.IsSuccess)
                {
                    await _next(context);
                }
                else
                {
                    _logger!.LogWarning("Token validation failed: {Errors}", string.Join(", ", userResult.Errors));
                    throw new SecurityTokenException("Invalid token");
                }
            }
        }

        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
        public class RolesAttribute : Attribute
        {
            public string Roles { get; }

            public RolesAttribute(string roles)
            {
                Roles = roles;
            }
        }
    }
}
