using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.BusinessEntity.Security;
using BPA.AppConfig.ServiceContracts.ServiceContracts.FaultContracts;
using System.Data;
using System.ServiceModel;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.Config
{
    [ServiceContract(Name = "CampaignServiceContract")]
    public interface ICampaignService : IDisposable
    {
        List<BEUserInfo> GetUserApproverListByProcess(int UserId, int iFormId, int ProcessId, BETenant oTenant);

        //[AuthenticationBehaviorAttribute] // issue
        [OperationContract(Name = "GetCampaignListLoggedWithProcessID")]
        [FaultContract(typeof(ServiceFault))]
        List<BECampaignInfo> GetCampaignList(int iLoggedinUserID, int iProcessID, string sCampaignName, bool bActiveCampaign, BETenant oTenant);

        [OperationContract(Name = "GetCampaignListWithCampaignID")]
        [FaultContract(typeof(ServiceFault))]
        List<BECampaignInfo> GetCampaignList(int iCampaignID, BETenant oTenant);

        // [AuthenticationBehaviorAttribute]
        [OperationContract(Name = "InsData")]
        [FaultContract(typeof(ServiceFault))]
        void InsertData(BECampaignInfo ocampaign, int iFormID, BETenant oTenant);

        [OperationContract(Name = "UpdData")]
        [FaultContract(typeof(ServiceFault))]
        void UpdateData(BECampaignInfo ocampaign, int iFormID, BETenant oTenant);

        [OperationContract(Name = "GetPandingCampaignApproval")]
        [FaultContract(typeof(ServiceFault))]
        DataTable GetPandingCampaignApproval(int iLoggedinUserID, DateTime dFrom, DateTime dTo, BETenant oTenant);

        [OperationContract(Name = "FetchCampaignRequestDetails")]
        [FaultContract(typeof(ServiceFault))]
        DataTable FetchCampaignRequestDetails(int iApprovalId, BETenant oTenant);

        [OperationContract(Name = "InsertCampaignRequestChange")]
        [FaultContract(typeof(ServiceFault))]
        void InsertCampaignRequestChange(int iApprovalId, int iUserId, int iUserLevel, string sChangeRequest, BETenant oTenant);

        [OperationContract(Name = "UpdateCampaignRequestChange")]
        [FaultContract(typeof(ServiceFault))]
        void UpdateCampaignRequestChange(BECampaignInfo oCampaign, BETenant oTenant);

        [OperationContract(Name = "GetProcessWiseCampaignList")]
        [FaultContract(typeof(ServiceFault))]
        List<BECampaignInfo> GetProcessWiseCampaignList(int iFormID, int iLoggedinUserID, int iProcessID, BETenant oTenant);
    }
}
