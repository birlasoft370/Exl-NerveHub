using MicUI.EmailManagement.Services.Configuration;
using MicUI.EmailManagement.Services.ServiceModel;

namespace MicUI.EmailManagement.Module.Common
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
