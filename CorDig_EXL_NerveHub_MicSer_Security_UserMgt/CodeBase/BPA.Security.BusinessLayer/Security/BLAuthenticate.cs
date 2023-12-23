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

using BPA.AD.Authenticate;
using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using BPA.Security.Datalayer.Security;
using BPA.Security.ServiceContracts.Security;
using BPA.Utility;

namespace BPA.Security.BusinessLayer.Security
{
    /// <summary>
    /// Authenticate User
    /// </summary>
    /// <seealso cref="BPA.Security.ServiceContracts.Security.IAuthenticateService" />
    /// <seealso cref="System.IDisposable" />
   // [ExceptionShielding("WCF Exception Shielding")]
    public class BLAuthenticate :  IAuthenticateService,IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BLAuthenticate" /> class.
        /// </summary>
        CacheHelper _UserInfoCache = null;

        public BLAuthenticate()
        {
            _UserInfoCache = new CacheHelper("UserInfo");
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }


        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <param name="LoginID">The login ID.</param>
        /// <param name="Password">The password.</param>
        /// <param name="SessionID">The session ID.</param>
        /// <param name="UserHostName">Name of the user host.</param>
        /// <param name="Disabled">if set to <c>true</c> [disabled].</param>
        /// <param name="oTenant">The tenant.</param>
        /// <returns></returns>
        /// <exception cref="ExceptionHandler.ExceptionType.BusinessLogicCustomException"></exception>
        public BEUserInfo AuthenticateUser(string LoginID, string Password, string SessionID, string UserHostName, out Boolean Disabled, BETenant oTenant)
        {
            if (LoginID == string.Empty || LoginID == "" || Password == string.Empty || Password == "")
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            using (DLPermission oPermission = new DLPermission(oTenant))
            {
                return oPermission.GetAuthenticateUser(LoginID, Password, SessionID, UserHostName, out Disabled);
            }
        }

        /// <summary>
        /// Determines whether [is ladp user] [the specified login name].
        /// </summary>
        /// <param name="LoginName">Name of the login.</param>
        /// <param name="oBESession">The o be session.</param>
        /// <param name="iSessionID">The i session identifier.</param>
        /// <param name="bProcessMap">if set to <c>true</c> [b process map].</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <param name="isWindowsAuthentication">if set to <c>true</c> [is windows authentication].</param>
        /// <returns></returns>
        public List<BEUserInfo> IsLADPUser(string LoginName, BESession oBESession, out int iSessionID, out bool bProcessMap, BETenant oTenant, bool isWindowsAuthentication = true, int cachDuration = 24)
        {
            return IsLADPUser(LoginName, oBESession, out iSessionID, out bProcessMap, false, oTenant, isWindowsAuthentication, cachDuration);
        }
        /// <summary>
        /// Determines whether [is LADP user] [the specified login name].
        /// </summary>
        /// <param name="LoginName">Name of the login.</param>
        /// <param name="oBESession">The BE session.</param>
        /// <param name="iSessionID">The i session identifier.</param>
        /// <param name="bProcessMap">if set to <c>true</c> [b process map].</param>
        /// <param name="isMossApplicationMenu">if set to <c>true</c> [is moss application menu].</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <param name="isWindowsAuthentication">if set to <c>true</c> [is windows authentication].</param>
        /// <returns>
        ///   <c>true</c> if [is LADP user] [the specified login name]; otherwise, <c>false</c>.
        /// </returns>
        public List<BEUserInfo> IsLADPUser(string LoginName, BESession oBESession, out int iSessionID, out bool bProcessMap, bool isMossApplicationMenu, BETenant oTenant, bool isWindowsAuthentication = true, int cachDuration = 24)
        {
            List<BEUserInfo> oAppUserCacheObject = new();
            string cacheName = $"UserInfo_{LoginName}_{oBESession.sIPAddress}";
            oAppUserCacheObject = (List<BEUserInfo>)_UserInfoCache.GetFromCache(cacheName);
            if (oAppUserCacheObject == null || oAppUserCacheObject.Count == 0)
            {
                using (DLPermission oDLPermission = new DLPermission(oTenant))
                {
                    oAppUserCacheObject = oDLPermission.IsLDAPUser(LoginName, oBESession, out iSessionID, out bProcessMap, isMossApplicationMenu, isWindowsAuthentication);
                    _UserInfoCache.AddToCache(cacheName, oAppUserCacheObject, TimeSpan.FromHours(cachDuration));
                }
            }
            iSessionID = 0;
            bProcessMap = false;
            return oAppUserCacheObject;
        }

        /// <summary>
        /// Inserts the session log out.
        /// </summary>
        /// <param name="ServerName">Name of the server.</param>
        /// <param name="UserAgent">The user agent.</param>
        /// <param name="IP">The ip.</param>
        /// <param name="Host">The host.</param>
        /// <param name="UserName">Name of the user.</param>
        /// <param name="URLTReferrer">The urlt referrer.</param>
        /// <param name="URL">The URL.</param>
        /// <param name="SessionID">The session identifier.</param>
        /// <param name="IsNewSession">if set to <c>true</c> [is new session].</param>
        /// <param name="SessionTimeOut">The session time out.</param>
        /// <param name="oTenant">The Tenant.</param>
        public void InsertSessionLogOut(string ServerName, string UserAgent, string IP, string Host, string UserName, string URLTReferrer, string URL, string SessionID, Boolean IsNewSession, int SessionTimeOut, BETenant oTenant)
        {
            using (DLPermission ObjPermission = new DLPermission(oTenant))
            {
                ObjPermission.InsertSessionLogOut(ServerName, UserAgent, IP, Host, UserName, URLTReferrer, URL, SessionID, IsNewSession, SessionTimeOut);
            }
        }

