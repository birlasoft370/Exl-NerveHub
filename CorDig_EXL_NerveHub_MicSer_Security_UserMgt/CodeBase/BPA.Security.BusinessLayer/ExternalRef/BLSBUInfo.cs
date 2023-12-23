using BPA.Security.BusinessEntity;
using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessLayer.ExtrernalRefre;
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
    public class BLSBUInfo : ISBUInfoService, IDisposable//, IDataOperation<BESBUInfo>
    {
        #region Dispose
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }
        #endregion

        #region GetParamterList

        /// <summary>
        /// Gets the SBU list.
        /// </summary>
        /// <param name="IsActiveSBU">if set to <c>true</c> [is active SBU].</param>
        /// <returns></returns>
        public List<BESBUInfo> GetSBUList(bool IsActiveSBU, BETenant oTenant)
        {
            return GetSBUList("", IsActiveSBU, oTenant);
        }


        /// <summary>
        /// Gets the SBU list.
        /// </summary>
        /// <param name="sName">Name of the s.</param>
        /// <param name="IsActiveSBU">if set to <c>true</c> [is active SBU].</param>
        /// <returns></returns>
        public List<BESBUInfo> GetSBUList(string sName, bool IsActiveSBU, BETenant oTenant)
        {
            using (DLSBUInfo objSBU = new DLSBUInfo(oTenant))
            {
                return objSBU.GetSBUList(sName, IsActiveSBU);
            }
        }




        /// <summary>
        /// Gets the SBU list bsed on client.
        /// </summary>
        /// <param name="iSBUID">The i SBUID.</param>
        /// <returns></returns>
        public List<BESBUInfo> GetSBUListBsedOnClient(int iSBUID, BETenant oTenant)
        {
            using (DLSBUInfo objSBU = new DLSBUInfo(oTenant))
            {
                return objSBU.GetSBUListBsedOnClient(iSBUID);
            }
        }

        /// <summary>
        /// Gets the SBU listall.
        /// </summary>
        /// <returns></returns>
        public List<BESBUInfo> GetSBUListall(BETenant oTenant)
        {
            using (DLSBUInfo objSBU = new DLSBUInfo(oTenant))
            {
                return objSBU.GetSBUListall();
            }
        }


        /// <summary>
        /// Gets the SBU list.
        /// </summary>
        /// <param name="iSBUID">The i SBUID.</param>
        /// <returns></returns>
        public List<BESBUInfo> GetSBUList(int iSBUID, BETenant oTenant)
        {
            using (DLSBUInfo objSBU = new DLSBUInfo(oTenant))
            {
                return objSBU.GetSBUList(iSBUID);
            }
        }

        /// <summary>
        /// Gets the SBU listbased ON client.
        /// </summary>
        /// <param name="iclientid">The iclientid.</param>
        /// <returns></returns>
        public List<BESBUInfo> GetSBUListbasedONClient(int iclientid, BETenant oTenant)
        {
            using (DLSBUInfo objSBU = new DLSBUInfo(oTenant))
            {
                return objSBU.GetSBUListbasedONClient(iclientid);
            }
        }

        /// <summary>
        /// Gets the max cliebt id.
        /// </summary>
        /// <returns></returns>
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
        #endregion

        #region InsertData


        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <param name="oSBU">The o SBU.</param>
        /// <param name="FormID">The form ID.</param>
        public string InsertData(BESBUInfo oSBU, int FormID, BETenant oTenant)
        {
            if (!BLCheckPermission.hasPermission(FormID, oSBU.iCreatedBy, PermissionSet.ADD))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }

            if (oSBU.sName == string.Empty || oSBU.sName == "")
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("SBU Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            using (DLSBUInfo objSBU = new DLSBUInfo(oTenant))
            {
                return objSBU.InsertData(oSBU);
            }
        }

        public void InsertDataSBU(BESBUInfo oSBU, int FormID, BETenant oTenant)
        {
            if (!BLCheckPermission.hasPermission(FormID, oSBU.iCreatedBy, PermissionSet.ADD))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }

            if (oSBU.sName == string.Empty || oSBU.sName == "")
            {
                //throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("SBU Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            using (DLSBUInfo objSBU = new DLSBUInfo(oTenant))
            {
                objSBU.InsertDataSBU(oSBU);
            }
        }
        public void UpdateDataSBU(BESBUInfo oSBU, int FormID, BETenant oTenant)
        {
            if (!BLCheckPermission.hasPermission(FormID, oSBU.iCreatedBy, PermissionSet.ADD))
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }

            if (oSBU.sName == string.Empty || oSBU.sName == "")
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("SBU Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            using (DLSBUInfo objSBU = new DLSBUInfo(oTenant))
            {
                objSBU.UpdateDataSBU(oSBU);
            }
        }


        #endregion

        #region UpdateData

        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <param name="oSBU">The o SBU.</param>
        /// <param name="FormID">The form ID.</param>
        public void UpdateData(BESBUInfo oSBU, int FormID, BETenant oTenant)
        {
            if (!BLCheckPermission.hasPermission(FormID, oSBU.iCreatedBy, PermissionSet.UPDATE))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }

            if (oSBU.sName == string.Empty || oSBU.sName == "")
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("SBU Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            using (DLSBUInfo objSBU = new DLSBUInfo(oTenant))
            {
                objSBU.UpdateData(oSBU);
            }
        }
        #endregion

        #region DeleteData

        /// <summary>
        /// Deletes the data.
        /// </summary>
        /// <param name="oSBU">The o SBU.</param>
        /// <param name="FormID">The form ID.</param>
        public void DeleteData(BESBUInfo oSBU, int FormID, BETenant oTenant)
        {
            if (!BLCheckPermission.hasPermission(FormID, oSBU.iCreatedBy, PermissionSet.DELETE))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.DeletePermission);
            }
            using (DLSBUInfo objSBU = new DLSBUInfo(oTenant))
            {
                objSBU.DeleteData(oSBU);
            }
        }
        #endregion


    }
}
