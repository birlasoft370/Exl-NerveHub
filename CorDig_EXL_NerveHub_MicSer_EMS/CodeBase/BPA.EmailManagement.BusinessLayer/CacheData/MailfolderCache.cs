using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.EmailManagement.BusinessLayer.CacheData
{
    public class MailfolderCache : IDisposable
    {
        // CacheHelper _EMailfolderCache = null;

        #region Construct

        /// <summary>
        /// Initializes a new instance of the <see cref="BLWorkItems"/> class.
        /// </summary>
        public MailfolderCache()
        {
            //_EMailfolderCache = new CacheHelper("EmailObjectCache"); 
        }

        public void Dispose()
        {
            //_EMailfolderCache = null;
        }
        #endregion


        public void RemoveCache(BEMailConfiguration mailConfig, int row, BETenant oTenant)
        {
            string cacheName = "MailFolder_" + oTenant.TenantID.ToString() + "_" + mailConfig.iCampaignID.ToString() + "_" + mailConfig.iMailConfigID.ToString() + "_" + row.ToString();
            //_EMailfolderCache.RemoveFromCache(cacheName);
        }

        public IList<BEMailReceivedDateTime> GetCacheItems(BEMailConfiguration mailConfig, int row, BETenant oTenant)
        {
            // string cacheName = "MailFolder_" + oTenant.TenantID.ToString() + "_" + mailConfig.iCampaignID.ToString() +"_" + mailConfig.iMailConfigID.ToString() + "_" + row.ToString();
            IList<BEMailReceivedDateTime> lMailReceive = null;//(List<BEMailReceivedDateTime>)_EMailfolderCache.GetFromCache(cacheName);
            if (lMailReceive == null)
            {
                using (BLMailConfiguration oMailConfig = new BLMailConfiguration())
                {
                    lMailReceive = oMailConfig.getMailReceiveDateTime(mailConfig.iCampaignID, mailConfig.iMailConfigID, row, oTenant);
                }
                //_EMailfolderCache.AddToCache(cacheName, lMailReceive, TimeSpan.FromHours(24));
            }
            return lMailReceive;
        }
    }
}
