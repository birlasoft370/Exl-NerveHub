using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.UserPreference
{
    public interface IUserPreferenceService
    {
        int SaveUpdateUserPreference(UserPreferenceViewModel objBEUserPreference);
        BEUserPreference GetUserPreferenceDetail();
        string GetMonthlyUsageReports(DateTime StartDate, DateTime EndDate);
    }
}
