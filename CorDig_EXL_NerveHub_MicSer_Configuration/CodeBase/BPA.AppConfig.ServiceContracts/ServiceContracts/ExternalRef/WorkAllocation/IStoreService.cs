using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef.Configuration;
using BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation;
using BPA.AppConfig.ServiceContracts.ServiceContracts.FaultContracts;
using System.ServiceModel;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.WorkAllocation
{
    [ServiceContract(Name = "StoreServiceContract")]
    public interface IStoreService : IDisposable
    {
        [OperationContract(Name = "GetClientProcessList")]
        [FaultContract(typeof(ServiceFault))]
        List<BECampaignClientProcess> GetClientProcessList(int iCampID, BETenant oTenant);

        [OperationContract(Name = "GetStoreListWithCampId")]
        [FaultContract(typeof(ServiceFault))]
        List<BEStoreInfo> GetStoreList(int iCampaignId, string sStoreName, bool bActiveStore, int UserId, BETenant oTenant);
    }
}
