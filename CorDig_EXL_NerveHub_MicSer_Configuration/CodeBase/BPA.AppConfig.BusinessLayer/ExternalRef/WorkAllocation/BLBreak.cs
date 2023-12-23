using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation;
using BPA.AppConfig.Datalayer.ExternalRef.WorkAllocation;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.WorkAllocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessLayer.ExternalRef.WorkAllocation
{
    public class BLBreak : IBreakService, IDisposable
    {
        public BLBreak()
        { }
        public void Dispose()
        { }

        public List<BEBreakInfo> GetBreakList(bool bGetActive, BETenant oTenant)
        {
            return GetBreakList("", bGetActive, oTenant);
        }

        public List<BEBreakInfo> GetBreakList(string sBreakName, bool bGetActive, BETenant oTenant)
        {
            using (DLBreakCode oBreakCode = new DLBreakCode(oTenant))
            {
                return oBreakCode.GetBreakList(sBreakName, bGetActive);
            }
        }

        public List<BEBreakInfo> GetBreakListByProcessID(int iProcessID, BETenant oTenant)
        {
            using (DLBreakCode oBreakCode = new DLBreakCode(oTenant))
            {
                return oBreakCode.GetBreakListByProcessID(iProcessID);
            }
        }
        public List<BEBreakInfo> GetBreakListBySearch(string sBreakName, int iProcessID, BETenant oTenant)
        {
            using (DLBreakCode oBreakCode = new DLBreakCode(oTenant))
            {
                return oBreakCode.GetBreakListBySearch(sBreakName, iProcessID);
            }
        }
        public List<BEBreakInfo> GetBreakList(int iBreakID, BETenant oTenant)
        {
            using (DLBreakCode oBreakCode = new DLBreakCode(oTenant))
            {
                return oBreakCode.GetBreakList(iBreakID);
            }
        }
        public void InsertData(BEBreakInfo oBreak, int iFormID, BETenant oTenant)
        {
            /*
            if (!BLCheckPermission.hasPermission(iFormID, oBreak.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }

            if (oBreak.sBreakName == string.Empty || oBreak.sBreakName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Break Name" + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            */
            using (DLBreakCode objBreak = new DLBreakCode(oTenant))
            {
                objBreak.InsertData(oBreak);
            }
        }
        public void UpdateData(BEBreakInfo oBreak, int iFormID, BETenant oTenant)
        {
            /*
            if (!BLCheckPermission.hasPermission(iFormID, oBreak.iCreatedBy, PermissionSet.UPDATE))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }
            if (oBreak.sBreakName == string.Empty || oBreak.sBreakName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Break Name" + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            */
            using (DLBreakCode objBreak = new DLBreakCode(oTenant))
            {
                objBreak.UpdateData(oBreak);
            }
        }
    }
}
