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
using System.Data;
using System.Text;

namespace BPA.Security.BusinessLayer.Security
{
    /// <summary>
    /// CDS Application Roles
    /// </summary>
    /// <seealso cref="BPA.Security.ServiceContracts.Security.IRolesService" />
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="BPA.Security.BusinessEntity.IDataOperation{BPA.Security.BusinessEntity.Security.BERoleInfo}" />
    public class BLRoles : IRolesService, IDisposable, IDataOperation<BERoleInfo>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="BLRoles" /> class.
        /// </summary>
        public BLRoles()
        { }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { 
        
        }

        #region Gets the form action.
        /// <summary>
        /// Gets the form action.
        /// </summary>
        /// <param name="RoleID">The role ID.</param>
        /// <param name="oTenant">The o tenant.</param>
        /// <returns></returns>
        public DataSet GetFormAction(int RoleID, BETenant oTenant)
        {
            using (DLFormAction objRoles = new DLFormAction(oTenant))
            {
                return objRoles.GetFormAction(RoleID);
            }
        }
        #endregion



        #region Gets the role list.
        /// <summary>
        /// Gets the role list.
        /// </summary>
        /// <param name="bActiveRoles">if set to <c>true</c> [b active roles].</param>
        /// <param name="oTenant">The o tenant.</param>
        /// <returns></returns>
        public List<BERoleInfo> GetRoleList(bool bActiveRoles, BETenant oTenant)
        {
            using (DLRoles objRole = new DLRoles(oTenant))
            {
                return objRole.GetRoleList(bActiveRoles);
            }
        }
        /// <summary>
        /// Gets the client role list.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="oTenant">The o tenant.</param>
        /// <returns></returns>
        public List<BERoleInfo> GetClientRoleListByUser(int userID,BETenant oTenant)
        {
            using (DLRoles objRole = new DLRoles(oTenant))
            {
                return objRole.GetClientRoleListByUser(userID);
            }
        }
        /// <summary>
        /// Gets the client role list.
        /// </summary>
        /// <param name="oTenant">The o tenant.</param>
        /// <returns></returns>
        public List<BERoleInfo> GetClientRoleList(BETenant oTenant)
        {
            using (DLRoles objRole = new DLRoles(oTenant))
            {
                return objRole.GetClientRoleList(true);
            }
        }


        /// <summary>
        /// Gets the role list Based on Role type on user typeBased.
        /// </summary>
        /// <param name="oTenant">The o tenant.</param>
        /// <returns></returns>
        public List<BERoleInfo> GetUsertRoleList(bool isClient, BETenant oTenant)
        {
            using (DLRoles objRole = new DLRoles(oTenant))
            {
                return objRole.GetUsertRoleList(isClient);
            }
        }

        /// <summary>
        /// Gets the role list.
        /// </summary>
        /// <param name="sRoleName">Name of the role.</param>
        /// <param name="bActiveRoles">if set to <c>true</c> [active roles].</param>
        /// <param name="oTenant">The o tenant.</param>
        /// <returns></returns>
        public List<BERoleInfo> GetRoleList(string sRoleName, bool bActiveRoles, BETenant oTenant)
        {
            using (DLRoles objRole = new DLRoles(oTenant))
            {
                return objRole.GetRoleList(sRoleName, bActiveRoles);
            }
        }

        /// <summary>
        /// Gets the role list.
        /// </summary>
        /// <param name="iRoleID">The role ID.</param>
        /// <param name="oTenant">The o tenant.</param>
        /// <returns></returns>
        public List<BERoleInfo> GetRoleList(int iRoleID, BETenant oTenant)
        {
            using (DLRoles objRole = new DLRoles(oTenant))
            {
                return objRole.GetRoleList(iRoleID);
            }
        }
        #endregion

