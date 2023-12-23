using MicUI.WorkManagement.Services.Configuration;
using MicUI.WorkManagement.Services.ServiceModel;

namespace MicUI.WorkManagement.Module.Common
{
    public interface ICampaignService
    {
        List<BECampaignInfo> GetProcessWiseCampaignList(int iProcessID);
    }
    public class CampaignService : ICampaignService
    {
        private readonly IConfigApiService _configService;

        public CampaignService(IConfigApiService configService)
        {
            _configService = configService;
        }

        public List<BECampaignInfo> GetProcessWiseCampaignList(int iProcessID)
        {
            var result = _configService.GetProcessWiseCampaignListAsync(iProcessID).GetAwaiter().GetResult();
            return result.data ?? new List<BECampaignInfo>();
        }

    }
}
