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
using System.Data;
using System.ServiceModel;

namespace BPA.Security.BusinessLayer.Security
{
    /// <summary>
    ///  This class is used to create, edit, deleted, 
    ///  lock unlock users of application
    /// </summary>
   // [ExceptionShielding("WCF Exception Shielding")]
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class BLUserAccessRequest : IUserAccessRequestService, IDisposable
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="BLUserAccessRequest"/> class.
        /// </summary>
        public BLUserAccessRequest()
        { }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }
        #endregion

        /// <summary>
        /// Gets the user details.
        /// </summary>
        /// <param name="iUserID">The i user ID.</param>
        /// <returns></returns>
        public DataSet GetUserDetails(int iUserID, BETenant oTenant)
        {
            using (DLUserAccessRequest oUAR = new DLUserAccessRequest(oTenant))
            {
                return oUAR.GetUserDetails(iUserID);
            }
        }
        /// <summary>
        /// Gets the client list.
        /// </summary>
        /// <param name="iLoggedinUserID">The i loggedin user ID.</param>
        /// <param name="bActiveClient">if set to <c>true</c> [b active client].</param>
        /// <returns></returns>
        public DataSet GetClientList(int iLoggedinUserID, bool bActiveClient, BETenant oTenant)
        {
            using (DLUserAccessRequest oUAR = new DLUserAccessRequest(oTenant))
            {
                return oUAR.GetClientList(iLoggedinUserID, bActiveClient);
            }
        }

        /// <summary>
        /// Gets the process list.
        /// </summary>
        /// <param name="iLoggedinUserID">The i loggedin user ID.</param>
        /// <param name="bActiveProcess">if set to <c>true</c> [b active process].</param>
        /// <param name="sClientID">The s client ID.</param>
        /// <returns></returns>
        public DataSet GetProcessList(int iLoggedinUserID, bool bActiveProcess, string sClientID, BETenant oTenant)
        {
            using (DLUserAccessRequest oUAR = new DLUserAccessRequest(oTenant))
            {
                return oUAR.GetProcessList(iLoggedinUserID, bActiveProcess, sClientID);
            }
        }

        /// <summary>
        /// Updates the user details.
        /// </summary>
        /// <param name="oUser">The o user.</param>
        public void UpdateUserDetails(BEUserInfo oUser, int iFormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(iFormID, oUser.iCreatedBy, PermissionSet.UPDATE))
            {
               // throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }
            using (DLUserAccessRequest oUAR = new DLUserAccessRequest(oTenant))
            {
                oUAR.UpdateUserDetails(oUser);
            }
        }
    }
}
