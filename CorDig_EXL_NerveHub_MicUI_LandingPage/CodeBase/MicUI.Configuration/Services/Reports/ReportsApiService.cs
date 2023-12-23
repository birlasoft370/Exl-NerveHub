using MicUI.Configuration.Models.Response;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Services.Reports
{
    public class ReportsApiService : BaseApiService, IReportsApiService
    {
        public ReportsApiService(HttpClient client, IHttpContextAccessor httpContextAccessor) : base(client, httpContextAccessor)
        {

        }

        public async Task<MessageResponse<List<BEJobCodeInfo>>> GetJobAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEJobCodeInfo>>>("ConfigurationReports/GetJob");
            return content ?? new MessageResponse<List<BEJobCodeInfo>>();
        }
        public async Task<MessageResponse<List<BEErpJobRoleMap>>> GetERPJobRoleMappingReportAsync(int roleJobId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEErpJobRoleMap>>>($"ConfigurationReports/GetERPJobRoleMappingReport?roleJobId={roleJobId}");
            return content ?? new MessageResponse<List<BEErpJobRoleMap>>();
        }

        public async Task<MessageResponse<List<ProcessOwnerReport>>> GetProcessOwnerReportAsync(int clientId, int processId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<ProcessOwnerReport>>>($"ConfigurationReports/GetProcessOwnerReport?ClientId={clientId}&ProcessId={processId}");
            return content ?? new MessageResponse<List<ProcessOwnerReport>>();
        }
        public async Task<MessageResponse<List<ProcessOwnerApprovalReport>>> GetProcessOwnerApprovalReportAsync(string startDate, string endDate, string status)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<ProcessOwnerApprovalReport>>>($"ConfigurationReports/GetProcessOwnerApprovalReport?startDate={startDate}&endDate={endDate}&status={status}");
            return content ?? new MessageResponse<List<ProcessOwnerApprovalReport>>();
        }
        public async Task<MessageResponse<List<LogDetailsReport>>> GetExceptionHandlerReportAsync(string StartDate, string EndDate, string SeverityFlag)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<LogDetailsReport>>>($"ConfigurationReports/GetExceptionHandlerReport?StartDate={StartDate}&EndDate={EndDate}&MachineName={SeverityFlag}");
            return content ?? new MessageResponse<List<LogDetailsReport>>();
        }
        public async Task<MessageResponse<LogDetailsReport>> GetExceptionHandlerReport_MessageAsync(int logId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<LogDetailsReport>>($"ConfigurationReports/GetExceptionHandlerReport_Message?logId={logId}");
            return content ?? new MessageResponse<LogDetailsReport>();
        }
        public async Task<MessageResponse<string>> GetMonthlyUsageReportsAsync(DateTime fromDate, DateTime toDate)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<string>>($"UsageReport/GetMonthlyUsageReports?fromDate={fromDate}&toDate={toDate}");
            return content ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<List<AccessControlReport>>> GetAccessControlReportAsync(string clientId, string processId, string teamId, string roleId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<AccessControlReport>>>($"ConfigurationReports/GetAccessControlReport?clientId={clientId}&processId={processId}&teamId={teamId}&roleId={roleId}");
            return content ?? new MessageResponse<List<AccessControlReport>>();
        }
        public async Task<MessageResponse<List<RoleFormAccessModel>>> GetRoleFormMapAsync(string roleJobId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<RoleFormAccessModel>>>($"ConfigurationReports/GetRoleFormAccessReport?Role={roleJobId}");
            return content ?? new MessageResponse<List<RoleFormAccessModel>>();
        }

        public async Task<MessageResponse<List<BERoleInfo>>> GetRoleListAsync(bool bActiveRoles)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BERoleInfo>>>($"ConfigurationReports/GetRoleFormAccessReport?roleJobId={bActiveRoles}");
            return content ?? new MessageResponse<List<BERoleInfo>>();
        }

        public async Task<MessageResponse<List<BEUserInfo>>> GetUserListAsync(string sLoginName, bool bActiveUser, int iLoggedinUserID, string SearchCondition)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEUserInfo>>>($"ConfigurationReports/GetRequestedBy?loggedinUserId={iLoggedinUserID}&activeUser={bActiveUser}&searchText={sLoginName}");
            return content ?? new MessageResponse<List<BEUserInfo>>();
        }
        public async Task<MessageResponse<List<UserAccessRequestStatus>>> GetUserAccessRequestStatusReportAsync(int UserId, string StartDate, string EndDate, int RequestedFor, int RequestedBy, int RequestedStatus)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<UserAccessRequestStatus>>>($"ConfigurationReports/GetUserAccessRequestStatusReport?userId={UserId}&startDate={StartDate}&endDate={EndDate}&requestedFor={RequestedFor}&requestedBy={RequestedBy}&requestedStatus={RequestedStatus}");
            return content ?? new MessageResponse<List<UserAccessRequestStatus>>();
        }
    }
}
