namespace BPA.AppConfiguration.Helper.Middleware;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string _correlationIdHeader = "X-Correlation-Id";

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        GetCorrelationId(context);
        //AddCorrelationIdHeaderToResponse(context, correlationId);

        await _next(context);
    }

    private static void GetCorrelationId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(_correlationIdHeader, out var correlationId))
        {
            context.Request.HttpContext.Items.Add(_correlationIdHeader, correlationId);
        }
    }
}

