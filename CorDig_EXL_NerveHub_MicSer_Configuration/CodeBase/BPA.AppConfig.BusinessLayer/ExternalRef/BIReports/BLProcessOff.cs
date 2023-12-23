using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef.BIReports;
using BPA.AppConfig.Datalayer.ExternalRef.BIReports;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.BIReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessLayer.ExternalRef.BIReports
{
    public class BLProcessOff : IProcessOffService, IDisposable
    {
        public void Dispose()
        { }

        public BLProcessOff() { }

        public IList<BEProcessOff> GetProcessOffList(int iProcessId, int iYear, int iMonth, bool bGetActive, BETenant oTenant)
        {
            using (DLProcessOff oProcessOff = new DLProcessOff(oTenant))
            {
                return oProcessOff.GetProcessOffList(iProcessId, iYear, iMonth, bGetActive);
            }
        }
        public IList<BEProcessOff> GetProcessOffListProcess(int iProcessID, int Month, int Year, BETenant oTenant)
        {
            using (DLProcessOff oProcessOff = new DLProcessOff(oTenant))
            {
                return oProcessOff.GetProcessOffListProcess(iProcessID, Month, Year);
            }
        }
        public string GetFirstLastDayOfCalender(int iProcessID, int Month, int Year, BETenant oTenant)
        {
            using (DLProcessOff oProcessOff = new DLProcessOff(oTenant))
            {
                return oProcessOff.GetFirstLastDayOfCalender(iProcessID, Month, Year);
            }
        }
        public IList<BEProcessOff> GetProcessOffListProcessDayWise(int iProcessID, int Month, int Year, BETenant oTenant)
        {
            using (DLProcessOff oProcessOff = new DLProcessOff(oTenant))
            {
                return oProcessOff.GetProcessOffListProcessDayWise(iProcessID, Month, Year);
            }
        }
        public void InsertData(BEProcessOff oProcessOff, int iFormID, BETenant oTenant)
        {
            /*
            if (!BLCheckPermission.hasPermission(iFormID, oProcessOff.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            */


            using (DLProcessOff objProcessOff = new DLProcessOff(oTenant))
            {
                objProcessOff.InsertData(oProcessOff);
            }
        }
        public void UpdateData(BEProcessOff oProcessOff, int iFormID, BETenant oTenant)
        {
            /*
           if (!BLCheckPermission.hasPermission(iFormID, oProcessOff.iCreatedBy, PermissionSet.UPDATE))
           {
               throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
           }
            */
            using (DLProcessOff objProcessOff = new DLProcessOff(oTenant))
            {
                objProcessOff.UpdateData(oProcessOff);
            }
        }
    }
}
