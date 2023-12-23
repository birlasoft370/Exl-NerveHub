using MicUI.WorkManagement.Services.ServiceModel;
using MicUI.WorkManagement.Services.WorkManagement;

namespace MicUI.WorkManagement.Module.WorkManagement.WorkMaster
{
    public interface IAgentDashBoardService
    {
        List<BETerminationCodeInfo> GetTerminationCodeDirect(int iCampaignID);
    }
    public class AgentDashBoardService : IAgentDashBoardService
    {
        private readonly IWorkManagementApiService _workManagementApiService;

        public AgentDashBoardService(IWorkManagementApiService workManagementApiService)
        {
            _workManagementApiService = workManagementApiService;
        }
        public List<BETerminationCodeInfo> GetTerminationCodeDirect(int iCampaignID)
        {
            var result = _workManagementApiService.GetTerminationCodeDirectAsync(iCampaignID).GetAwaiter().GetResult();
            return result.data ?? new List<BETerminationCodeInfo>();
        }

    }
}
