using MicSer.Security.UserMgt.Helper.Middleware;

namespace MicSer.Security.UserMgt.Services.Registration;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder AddCorrelationIdMiddleware(this IApplicationBuilder applicationBuilder)
        => applicationBuilder.UseMiddleware <CorrelationIdMiddleware>();
}