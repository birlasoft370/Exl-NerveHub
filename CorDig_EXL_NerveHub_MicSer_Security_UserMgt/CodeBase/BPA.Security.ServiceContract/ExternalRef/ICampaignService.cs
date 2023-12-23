using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using BPA.Security.ServiceContract.FaultContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BPA.Security.ServiceContract.ExternalRef
{
    [ServiceContract(Name = "CampaignServiceContract")]
    public interface ICampaignService : IDisposable
    {

       // [AuthenticationBehaviorAttribute]
        [OperationContract(Name = "GetCampaignList")]
        [FaultContract(typeof(ServiceFault))]
        List<BECampaignInfo> GetCampaignList(BETenant oTenant);

       // [AuthenticationBehaviorAttribute]
        [OperationContract(Name = "GetCampaignListLoggedActive")]
        [FaultContract(typeof(ServiceFault))]
        List<BECampaignInfo> GetCampaignList(int iLoggedinUserID, bool bActiveCampaign, BETenant oTenant);

       // [AuthenticationBehaviorAttribute]
        [OperationContract(Name = "GetCampaignListLoggedWithCampaign")]
        [FaultContract(typeof(ServiceFault))]
        List<BECampaignInfo> GetCampaignList(int iLoggedinUserID, string CampaignName, bool bActiveCampaign, BETenant oTenant);

      //  [AuthenticationBehaviorAttribute]
        [OperationContract(Name = "GetCampaignListDashboard")]
        [FaultContract(typeof(ServiceFault))]
        List<BECampaignInfo> GetCampaignList(int iLoggedinUserID, string CampaignName, bool bActiveCampaign, bool DashboardRequest, BETenant oTenant);


      //  [AuthenticationBehaviorAttribute]
        [OperationContract(Name = "GetCampaignListLoggedWithProcessID")]
        [FaultContract(typeof(ServiceFault))]
        List<BECampaignInfo> GetCampaignList(int iLoggedinUserID, int iProcessID, string sCampaignName, bool bActiveCampaign, BETenant oTenant);

       // [AuthenticationBehaviorAttribute]
        [OperationContract(Name = "GetCampaignListWithCampaignID")]
        [FaultContract(typeof(ServiceFault))]
        List<BECampaignInfo> GetCampaignList(int iCampaignID, BETenant oTenant);

       // [AuthenticationBehaviorAttribute]
        [OperationContract(Name = "GetCampaignAccessList")]
        [FaultContract(typeof(ServiceFault))]
        List<BECampaignInfo> GetCampaignAccessList(int iLoggedinUserID, int iAgentID, bool bActiveCampaign, BETenant oTenant);

       // [AuthenticationBehaviorAttribute]
        [OperationContract(Name = "GetProcessWiseCampaignList")]
        [FaultContract(typeof(ServiceFault))]
        List<BECampaignInfo> GetProcessWiseCampaignList(int iFormID, int iLoggedinUserID, int iProcessID, BETenant oTenant);

       // [AuthenticationBehaviorAttribute]
        [OperationContract(Name = "GetCampaignAndRoleList")]
        [FaultContract(typeof(ServiceFault))]
        DataSet GetCampaignAndRoleList(int iCampaignID, BETenant oTenant);

      //  [AuthenticationBehaviorAttribute]
        [OperationContract(Name = "GetCampaignDetails")]
        [FaultContract(typeof(ServiceFault))]
        DataSet GetCampaignDetails(int iProcessID, BETenant oTenant);

       // [AuthenticationBehaviorAttribute]
        [OperationContract(Name = "GetCampaignAndRoleDetails")]
        [FaultContract(typeof(ServiceFault))]
        DataSet GetCampaignAndRoleDetails(int iClientID, BETenant oTenant);

       // [AuthenticationBehaviorAttribute]
        [OperationContract(Name = "GetCampaignDetailsByUserID")]
        [FaultContract(typeof(ServiceFault))]
        DataSet GetCampaignDetailsByUserID(string sLoginName, BETenant oTenant);

       // [AuthenticationBehaviorAttribute]
        [OperationContract(Name = "GetUserId")]
        [FaultContract(typeof(ServiceFault))]
        int GetUserId(string sLoginName, BETenant oTenant);

       // [AuthenticationBehaviorAttribute]
        [OperationContract(Name = "GetRoleId")]
        [FaultContract(typeof(ServiceFault))]
        int GetRoleId(BETenant oTenant);

      //  [AuthenticationBehaviorAttribute]
        [OperationContract(Name = "InsData")]
        [FaultContract(typeof(ServiceFault))]
        void InsertData(BECampaignInfo ocampaign, int iFormID, BETenant oTenant);

      //  [AuthenticationBehaviorAttribute]
        [OperationContract(Name = "UpdData")]
        [FaultContract(typeof(ServiceFault))]
        void UpdateData(BECampaignInfo ocampaign, int iFormID, BETenant oTenant);

       // [AuthenticationBehaviorAttribute]
        [OperationContract(Name = "DelData")]
        [FaultContract(typeof(ServiceFault))]
        void DeleteData(BECampaignInfo ocampaign, int iFormID, BETenant oTenant);

      //  [AuthenticationBehaviorAttribute]
        [OperationContract(Name = "FetchCampaignRequestDetails")]
        [FaultContract(typeof(ServiceFault))]
        DataTable FetchCampaignRequestDetails(int iApprovalId, BETenant oTenant);

       // [AuthenticationBehaviorAttribute]
        [OperationContract(Name = "GetPandingCampaignApproval")]
        [FaultContract(typeof(ServiceFault))]
        DataTable GetPandingCampaignApproval(int iLoggedinUserID, DateTime dFrom, DateTime dTo, BETenant oTenant);

      //  [AuthenticationBehaviorAttribute]
        [OperationContract(Name = "InsertCampaignRequestChange")]
        [FaultContract(typeof(ServiceFault))]
        void InsertCampaignRequestChange(int iApprovalId, int iUserId, int iUserLevel, string sChangeRequest, BETenant oTenant);

       // [AuthenticationBehaviorAttribute]
        [OperationContract(Name = "UpdateCampaignRequestChange")]
        [FaultContract(typeof(ServiceFault))]
        void UpdateCampaignRequestChange(BECampaignInfo oCampaign, BETenant oTenant);



        List<BEUserInfo> GetUserApproverListByProcess(int UserId, int iFormId, int ProcessId, BETenant oTenant);

        string GetDStoreID(string Campaignid, BETenant oTenant);
    }
}
