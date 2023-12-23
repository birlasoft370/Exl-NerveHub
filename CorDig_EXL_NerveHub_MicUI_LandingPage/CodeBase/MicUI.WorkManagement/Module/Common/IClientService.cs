using MicUI.WorkManagement.Services.Configuration;
using MicUI.WorkManagement.Services.ServiceModel;

namespace MicUI.WorkManagement.Module.Common
{
    public interface IClientService
    {
        List<BEClientInfo> GetClientList(bool isActive, string? searchText = null);
    }
    public class ClientService : IClientService
    {
        private readonly IConfigApiService _configService;
        public ClientService(IConfigApiService configService)
        {
            _configService = configService;
        }

        public List<BEClientInfo> GetClientList(bool isActive, string? searchText = null)
        {
            var result = _configService.GetClientAsync(searchText, isActive).GetAwaiter().GetResult();
            return result.data ?? new List<BEClientInfo>();
        }
    }
}
