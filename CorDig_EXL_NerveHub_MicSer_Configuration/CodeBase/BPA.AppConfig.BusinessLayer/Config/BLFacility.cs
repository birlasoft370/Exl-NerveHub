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
    public class BLFacility : IFacilityService, IDisposable, IDataOperation<BEFacility>
    {
        public BLFacility()
        { }

        public void Dispose()
        { }

        public List<BEFacility> GetFacilityList(bool bActiveFacility, BETenant oTenant)
        {
            return GetFacilityList("", bActiveFacility, oTenant);
        }
        public List<BEFacility> GetFacilityList(string FacilityName, bool bActiveFacility, BETenant oTenant)
        {
            using (DLFacility objFacility = new DLFacility(oTenant))
            {
                return objFacility.GetFacilityList(FacilityName, bActiveFacility);
            }
        }
        public List<BEFacility> GetFacilityList(int FacilityID, BETenant oTenant)
        {
            using (DLFacility objFacility = new DLFacility(oTenant))
            {
                return objFacility.GetFacilityList(FacilityID);
            }
        }
        public void InsertData(BEFacility oFacility, int FormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(FormID, oFacility.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            if (oFacility.sFacilityName == string.Empty || oFacility.sFacilityName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Facility Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }*/
            using (DLFacility objFacility = new DLFacility(oTenant))
            {
                objFacility.ManageFacility(oFacility, "Add");
            }
        }
        public void UpdateData(BEFacility oFacility, int FormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(FormID, oFacility.iCreatedBy, PermissionSet.UPDATE))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }
            if (oFacility.sFacilityName == string.Empty || oFacility.sFacilityName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Facility Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }*/
            using (DLFacility objFacility = new DLFacility(oTenant))
            {
                objFacility.ManageFacility(oFacility, "Update");
            }
        }
        public void DeleteData(BEFacility ClObject, int iFormID, BETenant oTenant)
        {
            throw new NotImplementedException();
        }

    }
}
