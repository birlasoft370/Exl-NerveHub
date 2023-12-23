using MicUI.Configuration.Models.Response;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Services.Reports
{
    public interface IReportsApiService
    {
        Task<MessageResponse<List<BEJobCodeInfo>>> GetJobAsync();
        Task<MessageResponse<List<BEErpJobRoleMap>>> GetERPJobRoleMappingReportAsync(int roleJobId);
        Task<MessageResponse<List<ProcessOwnerReport>>> GetProcessOwnerReportAsync(int clientId, int processId);
        Task<MessageResponse<List<ProcessOwnerApprovalReport>>> GetProcessOwnerApprovalReportAsync(string startDate, string endDate, string status);
        Task<MessageResponse<List<LogDetailsReport>>> GetExceptionHandlerReportAsync(string StartDate, string EndDate, string SeverityFlag);
        Task<MessageResponse<LogDetailsReport>> GetExceptionHandlerReport_MessageAsync(int logId);
        Task<MessageResponse<string>> GetMonthlyUsageReportsAsync(DateTime fromDate, DateTime toDate);
        Task<MessageResponse<List<AccessControlReport>>> GetAccessControlReportAsync(string clientId,string processId,string teamId, string roleId);
        Task<MessageResponse<List<RoleFormAccessModel>>> GetRoleFormMapAsync(string roleJobId);
        Task<MessageResponse<List<BERoleInfo>>> GetRoleListAsync(bool bActiveRoles);
        Task<MessageResponse<List<BEUserInfo>>> GetUserListAsync(string sLoginName, bool bActiveUser, int iLoggedinUserID, string SearchCondition);
        Task<MessageResponse<List<UserAccessRequestStatus>>> GetUserAccessRequestStatusReportAsync(int UserId, string StartDate, string EndDate, int RequestedFor, int RequestedBy, int RequestedStatus);
    }
}
