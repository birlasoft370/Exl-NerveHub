using MicUI.WorkManagement.Services.Authentication;
using MicUI.WorkManagement.Services.ServiceModel;

namespace MicUI.WorkManagement.Module.Authentication
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
