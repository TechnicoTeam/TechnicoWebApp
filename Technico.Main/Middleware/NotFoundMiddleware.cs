using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace Technico.Main.Middleware;

public class NotFoundMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<NotFoundMiddleware> _logger;

    public NotFoundMiddleware(RequestDelegate next, ILogger<NotFoundMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
        {
            _logger.LogWarning($"404 Error for path: {context.Request.Path}");

            // Clear the response and reset the pipeline
            context.Response.Clear();
            context.SetEndpoint(null);
            context.Request.Path = "/Home/NotFound";  // Assuming you have a Home controller

            await _next(context);
        }
    }
}
