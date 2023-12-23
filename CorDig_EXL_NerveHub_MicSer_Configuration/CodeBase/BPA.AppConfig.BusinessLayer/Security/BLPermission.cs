using BPA.AppConfig.BusinessEntity.Security;
using BPA.AppConfig.BusinessEntity;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.Datalayer.Security;
using System.Data;

namespace BPA.AppConfig.BusinessLayer.Security
{
    public class BLPermission : IPermissionService, IDisposable, IDataOperation<BEUserInfo>
    {
        public BLPermission()
        { }

        public void Dispose()
        { }

        public DataSet GetUserListD(string sLoginName, bool bActiveUser, int iLoggedinUserID, int iClientUser, string SearchCondition, BETenant oTenant)
        {
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                return oPermission.GetUserListD(sLoginName, bActiveUser, iLoggedinUserID, 0, SearchCondition);
            }
        }

        public void InsertData(BEUserInfo oUser, int iFormID, BETenant oTenant)
        {
            /*if (oUser.sLoginName == string.Empty || oUser.sLoginName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Login Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            if (!CheckPermission.hasPermission(iFormID, oUser.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }*/
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                oPermission.InsertData(oUser);

            }

        }
        public void UpdateData(BEUserInfo oUser, int iFormID, BETenant oTenant)
        {
            /*
            if (oUser.sLoginName == string.Empty || oUser.sLoginName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            if (!CheckPermission.hasPermission(iFormID, oUser.iCreatedBy, PermissionSet.UPDATE))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }*/
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                oPermission.UpdateData(oUser);
            }
        }
        public void DeleteData(BEUserInfo ClObject, int iFormID, BETenant oTenant)
        {
            throw new NotImplementedException();
        }

    }
}
