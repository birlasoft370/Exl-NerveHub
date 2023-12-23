using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.ServiceContracts.ServiceContracts.FaultContracts;
using System.Collections;
using System.ServiceModel;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.Config
{
    [ServiceContract(Name = "ProcessServiceContract")]
    public interface IProcessService : IDisposable
    {
        [OperationContract(Name = "GetERPProcessList")]
        [FaultContract(typeof(ServiceFault))]
        List<BEERPProcess> GetERPProcessList(string sERPProcess, int iLocationID, int iProcessID, BETenant oTenant);

        List<BEProcessInfo> GetProcessListSearch(int iLoggedinUserID, int iClientID, string ProcessName, bool bActiveProcess, BETenant oTenant);

        [OperationContract(Name = "GetProcessDetails")]
        [FaultContract(typeof(ServiceFault))]
        BEProcessInfo GetProcessDetails(int iProcessID, BETenant oTenant);

        [OperationContract(Name = "InsData")]
        [FaultContract(typeof(ServiceFault))]
        void InsertData(BEProcessInfo oProcess, int iFormID, BETenant oTenant);

        [OperationContract(Name = "UpdData")]
        [FaultContract(typeof(ServiceFault))]
        void UpdateData(BEProcessInfo oProcess, int iFormID, BETenant oTenant);

        [OperationContract(Name = "CheckCalenderExistance")]
        [FaultContract(typeof(ServiceFault))]
        int CheckCalenderExistance(int iProcessId, DateTime dStartDate, DateTime dEndDate, int iType, BETenant oTenant);
        [OperationContract(Name = "GetProcessListWithProcessID")]
        [FaultContract(typeof(ServiceFault))]
        List<BEProcessInfo> GetProcessList(int iProcessID, BETenant oTenant);

        [OperationContract(Name = "GetProcessListActiveLoggedWithClientProcess")]
        [FaultContract(typeof(ServiceFault))]
        List<BEProcessInfo> GetProcessList(int iLoggedinUserID, int iClientID, string sProcessName, bool bActiveProcess, BETenant oTenant);

        [OperationContract(Name = "GetProcessListActiveLogged")]
        [FaultContract(typeof(ServiceFault))]
        List<BEProcessInfo> GetProcessList(int iLoggedinUserID, bool bActiveProcess, BETenant oTenant);

        [OperationContract(Name = "GetERPProcessListWithProcessId")]
        [FaultContract(typeof(ServiceFault))]
        List<BEERPProcess> GetERPProcessList(ArrayList aryDistinctERPProcessId, BETenant oTenant);

        [OperationContract(Name = "CheckRoleForOrgProcess")]
        [FaultContract(typeof(ServiceFault))]
        int CheckRoleForOrgProcess(int iUserId, BETenant oTenant);

        [OperationContract(Name = "CheckProcessByClientForUniqueness")]
        [FaultContract(typeof(ServiceFault))]
        bool CheckProcessByClientForUniqueness(string sProcessName, int iClientId, int iProcessID, BETenant oTenant);
    }
}
