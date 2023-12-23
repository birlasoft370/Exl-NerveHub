using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Services.Configuration;
using MicUI.Configuration.Services.Reports;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.UserPreference
{
    public class UserPreferenceService : IUserPreferenceService
    {
        private readonly IConfigApiService _configService;
        private readonly IReportsApiService _reportsService;
        public UserPreferenceService(IConfigApiService configService, IReportsApiService reportsService)
        {
            _configService = configService;
            _reportsService = reportsService;
        }

        public BEUserPreference GetUserPreferenceDetail()
        {
            var result = _configService.GetUserPreferenceAsync().GetAwaiter().GetResult();
            return result.data;
        }

        public int SaveUpdateUserPreference(UserPreferenceViewModel objBEUserPreference)
        {
            var result = _configService.SaveUpdateUserPreferenceAsync(objBEUserPreference).GetAwaiter().GetResult();
            return result.data;
        }
        public string GetMonthlyUsageReports(DateTime StartDate, DateTime EndDate)
        {
            var result = _reportsService.GetMonthlyUsageReportsAsync(StartDate, EndDate).GetAwaiter().GetResult();
            return result.data ?? "";
        }
    }
}
