using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.Datalayer.Config;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;

namespace BPA.AppConfig.BusinessLayer.Config
{
    public class BLCalendar : IDisposable, ICalenderService
    {

        public BLCalendar()
        {

        }      
        public void Dispose()
        {

        }
        public IList<BECalendarInfo> GetCalendarList(bool IsActiveCalendar, BETenant oTenant)
        {
            return GetCalendarList("", IsActiveCalendar, oTenant);
        }
        public IList<BECalendarInfo> GetCalendarList(string CalendarName, bool IsActiveCalendar, BETenant oTenant)
        {
            using (DLCalendar objCalendar = new DLCalendar(oTenant))
            {
                return objCalendar.GetCalendarList(CalendarName, IsActiveCalendar);
            }
        }
        public IList<BECalendarInfo> GetCalendarList(int CalendarID, BETenant oTenant)
        {
            using (DLCalendar objCalendar = new DLCalendar(oTenant))
            {
                return objCalendar.GetCalendarList(CalendarID);
            }
        }
        public int InsertData(BECalendarInfo oCalendar, int FormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(FormID, oCalendar.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }

            if (oCalendar.sCalendarName == string.Empty || oCalendar.sCalendarName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Calendar Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }*/
            using (DLCalendar objCalendar = new DLCalendar(oTenant))
            {
                return (objCalendar.InsertData(oCalendar));
            }
        }
        public void UpdateData(BECalendarInfo oCalendar, int FormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(FormID, oCalendar.iCreatedBy, PermissionSet.UPDATE))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }

            if (oCalendar.sCalendarName == string.Empty || oCalendar.sCalendarName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Calendar Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }*/
            using (DLCalendar objCalendar = new DLCalendar(oTenant))
            {
                objCalendar.UpdateData(oCalendar);
            }
        }
        public IList<BECalendarInfo> GetCalendarDataList(int CalendarID, int Year, int Month, bool IsActiveCalendar, BETenant oTenant)
        {
            using (DLCalendar objCalendar = new DLCalendar(oTenant))
            {
                return objCalendar.GetCalendarDataList(CalendarID, Year, Month, IsActiveCalendar);
            }
        }
        public IList<BECalendarInfo> GetCalendarDataList(bool IsActiveCalendar, BETenant oTenant)
        {
            return GetCalendarDataList("", IsActiveCalendar, oTenant);
        }
        public IList<BECalendarInfo> GetCalendarDataList(string CalendarDataName, bool IsActiveCalendar, BETenant oTenant)
        {
            using (DLCalendar objCalendar = new DLCalendar(oTenant))
            {
                return objCalendar.GetCalendarDataList(CalendarDataName, IsActiveCalendar);
            }
        }
        public IList<BECalendarInfo> GetCalendarDataList(BECalendarInfo oCalendar, BETenant oTenant)
        {
            using (DLCalendar objCalendar = new DLCalendar(oTenant))
            {
                return objCalendar.GetCalendarDataList(oCalendar);
            }
        }
        public int GetMaxWeek(BECalendarInfo oCalendar, BETenant oTenant)
        {
            using (DLCalendar objCalendar = new DLCalendar(oTenant))
            {
                return objCalendar.GetMaxWeek(oCalendar);
            }
        }
        public string ManageCalendarData(BECalendarInfo oCalendar, int FormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(FormID, oCalendar.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }*/
            using (DLCalendar objCalendar = new DLCalendar(oTenant))
            {
                return objCalendar.ManageCalendarData(oCalendar);
            }
        }
        public void DeleteData(BECalendarInfo oCalendar, int FormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(FormID, oCalendar.iCreatedBy, PermissionSet.DELETE))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.DeletePermission);
            }*/
            using (DLCalendar objCalendar = new DLCalendar(oTenant))
            {
                objCalendar.DeleteData(oCalendar);
            }
        }
    }
}
