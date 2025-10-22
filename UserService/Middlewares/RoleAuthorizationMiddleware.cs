using System.Security.Claims;

namespace UserService.Middlewares
{
    public class RoleAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RoleAuthorizationMiddleware> _logger;

        public RoleAuthorizationMiddleware(RequestDelegate next, ILogger<RoleAuthorizationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            // Check if endpoint has [Roles("admin,manager")] attribute
            var roleAttr = endpoint?.Metadata.GetMetadata<RolesAttribute>();

            if (roleAttr != null)
            {
                var userRoles = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

                if (!userRoles.Intersect(roleAttr.Roles.Split(',')).Any())
                {
                    _logger.LogWarning("User lacks required roles: {Roles}", roleAttr.Roles);
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsJsonAsync(new { message = "Forbidden: insufficient role" });
                    return;
                }
            }

            await _next(context);
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
