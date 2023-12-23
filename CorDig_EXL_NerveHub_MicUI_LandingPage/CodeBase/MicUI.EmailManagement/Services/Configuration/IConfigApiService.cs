using MicUI.EmailManagement.Models.Response;
using MicUI.EmailManagement.Services.ServiceModel;

namespace MicUI.EmailManagement.Services.Configuration
{
    public interface IConfigApiService
    {
        Task<MessageResponse<List<BEClientInfo>>> GetClientAsync(string? searchText, bool isActive);
        Task<MessageResponse<List<BEProcessInfo>>> GetProcessListByClientIdAsync(int clientId, string? processName, bool activeProcess);
        Task<MessageResponse<List<BECampaignInfo>>> GetProcessWiseCampaignListAsync(int processId);
        Task<MessageResponse<List<BEStoreInfo>>> GetStoreListWithCampIdAsync(int campaignId, string? storeName);
        Task<MessageResponse<List<BETimeZoneInfo>>> GetTimeZoneListAsync();
        Task<MessageResponse<int>> GetCheckUserIsSuperOrFunctionalAdminAsync();
    }
    public class ConfigApiService : BaseApiService, IConfigApiService
    {
        public ConfigApiService(HttpClient client, IHttpContextAccessor httpContextAccessor) : base(client, httpContextAccessor)
        {

        }
        public async Task<MessageResponse<List<BEClientInfo>>> GetClientAsync(string? searchText, bool isActive)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEClientInfo>>>($"Client/GetClient?searchText={searchText}&isActive={isActive}");
            return content ?? new MessageResponse<List<BEClientInfo>>();
        }

        public async Task<MessageResponse<List<BECampaignInfo>>> GetProcessWiseCampaignListAsync(int processId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BECampaignInfo>>>($"Campaign/GetProcessWiseCampaignList?processId={processId}");
            return content ?? new MessageResponse<List<BECampaignInfo>>();
        }
        public async Task<MessageResponse<List<BEProcessInfo>>> GetProcessListByClientIdAsync(int clientId, string? processName, bool activeProcess)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEProcessInfo>>>($"Process/GetProcessListByClientId?clientId={clientId}&processName={processName}&activeProcess={activeProcess}");
            return content ?? new MessageResponse<List<BEProcessInfo>>();
        }
        public async Task<MessageResponse<List<BEStoreInfo>>> GetStoreListWithCampIdAsync(int campaignId, string? storeName)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEStoreInfo>>>($"WorkDefinition/GetStoreListWithCampId?campaignId={campaignId}&storeName={storeName}");
            return content ?? new MessageResponse<List<BEStoreInfo>>();
        }
        public async Task<MessageResponse<List<BETimeZoneInfo>>> GetTimeZoneListAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BETimeZoneInfo>>>("TimeZone/GetTimeZoneList");
            return content ?? new MessageResponse<List<BETimeZoneInfo>>();
        }
        public async Task<MessageResponse<int>> GetCheckUserIsSuperOrFunctionalAdminAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<int>>("Campaign/GetCheckUserIsSuperOrFunctionalAdmin");
            return content ?? new MessageResponse<int>();
        }
    }
}
