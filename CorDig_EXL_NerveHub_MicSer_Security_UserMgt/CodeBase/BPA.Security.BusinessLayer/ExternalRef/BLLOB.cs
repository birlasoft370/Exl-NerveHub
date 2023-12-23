using BPA.Security.BusinessEntity;
using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.DataLayer.ExternalRef;
using BPA.Security.ServiceContract.ExternalRef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.Security.BusinessLayer.ExternalRef
{/// <summary>
 /// This class will use business logic to 
 /// validate rules define for LOB
 /// </summary>
    public class BLLOB : ILOBService, IDisposable
    {
        public void Dispose()
        { }

        #region GetParamterList
        /// <summary>
        /// Gets the LOB list.
        /// </summary>
        /// <returns></returns>
        //[AutoComplete]
        public List<BELOBInfo> GetLOBList(bool IsActiveLOB, BETenant oTenant)
        {
            return GetLOBList("", IsActiveLOB, oTenant);
        }
        /// <summary>
        /// Gets the LOB list.
        /// </summary>
        /// <param name="LOBName">Name of the LOB.</param>
        /// <param name="IsActiveLOB">if set to <c>true</c> [is active LOB].</param>
        /// <returns></returns>
        public List<BELOBInfo> GetLOBList(string LOBName, bool IsActiveLOB, BETenant oTenant)
        {
            using (DLLOB objLOB = new DLLOB(oTenant))
            {
                return objLOB.GetLOBList(LOBName, IsActiveLOB);
            }
        }

        /// <summary>
        /// Gets the LOB list.
        /// </summary>
        /// <param name="LOBID">The LOB ID.</param>
        /// <returns></returns>
        public List<BELOBInfo> GetLOBList(int LOBID, BETenant oTenant)
        {
            using (DLLOB objLOB = new DLLOB(oTenant))
            {
                return objLOB.GetLOBList(LOBID);
            }
        }
        #endregion

        #region InsertData
        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <param name="oLOB">LOB.</param>
        /// <param name="FormID">form ID.</param>

        public void InsertData(BELOBInfo oLOB, int FormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, oLOB.iCreatedBy, PermissionSet.ADD))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }

            if (oLOB.sLOBName == string.Empty || oLOB.sLOBName == "")
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("LOB Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            using (DLLOB objLOB = new DLLOB(oTenant))
            {
                objLOB.ManageLOB(oLOB, "Add");
            }
        }
        #endregion

        #region UpdateData
        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <param name="oLOB">LOB.</param>
        /// <param name="FormID">form ID.</param>
        public void UpdateData(BELOBInfo oLOB, int FormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, oLOB.iCreatedBy, PermissionSet.UPDATE))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }

            if (oLOB.sLOBName == string.Empty || oLOB.sLOBName == "")
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("LOB Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            using (DLLOB objLOB = new DLLOB(oTenant))
            {
                objLOB.ManageLOB(oLOB, "Update");
            }
        }
        #endregion

        #region DeleteData
        /// <summary>
        /// Deletes the data.
        /// </summary>
        /// <param name="oLOB">LOB.</param>
        /// <param name="FormID">form ID.</param>
        public void DeleteData(BELOBInfo oLOB, int FormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, oLOB.iCreatedBy, PermissionSet.DELETE))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.DeletePermission);
            }
            using (DLLOB objLOB = new DLLOB(oTenant))
            {
                objLOB.ManageLOB(oLOB, "Delete");
            }
        }
        #endregion
    }
}
