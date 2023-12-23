using MicUI.Configuration.Services.Reports;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Reports
{
    public interface IReportsService
    {
        List<ProcessOwnerReport> GetProcessOwnerReport(int ClientID, int ProcessID);
        List<ProcessOwnerApprovalReport> GetProcessOwnerApprovalReport(string startDate, string endDate, string status);
        List<LogDetailsReport> GetLogDetailsReport(string StartDate, string EndDate, string MachineName);
        LogDetailsReport GetMessageDetails(int LogID);
        List<AccessControlReport> GetMultiACLReport(string sClientID, string sProcessID, string sTeamID, string sRoleID);
        List<UserAccessRequestStatus> GetUserAccessRequestStatusReport(int UserId, string StartDate, string EndDate, int RequestedFor, int RequestedBy, int RequestedStatus);
    }
    public class ReportsService : IReportsService
    {
        private readonly IReportsApiService _reportsService;

        public ReportsService(IReportsApiService reportsService)
        {
            _reportsService = reportsService;
        }

        public List<ProcessOwnerReport> GetProcessOwnerReport(int ClientID, int ProcessID)
        {
            var result = _reportsService.GetProcessOwnerReportAsync(ClientID, ProcessID).GetAwaiter().GetResult();
            return result.data ?? new List<ProcessOwnerReport>();
        }
        public List<ProcessOwnerApprovalReport> GetProcessOwnerApprovalReport(string startDate, string endDate, string status)
        {
            var result = _reportsService.GetProcessOwnerApprovalReportAsync(startDate, endDate, status).GetAwaiter().GetResult();
            return result.data ?? new List<ProcessOwnerApprovalReport>();
        }

        public List<LogDetailsReport> GetLogDetailsReport(string StartDate, string EndDate, string MachineName)
        {
            var result = _reportsService.GetExceptionHandlerReportAsync(StartDate, EndDate, MachineName).GetAwaiter().GetResult();
            return result.data ?? new List<LogDetailsReport>();
        }
        public LogDetailsReport GetMessageDetails(int LogID)
        {
            var result = _reportsService.GetExceptionHandlerReport_MessageAsync(LogID).GetAwaiter().GetResult();
            return result.data ?? new LogDetailsReport();
        }
        public List<AccessControlReport> GetMultiACLReport(string sClientID, string sProcessID, string sTeamID, string sRoleID)
        {
            var result = _reportsService.GetAccessControlReportAsync(sClientID, sProcessID, sTeamID, sRoleID).GetAwaiter().GetResult();
            return result.data ?? new List<AccessControlReport>();
        }
        public List<UserAccessRequestStatus> GetUserAccessRequestStatusReport(int UserId, string StartDate, string EndDate, int RequestedFor, int RequestedBy, int RequestedStatus)
        {
            var result = _reportsService.GetUserAccessRequestStatusReportAsync(UserId, StartDate, EndDate, RequestedFor, RequestedBy, RequestedStatus).GetAwaiter().GetResult();
            return result.data ?? new List<UserAccessRequestStatus>();
        }
    }
}
