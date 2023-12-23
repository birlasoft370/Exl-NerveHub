using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity;
using BPA.Security.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.Security.BusinessLayer
{
    /// <summary>
    /// Cache Menu Items
    /// </summary>
    public class BLMenuItems : IDisposable
    {
        private int _iRoleID;
        private BETenant _oTenant = null;
        private bool _isMossApplicationMenu;
      //  CacheHelper _MenuCache = null;
        #region Dispose
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
           // _MenuCache = null;
        }
        #endregion

        #region Construct


        /// <summary>
        /// Initializes a new instance of the <see cref="BLMenuItems"/> class.
        /// </summary>
        public BLMenuItems()
        {

           // _MenuCache = new CacheHelper("BPAMenuItems");

        }


        /// <summary>
        /// Initializes a new instance of the <see cref="BLMenuItems"/> class.
        /// </summary>
        /// <param name="iRoleID">The i role ID.</param>
        /// <param name="isMossApplicationMenu">if set to <c>true</c> [is moss application menu].</param>
        public BLMenuItems(int iRoleID, bool isMossApplicationMenu, BETenant oTenant)
        {
            _iRoleID = iRoleID;
            _isMossApplicationMenu = isMossApplicationMenu;
            _oTenant = oTenant;
           // _MenuCache = new CacheHelper("BPAMenuItems");

        }
        #endregion


        #region public method
        /// <summary>
        /// Gets the get work items.
        /// </summary>
        /// <value>The get work items.</value>
        public List<BEMenuItems> GetMenuItems
        {
            get { return GetCacheItems(_iRoleID, _isMossApplicationMenu, _oTenant); }
        }

        #endregion

        /// <summary>
        /// Gets the cache items.
        /// </summary>
        /// <param name="iRoleID">The campaign ID.</param>
        /// <returns></returns>
        private List<BEMenuItems> GetCacheItems(int iRoleID, bool isMossApplicationMenu, BETenant oTenant)
        {
           
            List<BEMenuItems> lMenuitem = new List<BEMenuItems>();
            if (lMenuitem == null)
            {
                using (DLMenu oMenuItems = new DLMenu(oTenant))
                {
                    lMenuitem = oMenuItems.GetRoleWiseMenu(iRoleID, isMossApplicationMenu);
                }
               
            }
            return lMenuitem;
        }

      


    }
}