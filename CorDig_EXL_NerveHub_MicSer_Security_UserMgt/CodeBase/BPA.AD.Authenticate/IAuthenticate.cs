using BPA.Security.BusinessEntity.Security;

namespace BPA.AD.Authenticate
{
    /// <summary>
    /// Interface use for LDAP authentication
    /// </summary>
    public interface IAuthenticate : IDisposable
    {
        /// <summary>
        /// Authenticates the specified user name.
        /// </summary>
        /// <returns></returns>
        bool Authenticate();

        /// <summary>
        /// Gets the user list.
        /// </summary>
        /// <param name="LoginName">Login Name.</param>
        /// <param name="Domain">The domain.</param>
        /// <returns>List of All the Active User</returns>
        IList<BELdapUserInfo> GetUserList(string LoginName, SearchByEnum searchby);

        /// <summary>
        /// Checks the password expire.
        /// </summary>
        /// <param name="strLoginName">Name of the STR login.</param>
        /// <param name="output">The output.</param>
        /// <param name="DaysLeft">The days left.</param>
        /// <returns></returns>
        string CheckPasswordExpire(string strLoginName, out string output, out int DaysLeft);


        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="sLoginName">Name of the s login.</param>
        /// <param name="sCurrentPassword">The s current password.</param>
        /// <param name="sNewPass">The s new pass.</param>
        /// <returns></returns>
        bool ChangePd(string sLoginName, string sCurrentPd, string sNewPass);

        /// <summary>
        /// Gets the users group.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="UserName">Name of the user.</param>
        /// <returns></returns>
        IList<BELdapUserInfo> GetUsersGroup(string UserName);

        /// <summary>
        /// Gets the users in group.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        IList<BELdapUserInfo> GetUsersInGroup(string group);
    }
}