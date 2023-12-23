using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Services.Security;
using MicUI.Configuration.Services.ServiceModel;
using System.Data;
using System.Diagnostics;

namespace MicUI.Configuration.Module.Security
{
    public class ProcessOwnerService: IProcessOwnerService
    {
        private readonly ISecurityApiService _securityApiService;
        public ProcessOwnerService(ISecurityApiService securityApiService)
        {
            _securityApiService = securityApiService;
        }
        public List<BEProcessInfo> GetClientWiseProcessList(int userId, int ClientId)
        { 
            var result = _securityApiService.GetClientWiseProcessListAsync(userId,ClientId).GetAwaiter().GetResult();
            return result.data;
        }
        public List<BEApproval> GetUserProcessOwner(int processId)
        {
            var result = _securityApiService.GetUserProcessOwnerAsync(processId).GetAwaiter().GetResult();
            return result.data;

        }
        public ProcessOwnerApprovalUser GetUserProcessOwnerUserList(int processId)
        {
            var result = _securityApiService.GetUserProcessOwnerUserListAsync(processId).GetAwaiter().GetResult();
            return result.data;

        }
        public string ExistingUserRequest(int ProcessId, string ProcessOwner)
        {
            var result = _securityApiService.ExistingUserRequestAsync(ProcessId, ProcessOwner).GetAwaiter().GetResult();
            return result.data;
        }
        public int SendApproveProcessReqest(ProcessOwnerViewModel oProcess, int FormID, int ProcRequest_Id, int Action)
        {
            var result = _securityApiService.SendApproveProcessRequestAsync(oProcess, FormID, ProcRequest_Id, Action).GetAwaiter().GetResult();
            return result.data;
        }
        public string CheckProcessOwnerApproverLevel(ProcessOwnerViewModel oProcess)
        {
            var result = _securityApiService.CheckProcessOwnerApproverLevelAsync(oProcess).GetAwaiter().GetResult();
            return result.data;
        }
        public List<ProcessApproval> GetPandingApproval(int iUserId, string sFromDate, string sToDate)
        {
            var result = _securityApiService.GetPandingApprovalAsync(iUserId, sFromDate, sToDate).GetAwaiter().GetResult();
            return result.data;
        }
    }
}
