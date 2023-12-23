using MicUI.WorkManagement.Models.ViewModels;
using MicUI.WorkManagement.Services.ServiceModel;

namespace MicUI.WorkManagement.Module.Administration
{
    public interface IHolidayCalenderMaintenanceService
    {
        List<BEClientInfo> GetClientList(bool isActive, string? searchText = null);
        List<BEProcessInfo> GetProcessListSearch(int iClientID, string ProcessName);
        string GetFirstLastDayOfCalender(int processId, int month, int year);
        string AddProcessOffs(ProcessOffModel objProcessOff);
        string UpdateProcessOffs(ProcessOffModel objProcessOff);
        IList<BEProcessOff> GetProcessOffList(int processId, int month, int year);
        string AddCalendar(CalendarInfoMasterModel objProcessOff);
        string UpdateCalendar(CalendarInfoMasterModel objProcessOff);
        List<CalendarInfoMasterModel> GetCalendarList(string CalenderName,bool IsActive);
        CalendarInfoMasterModel GetCalendarListID(int CalenderId);
        ProcessOffDisplayDetail GetProcessOffsById(int processId, int month, int year);
    }
}
