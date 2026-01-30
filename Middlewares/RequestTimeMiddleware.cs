using System.Diagnostics;
namespace WebApi.Middlewares;

public class RequestTimeMiddleware(RequestDelegate next, ILogger<RequestTimeMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<RequestTimeMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation(
            "Incoming request: {Method} {Path}",
            context.Request.Method,
            context.Request.Path
        );

        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Request failed");
            throw;
        }
        finally
        {
            stopwatch.Stop();
            _logger.LogInformation(
                "Request finished in {Time} ms",
                stopwatch.ElapsedMilliseconds
            );
        }
    }
}
