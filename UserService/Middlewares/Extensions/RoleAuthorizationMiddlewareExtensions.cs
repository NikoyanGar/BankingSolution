namespace UserService.Middlewares.Extensions
{
    public static class RoleAuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseRoleAuthorization(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RoleAuthorizationMiddleware>();
        }
    }
}
