using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity;
using BPA.Security.ServiceContract.ExternalRef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPA.Security.DataLayer.ExternalRef;

namespace BPA.Security.BusinessLayer.ExternalRef
{
    public class BLFacility : IFacilityService, IDisposable, IDataOperation<BEFacility>
    {
        #region Constructor and Dispose

        /// <summary>
        /// Initializes a new instance of the <see cref="BLERSInfo"/> class.
        /// </summary>
        public BLFacility()
        { }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }

        #endregion Constructor and Dispose

        #region Get Facility List

        /// <summary>
        /// Gets the Facility list.
        /// </summary>
        /// <param name="bActiveFacility">if set to <c>true</c> [active Facility].</param>
        /// <returns>
        /// If bStatus is true,retunr List of all the Active Facility
        /// </returns>
        public List<BEFacility> GetFacilityList(bool bActiveFacility, BETenant oTenant)
        {
            return GetFacilityList("", bActiveFacility, oTenant);
        }

        /// <summary>
        /// Gets the Facility list.
        /// </summary>
        /// <param name="FacilityName">Name of the Facility.</param>
        /// <param name="bActiveFacility">if set to <c>true</c> [active Facility].</param>
        /// <returns>
        /// If bStatus is true,retunr List of all the Active Facility
        /// </returns>
        public List<BEFacility> GetFacilityList(string FacilityName, bool bActiveFacility, BETenant oTenant)
        {
            using (DLFacility objFacility = new DLFacility(oTenant))
            {
                return objFacility.GetFacilityList(FacilityName, bActiveFacility);
            }
        }

        /// <summary>
        /// Gets the Facility list.
        /// </summary>
        /// <param name="FacilityID">The Facility ID.</param>
        /// <returns></returns>
        public List<BEFacility> GetFacilityList(int FacilityID, BETenant oTenant)
        {
            using (DLFacility objFacility = new DLFacility(oTenant))
            {
                return objFacility.GetFacilityList(FacilityID);
            }
        }

        /// <summary>
        /// Gets the Facility list.
        /// </summary>
        /// <param name="LocationID">The Location ID.</param>
        /// <returns></returns>
        public List<BEFacility> GetLocationWiseFacilityList(int LocationID, BETenant oTenant)
        {
            using (DLFacility objFacility = new DLFacility(oTenant))
            {
                return objFacility.GetLocationWiseFacilityList(LocationID);
            }
        }

        public List<BEFacility> GetFacilityList(BETenant oTenant)
        {
            using (DLFacility objFacility = new DLFacility(oTenant))
            {
                return objFacility.GetFacilityList(true);
            }
        }

        #endregion Get Facility List

        #region Insert Facility Details

        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <param name="oFacility">Facility.</param>
        /// <param name="FormID">The form ID.</param>
        public void InsertData(BEFacility oFacility, int FormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, oFacility.iCreatedBy, PermissionSet.ADD))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            if (oFacility.sFacilityName == string.Empty || oFacility.sFacilityName == "")
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Facility Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            using (DLFacility objFacility = new DLFacility(oTenant))
            {
                objFacility.ManageFacility(oFacility, "Add");
            }
        }

        #endregion Insert Facility Details

        #region Update Facility Details

        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <param name="oFacility">Facility.</param>
        /// <param name="FormID">The form ID.</param>
        public void UpdateData(BEFacility oFacility, int FormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, oFacility.iCreatedBy, PermissionSet.UPDATE))
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }
            if (oFacility.sFacilityName == string.Empty || oFacility.sFacilityName == "")
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Facility Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            using (DLFacility objFacility = new DLFacility(oTenant))
            {
                objFacility.ManageFacility(oFacility, "Update");
            }
        }

        #endregion Update Facility Details

        #region Delete Facility Details

        /// <summary>
        /// Deletes the data.
        /// </summary>
        /// <param name="oFacility">Facility.</param>
        /// <param name="FormID">The form ID.</param>
        public void DeleteData(BEFacility oFacility, int FormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, oFacility.iCreatedBy, PermissionSet.DELETE))
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.DeletePermission);
            }
            if (oFacility.sFacilityName == string.Empty || oFacility.sFacilityName == "")
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Facility Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            using (DLFacility objFacility = new DLFacility(oTenant))
            {
                objFacility.ManageFacility(oFacility, "Delete");
            }
        }

        #endregion Delete Facility Details
    }
}