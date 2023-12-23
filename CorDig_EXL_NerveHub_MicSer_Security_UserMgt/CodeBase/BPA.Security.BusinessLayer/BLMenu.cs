using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity;
using BPA.Security.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPA.Security.ServiceContract;

namespace BPA.Security.BusinessLayer
{

    /// <summary>
    /// Application Menu Items
    /// </summary>
    public class BLMenu : IMenuService, IDisposable
    {

        #region Dispose
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }
        #endregion


        /// <summary>
        /// Gets the menu.
        /// </summary>
        /// <param name="UserId">The user id.</param>
        /// <returns></returns>
        public DataSet GetMenu(int UserId, BETenant oTenant)
        {
            return GetMenu(UserId, false, oTenant);
        }


        /// <summary>
        /// Gets the menu.
        /// </summary>
        /// <param name="UserId">Logged in UserID</param>
        /// <param name="isMossApplicationMenu">if set to <c>true</c> [is moss application menu].</param>
        /// <returns></returns>
        public DataSet GetMenu(int UserId, bool isMossApplicationMenu, BETenant oTenant)
        {
            using (DLMenu u = new DLMenu(oTenant))
            {
                return u.GetMenuData(UserId, isMossApplicationMenu);
            }
        }

        /// <summary>
        /// Gets the smart menu.
        /// </summary>
        /// <param name="UserId">The user id.</param>
        /// <param name="isMossApplicationMenu">if set to <c>true</c> [is moss application menu].</param>
        /// <returns></returns>
        public DataSet GetSmartMenu(int UserId, bool isMossApplicationMenu, BETenant oTenant)
        {
            using (DLMenu u = new DLMenu(oTenant))
            {
                return u.GetSmartMenuData(UserId, isMossApplicationMenu);
            }
        }

        /// <summary>
        /// Gets the role wise menu.
        /// </summary>
        /// <param name="UserId">The user id.</param>
        /// <param name="isMossApplicationMenu">if set to <c>true</c> [is moss application menu].</param>
        /// <returns></returns>
        public List<BEMenuItems> GetRoleWiseMenu(int _iRoleID, bool isMossApplicationMenu, BETenant oTenant)
        {
          return GetCacheItems(_iRoleID, isMossApplicationMenu, oTenant); 
           
        }
        /// <summary>
        /// Gets the cache items.
        /// </summary>
        /// <param name="iRoleID">The campaign ID.</param>
        /// <returns></returns>
        private List<BEMenuItems> GetCacheItems(int iRoleID, bool isMossApplicationMenu, BETenant oTenant)
        {

            List<BEMenuItems> lMenuitem = new List<BEMenuItems>();
          
                using (DLMenu oMenuItems = new DLMenu(oTenant))
                {
                    lMenuitem = oMenuItems.GetRoleWiseMenu(iRoleID, isMossApplicationMenu);
                }

            
            return lMenuitem;
        }

        public DataSet GetLandingData(BETenant oTenant)
        {
            using (DLMenu u = new DLMenu(oTenant))
            {
                return u.GetLandingData();
            }
        }

    }
}
