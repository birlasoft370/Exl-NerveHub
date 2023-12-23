using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.EmailManagement.BusinessLayer.CacheData
{
    public class BLMailConfigCache : IDisposable
    {
        CacheHelper _EmailConfigurationCache = null;
        #region Construct

        /// <summary>
        /// Initializes a new instance of the <see cref="BLWorkItems"/> class.
        /// </summary>
        public BLMailConfigCache()
        {
            _EmailConfigurationCache = new CacheHelper("EmailObjectCache");
        }
        public void Dispose()
        {
            _EmailConfigurationCache = null;
        }
        #endregion


        #region public method
        /// <summary>
        /// Gets the get work items.
        /// </summary>
        /// <value>The get work items.</value>
        public IList<BEMailConfiguration> GetMailConfiguration(int iStoreID, BETenant oTenant)
        {
            return GetCacheItems(iStoreID, oTenant);
        }

        public void RemoveCache(int iStoreID, BETenant oTenant)
        {
            string cacheName = "MailConfiguration_" + oTenant.TenantID.ToString() + "_" + iStoreID.ToString();
            _EmailConfigurationCache.RemoveFromCache(cacheName);
        }

        /// <summary>
        /// Gets the cache items.
        /// </summary>
        /// <param name="iRoleID">The campaign ID.</param>
        /// <returns></returns>
        private IList<BEMailConfiguration> GetCacheItems(int iStoreID, BETenant oTenant)
        {
            string cacheName = "MailConfiguration_" + oTenant.TenantID.ToString() + "_" + iStoreID.ToString();
            IList<BEMailConfiguration> lMailitem = (List<BEMailConfiguration>)_EmailConfigurationCache.GetFromCache(cacheName);
            if (lMailitem == null)
            {
                using (DLMailConfiguration oMailConfigItems = new DLMailConfiguration(oTenant))
                {
                    lMailitem = oMailConfigItems.GetCampaignWiseList(iStoreID, true);
                }
                _EmailConfigurationCache.AddToCache(cacheName, lMailitem, TimeSpan.FromHours(24));
            }
            return lMailitem;
        }

        #endregion
    }
}
