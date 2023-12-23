using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.BusinessEntity;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.Datalayer.Config;

namespace BPA.AppConfig.BusinessLayer.Config
{
    public class BLTimeZone : ITimeZoneService, IDisposable, IDataOperation<BETimeZoneInfo>
    {
        #region Dispose
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="BLTimeZone"/> class.
        /// </summary>
        public BLTimeZone()
        { }
        #endregion

        public List<BETimeZoneInfo> GetTimeZoneList(bool bGetAll, BETenant oTenant)
        {
            return GetTimeZoneList("", bGetAll, oTenant);
        }

        public List<BETimeZoneInfo> GetTimeZoneList(string sTimeZoneName, bool bGetAll, BETenant oTenant)
        {
            using (DLTimeZone oTimeZone = new DLTimeZone(oTenant))
            {
                return oTimeZone.GetTimeZoneList(sTimeZoneName, bGetAll);
            }
        }

        public List<BETimeZoneInfo> GetTimeZoneList(int iTimeZoneID, BETenant oTenant)
        {
            using (DLTimeZone oTimeZone = new DLTimeZone(oTenant))
            {
                return oTimeZone.GetTimeZoneList(iTimeZoneID);
            }
        }

        public void InsertData(BETimeZoneInfo oTimeZone, int iFormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(iFormID, oTimeZone.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }

            if (oTimeZone.sTimeZoneName == string.Empty || oTimeZone.sTimeZoneName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("TimeZone Name" + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            */
            using (DLTimeZone objTimeZone = new DLTimeZone(oTenant))
            {
                objTimeZone.ManageTimeZone(oTimeZone, "Add");
            }
        }

        public void UpdateData(BETimeZoneInfo oTimeZone, int iFormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(iFormID, oTimeZone.iCreatedBy, PermissionSet.UPDATE))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }
            if (oTimeZone.sTimeZoneName == string.Empty || oTimeZone.sTimeZoneName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("TimeZone Name" + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }*/

            using (DLTimeZone objTimeZone = new DLTimeZone(oTenant))
            {
                objTimeZone.ManageTimeZone(oTimeZone, "Update");
            }
        }

        public void DeleteData(BETimeZoneInfo ClObject, int iFormID, BETenant oTenant)
        {
            throw new NotImplementedException();
        }
    }
}
