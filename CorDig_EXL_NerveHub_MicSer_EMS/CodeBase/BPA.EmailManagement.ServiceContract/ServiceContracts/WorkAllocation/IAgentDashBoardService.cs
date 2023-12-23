using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.ServiceContract.ExternalRef.FaultContracts;
using System.ServiceModel;

namespace BPA.EmailManagement.ServiceContract.ServiceContracts.WorkAllocation
{
    [ServiceContract(Name = "AgentDashBoardServiceContract")]
    public interface IAgentDashBoardService : IDisposable
    {
        [OperationContract(Name = "GetDStoreID")]
        [FaultContract(typeof(ServiceFault))]
        int GetDStoreID(int CampaignID, out bool isMailCampaign, out bool isPGT, out bool IsTabConfiguration, out int iIncreaseSearch, out bool IsGridConfiguration, out bool isDistributionBot, BETenant oTenant);

    }
}
