using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.Config
{
    public interface ICalenderService : IDisposable
    {
        IList<BECalendarInfo> GetCalendarList(bool IsActiveCalendar, BETenant oTenant);
        IList<BECalendarInfo> GetCalendarList(string CalendarName, bool IsActiveCalendar, BETenant oTenant);
        IList<BECalendarInfo> GetCalendarList(int CalendarID, BETenant oTenant);
        int InsertData(BECalendarInfo oCalendar, int FormID, BETenant oTenant);
        void UpdateData(BECalendarInfo oCalendar, int FormID, BETenant oTenant);
        IList<BECalendarInfo> GetCalendarDataList(int CalendarID, int Year, int Month, bool IsActiveCalendar, BETenant oTenant);
        IList<BECalendarInfo> GetCalendarDataList(string CalendarDataName, bool IsActiveCalendar, BETenant oTenant);
        IList<BECalendarInfo> GetCalendarDataList(BECalendarInfo oCalendar, BETenant oTenant);
        string ManageCalendarData(BECalendarInfo oCalendar, int FormID, BETenant oTenant);
        void DeleteData(BECalendarInfo oCalendar, int FormID, BETenant oTenant);
        int GetMaxWeek(BECalendarInfo oCalendar, BETenant oTenant);
    }
}
