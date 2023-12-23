/* Copyright © 2012 ExlService (I) Pvt. Ltd.
 * project Name                 :   
 * Class Name                   :   
 * Namespace                    :   
 * Purpose                      :
 * Description                  :
 * Dependency                   :   
 * Related Table                :
 * Related Class                :
 * Related StoreProcdure        :
 * Author                       :   
 * Created on                   :   
 * Reviewed on                  :          
 * Reviewed by                  :
 * Tested on                    :
 * Tested by                    :
 * Modification history         :
 * modify1 on                   :
 * modify1 By                   :
 * Overall effect               :
 */

using BPA.Security.BusinessEntity;
using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using BPA.Security.Datalayer.Security;
using BPA.Security.ServiceContracts.Security;
using System;
using System.Collections.Generic;

namespace BPA.Security.BusinessLayer.Security
{
    /// <summary>
    /// CDS Action
    /// </summary>
    public class BLUserLevel : IUserLevelService, IDisposable, IDataOperation<BEUserLevelInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BLAction"/> class.
        /// </summary>
        public BLUserLevel()
        { }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }

        /// <summary>
        /// Gets the user level less.
        /// </summary>
        /// <param name="iLevel">The level.</param>
        /// <returns></returns>
        public List<BEUserLevelInfo> GetUserLevelLess(int iLevel, BETenant oTenant)
        {
            List<BEUserLevelInfo> lUserLevel = GetUserLevelList("", true, oTenant);
            List<BEUserLevelInfo> lUserLevel1 = new List<BEUserLevelInfo>();
            lUserLevel1 = GetUserLevelLessHelp(lUserLevel, iLevel, oTenant);
            for (int i = 0; i < lUserLevel.Count; i++)
            {
                if (lUserLevel[i].iUserLevelID == iLevel)
                {
                    lUserLevel1.Add(lUserLevel[i]);
                }
            }
            return lUserLevel1;
        }


        /// <summary>
        /// Get the user level less help.
        /// </summary>
        /// <param name="lUserLevel">The user level.</param>
        /// <param name="iLevel">The level.</param>
        /// <returns></returns>
        public List<BEUserLevelInfo> GetUserLevelLessHelp(List<BEUserLevelInfo> lUserLevel, int iLevel, BETenant oTenant)
        {
            List<BEUserLevelInfo> NewUserLevel=new List<BEUserLevelInfo>();
            for (int i = 0; i < lUserLevel.Count; i++)
            {
                if (lUserLevel[i].iParentID == iLevel)
                {
                    NewUserLevel.Add(lUserLevel[i]);
                    List<BEUserLevelInfo> temp = GetUserLevelLessHelp(lUserLevel, lUserLevel[i].iUserLevelID, oTenant);
                    for (int j = 0; j < temp.Count; j++)
                    {
                        NewUserLevel.Add(temp[j]);
                    }
                }
            }
            return NewUserLevel;
        }

        #region Get the user level list.
        /// <summary>
        /// Get the user level list.
        /// </summary>
        /// <param name="bActiveLevel">if set to <c>true</c> [active level].</param>
        /// <returns></returns>
        public List<BEUserLevelInfo> GetUserLevelList(bool bActiveLevel, BETenant oTenant)
        {
            return GetUserLevelList("", bActiveLevel, oTenant);
        }


        /// <summary>
        /// Gets the user level list.
        /// </summary>
        /// <param name="sUserLevel">The user level.</param>
        /// <param name="bActiveLevel">if set to <c>true</c> [active level].</param>
        /// <returns></returns>
        public List<BEUserLevelInfo> GetUserLevelList(string sUserLevel, bool bActiveLevel, BETenant oTenant)
        {
            using (DLUserLevel objUserLevel = new DLUserLevel(oTenant))
            {
                return objUserLevel.GetUserLevelList(sUserLevel, bActiveLevel);
            }
        }

        /// <summary>
        /// Gets the user level list.
        /// </summary>
        /// <param name="iUserLevelID">The user level ID.</param>
        /// <returns></returns>
        public List<BEUserLevelInfo> GetUserLevelList(int iUserLevelID, BETenant oTenant)
        {
            using (DLUserLevel objUserLevel = new DLUserLevel(oTenant))
            {
                return objUserLevel.GetUserLevelList(iUserLevelID);
            }
        }
        #endregion

        #region Insert the record.
        /// <summary>
        /// Insert the record.
        /// </summary>
        /// <param name="oUserLevel">The user level.</param>
        /// <param name="iFormID">The form ID.</param>
        public void InsertData(BEUserLevelInfo oUserLevel, int iFormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(iFormID, oUserLevel.iCreatedBy, PermissionSet.ADD))
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            if (oUserLevel.sLevelName == string.Empty || oUserLevel.sLevelName == "")
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Level Name "+BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            using (DLUserLevel objUserLevel = new DLUserLevel(oTenant))
            {
                objUserLevel.InsertData(oUserLevel);
            }

        }
        #endregion

        #region Update the record.
        /// <summary>
        /// Update the record.
        /// </summary>
        /// <param name="oUserLevel">The user level.</param>
        /// <param name="iFormID">The form ID.</param>
        public void UpdateData(BEUserLevelInfo oUserLevel, int iFormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(iFormID, oUserLevel.iCreatedBy, PermissionSet.UPDATE))
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }
            if (oUserLevel.sLevelName == string.Empty || oUserLevel.sLevelName == "")
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Level Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            using (DLUserLevel objUserLevel = new DLUserLevel(oTenant))
            {
                objUserLevel.UpdateData(oUserLevel);
            }

        }
        #endregion

        #region Delete the Record
        /// <summary>
        /// Delete the Record
        /// </summary>
        /// <param name="oUserLevel">The user level.</param>
        /// <param name="iFormID">The form ID.</param>
        public void DeleteData(BEUserLevelInfo oUserLevel, int iFormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(iFormID, oUserLevel.iCreatedBy, PermissionSet.DELETE))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.DeletePermission);
            }
            using (DLUserLevel objUserLevel = new DLUserLevel(oTenant))
            {
                objUserLevel.DeleteData(oUserLevel);
            }
        }
        #endregion
    }
}
