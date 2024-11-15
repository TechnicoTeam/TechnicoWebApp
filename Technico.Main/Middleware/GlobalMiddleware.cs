using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Diagnostics;

namespace Technico.Main.Middleware;

public class GlobalMiddleware : IMiddleware
{
    private readonly ILogger<GlobalMiddleware> _logger;
    private readonly IServiceProvider _serviceProvider;

    public GlobalMiddleware(ILogger<GlobalMiddleware> logger, IServiceProvider serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");

            var result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(
                    new EmptyModelMetadataProvider(),
                    new ModelStateDictionary())
                {
                    Model = new Models.ErrorViewModel 
                    {
                        RequestId = Activity.Current?.Id ?? context.TraceIdentifier
                    }
                }
            };

            var actionContext = new ActionContext(
                context,
                context.GetRouteData() ?? new RouteData(),
                new ActionDescriptor()
            );

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await result.ExecuteResultAsync(actionContext);
        }
    }
}