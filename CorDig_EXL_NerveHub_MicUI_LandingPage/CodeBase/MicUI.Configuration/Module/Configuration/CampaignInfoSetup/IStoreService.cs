using MicUI.Configuration.Services.Configuration;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Configuration.CampaignInfoSetup
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
