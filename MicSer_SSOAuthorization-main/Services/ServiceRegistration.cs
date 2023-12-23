using MicSer.SSOAuthorization.Repository;
//using Microsoft.Extensions.DependencyInjection;
using MicSer.SSOAuthorization.Services;
using Microsoft.AspNetCore.Builder;

namespace MicSer.SSOAuthorization
{
    public static class ServiceRegistration
    {
        public static IServiceCollection ServiceCollectionExtension(this IServiceCollection services)
        {
            services.AddTransient<IAuthenticationOperations, AuthenticationOperations>();
            services.AddTransient<IJwtTokenService, JwtTokenService>();
            services.AddTransient<IAuthenticateService, AuthenticateService>();
            services.AddTransient<IAuthenticateServiceDummy, AuthenticateServiceDummy>();
            return services;
        }
    }
}
