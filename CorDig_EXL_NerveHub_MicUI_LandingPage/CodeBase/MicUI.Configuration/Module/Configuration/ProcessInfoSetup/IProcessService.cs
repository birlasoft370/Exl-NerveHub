using MicUI.Configuration.Helper;
using MicUI.Configuration.Services.Configuration;
using MicUI.Configuration.Services.Security;
using MicUI.Configuration.Services.ServiceModel;
using System.Collections;

namespace MicUI.Configuration.Module.Configuration.ProcessInfoSetup
{
    public interface IProcessService
    {
        List<BEERPProcess> GetERPProcessList(string sERPProcess, int iLocationID);
        List<BEERPProcess> GetERPProcessList(ArrayList aryDistinctERPProcessId);
        int CheckRoleForOrgProcess();
        bool CheckProcessByClientForUniqueness(string sProcessName, int iClientId, int iProcessID);
        string InsertData(ProcessModel oProcess);
        string UpdateData(ProcessModel oProcess);
        List<BEProcessInfo> GetProcessListSearch(int iClientID, string ProcessName);
        BEProcessInfo GetProcessDetails(int iProcessID);
        List<BEProcessInfo> GetProcessList();
        List<BEProcessInfo> GetProcessAccessList(int iAgentID, bool bActiveProcess);
    }

    public class ProcessService : IProcessService
    {
        private readonly IConfigApiService _configService;
        private readonly ISecurityApiService _securityService;

        public ProcessService(IConfigApiService configService, ISecurityApiService securityService)
        {
            _configService = configService;
            _securityService = securityService;
        }
        public List<BEERPProcess> GetERPProcessList(string sERPProcess, int iLocationID)
        {
            var result = _configService.GetERPGridWithSearchListAsync(sERPProcess, iLocationID).GetAwaiter().GetResult();
            return result.data;
        }

        public List<BEERPProcess> GetERPProcessList(ArrayList aryDistinctERPProcessId)
        {
            var result = _configService.GetERPProcessListByERPProcessIdsAsync(aryDistinctERPProcessId).GetAwaiter().GetResult();
            return result.data;
        }
        public int CheckRoleForOrgProcess()
        {
            var result = _configService.CheckRoleForOrgProcessAsync().GetAwaiter().GetResult();
            return result.data;
        }
        public bool CheckProcessByClientForUniqueness(string processName, int clientId, int processId)
        {
            var result = _configService.GetCheckProcessByClientForUniquenessAsync(processName, clientId, processId).GetAwaiter().GetResult();
            return result.data;
        }
        public string InsertData(ProcessModel oProcess)
        {
            var result = _configService.AddProcessAsync(oProcess).GetAwaiter().GetResult();

            if (result != null && result.message != null)
            {
                if (result.message.ToLower().Contains(GlobalConstant.ExNumber547NullInsert))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else if (result.message.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                else
                {
                    throw new Exception(result.message);
                }
            }

            return result.data;
        }
        public string UpdateData(ProcessModel oProcess)
        {
            var result = _configService.UpdateProcessAsync(oProcess).GetAwaiter().GetResult();

            if (!result.status)
            {
                if (result.message.ToLower().Contains(GlobalConstant.ExNumber547NullInsert))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else if (result.message.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                else
                {
                    throw new Exception(result.message);
                }
            }
            return result.data;
        }
        public List<BEProcessInfo> GetProcessListSearch(int iClientID, string ProcessName)
        {
            var result = _configService.GetProcessListSearchAsync(iClientID, ProcessName).GetAwaiter().GetResult();
            return result.data;
        }
        public BEProcessInfo GetProcessDetails(int iProcessID)
        {
            var result = _configService.GetProcessDetailsByIdAsync(iProcessID).GetAwaiter().GetResult();
            return result.data;
        }
        public List<BEProcessInfo> GetProcessList()
        {
            var result = _configService.GetProcessListReportsAsync().GetAwaiter().GetResult();
            return result.data ?? new List<BEProcessInfo>();
        }
        public List<BEProcessInfo> GetProcessAccessList(int iAgentID, bool bActiveProcess)
        {
            var result = _securityService.GetProcessAccessListAsync(iAgentID, bActiveProcess).GetAwaiter().GetResult();
            return result.data ?? new List<BEProcessInfo>();
        }
    }
}
