using MicUI.EmailManagement.Services.Authentication;
using MicUI.EmailManagement.Services.ServiceModel;

namespace MicUI.EmailManagement.Module.Authentication
{
    public interface IAuthenticationTokenService
    {
        public BETenantInfo GetTanentInfo(string token);
    }

    public class AuthenticationTokenService : IAuthenticationTokenService
    {
        private readonly IAuthenticationApiService _authenticationApiService;

        public AuthenticationTokenService(IAuthenticationApiService authenticationApiService)
        {
            _authenticationApiService = authenticationApiService;
        }

        public BETenantInfo GetTanentInfo(string token)
        {
            var tenant = _authenticationApiService.SSOValidationAsync(token).GetAwaiter().GetResult();
            return tenant;
        }
    }
}
