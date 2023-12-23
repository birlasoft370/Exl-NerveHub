using BPA.AppConfig.BusinessEntity;
using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.Datalayer.Config;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessLayer.Config
{
    public class BLVertical : IVerticalService, IDisposable, IDataOperation<BEVerticalInfo>
    {
        public void Dispose()
        { }

        public List<BEVerticalInfo> GetVerticalList(bool IsActiveVertical, BETenant oTenant)
        {
            return GetVerticalList("", IsActiveVertical, oTenant);
        }

        public List<BEVerticalInfo> GetVerticalList(string VerticalName, bool IsActiveVertical, BETenant oTenant)
        {
            using (DLVertical objVertical = new DLVertical(oTenant))
            {
                return objVertical.GetVerticalList(VerticalName, IsActiveVertical);
            }
        }
        public List<BEVerticalInfo> GetVerticalList(int VerticalID, BETenant oTenant)
        {
            using (DLVertical objVertical = new DLVertical(oTenant))
            {
                return objVertical.GetVerticalList(VerticalID);
            }
        }

        public void InsertData(BEVerticalInfo oVertical, int FormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(FormID, oVertical.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }

            if (oVertical.sVerticalName == string.Empty || oVertical.sVerticalName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Vertical Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }*/
            using (DLVertical objVertical = new DLVertical(oTenant))
            {
                objVertical.ManageVertical(oVertical, "Add");
            }
        }

        public void UpdateData(BEVerticalInfo oVertical, int FormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(FormID, oVertical.iCreatedBy, PermissionSet.UPDATE))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }

            if (oVertical.sVerticalName == string.Empty || oVertical.sVerticalName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Vertical Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }*/
            using (DLVertical objVertical = new DLVertical(oTenant))
            {
                objVertical.ManageVertical(oVertical, "Update");
            }
        }

        public void DeleteData(BEVerticalInfo ClObject, int iFormID, BETenant oTenant)
        {
            throw new NotImplementedException();
        }   
    }
}