        #region Delete Role Data
        /// <summary>
        /// Delete Role Data
        /// </summary>
        /// <param name="oRole">Object Role info</param>
        /// <param name="iFormID">The form ID.</param>
        /// <param name="oTenant">The o tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void DeleteData(BERoleInfo oRole, int iFormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(iFormID, oRole.iCreatedBy, PermissionSet.DELETE))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.DeletePermission);
            }
            using (DLRoles objRole = new DLRoles(oTenant))
            {
                objRole.DeleteData(oRole);
            }
        }
        /// <summary>
        /// Delete Role Data
        /// </summary>
        /// <param name="oRole">The o role.</param>
        /// <param name="iApproverId">The i approver identifier.</param>
        /// <param name="iFormID">The i form identifier.</param>
        /// <param name="sbMailBody">The sb mail body.</param>
        /// <param name="oTenant">The o tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void DeleteData(BERoleInfo oRole, int iApproverId, int iFormID, StringBuilder sbMailBody, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(iFormID, oRole.iCreatedBy, PermissionSet.DELETE))
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.DeletePermission);
            }
            using (DLRoles objRole = new DLRoles(oTenant))
            {
                objRole.InsertData(oRole, iApproverId, 3, sbMailBody);
            }
        }
        #endregion

        #region Insert Role Data
        /// <summary>
        /// Insert Role Data
        /// </summary>
        /// <param name="oRole">Object Role info</param>
        /// <param name="iFormID">The form ID.</param>
        /// <param name="oTenant">The o tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException">
        /// </exception>
        public void InsertData(BERoleInfo oRole, int iFormID, BETenant oTenant)
        {
            if (oRole.sRoleName == string.Empty || oRole.sRoleName == "")
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            if (!CheckPermission.hasPermission(iFormID, oRole.iCreatedBy, PermissionSet.ADD))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLRoles DLRoles = new DLRoles(oTenant))
            {
                DLRoles.InsertData(oRole);
            }
        }
        /// <summary>
        /// Inserts Role Data
        /// </summary>
        /// <param name="oRole">The o role.</param>
        /// <param name="iApproverId">The i approver identifier.</param>
        /// <param name="iFormID">The i form identifier.</param>
        /// <param name="sbMailBody">The sb mail body.</param>
        /// <param name="oTenant">The o tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException">
        /// </exception>
        public void InsertData(BERoleInfo oRole, int iApproverId, int iFormID, StringBuilder sbMailBody, BETenant oTenant)
        {
            if (oRole.sRoleName == string.Empty || oRole.sRoleName == "")
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            if (!CheckPermission.hasPermission(iFormID, oRole.iCreatedBy, PermissionSet.ADD))
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLRoles DLRoles = new DLRoles(oTenant))
            {
                DLRoles.InsertData(oRole, iApproverId, 1, sbMailBody);
            }
        }
        #endregion

        #region Update Role Data
        /// <summary>
        /// Update Role Data
        /// </summary>
        /// <param name="oRole">Object Role info</param>
        /// <param name="iFormID">The i form ID.</param>
        /// <param name="oTenant">The o tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException">
        /// </exception>
        public void UpdateData(BERoleInfo oRole, int iFormID, BETenant oTenant)
        {
            if (oRole.sRoleName == string.Empty || oRole.sRoleName == "")
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            if (!CheckPermission.hasPermission(iFormID, oRole.iCreatedBy, PermissionSet.UPDATE))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }
            using (DLRoles DLRoles = new DLRoles(oTenant))
            {
                DLRoles.UpdateData(oRole);
            }
        }
        /// <summary>
        /// Updates role data
        /// </summary>
        /// <param name="oRole">The o role.</param>
        /// <param name="iApproverId">The i approver identifier.</param>
        /// <param name="iFormID">The i form identifier.</param>
        /// <param name="sbMailBody">The sb mail body.</param>
        /// <param name="oTenant">The o tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException">
        /// </exception>
        public void UpdateData(BERoleInfo oRole, int iApproverId, int iFormID, StringBuilder sbMailBody, BETenant oTenant)
        {
            if (oRole.sRoleName == string.Empty || oRole.sRoleName == "")
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            if (!CheckPermission.hasPermission(iFormID, oRole.iCreatedBy, PermissionSet.UPDATE))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }
            using (DLRoles DLRoles = new DLRoles(oTenant))
            {
                DLRoles.InsertData(oRole,iApproverId,2, sbMailBody);
            }
        }
        #endregion

        #region Request Approval Workflow
        /// <summary>
        /// Gets the Role Request status.
        /// </summary>
        /// <param name="iUser">The i user.</param>
        /// <param name="dtFromDate">The dt from date.</param>
        /// <param name="dtToDate">The dt to date.</param>
        /// <param name="oTenant">The o tenant.</param>
        /// <returns></returns>
        public DataSet GetRoleRequestStatus(int iUser, DateTime dtFromDate, DateTime dtToDate, BETenant oTenant)
        {
            using (DLRoles oPer = new DLRoles(oTenant))
            {
                return oPer.GetRoleRequestStatus(iUser, dtFromDate, dtToDate);
            }
        }
        /// <summary>
        /// Gets the Role approval list.
        /// </summary>
        /// <param name="iUser">The i user.</param>
        /// <param name="oTenant">The o tenant.</param>
        /// <returns></returns>
        public DataSet GetRoleApprovalList(int iUser, BETenant oTenant)
        {
            using (DLRoles oPer = new DLRoles(oTenant))
            {
                return oPer.GetRoleApprovalList(iUser);
            }
        }
        /// <summary>
        /// Rejects Role Request
        /// </summary>
        /// <param name="iRequestID">The i request identifier.</param>
        /// <param name="iUserID">The i user identifier.</param>
        /// <param name="iFormID">The i form identifier.</param>
        /// <param name="oTenant">The o tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void RejectRoleRequest(int iRequestID, int iUserID, int iFormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(iFormID, iUserID, PermissionSet.ADD))
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLRoles oPer = new DLRoles(oTenant))
            {
                oPer.RejectRoleRequest(iRequestID, iUserID);
            }
        }
        /// <summary>
        /// Approve Role request
        /// </summary>
        /// <param name="iRequestID">The i request identifier.</param>
        /// <param name="iUserID">The i user identifier.</param>
        /// <param name="iFormID">The i form identifier.</param>
        /// <param name="oTenant">The o tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void ApproveRoleRequest(int iRequestID, int iUserID, int iFormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(iFormID, iUserID, PermissionSet.ADD))
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLRoles oPer = new DLRoles(oTenant))
            {
                oPer.ApproveRoleRequest(iRequestID, iUserID);
            }
        }
        /// <summary>
        /// Cancels ERP Job Role Request
        /// </summary>
        /// <param name="iRequestID">The i request identifier.</param>
        /// <param name="iUserID">The i user identifier.</param>
        /// <param name="iFormID">The i form identifier.</param>
        /// <param name="oTenant">The o tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void CancelRoleRequest(int iRequestID, int iUserID, int iFormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(iFormID, iUserID, PermissionSet.DELETE))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLRoles oPer = new DLRoles(oTenant))
            {
                oPer.CancelRoleRequest(iRequestID, iUserID);
            }
        }
        #endregion


        /// <summary>
        /// Gets the user job code.
        /// </summary>
        /// <param name="oTenant">The o tenant.</param>
        /// <returns></returns>
        public List<BEUserInfo> GetUserJobCode(BETenant oTenant)
        {
            using (DLRoles objRole = new DLRoles(oTenant))
            {
                return objRole.GetUserJobCode();
            }
        }
        
    }
}
