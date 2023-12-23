using BPA.AppConfig.BusinessEntity;
using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.Datalayer.Security;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Security;

namespace BPA.AppConfig.BusinessLayer.Security
{
    /// <summary>
    /// Authorization
    /// </summary>
    public class BLAuthorization : IAuthorizationUserService, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BLAuthorization" /> class.
        /// </summary>
        public BLAuthorization()
        { }
        public void Dispose()
        { }

        /// <summary>
        /// Checks the permission.
        /// </summary>
        /// <param name="FormID">form ID.</param>
        /// <param name="UserID">user ID.</param>
        /// <param name="Action">action.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public bool CheckPermission(int FormID, int UserID, PermissionSet Action, BETenant oTenant)
        {
            using (DLPermission oPermision = new DLPermission(oTenant))
            {
                return oPermision.IsAuthorization(FormID, UserID, Action.ToString());
            }
        }

    }
}
