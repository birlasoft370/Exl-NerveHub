using MicUI.WorkManagement.Models.ViewModels;
using MicUI.WorkManagement.Services.Configuration;
using System;
using Telerik.SvgIcons;

namespace MicUI.WorkManagement.Module.Administration
{
    public class CalenderService:ICalenderService
    {
        private readonly IConfigApiService _configService;
        public CalenderService(IConfigApiService configService)
        {
            _configService = configService;
        }
        public int GetMaxWeek(int calanderId, int Year)
        {
            var result = _configService.GetMaxWeekAsync(calanderId, Year).GetAwaiter().GetResult();
            return result.data;
        }
        public List<CalendarDataModel> GetCalendarDataList(int CalendarID, int Month, int Year)
         {
            var result = _configService.GetCalendarDataListAsync(CalendarID, Month, Year).GetAwaiter().GetResult();
            return result.data;
        }
        public CalendarDataDetails GetCalendarDataById(int CalendarID, int Month, int Year)
        {
            var result = _configService.GetCalendarDataIDAsync(CalendarID, Month, Year).GetAwaiter().GetResult();
            return result.data;
        }
        public string AddCalenderData(CalendarDataDetails strCalendarDataModel)
        {
            var result = _configService.AddCalenderDataAsync(strCalendarDataModel).GetAwaiter().GetResult();
            return result.data;
        }
       public string UpdateCalenderData(CalendarDataDetails strCalendarDataModel)
        {
            var result = _configService.UpdateCalenderDataAsync(strCalendarDataModel).GetAwaiter().GetResult();
            return result.data;
        }

    }
}
