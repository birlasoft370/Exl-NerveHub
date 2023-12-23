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

namespace BPA.Security.BusinessLayer.Security
{
    /// <summary>
    /// This class is used to create, edit, deleted,
    /// lock unlock users of application
    /// </summary>
    /// <seealso cref="BPA.Security.ServiceContracts.Security.IPermissionService" />
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="BPA.Security.BusinessEntity.IDataOperation{BPA.Security.BusinessEntity.Security.BEUserInfo}" />
    public class BLPermission : IPermissionService, IDisposable, IDataOperation<BEUserInfo>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BLPermission"/> class.
        /// </summary>
        public BLPermission()
        { }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }
        #endregion

        #region Get the supervisor list
        /// <summary>
        /// Get the supervisor list.
        /// </summary>
        /// <param name="iUserID">The user ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public List<BEUserInfo> GetSupervisorList(int iUserID, BETenant oTenant)
        {
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                return oPermission.GetSupervisorList(iUserID);
            }
        }
        #endregion

        #region Gets the user list
        /// <summary>
        /// Gets the user list.
        /// </summary>
        /// <param name="bActiveUser">if set to <c>true</c> [b active user].</param>
        /// <param name="iLoggedinUserID">The loggedin user ID.</param>
        /// <param name="SearchCondition">The search condition.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public List<BEUserInfo> GetUserList(bool bActiveUser, int iLoggedinUserID, string SearchCondition, BETenant oTenant)
        {
            return GetUserList("", bActiveUser, iLoggedinUserID, SearchCondition, oTenant);
        }

        /// <summary>
        /// Gets the user list.
        /// </summary>
        /// <param name="sLoginName">Name of the login.</param>
        /// <param name="bActiveUser">if set to <c>true</c> [b active user].</param>
        /// <param name="iLoggedinUserID">The loggedin user ID.</param>
        /// <param name="SearchCondition">The search condition.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public List<BEUserInfo> GetUserList(string sLoginName, bool bActiveUser, int iLoggedinUserID, string SearchCondition, BETenant oTenant)
        {
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                return oPermission.GetUserList(sLoginName, bActiveUser, iLoggedinUserID, 0, SearchCondition);
            }
        }

        /// <summary>
        /// Gets the user list d.
        /// </summary>
        /// <param name="sLoginName">Name of the s login.</param>
        /// <param name="bActiveUser">if set to <c>true</c> [b active user].</param>
        /// <param name="iLoggedinUserID">The loggedin user identifier.</param>
        /// <param name="iClientUser">The client user.</param>
        /// <param name="SearchCondition">The search condition.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet GetUserListD(string sLoginName, bool bActiveUser, int iLoggedinUserID, int iClientUser, string SearchCondition, BETenant oTenant)
        {
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                return oPermission.GetUserListD(sLoginName, bActiveUser, iLoggedinUserID, 0, SearchCondition);
            }
        }
        /// <summary>
        /// Gets the user list.
        /// </summary>
        /// <param name="sLoginName">Name of the login.</param>
        /// <param name="bActiveUser">if set to <c>true</c> [b active user].</param>
        /// <param name="iLoggedinUserID">The loggedin user ID.</param>
        /// <param name="SearchCondition">The search condition.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public List<BEUserInfo> GetClientUserList(string sLoginName, bool bActiveUser, int iLoggedinUserID, string SearchCondition, BETenant oTenant)
        {
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                return oPermission.GetUserList(sLoginName, bActiveUser, iLoggedinUserID, 1, SearchCondition);
            }
        }

        /// <summary>
        /// Gets the user listby client.
        /// </summary>
        /// <param name="bActiveUser">if set to <c>true</c> [b active user].</param>
        /// <param name="iClientID">The client identifier.</param>
        /// <param name="iUserID">The user identifier.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public IList<BEUserInfo> GetUserListbyClient(bool bActiveUser, int iClientID, int iUserID, BETenant oTenant)
        {
            return GetUserListbyClient("", bActiveUser, iClientID, iUserID, oTenant);
        }


        /// <summary>
        /// Gets the user listby client.
        /// </summary>
        /// <param name="sLoginName">Name of the s login.</param>
        /// <param name="bActiveUser">if set to <c>true</c> [b active user].</param>
        /// <param name="iClientID">The client identifier.</param>
        /// <param name="iUserID">The user identifier.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public IList<BEUserInfo> GetUserListbyClient(string sLoginName, bool bActiveUser, int iClientID, int iUserID, BETenant oTenant)
        {
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                return oPermission.GetUserListbyClient(sLoginName, bActiveUser, iClientID, iUserID);
            }
        }



        /// <summary>
        /// Gets the user list with FM role.
        /// </summary>
        /// <param name="sLoginName">Name of the s login.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public List<BEUserInfo> GetUserListWithFMRole(string sLoginName, BETenant oTenant)
        {
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                return oPermission.GetUserListWithFMRole(sLoginName);
            }
        }
        /// <summary>
        /// Gets the user list process montee.
        /// </summary>
        /// <param name="sProcess">The s process.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public List<BEUserInfo> GetUserListProcessMontee(string sProcess, BETenant oTenant)
        {
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                return oPermission.GetUserListProcessMontee(sProcess);
            }
        }

        /// <summary>
        /// Gets the user based momtee.
        /// </summary>
        /// <param name="USER1">The USE r1.</param>
        /// <param name="USER2">The USE r2.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet GetUserBasedMomteeID(string USER1, string USER2, BETenant oTenant)
        {
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                return oPermission.GetUserBasedMomteeID(USER1, USER2);
            }
        }




        /// <summary>
        /// Gets the user list.
        /// </summary>
        /// <param name="iUserID">The user identifier.</param>
        /// <param name="bAllActiveUser">if set to <c>true</c> [b all active user].</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public List<BEUserInfo> GetUserList(int iUserID, bool bAllActiveUser, BETenant oTenant)
        {
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                return oPermission.GetUserList(iUserID, bAllActiveUser);
            }
        }

        /// <summary>
        /// Gets the client user details.
        /// </summary>
        /// <param name="iUserID">The user ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public BEUserInfo GetClientUserDetails(int iUserID, BETenant oTenant)
        {
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                return oPermission.GetClientUserDetails(iUserID);
            }
        }
        /// <summary>
        /// Gets the user details with role.
        /// </summary>
        /// <param name="iUserID">The user ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public BEUserInfo GetUserDetailsWithRole(int iUserID, BETenant oTenant)
        {
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                return oPermission.GetUserDetailsWithRole(iUserID);
            }
        }
        #endregion

        #region Inserts the record
        /// <summary>
        /// Inserts the record.
        /// </summary>
        /// <param name="oUser">The user.</param>
        /// <param name="iFormID">The form ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException">
        /// Login Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField
        /// or
        /// </exception>
        public void InsertData(BEUserInfo oUser, int iFormID, BETenant oTenant)
        {
            if (oUser.sLoginName == string.Empty || oUser.sLoginName == "")
            {
                //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Login Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            if (!CheckPermission.hasPermission(iFormID, oUser.iCreatedBy, PermissionSet.ADD))
            {
                //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                oPermission.InsertData(oUser);

            }

        }
        /// <summary>
        /// Inserts Client user the record.
        /// </summary>
        /// <param name="oUser">The user.</param>
        /// <param name="iFormID">The form ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void InsertClientUserRecord(BEUserInfo oUser, int iFormID, BETenant oTenant)
        {
            //Commented by Omkar
            //if (oUser.sLoginName == string.Empty || oUser.sLoginName == "")
            //{
            //    throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Login Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            //}
            if (!CheckPermission.hasPermission(iFormID, oUser.iCreatedBy, PermissionSet.ADD))
            {
                // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                oPermission.InsertClientUserData(oUser);

            }

        }

        /// <summary>
        /// Inserts Client user the record.
        /// </summary>
        /// <param name="oUser">The user.</param>
        /// <param name="iMode">The mode.</param>
        /// <param name="iFormID">The form ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void InsertUserRoleData(BEUserInfo oUser, int iMode, int iFormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(iFormID, oUser.iCreatedBy, PermissionSet.ADD))
            {
                //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                oPermission.InsertUserRoleData(oUser, iMode);

            }

        }
        #endregion


        //public List<BEUserInfo> GetCustomers()
        //{
        //    using (DLPermission objPermission = new DLPermission(oTenant))
        //    {
        //        return objPermission.GetCustomers();
        //    }
        //}

        /// <summary>
        /// Inserts the agent migration data.
        /// </summary>
        /// <param name="UserId">The user id.</param>
        /// <param name="strCampaignRole">The STR campaign role.</param>
        /// <param name="CreatedBy">The created by.</param>
        /// <param name="oTenant">The Tenant.</param>
        public void InsertAgentMigrationData(int UserId, string strCampaignRole, int CreatedBy, BETenant oTenant)
        {

            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                oPermission.InsertAgentMigrationData(UserId, strCampaignRole, CreatedBy);
            }

        }
        /// <summary>
        /// Insert the unsuccessfull attempt made by user
        /// </summary>
        /// <param name="UserID">The user identifier.</param>
        /// <param name="SessionID">The session identifier.</param>
        /// <param name="HostName">Name of the host.</param>
        /// <param name="oTenant">The Tenant.</param>

        public void InsertUnsuccessfullLogin(int UserID, string SessionID, string HostName, BETenant oTenant)
        {

            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                oPermission.InsertUnsuccessfullLogin(UserID, SessionID, HostName);
            }

        }

        #region Updates the record
        /// <summary>
        /// Updates the record.
        /// </summary>
        /// <param name="oUser">The user.</param>
        /// <param name="iFormID">The form ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException">
        /// </exception>
        public void UpdateData(BEUserInfo oUser, int iFormID, BETenant oTenant)
        {
            if (oUser.sLoginName == string.Empty || oUser.sLoginName == "")
            {
                // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            if (!CheckPermission.hasPermission(iFormID, oUser.iCreatedBy, PermissionSet.UPDATE))
            {
                //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                oPermission.UpdateData(oUser);
            }

        }
        /// <summary>
        /// Updates the client user record.
        /// </summary>
        /// <param name="oUser">The o user.</param>
        /// <param name="iFormID">The form ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException">
        /// </exception>
        public void UpdateClientUserRecord(BEUserInfo oUser, int iFormID, BETenant oTenant)
        {
            if (oUser.sLoginName == string.Empty || oUser.sLoginName == "")
            {
                // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            if (!CheckPermission.hasPermission(iFormID, oUser.iCreatedBy, PermissionSet.UPDATE))
            {
                // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                oPermission.UpdatetClientUserData(oUser);
            }

        }
        #endregion

        #region Change Password
        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="oUserInfo">The user info.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException">Password " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField</exception>
        public void ChangePd(BEUserInfo oUserInfo, BETenant oTenant)
        {
            if (oUserInfo.sPassword == string.Empty || oUserInfo.sPassword == "")
            {
                // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Password " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                oPermission.ChangePd(oUserInfo);
            }
        }
        #endregion

        /// <summary>
        /// Change User Status
        /// </summary>
        /// <param name="UserID">The user identifier.</param>
        /// <param name="oTenant">The Tenant.</param>
        public void ChangeUserStatus(int UserID, BETenant oTenant)
        {
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                oPermission.ChangeUserStatus(UserID);
            }
        }

        /// <summary>
        /// Changes the password log.
        /// </summary>
        /// <param name="oUserInfo">The o user information.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException">Password " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField</exception>
        public int ChangePdLog(BEUserInfo oUserInfo, BETenant oTenant)
        {
            if (oUserInfo.sPassword == string.Empty || oUserInfo.sPassword == "")
            {
                // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Password " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                return oPermission.ChangePdLog(oUserInfo);
            }
        }

        #region Delete the Record
        /// <summary>
        /// Delete the Record
        /// </summary>
        /// <param name="oUser">The user.</param>
        /// <param name="iFormID">The form ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void DeleteData(BEUserInfo oUser, int iFormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(iFormID, oUser.iCreatedBy, PermissionSet.DELETE))
            {
                // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.DeletePermission);
            }
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                oPermission.DeleteData(oUser);
            }
        }
        /// <summary>
        /// Deletes the client user record.
        /// </summary>
        /// <param name="oUser">The o user.</param>
        /// <param name="iFormID">The form ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void DeleteClientUserRecord(BEUserInfo oUser, int iFormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(iFormID, oUser.iCreatedBy, PermissionSet.DELETE))
            {
                // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.DeletePermission);
            }
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                oPermission.DeleteClientUserData(oUser);
            }
        }
        /// <summary>
        /// Deletes the user role data.
        /// </summary>
        /// <param name="oUser">The o user.</param>
        /// <param name="iFormID">The form ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void DeleteUserRoleData(BEUserInfo oUser, int iFormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(iFormID, oUser.iCreatedBy, PermissionSet.DELETE))
            {
                // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.DeletePermission);
            }
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                oPermission.DeleteUserRoleData(oUser);
            }
        }
        #endregion


        /// <summary>
        /// Gets the campaign user list.
        /// </summary>
        /// <param name="iCampID">The camp ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet GetCampaignUserList(int iCampID, BETenant oTenant)
        {
            using (DLPermission objPermission = new DLPermission(oTenant))
            {
                return objPermission.GetCampaignUserList(iCampID);
            }
        }

        /// <summary>
        /// Gets the user campaigns.
        /// </summary>
        /// <param name="Clients">The clients.</param>
        /// <param name="UserId">The user id.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet GetUserCampaigns(string Clients, int UserId, BETenant oTenant)
        {
            using (DLPermission objPermission = new DLPermission(oTenant))
            {
                return objPermission.GetUserCampaigns(Clients, UserId);
            }
        }

        /// <summary>
        /// Gets the user roles.
        /// </summary>
        /// <param name="CampaignId">The campaign id.</param>
        /// <param name="UserId">The user id.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet GetUserRoles(int CampaignId, int UserId, BETenant oTenant)
        {
            using (DLPermission objPermission = new DLPermission(oTenant))
            {
                return objPermission.GetUserRoles(CampaignId, UserId);
            }
        }

        /// <summary>
        /// Gets the user roles.
        /// </summary>
        /// <param name="UserId">The user id.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public List<BERoleInfo> GetUserRoles(int UserId, BETenant oTenant)
        {
            using (DLPermission objPermission = new DLPermission(oTenant))
            {
                return objPermission.GetUserRoles(UserId);
            }
        }
        /// <summary>
        /// Checks the user.
        /// </summary>
        /// <param name="LoginName">Name of the login.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet CheckUser(string LoginName, BETenant oTenant)
        {
            using (DLPermission objPermission = new DLPermission(oTenant))
            {
                return objPermission.CheckUser(LoginName);
            }
        }



        /// <summary>
        /// Gets UserID
        /// </summary>
        /// <param name="LoginName">Name of the login.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet GetUserID(string LoginName, BETenant oTenant)
        {
            using (DLPermission objPermission = new DLPermission(oTenant))
            {
                return objPermission.GetUserID(LoginName);
            }
        }

        /// <summary>
        /// Checks the old pass word.
        /// </summary>
        /// <param name="UserID">The user identifier.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet CheckOldPd(int UserID, BETenant oTenant)
        {
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                return oPermission.CheckOldPd(UserID);
            }
        }

        /// <summary>
        /// Gets the last password change.
        /// </summary>
        /// <param name="LoginName">Name of the login.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DateTime GetLastPasswordChange(string LoginName, BETenant oTenant)
        {
            using (DLPermission objPermission = new DLPermission(oTenant))
            {
                return objPermission.GetLastPasswordChange(LoginName);
            }
        }


        /// <summary>
        /// Changes the type of the user.
        /// </summary>
        /// <param name="LoginName">Name of the login.</param>
        /// <param name="UserType">Type of the user.</param>
        /// <param name="oTenant">The Tenant.</param>
        public void ChangeUserType(string LoginName, string UserType, BETenant oTenant)
        {
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                if (UserType == "1")
                {
                    oPermission.ChangeUser_LANIDUser(LoginName);
                }
                else
                {
                    oPermission.ChangeUser_NONLANIDUser(LoginName);
                }
            }
        }


        /// <summary>
        /// Changes the user password.
        /// </summary>
        /// <param name="LoginName">Name of the login.</param>
        /// <param name="oTenant">The Tenant.</param>
        public void ChangeUserPd(string LoginName, BETenant oTenant)
        {
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                oPermission.ChangeUserPd(LoginName);
            }


        }


        /// <summary>
        /// Determines whether [is LAN user] [the specified login name].
        /// </summary>
        /// <param name="LoginName">Name of the login.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns>
        ///   <c>true</c> if [is LAN user] [the specified login name]; otherwise, <c>false</c>.
        /// </returns>
        public Boolean IsLANUser(string LoginName, BETenant oTenant)
        {
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                return oPermission.IsLANUser(LoginName);
            }
        }


        /// <summary>
        /// Gets the user list.
        /// </summary>
        /// <param name="Process">The process.</param>
        /// <param name="Campaign">The campaign.</param>
        /// <param name="Team">The team.</param>
        /// <param name="StartDate">The start date.</param>
        /// <param name="EndDate">The end date.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet GetUserList(int CLientID, int Process, int Campaign, int Team, DateTime? StartDate, DateTime? EndDate, BETenant oTenant)
        {
            using (DLPermission objPermission = new DLPermission(oTenant))
            {
                return objPermission.GetUserList(CLientID, Process, Campaign, Team, StartDate, EndDate);
            }
        }

        /// <summary>
        /// Gets the process family owner list.
        /// </summary>
        /// <param name="sUserName">Name of the s user.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public List<BEUserInfo> GetProcessFamilyOwnerList(string sUserName, BETenant oTenant)
        {
            using (DLPermission objPermission = new DLPermission(oTenant))
            {
                return objPermission.GetProcessFamilyOwnerList(sUserName);
            }
        }
        /// <summary>
        /// Gets the user list process.
        /// </summary>
        /// <param name="iProcess">The process.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet GetUserListProcess(int iProcess, BETenant oTenant)
        {
            using (DLPermission objPermission = new DLPermission(oTenant))
            {
                return objPermission.GetUserListProcess(iProcess);
            }
        }

        /// <summary>
        /// Gets the user list process.
        /// </summary>
        /// <param name="iProcess">The process.</param>
        /// <param name="IsAllUsers">if set to <c>true</c> [is all users].</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet GetUserListProcess(int iProcess, bool IsAllUsers, BETenant oTenant)
        {
            using (DLPermission objPermission = new DLPermission(oTenant))
            {
                return objPermission.GetUserListProcess(iProcess, IsAllUsers);
            }
        }

        /// <summary>
        /// Gets the user list process.
        /// </summary>
        /// <param name="iProcess">The process.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet GetUserListProcessAgentAM(int iProcess, BETenant oTenant)
        {
            using (DLPermission objPermission = new DLPermission(oTenant))
            {
                return objPermission.GetUserListProcessAgentAM(iProcess);
            }
        }
        /// <summary>
        /// Gets the user list process agent AM client QCA.
        /// </summary>
        /// <param name="iProcess">The process.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet GetUserListProcessAgentAMClientQCA(int iProcess, BETenant oTenant)
        {
            using (DLPermission objPermission = new DLPermission(oTenant))
            {
                return objPermission.GetUserListProcessAgentAMClientQCA(iProcess);
            }
        }

        //added by Omkar
        /// <summary>
        /// Return Mentor with TM Name
        /// </summary>
        /// <param name="iTMId">The tm identifier.</param>
        /// <param name="sStartDate">The s start date.</param>
        /// <param name="sEndDate">The s end date.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet GetMentorWithTMid(int iTMId, string sStartDate, string sEndDate, BETenant oTenant)
        {
            using (DLPermission objPermission = new DLPermission(oTenant))
            {
                return objPermission.GetMentorWithTMid(iTMId, sStartDate, sEndDate);
            }
        }

        /// <summary>
        /// Gets the user list process.
        /// </summary>
        /// <param name="iProcess">The process.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet GetAllUserListProcessAgentAM(int iProcess, BETenant oTenant)
        {
            using (DLPermission objPermission = new DLPermission(oTenant))
            {
                return objPermission.GetAllUserListProcessAgentAM(iProcess);
            }
        }

        /// <summary>
        /// Gets the user list process agent.
        /// </summary>
        /// <param name="iProcess">The process.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet GetUserListProcessAgent(int iProcess, BETenant oTenant)
        {
            using (DLPermission objPermission = new DLPermission(oTenant))
            {
                return objPermission.GetUserListProcessAgent(iProcess);
            }
        }
        /// <summary>
        /// Gets the user list process VP and above.
        /// </summary>
        /// <param name="sUser">The s user.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet GetUserListVPAndAbove(string sUser, BETenant oTenant)
        {
            using (DLPermission objPermission = new DLPermission(oTenant))
            {
                return objPermission.GetUserListVPAndAbove(sUser);
            }
        }

        /// <summary>
        /// Gets the user setting.
        /// </summary>
        /// <param name="iUserID">The user ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public List<BEUserSetting> GetUserSetting(int iUserID, BETenant oTenant)
        {
            using (DLPermission objPermission = new DLPermission(oTenant))
            {
                return objPermission.GetUserSetting(iUserID);
            }
        }


        //public List<BEUserSetting> GetRolesUserClient(int iUserID,bool isclient, BETenant oTenant)
        //{
        //    using (DLPermission objPermission = new DLPermission(oTenant))
        //    {
        //        return objPermission.GetRolesUserClient(iUserID, isclient);
        //    }
        //}

        /// <summary>
        /// Gets the ERP team.
        /// </summary>
        /// <param name="iUserID">The user ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public List<BEUserInfo> GetERPTeam(int iUserID, BETenant oTenant)
        {
            using (DLPermission objPermission = new DLPermission(oTenant))
            {
                return objPermission.GetERPTeam(iUserID);
            }
        }

        /// <summary>
        /// Gets the user process map.
        /// </summary>
        /// <param name="iUserID">The user ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public BEUserMapping GetUserProcessMap(int iUserID, BETenant oTenant)
        {
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                return oPer.GetUserProcessMap(iUserID);
            }
        }
        /// <summary>
        /// Gets the user mapping.
        /// </summary>
        /// <param name="iUserID">The user ID.</param>
        /// <param name="iRoleID">The role ID.</param>
        /// <param name="iMappedOn">The mapped on.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public BEUserMapping GetUserMapping(int iUserID, int iRoleID, int iMappedOn, BETenant oTenant)
        {
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                return oPer.GetUserMapping(iUserID, iRoleID, iMappedOn);
            }
        }
        /// <summary>
        /// Inserts the user mapping.
        /// </summary>
        /// <param name="oUserMapping">The object user mapping.</param>
        /// <param name="sDeletedNodes">The s deleted nodes.</param>
        /// <param name="iFormID">The form identifier.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void InsertUserMapping(BEUserMapping oUserMapping, string sDeletedNodes, int iFormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(iFormID, oUserMapping.iCreatedBy, PermissionSet.ADD))
            {
                //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                oPer.InsertUserMapping(oUserMapping, sDeletedNodes);
            }
        }


        /// <summary>
        /// Cancels the access request.
        /// </summary>
        /// <param name="iRequestID">The request ID.</param>
        /// <param name="iUserID">The user ID.</param>
        /// <param name="iFormID">The form ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void CancelAccessRequest(int iRequestID, int iUserID, int iFormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(iFormID, iUserID, PermissionSet.DELETE))
            {
                //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                oPer.CancelAccessRequest(iRequestID, iUserID);
            }
        }
        /// <summary>
        /// Cancels the request in between.
        /// </summary>
        /// <param name="iRequestID">The request identifier.</param>
        /// <param name="iUserID">The user identifier.</param>
        /// <param name="iFormID">The form identifier.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void CancelRequestInBetween(int iRequestID, int iUserID, int iFormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(iFormID, iUserID, PermissionSet.DELETE))
            {
                //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                oPer.CancelRequestInBetween(iRequestID, iUserID);
            }
        }

        /// <summary>
        /// Approves the access request.
        /// </summary>
        /// <param name="iRequestID">The request ID.</param>
        /// <param name="iRequestTypeID">The request type ID.</param>
        /// <param name="iRequestType">Type of the request.</param>
        /// <param name="iApprovalLevel">The approval level.</param>
        /// <param name="iUserID">The user ID.</param>
        /// <param name="iFormID">The form ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void ApproveAccessRequest(int iRequestID, int iRequestTypeID, int iRequestType, int iApprovalLevel, int iUserID, int iFormID, BETenant oTenant)
        {
            // iUserID = 124097;//Sowmya Choudary
            if (!CheckPermission.hasPermission(iFormID, iUserID, PermissionSet.ADD))
            {
                //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                oPer.ApproveAccessRequest(iRequestID, iRequestTypeID, iRequestType, iApprovalLevel, iUserID);
            }
        }
        /// <summary>
        /// Rejects the access request.
        /// </summary>
        /// <param name="iRequestID">The request ID.</param>
        /// <param name="iRequestTypeID">The request type ID.</param>
        /// <param name="iRequestType">Type of the request.</param>
        /// <param name="iApprovalLevel">The approval level.</param>
        /// <param name="iUserID">The user ID.</param>
        /// <param name="iFormID">The form ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void RejectAccessRequest(int iRequestID, int iRequestTypeID, int iRequestType, int iApprovalLevel, int iUserID, int iFormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(iFormID, iUserID, PermissionSet.ADD))
            {
                //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                oPer.RejectAccessRequest(iRequestID, iRequestTypeID, iRequestType, iApprovalLevel, iUserID);
            }
        }

        /// <summary>
        /// Inserts the user mapping for approval.
        /// </summary>
        /// <param name="oUserMapping">The o user mapping.</param>
        /// <param name="sDeletedNodes">The s deleted nodes.</param>
        /// <param name="iFormID">The form ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public int InsertUserMappingForApproval(BEUserMapping oUserMapping, string sDeletedNodes, int iFormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(iFormID, oUserMapping.iCreatedBy, PermissionSet.ADD))
            {
                //   throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                return oPer.InsertUserMappingForApproval(oUserMapping, sDeletedNodes);
            }
        }
        /// <summary>
        /// Inserts the user mapping approvers.
        /// </summary>
        /// <param name="dtApproverList">The dt approver list.</param>
        /// <param name="iUserID">The user ID.</param>
        /// <param name="iFormID">The form ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public void InsertUserMappingApprovers(DataTable dtApproverList, int iUserID, int iFormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(iFormID, iUserID, PermissionSet.ADD))
            {
                // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                oPer.InsertUserMappingApprovers(dtApproverList, iUserID);
            }
        }
        #region IsAdminUser
        /// <summary>
        /// Determines whether [is admin user] [the specified userID].
        /// </summary>
        /// <param name="iUserID">The user ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns>
        ///   <c>true</c> if [is admin user] [the specified userID]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAdminUser(int iUserID, BETenant oTenant)
        {
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                return oPer.IsAdminUser(iUserID);
            }
        }
        #endregion

        /// <summary>
        /// Gets the default page.
        /// </summary>
        /// <param name="iUserID">The user ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public string GetDefaultPage(int iUserID, BETenant oTenant)
        {
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                return oPer.GetDefaultPage(iUserID);
            }
        }

        /// <summary>
        /// Gets the type of the user request.
        /// </summary>
        /// <param name="oUserMapping">The o user mapping.</param>
        /// <param name="RequestId">The request id.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet GetUserRequestType(BEUserMapping oUserMapping, int RequestId, BETenant oTenant)
        {
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                return oPer.GetUserRequestType(oUserMapping, RequestId);
            }
        }
        /// <summary>
        /// Gets the user request status.
        /// </summary>
        /// <param name="iUser">The user.</param>
        /// <param name="dtFromDate">The dt from date.</param>
        /// <param name="dtToDate">The dt to date.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet GetUserRequestStatus(int iUser, DateTime dtFromDate, DateTime dtToDate, BETenant oTenant)
        {
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                return oPer.GetUserRequestStatus(iUser, dtFromDate, dtToDate);
            }
        }
        /// <summary>
        /// Gets the user approval list.
        /// </summary>
        /// <param name="iUser">The user.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet GetUserApprovalList(int iUser, BETenant oTenant)
        {
            // iUser = 124097;//Sowmya Choudary
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                return oPer.GetUserApprovalList(iUser);
            }
        }
        /// <summary>
        /// Gets the user approver list. New Query Create Date - 26/9/2016
        /// </summary>
        /// <param name="UserId">The user id.</param>
        /// <param name="iFormId">The form identifier.</param>
        /// <param name="ProcessId">The process id.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public List<BEUserInfo> GetUserApproverListByProcessId(int UserId, int iFormId, int ProcessId, BETenant oTenant)
        {
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                return oPer.GetUserApproverListByProcess(UserId, iFormId, ProcessId);
            }
        }
        /// <summary>
        /// Gets the user approver list.
        /// </summary>
        /// <param name="UserId">The user id.</param>
        /// <param name="ClientId">The client id.</param>
        /// <param name="ProcessId">The process id.</param>
        /// <param name="Flag">The flag.</param>
        /// <param name="iFormID">The form identifier.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet GetUserApproverList(int UserId, int ClientId, int ProcessId, int Flag, int iFormID, BETenant oTenant)
        {
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                return oPer.GetUserApproverList(UserId, ClientId, ProcessId, Flag, iFormID);
            }
        }
        /// <summary>
        /// Gets the user role list user role.
        /// </summary>
        /// <param name="iUserid">The userid.</param>
        /// <param name="iFormId">The form identifier.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public List<BEUserInfo> GetUserRoleListUserRole(int iUserid, int iFormId, BETenant oTenant)
        {
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                return oPer.GetUserRoleApproverListUserRole(iUserid, iFormId);
            }
        }
        /// <summary>
        /// Gets the user role approver list.
        /// </summary>
        /// <param name="iUserid">The iUserid.</param>
        /// <param name="iFormId">The iFormId.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet GetUserRoleApproverListUserRole(int iUserid, int iFormId, BETenant oTenant)
        {
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                return oPer.GetUserRoleApproverList(iUserid, iFormId);
            }
        }
        /// <summary>
        /// Gets the user role approver list.
        /// </summary>
        /// <param name="RoleId">The role id.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet GetUserRoleApproverList(int RoleId, BETenant oTenant)
        {
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                return oPer.GetUserRoleApproverList(RoleId, 4);
            }
        }
        /// <summary>
        /// Gets the client request approver list.
        /// </summary>
        /// <param name="ProcessId">The process id.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public DataSet GetClientRequestApproverList(int ProcessId, BETenant oTenant)
        {
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                return oPer.GetClientRequestApproverList(ProcessId);
            }
        }


        public BEUserInfo GetUserInformation(string sHostName, string sIPAddress, string UserName, BETenant oTenant, bool isWindowsAuthentication = true)
        {
            isWindowsAuthentication = true;
            BEUserInfo oLDAPUser = new BEUserInfo();
            try
            {

                BLAuthenticate oBLAuthenticate = new BLAuthenticate();
                BESession oBESession = new BESession();
                BLPermission oPermission = new BLPermission();
                int SessionID = 0;
                bool bProcessMapped = false;
                oBESession.sSystemSessionID = "";
                oBESession.sHostName = sHostName;
                oBESession.sIPAddress = sIPAddress;
                oLDAPUser = oBLAuthenticate.IsLADPUser(UserName, oBESession, out SessionID, out bProcessMapped, oTenant, isWindowsAuthentication)[0];
            }
            catch (Exception ex)
            {
                oLDAPUser.sInvalidReason = ex.Message;
            }
            return oLDAPUser;
        }

        public List<BEUserInfo> GetBotUserList(int iUserID, int iProcessId, bool bActive, BETenant oTenant)
        {
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                return oPer.GetBotUserList(iUserID, iProcessId, bActive);
            }
        }

        //public DataSet GetBotUserList(int iUserID, int iProcessId, bool bActive, BETenant oTenant)
        //{
        //    using (DLPermission oPer = new DLPermission(oTenant))
        //    {
        //        return oPer.GetBotUserList( iUserID,  iProcessId,  bActive);
        //    }
        //}
        //public DataSet GetDowntimeReason(bool bActive, BETenant oTenant)
        //{
        //    using (DLPermission oPer = new DLPermission(oTenant))
        //    {
        //        return oPer.GetDowntimeReason(bActive);
        //    }
        //}
        public List<BEBOTDowntimeInfo> GetSearchDataList(int iUserId, int CaptureId, BETenant oTenant)
        {
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                return oPer.GetSearchDataList(iUserId, CaptureId);
            }
        }

        public List<BEBOTDowntimeInfo> GetSearchData(int iUserId, string CampaignName, BETenant oTenant)
        {
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                return oPer.GetSearchData(iUserId, CampaignName);
            }
        }

        public List<BEBOTDowntimeInfo> GetDowntimeReason(bool bActive, BETenant oTenant)
        {
            using (DLPermission oPer = new DLPermission(oTenant))
            {
                return oPer.GetDowntimeReason(bActive);
            }
        }
    }
}
