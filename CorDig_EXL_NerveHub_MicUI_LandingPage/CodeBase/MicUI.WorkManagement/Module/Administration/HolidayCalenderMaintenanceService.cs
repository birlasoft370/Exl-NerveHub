using MicUI.WorkManagement.Models.ViewModels;
using MicUI.WorkManagement.Services.Configuration;
using MicUI.WorkManagement.Services.ServiceModel;

namespace MicUI.WorkManagement.Module.Administration
{
    public class HolidayCalenderMaintenanceService : IHolidayCalenderMaintenanceService
    {
        private readonly IConfigApiService _configService;
        public HolidayCalenderMaintenanceService(IConfigApiService configService)
        {
            _configService = configService; 
        }
        public List<BEClientInfo> GetClientList(bool isActive, string? searchText = null)
        {
            var result = _configService.GetClientAsync(searchText, isActive).GetAwaiter().GetResult();
            return result.data;
        }
        public List<BEProcessInfo> GetProcessListSearch(int iClientID, string ProcessName)
        {
            var result = _configService.GetProcessListSearchAsync(iClientID, ProcessName).GetAwaiter().GetResult();
            return result.data;
        }
        public string GetFirstLastDayOfCalender(int processId, int month, int year)
        {
            var result = _configService.GetFirstLastDayOfCalenderAsync(processId, month, year).GetAwaiter().GetResult();
            return result.data;
        }
        public string AddProcessOffs(ProcessOffModel objProcessOff)
        {
            var result = _configService.AddProcessOffsAsync(objProcessOff).GetAwaiter().GetResult();
            return result.data;
        }
        public string UpdateProcessOffs(ProcessOffModel objProcessOff)
        {
            var result = _configService.UpdateProcessOffsAsync(objProcessOff).GetAwaiter().GetResult();
            return result.data;
        }
        public IList<BEProcessOff> GetProcessOffList(int processId, int month, int year)
        {
            var result = _configService.GetProcessOffListAsync(processId, month, year).GetAwaiter().GetResult();
            return result.data;
        }

        public string AddCalendar(CalendarInfoMasterModel objProcessOff)
        {
            var result = _configService.AddCalendarAsync(objProcessOff).GetAwaiter().GetResult();
            return result.data;
        }
        public string UpdateCalendar(CalendarInfoMasterModel objProcessOff)
        {
            var result = _configService.UpdateCalendarAsync(objProcessOff).GetAwaiter().GetResult();
            return result.data;
        }
        public List<CalendarInfoMasterModel> GetCalendarList(string CalenderName, bool IsActive)
        {
            var result = _configService.GetCalendarListAsync(CalenderName, IsActive).GetAwaiter().GetResult();
            return result.data;
        }
        public CalendarInfoMasterModel GetCalendarListID(int CalenderId)
        {
            var result = _configService.GetCalendarByIdAsync(CalenderId).GetAwaiter().GetResult();
            return result.data;
        }
        public ProcessOffDisplayDetail GetProcessOffsById(int processId, int month, int year)
        {
            var result = _configService.GetProcessOffsByIdAsync(processId,month,year).GetAwaiter().GetResult();
            return result.data;
        }
    }
}
