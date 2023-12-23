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
using Microsoft.SqlServer.Server;

namespace BPA.AppConfig.BusinessLayer.Config
{
    public class BLLocation : ILocationService, IDataOperation<BELocation>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BLLocation"/> class.
        /// </summary>
        public BLLocation()
        { }

        public void DeleteData(BELocation ClObject, int iFormID, BETenant oTenant)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }

        #endregion

        public List<BELocation> GetLocationList(bool bActiveLocation, BETenant oTenant)
        {
            return GetLocationList("", bActiveLocation, oTenant);
        }

        public List<BELocation> GetLocationList(string LocationName, bool bActiveLocation, BETenant oTenant)
        {
            using (DLLocation objLocation = new DLLocation(oTenant))
            {
                return objLocation.GetLocationList(LocationName, bActiveLocation);
            }
        }

        public List<BELocation> GetLocationList(int LocationID, BETenant oTenant)
        {
            using (DLLocation objLocation = new DLLocation(oTenant))
            {
                return objLocation.GetLocationList(LocationID);
            }
        }

        public void InsertData(BELocation oLocation, int FormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(FormID, oLocation.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }

            if (oLocation.sLocationName == string.Empty || oLocation.sLocationName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Location Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }*/
            using (DLLocation objLocation = new DLLocation(oTenant))
            {
                objLocation.ManageLocation(oLocation, "Add");
            }
        }

        public void UpdateData(BELocation oLocation, int FormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(FormID, oLocation.iCreatedBy, PermissionSet.UPDATE))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }

            if (oLocation.sLocationName == string.Empty || oLocation.sLocationName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Location Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }*/
            using (DLLocation objLocation = new DLLocation(oTenant))
            {
                objLocation.ManageLocation(oLocation, "Update");
            }
        }
    }
}
