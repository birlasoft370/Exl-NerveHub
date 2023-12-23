using BPA.AppConfig.BusinessEntity;
using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.Datalayer.Config;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessLayer.Config
{
    public class BLMasterTable : IMasterTableService, IDisposable, IDataOperation<BEMasterType>
    {
        #region Constructor
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="BLMasterTable"/> class.
        /// </summary>
        public BLMasterTable()
        { }
        #endregion
        public List<BEMasterTable> GetMasterList(int iFieldID, BETenant oTenant)
        {
            using (DLMasterTable oMasterTable = new DLMasterTable(oTenant))
            {
                return oMasterTable.GetMasterList(iFieldID);
            }
        }

        public BEMasterType GetMasterDetails(int iFieldID, BETenant oTenant)
        {
            using (DLMasterTable objMasterTable = new DLMasterTable(oTenant))
            {
                return objMasterTable.GetMasterDetails(iFieldID);
            }
        }

        public void InsertData(BEMasterType oMasterType, int FormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(FormID, oMasterType.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            */
            using (DLMasterTable objMasterTable = new DLMasterTable(oTenant))
            {
                objMasterTable.InsertData(oMasterType);
            }
        }

        public void UpdateData(BEMasterType oMasterType, int FormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(FormID, oMasterType.iCreatedBy, PermissionSet.UPDATE))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }*/
            using (DLMasterTable objMasterTable = new DLMasterTable(oTenant))
            {
                objMasterTable.UpdateData(oMasterType);
            }
        }

        public void DeleteData(BEMasterType ClObject, int iFormID, BETenant oTenant)
        {
            throw new NotImplementedException();
        }

        public List<BEMasterType> GetMasterList(string sFieldDesc, bool IsActivePR, BETenant oTenant)
        {
            using (DLMasterTable objMasterTable = new DLMasterTable(oTenant))
            {
                return objMasterTable.GetMasterList(sFieldDesc, IsActivePR);
            }
        }
    }
}
