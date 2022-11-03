namespace WebApplication1;

using Microsoft.AspNetCore.Http.Extensions;

public class LogMiddleware
{
    private readonly RequestDelegate next;

    public LogMiddleware(RequestDelegate next)
        => this.next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers["X-LOGGING"].Any())
        {
            await this.next.Invoke(context);
            return;
        }

        var loggerFactory = context.RequestServices.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("First Middleware");
        logger.LogInformation("Test de middleware avant {Url}", context.Request.GetDisplayUrl());
        await this.next.Invoke(context);
        logger.LogInformation("Test de middleware après");
    }
}
