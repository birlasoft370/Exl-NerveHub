using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Services.ServiceModel;
using System.Data;

namespace MicUI.Configuration.Module.Security
{
    public interface IProcessOwnerService
    {
       List<BEProcessInfo> GetClientWiseProcessList(int userId, int ClientId);
        List<BEApproval> GetUserProcessOwner(int processId);
       string ExistingUserRequest(int ProcessId, string ProcessOwner);
       int SendApproveProcessReqest(ProcessOwnerViewModel oProcess, int FormID, int ProcRequest_Id, int Action);
        string CheckProcessOwnerApproverLevel(ProcessOwnerViewModel oProcess);
        List<ProcessApproval> GetPandingApproval(int iUserId, string sFromDate, string sToDate);
        ProcessOwnerApprovalUser GetUserProcessOwnerUserList(int processId);

    }
}
