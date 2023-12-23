using MicUI.WorkManagement.Models.Response;
using MicUI.WorkManagement.Services.ServiceModel;
using System.Net.Http;

namespace MicUI.WorkManagement.Services.ModuleMenus
{
    public interface ISecurityApiService
    {
        Task<MessageResponse<List<BEUserInfo>>> IsLADPUserAsync();
        Task<MessageResponse<List<BEMenuItems>>> GetRoleWiseMenuAsync(int roleId);
    }

    public class SecurityApiService : BaseApiService, ISecurityApiService
    {
        public SecurityApiService(HttpClient client, IHttpContextAccessor httpContextAccessor) : base(client, httpContextAccessor)
        {

        }

        public async Task<MessageResponse<List<BEUserInfo>>> IsLADPUserAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEUserInfo>>>($"UserManagement/IsLADPUser?LoginName={SessionLoginName}");
            return content ?? new MessageResponse<List<BEUserInfo>>();
        }

        public async Task<MessageResponse<List<BEMenuItems>>> GetRoleWiseMenuAsync(int roleId = 1)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEMenuItems>>>($"Menus/GetRoleWiseMenu?RoleId={roleId}&isMossApplicationMenu=true");
            return content ?? new MessageResponse<List<BEMenuItems>>();
        }
    }
}
