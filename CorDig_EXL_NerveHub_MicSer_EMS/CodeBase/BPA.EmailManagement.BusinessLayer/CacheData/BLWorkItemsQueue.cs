using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.EmailManagement.BusinessLayer.CacheData
{
    public class BLWorkItemsQueue : IDisposable
    {
        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        private CacheHelper _WorkItemsQueueCache = null;
        private BETenant _oTenant;

        public BLWorkItemsQueue(BETenant oTenant)
        {
            _oTenant = oTenant;
            _WorkItemsQueueCache = new CacheHelper("WorkItems");
        }


        public void RemoveCache(int iCampaignID)
        {
            string NewcacheName = _oTenant.TenantID + "_" + iCampaignID.ToString() + "_WorkItemCacheReference";
            IList<string> _oCampaignCache = (IList<string>)_WorkItemsQueueCache.GetFromCache(NewcacheName);
            if (_oCampaignCache != null)
            {
                int iCount = _oCampaignCache.Count;
                for (int i = 0; i < iCount; i++)
                {
                    _WorkItemsQueueCache.RemoveFromCache(_oCampaignCache[i]);
                }
                _WorkItemsQueueCache.RemoveFromCache(NewcacheName);
            }
        }
    }
}
