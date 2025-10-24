namespace ClientLoanServiceAPI.Middlewares.Extensions
{
    //Delete this file if not needed
    public static class GlobalExceptionHandlingExtension
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        }
    }
}
