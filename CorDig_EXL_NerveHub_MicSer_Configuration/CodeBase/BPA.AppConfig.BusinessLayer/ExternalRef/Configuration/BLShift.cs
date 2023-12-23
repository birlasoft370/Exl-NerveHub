using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation;
using BPA.AppConfig.Datalayer.ExternalRef.WorkAllocation;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessLayer.ExternalRef.Configuration
{
    public class BLShift : IShiftService, IDisposable //,  IDataOperation<BEShiftInfo>
    {
        public void Dispose()
        { }
        public List<BEShiftInfo> GetShiftList(bool bGetAll, BETenant oTenant)
        {
            return GetShiftList("", bGetAll, oTenant);
        }
        public List<BEShiftInfo> GetShiftList(string sShiftName, bool bGetAll, BETenant oTenant)
        {
            using (DLShift oShift = new DLShift(oTenant))
            {
                return oShift.GetShiftList(sShiftName, bGetAll);
            }
        }

        public List<BEShiftInfo> GetShiftList(int iShiftID, BETenant oTenant)
        {
            using (DLShift oShift = new DLShift(oTenant))
            {
                return oShift.GetShiftList(iShiftID);
            }
        }
        public string InsertData(BEShiftInfo oShift, int iFormID, BETenant oTenant)
        {
            /*
            if (!BLCheckPermission.hasPermission(iFormID, oShift.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            if (oShift.sShiftName == string.Empty || oShift.sShiftName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Shift Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }*/
            using (DLShift objShift = new DLShift(oTenant))
            {
                return objShift.InsertData(oShift);
            }
        }

        public void UpdateData(BEShiftInfo oShift, int iFormID, BETenant oTenant)
        {
            /*
            if (oShift.sShiftName == string.Empty || oShift.sShiftName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Shift Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            if (!BLCheckPermission.hasPermission(iFormID, oShift.iCreatedBy, PermissionSet.UPDATE))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }*/
            using (DLShift objShift = new DLShift(oTenant))
            {
                objShift.UpdateData(oShift);
            }

        }

    }
}
