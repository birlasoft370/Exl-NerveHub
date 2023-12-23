using MicUI.EmailManagement.Services.ServiceModel;

namespace MicUI.EmailManagement.Services.Authentication
{
    public interface IAuthenticationApiService
    {
        Task<BETenantInfo> SSOValidationAsync(string token);
    }
    public class AuthenticationApiService : BaseApiService, IAuthenticationApiService
    {
        public AuthenticationApiService(HttpClient client, IHttpContextAccessor httpContextAccessor) : base(client, httpContextAccessor)
        {

        }

        public async Task<BETenantInfo> SSOValidationAsync(string token)
        {
            var response = await _client.PostAsync("SSOValidation", null);
            return await response.Content.ReadFromJsonAsync<BETenantInfo>();
        }
    }
}
