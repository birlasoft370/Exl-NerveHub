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
using BPA.Security.Datalayer.Security;
using System;

namespace BPA.Security.BusinessLayer.Security
{
    /// <summary>
    /// Authorization
    /// </summary>
    public class BLAuthorization
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BLAuthorization" /> class.
        /// </summary>
        private BLAuthorization()
        { }


        /// <summary>
        /// Checks the permission.
        /// </summary>
        /// <param name="FormID">form ID.</param>
        /// <param name="UserID">user ID.</param>
        /// <param name="Action">action.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public static bool CheckPermission(int FormID, int UserID, PermissionSet Action, BETenant oTenant)
        {
            using (DLPermission oPermision = new DLPermission(oTenant))
            {
                return oPermision.IsAuthorization(FormID, UserID, Action.ToString()); 
            }
        }
        
    }
}
