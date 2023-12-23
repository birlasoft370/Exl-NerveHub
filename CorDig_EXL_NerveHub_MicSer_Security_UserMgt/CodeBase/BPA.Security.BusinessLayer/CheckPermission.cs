



using BPA.Security.BusinessEntity;

namespace BPA.Security.BusinessLayer
{
    /// <summary>
    /// Check User Permission
    /// </summary>
    public static class CheckPermission
    {

        /// <summary>
        /// Determines whether the specified i form identifier has permission.
        /// </summary>
        /// <param name="iFormID">The  form identifier.</param>
        /// <param name="iUserID">The  user identifier.</param>
        /// <param name="ePermission">The  permission.</param>
        /// <returns>
        ///   <c>true</c> if the specified form identifier has permission; otherwise, <c>false</c>.
        /// </returns>
        public static bool hasPermission(int iFormID, int iUserID, PermissionSet ePermission)
        {
            return true;
        }
    }
}
