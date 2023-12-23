using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity;
using BPA.Security.DataLayer.ExternalRef;
using BPA.Security.ServiceContract.ExternalRef;

namespace BPA.Security.BusinessLayer.ExternalRef
{/// <summary>
 /// Application Master Table BL Class
 /// </summary>
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

        #region Get Master List
        /// <summary>
        /// Gets the master list.
        /// </summary>
        /// <param name="iFieldID">The field ID.</param>
        /// <returns></returns>
        public List<BEMasterTable> GetMasterList(int iFieldID, BETenant oTenant)
        {
            using (DLMasterTable oMasterTable = new DLMasterTable(oTenant))
            {
                return oMasterTable.GetMasterList(iFieldID);
            }
        }
        /// <summary>
        /// Gets the master list.
        /// </summary>
        /// <param name="sFieldID">The s field ID.</param>
        /// <returns></returns>
        public List<BEMasterTable> GetMasterList(string sFieldID, BETenant oTenant)
        {
            using (DLMasterTable oMasterTable = new DLMasterTable(oTenant))
            {
                return oMasterTable.GetMasterList(sFieldID);
            }
        }
        /// <summary>
        /// Gets the master list.
        /// </summary>
        /// <param name="IsActivePR">if set to <c>true</c> [is active PR].</param>
        /// <returns></returns>
        public List<BEMasterType> GetMasterList(bool IsActivePR, BETenant oTenant)
        {
            return GetMasterList("", IsActivePR, oTenant);
        }

        /// <summary>
        /// Gets the master list.
        /// </summary>
        /// <param name="sFieldDesc">The s field desc.</param>
        /// <param name="IsActivePR">if set to <c>true</c> [is active PR].</param>
        /// <returns></returns>
        public List<BEMasterType> GetMasterList(string sFieldDesc, bool IsActivePR, BETenant oTenant)
        {
            using (DLMasterTable objMasterTable = new DLMasterTable(oTenant))
            {
                return objMasterTable.GetMasterList(sFieldDesc, IsActivePR);
            }
        }



        /// <summary>
        /// Gets the Master Type Details.
        /// </summary>
        /// <param name="iPayRatingID">The iFieldID</param>
        /// <returns></returns>
        public BEMasterType GetMasterDetails(int iFieldID, BETenant oTenant)
        {
            using (DLMasterTable objMasterTable = new DLMasterTable(oTenant))
            {
                return objMasterTable.GetMasterDetails(iFieldID);
            }
        }
        #endregion

        #region InsertData


        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <param name="oMasterType">The MasterType</param>
        /// <param name="FormID">The form ID.</param>
        public void InsertData(BEMasterType oMasterType, int FormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, oMasterType.iCreatedBy, PermissionSet.ADD))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }

            using (DLMasterTable objMasterTable = new DLMasterTable(oTenant))
            {
                objMasterTable.InsertData(oMasterType);
            }
        }
        #endregion

        #region UpdateData
        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <param name="oMasterType">The MasterType object.</param>
        /// <param name="FormID">The form ID.</param>
        public void UpdateData(BEMasterType oMasterType, int FormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, oMasterType.iCreatedBy, PermissionSet.UPDATE))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }
            using (DLMasterTable objMasterTable = new DLMasterTable(oTenant))
            {
                objMasterTable.UpdateData(oMasterType);
            }
        }
        #endregion

        #region DeleteData

        /// <summary>
        /// Deletes the data.
        /// </summary>
        /// <param name="oMasterType">The MasterType object.</param>
        /// <param name="FormID">The form ID.</param>
        public void DeleteData(BEMasterType oMasterType, int FormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, oMasterType.iCreatedBy, PermissionSet.DELETE))
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.DeletePermission);
            }
            using (DLMasterTable objMasterTable = new DLMasterTable(oTenant))
            {
                objMasterTable.DeleteData(oMasterType);
            }
        }
        #endregion


        public IList<BEMasterTable> FillRoleLevel(BETenant oTenant)
        {
            using (DLMasterTable objMasterTable = new DLMasterTable(oTenant))
            {
                return objMasterTable.FillRoleLevel();
            }
        }
    }
}
