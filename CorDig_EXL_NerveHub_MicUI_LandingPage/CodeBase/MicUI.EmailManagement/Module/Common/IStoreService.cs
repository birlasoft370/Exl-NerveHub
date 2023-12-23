using MicUI.EmailManagement.Services.Configuration;
using MicUI.EmailManagement.Services.ServiceModel;

namespace MicUI.EmailManagement.Module.Common
{
    public interface IStoreService
    {
        List<BEStoreInfo> GetWorkObjList(int campaignId, string? storeName = null);
    }

    public class StoreService : IStoreService
    {

        private readonly IConfigApiService _configService;
        public StoreService(IConfigApiService configService)
        {
            _configService = configService;
        }
        public List<BEStoreInfo> GetWorkObjList(int campaignId, string? storeName = null)
        {
            var result = _configService.GetStoreListWithCampIdAsync(campaignId, storeName).GetAwaiter().GetResult();
            return result.data ?? new List<BEStoreInfo>();
        }
    }
}
