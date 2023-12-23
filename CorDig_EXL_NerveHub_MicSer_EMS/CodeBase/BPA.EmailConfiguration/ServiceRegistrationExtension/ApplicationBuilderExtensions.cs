using MicSer.EmailConfiguration.Helper.Middleware;

namespace MicSer.EmailConfiguration.ServiceRegistrations
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddCorrelationIdMiddleware(this IApplicationBuilder applicationBuilder)
            => applicationBuilder.UseMiddleware<CorrelationIdMiddleware>();
    }
}
