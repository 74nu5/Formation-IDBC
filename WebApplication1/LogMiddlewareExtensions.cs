namespace WebApplication1;

public static class LogMiddlewareExtensions
{
    public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder)
        => builder.UseMiddleware<LogMiddleware>();
}
