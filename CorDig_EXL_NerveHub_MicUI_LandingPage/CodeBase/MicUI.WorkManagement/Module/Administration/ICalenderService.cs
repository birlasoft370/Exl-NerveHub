using MicUI.WorkManagement.Models.ViewModels;

namespace MicUI.WorkManagement.Module.Administration
{
    public interface ICalenderService
    {
        int GetMaxWeek(int calanderId, int Year);
        List<CalendarDataModel> GetCalendarDataList(int CalendarID, int Month, int Year);
         CalendarDataDetails GetCalendarDataById(int CalendarID, int Month, int Year);
        string AddCalenderData(CalendarDataDetails strCalendarDataModel);
        string UpdateCalenderData(CalendarDataDetails strCalendarDataModel);


    }
}
