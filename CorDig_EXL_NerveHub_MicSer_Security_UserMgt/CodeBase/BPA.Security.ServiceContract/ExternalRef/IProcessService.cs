using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.ServiceContract.FaultContracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BPA.Security.ServiceContract.ExternalRef
{
    [ServiceContract(Name = "ProcessServiceContract")]
    public interface IProcessService : IDisposable
    {
        [OperationContract(Name = "GetProcessListWithProcessID")]
        [FaultContract(typeof(ServiceFault))]
        List<BEProcessInfo> GetProcessList(int iProcessID, BETenant oTenant);

        [OperationContract(Name = "GetProcessListActiveLogged")]
        [FaultContract(typeof(ServiceFault))]
        List<BEProcessInfo> GetProcessList(int iLoggedinUserID, bool bActiveProcess, BETenant oTenant);

        [OperationContract(Name = "GetProcessListActiveLoggedWithProcess")]
        [FaultContract(typeof(ServiceFault))]
        List<BEProcessInfo> GetProcessList(int iLoggedinUserID, string sProcessName, bool bActiveProcess, BETenant oTenant);

        [OperationContract(Name = "GetProcessListActiveLoggedWithClientProcess")]
        [FaultContract(typeof(ServiceFault))]
        List<BEProcessInfo> GetProcessList(int iLoggedinUserID, int iClientID, string sProcessName, bool bActiveProcess, BETenant oTenant);

        [OperationContract(Name = "GetProcessAccessList")]
        [FaultContract(typeof(ServiceFault))]
        List<BEProcessInfo> GetProcessAccessList(int iLoggedinUserID, int iAgentID, bool bActiveProcess, BETenant oTenant);

        [OperationContract(Name = "GetOverRatingProcessList")]
        [FaultContract(typeof(ServiceFault))]
        List<BEProcessInfo> GetOverRatingProcessList(int iLoggedinUserID, string sProcessName, bool bActiveProcess, BETenant oTenant);

        [OperationContract(Name = "GetHealthProcessList")]
        [FaultContract(typeof(ServiceFault))]
        List<BEProcessInfo> GetHealthProcessList(int iLoggedinUserID, int iClientID, string sProcessName, bool bActiveProcess, BETenant oTenant);

        //[OperationContract(Name = "GetClientList")]
        //[FaultContract(typeof(ServiceFault))]
        //List<BEProcessInfo> GetClientList(int iProcessID);

        [OperationContract(Name = "GetProcessDetails")]
        [FaultContract(typeof(ServiceFault))]
        BEProcessInfo GetProcessDetails(int iProcessID, BETenant oTenant);

        [OperationContract(Name = "GetClientWiseProcessList")]
        [FaultContract(typeof(ServiceFault))]
        List<BEProcessInfo> GetClientWiseProcessList(int iLoggedinUserID, int iClientID, BETenant oTenant);

        [OperationContract(Name = "GetMultiClientWiseProcessList")]
        [FaultContract(typeof(ServiceFault))]
        List<BEProcessInfo> GetMultiClientWiseProcessList(int iLoggedinUserID, string sClientID, BETenant oTenant);

        [OperationContract(Name = "GetClientListWiseProcessList")]
        [FaultContract(typeof(ServiceFault))]
        List<BEProcessInfo> GetClientListWiseProcessList(int iLoggedinUserID, string sClientID, BETenant oTenant);

        [OperationContract(Name = "GetProcessManager")]
        [FaultContract(typeof(ServiceFault))]
        DataSet GetProcessManager(int iProcessId, BETenant oTenant);

        [OperationContract(Name = "GetCampaignBatchList")]
        [FaultContract(typeof(ServiceFault))]
        List<BEPendingApproval> GetCampaignBatchList(int iCampaignId, string sBatchCode, string sFromDate, string sToDate, BETenant oTenant);

        [OperationContract(Name = "GetCampaignBatchList")]
        [FaultContract(typeof(ServiceFault))]
        List<BEPendingApproval> GetCampaignBatchQMSList(int iCampaignId, string sBatchCode, string sFromDate, string sToDate, string UserID, BETenant oTenant);

        [OperationContract(Name = "GetPendingCampaignBatchList")]
        [FaultContract(typeof(ServiceFault))]
        List<BEPendingApproval> GetPendingCampaignBatchList(int iUserID, string sFromDate, string sToDate, BETenant oTenant);

        [OperationContract(Name = "RequestCampaignBatchDelete")]
        [FaultContract(typeof(ServiceFault))]
        int RequestCampaignBatchDelete(int FormID, BECampaignInfo oCampaign, string sBatchCode, int iNoOfRecords, string sFromDate, string sToDate, int iRequestBy, BETenant oTenant);

        [OperationContract(Name = "StatusCampaignBatchReqest")]
        [FaultContract(typeof(ServiceFault))]
        int StatusCampaignBatchReqest(int iUserId, int iFormID, int iBatchApprovalID, int iAction, BETenant oTenant);

        //[OperationContract(Name = "GetRoleDetails")]
        //[FaultContract(typeof(ServiceFault))]
        //DataSet GetRoleDetails();

        [OperationContract(Name = "GetProcessSLA")]
        [FaultContract(typeof(ServiceFault))]
        DataSet GetProcessSLA(int iFieldID, BETenant oTenant);

        [OperationContract(Name = "GetProcessSLAList")]
        [FaultContract(typeof(ServiceFault))]
        BEProcessSLA GetProcessSLAList(int iProcessSLAID, BETenant oTenant);

        [OperationContract(Name = "GetERPProcessListWithProcessId")]
        [FaultContract(typeof(ServiceFault))]
        List<BEERPProcess> GetERPProcessList(ArrayList aryDistinctERPProcessId, BETenant oTenant);

        [OperationContract(Name = "GetERPProcessList")]
        [FaultContract(typeof(ServiceFault))]
        List<BEERPProcess> GetERPProcessList(string sERPProcess, int iLocationID, int iProcessID, BETenant oTenant);

        [OperationContract(Name = "InsertUpdate")]
        [FaultContract(typeof(ServiceFault))]
        void InsertUpdate(BEProcessInfo oProcess, string sActionType, BETenant oTenant);

        [OperationContract(Name = "InsData")]
        [FaultContract(typeof(ServiceFault))]
        void InsertData(BEProcessInfo oProcess, int iFormID, BETenant oTenant);

        [OperationContract(Name = "ManageProcessOwnerData")]
        [FaultContract(typeof(ServiceFault))]
        void ManageProcessOwnerData(BEProcessInfo oProcess, int iFormID, BETenant oTenant);

        [OperationContract(Name = "SendApproveProcessReqest")]
        [FaultContract(typeof(ServiceFault))]
        int SendApproveProcessReqest(BEProcessInfo oProcess, int iFormID, int iProcRequest_Id, int iAction, BETenant oTenant);

        [OperationContract(Name = "GetPandingApproval")]
        [FaultContract(typeof(ServiceFault))]
        DataTable GetPandingApproval(int iUserId, string sFromDate, string sToDate, BETenant oTenant);

        [OperationContract(Name = "UpdData")]
        [FaultContract(typeof(ServiceFault))]
        void UpdateData(BEProcessInfo oProcess, int iFormID, BETenant oTenant);

        [OperationContract(Name = "DelData")]
        [FaultContract(typeof(ServiceFault))]
        void DeleteData(BEProcessInfo oProcess, int iFormID, BETenant oTenant);


        [OperationContract(Name = "GetProcessComplexity")]
        [FaultContract(typeof(ServiceFault))]
        DataSet GetProcessComplexity(BETenant oTenant);

        [OperationContract(Name = "CheckProcessForUniqueness")]
        [FaultContract(typeof(ServiceFault))]
        bool CheckProcessForUniqueness(string sProcessName, BETenant oTenant);

        // Added by ManishDwivedi-20-Jan-2022
        [OperationContract(Name = "CheckProcessByClientForUniqueness")]
        [FaultContract(typeof(ServiceFault))]
        bool CheckProcessByClientForUniqueness(string sProcessName, int iClientId, int iProcessID, BETenant oTenant);
        //end
        [OperationContract(Name = "CheckCalenderExistance")]
        [FaultContract(typeof(ServiceFault))]
        int CheckCalenderExistance(int iProcessId, DateTime dStartDate, DateTime dEndDate, int iType, BETenant oTenant);

        [OperationContract(Name = "CheckRoleForOrgProcess")]
        [FaultContract(typeof(ServiceFault))]
        int CheckRoleForOrgProcess(int iUserId, BETenant oTenant);

        [OperationContract(Name = "GetProcessType")]
        [FaultContract(typeof(ServiceFault))]
        int GetProcessType(int iProcessId, BETenant oTenant);

        [OperationContract(Name = "GetUserProcessOwner")]
        [FaultContract(typeof(ServiceFault))]
        DataSet GetUserProcessOwner(int iProcessId, BETenant oTenant);

        [OperationContract(Name = "GetUserProcessList")]
        [FaultContract(typeof(ServiceFault))]
        DataTable GetUserProcessList(int iClientId, int iProcessId, BETenant oTenant);

        [OperationContract(Name = "ExistingUserRequest")]
        [FaultContract(typeof(ServiceFault))]
        string ExistingUserRequest(int iProcessId, string sProcessOwner, BETenant oTenant);

        [OperationContract(Name = "GetProcessAVPAbove")]
        [FaultContract(typeof(ServiceFault))]
        List<BEApproval> GetProcessAVPAbove(int iUserId, int iFormId, int iProcessId, BETenant oTenant);

        [OperationContract(Name = "GetClientProcByCampID")]
        [FaultContract(typeof(ServiceFault))]
        List<BEProcessInfo> GetClientProcByCampID(int iCampaignId, BETenant oTenant);

        [OperationContract(Name = "GetProcessListByUserID")]
        [FaultContract(typeof(ServiceFault))]
        List<BEProcessInfo> GetProcessListByUserID(int iLoggedinUserID, BETenant oTenant);

        [OperationContract(Name = "CheckProcessOwnerApproverLevel")]
        [FaultContract(typeof(ServiceFault))]
        string CheckProcessOwnerApproverLevel(BEProcessInfo oProcess, BETenant oTenant);

        List<BEProcessInfo> GetProcessListSearch(int iLoggedinUserID, int iClientID, string ProcessName, bool bActiveProcess, BETenant oTenant);

        //// Added by ManishDwivedi-20-Jan-2022
        //[OperationContract(Name = "CheckProcessByClientForUniqueness")]
        //[FaultContract(typeof(ServiceFault))]
        //bool CheckProcessByClientForUniqueness(string sProcessName, int iClientId, int iProcessID, BETenant oTenant);
    }
}