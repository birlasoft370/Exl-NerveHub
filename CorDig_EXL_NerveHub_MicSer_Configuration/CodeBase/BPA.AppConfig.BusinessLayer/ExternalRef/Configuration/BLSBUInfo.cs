using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef.Configuration;
using BPA.AppConfig.Datalayer.ExternalRef.Configuration;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.Configuration;
using System.Security;

namespace BPA.AppConfig.BusinessLayer.ExternalRef.Configuration
{
    public class BLSBUInfo : ISBUInfoService, IDisposable//, IDataOperation<BESBUInfo>
    {
        #region Dispose
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }
        #endregion

        public List<BESBUInfo> GetSBUList(bool IsActiveSBU, BETenant oTenant)
        {
            return GetSBUList("", IsActiveSBU, oTenant);
        }
        public List<BESBUInfo> GetSBUList(string sName, bool IsActiveSBU, BETenant oTenant)
        {
            using (DLSBUInfo objSBU = new DLSBUInfo(oTenant))
            {
                return objSBU.GetSBUList(sName, IsActiveSBU);
            }
        }

        public List<BESBUInfo> GetSBUList(int iSBUID, BETenant oTenant)
        {
            using (DLSBUInfo objSBU = new DLSBUInfo(oTenant))
            {
                return objSBU.GetSBUList(iSBUID);
            }
        }

        public int GetMaxClientId(BETenant oTenant)
        {
            //if (oCalendar.sCalendarName == string.Empty || oCalendar.sCalendarName == "")
            //{
            //    throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Calendar Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            //}
            using (DLSBUInfo objsbUiNFO = new DLSBUInfo(oTenant))
            {
                int flag = objsbUiNFO.GetMaxClientId();
                return flag;
            }
        }

        public List<BESBUInfo> GetSBUListbasedONClient(int iclientid, BETenant oTenant)
        {
            using (DLSBUInfo objSBU = new DLSBUInfo(oTenant))
            {
                return objSBU.GetSBUListbasedONClient(iclientid);
            }
        }

        public void InsertDataSBU(BESBUInfo oSBU, int FormID, BETenant oTenant)
        {
            /*
            if (!BLCheckPermission.hasPermission(FormID, oSBU.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }

            if (oSBU.sName == string.Empty || oSBU.sName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("SBU Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }*/
            using (DLSBUInfo objSBU = new DLSBUInfo(oTenant))
            {
                objSBU.InsertDataSBU(oSBU);
            }
        }

        public void UpdateDataSBU(BESBUInfo oSBU, int FormID, BETenant oTenant)
        {
            /*
            if (!BLCheckPermission.hasPermission(FormID, oSBU.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }

            if (oSBU.sName == string.Empty || oSBU.sName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("SBU Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }*/
            using (DLSBUInfo objSBU = new DLSBUInfo(oTenant))
            {
                objSBU.UpdateDataSBU(oSBU);
            }
        }

        public string InsertData(BESBUInfo oSBU, int FormID, BETenant oTenant)
        {
            /*
            if (!BLCheckPermission.hasPermission(FormID, oSBU.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }

            if (oSBU.sName == string.Empty || oSBU.sName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("SBU Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }*/
            using (DLSBUInfo objSBU = new DLSBUInfo(oTenant))
            {
                return objSBU.InsertData(oSBU);
            }
        }

        public void UpdateData(BESBUInfo oSBU, int FormID, BETenant oTenant)
        {
            /*
            if (!BLCheckPermission.hasPermission(FormID, oSBU.iCreatedBy, PermissionSet.UPDATE))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }

            if (oSBU.sName == string.Empty || oSBU.sName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("SBU Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }*/
            using (DLSBUInfo objSBU = new DLSBUInfo(oTenant))
            {
                objSBU.UpdateData(oSBU);
            }
        }

    }
}
