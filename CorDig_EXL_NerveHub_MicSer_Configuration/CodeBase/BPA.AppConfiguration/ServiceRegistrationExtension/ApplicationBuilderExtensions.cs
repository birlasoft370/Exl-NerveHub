using BPA.AppConfiguration.Helper.Middleware;

namespace BPA.AppConfiguration.ServiceRegistrationExtension
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddCorrelationIdMiddleware(this IApplicationBuilder applicationBuilder)
            => applicationBuilder.UseMiddleware<CorrelationIdMiddleware>();
        public static IApplicationBuilder AddGlobalExceptionHandlingMiddleware(this IApplicationBuilder applicationBuilder)
           => applicationBuilder.UseMiddleware<GlobalExceptionHandlingMiddleware>();

    }
}