        /// <summary>
        /// Determines whether the specified user name is authenticated.
        /// </summary>
        /// <param name="UserName">Name of the user.</param>
        /// <param name="PasPhrase">The pas phrase.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns>
        ///   <c>true</c> if the specified user name is authenticated; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAuthenticated(string UserName, string PasPhrase, BETenant oTenant)
        {
            bool returnvalue = false;
            using (IAuthenticate iA = new LDAPUser(UserName, PasPhrase, "CORP"))
            {
                returnvalue = iA.Authenticate();
            }
            return returnvalue;
        }


        /// <summary>
        /// Authenticates the specified Tenant.
        /// </summary>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Authenticate(BETenant oTenant)
        {
            throw new NotImplementedException();
        }

        ///// <summary>
        ///// Checks the password expire.
        ///// </summary>
        ///// <param name="strLoginName">Name of the string login.</param>
        ///// <param name="output">The output.</param>
        ///// <param name="DaysLeft">The days left.</param>
        ///// <param name="oTenant">The Tenant.</param>
        ///// <returns></returns>
        ///// <exception cref="NotImplementedException"></exception>
        //public string CheckPasswordExpire(string strLoginName, out string output, out int DaysLeft, BETenant oTenant)
        //{
        //    output = "";
        //    DaysLeft = 0;
        //    return string.Empty;
        //    //throw new NotImplementedException();
        //}

        /// <summary>
        /// Gets the user list.
        /// </summary>
        /// <param name="LoginName">Name of the login.</param>
        /// <param name="Domain">The domain.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public IList<BELdapUserInfo> GetUserList(string LoginName, string Domain, BETenant oTenant)
        {
            IList<BELdapUserInfo> lstUserInfo = new List<BELdapUserInfo>();
            using (LDAPUser adUser = new LDAPUser(Domain))
            {
                lstUserInfo = adUser.GetUserList(LoginName, SearchByEnum.UserID);
            }
            return lstUserInfo;
        }


        /// <summary>
        /// Change Password of User.
        /// </summary>
        /// <param name="sLoginName">The login Name.</param>
        /// <param name="sCurrentPassword">The current password.</param>
        /// <param name="sNewPass">The New Password</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <param name="isWindowsAuthenticate">if set to <c>true</c> [is windows authenticate].</param>
        /// <param name="iUserId">The i user identifier.</param>
        /// <returns></returns>

        public string ChangePd(string sLoginName, string sCurrentPd, string sNewPass, BETenant oTenant, bool isWindowsAuthenticate = true, int iUserId = 0)
        {          
            if (isWindowsAuthenticate)
            {
                //using (LDAPUser adUser = new LDAPUser())
                //{
                //  var  isSuccess = adUser.ChangePassword(sLoginName, sCurrentPassword, sNewPass);
                //  return isSuccess == true ? "Password Changed Sucessfully." : "Error in password change.Please contact adminstrator.";
                //}
                return "Password Changed Sucessfully.";
            }
            else
            {
                using (DLPermission oDLPermission = new DLPermission(oTenant))
                {
                    return oDLPermission.InsertUpdatePassword(sLoginName, sCurrentPd, sNewPass, iUserId);
                }
            }
        }
        /// <summary>
        /// Gets the users group.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="UserName">Name of the user.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IList<BELdapUserInfo> GetUsersGroup(string domain, string UserName, BETenant oTenant)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the users in group.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="group">The group.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IList<BELdapUserInfo> GetUsersInGroup(string domain, string group, BETenant oTenant)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Checks the password expire.
        /// </summary>
        /// <param name="strLoginName">Name of the string login.</param>
        /// <param name="output">The output.</param>
        /// <param name="DaysLeft">The days left.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string CheckPasswordExpire(string strLoginName, out string output, out int DaysLeft, string Domain, BETenant oTenant)
        {
            string DisplayText = "";
            using (LDAPUser adUser = new LDAPUser(Domain))
            {
                DisplayText = adUser.CheckPasswordExpire(strLoginName, out output, out DaysLeft);
            }
            return DisplayText;
        }
        /// <summary>
        /// Change Password of User.
        /// </summary>
        /// <param name="sLoginName">The login Name.</param>
        /// <param name="sCurrentPassword">The current password.</param>
        /// <param name="sNewPass">The New Password</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <param name="Domain">if set to <c>true</c> [is windows authenticate].</param>

        /// <returns></returns>

        public string Change_Password(string sLoginName, string sCurrentPassword, string sNewPass, string Domain, BETenant oTenant)
        {

            using (LDAPUser adUser = new LDAPUser(Domain))
            {
                string ErrorMsg = string.Empty;
                var isSuccess = adUser.ChangePassword(sLoginName, sCurrentPassword, sNewPass, out ErrorMsg);
                //return isSuccess == true ? "Password Changed Sucessfully." : "Error in password change.Please contact adminstrator.";
                return isSuccess == true ? "Sucessfully" : ErrorMsg;
            }
            // return "Password Changed Sucessfully.";

        }
        public void InsertUserPageLogin(string SystemSessionID, string HostName, string IPAddress, string PageName, int iUserId, BETenant oTenant)
        {
            using (DLPermission oDLPermission = new DLPermission(oTenant))
            {
                oDLPermission.InsertUserPageLogin(SystemSessionID, HostName, IPAddress, PageName, iUserId);
            }
        }

        public bool IsDomainActivation(BETenant oTenant)
        {
            using (DLPermission ObjPermission = new DLPermission(oTenant))
            {
               return ObjPermission.IsDomainActivation();
            }
        }
    }
}
