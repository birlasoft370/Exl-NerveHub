using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessLayer.CacheData;
using BPA.EmailManagement.DataLayer.ExternalRef.WorkAllocation;
using BPA.EmailManagement.ServiceContract.ServiceContracts.WorkAllocation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.EmailManagement.BusinessLayer.ExternalRef.WorkAllocation
{
    public class BLWorkUpload : IWorkUploadService, IDisposable
    {

        public void Dispose()
        {

        }

        public void SendErrorMail(string sWorkTable, string strErr, int totalRec, string StartDate, string Subject, BETenant oTenant)
        {
            using (DLWorkUpload objWork = new DLWorkUpload(oTenant))
            {
                objWork.SendErrorMail(sWorkTable, strErr, totalRec, StartDate, Subject);
            }
        }

        public int GetBatchIdValue(int CampaignId, string BatchCode, BETenant oTenant)
        {
            using (DLWorkUpload objWorkUpload = new DLWorkUpload(oTenant))
            {
                return objWorkUpload.GetBatchIdValue(CampaignId, BatchCode);
            }
        }

        public DataTable GetUserOffSet(BETenant oTenant)
        {
            using (DLWorkUpload objWork = new DLWorkUpload(oTenant))
            {
                return objWork.GetUserOffSet();
            }
        }
        public string BulkUpload(int iCampaignID, DataTable dt, string sWorkTable, BETenant oTenant)
        {
            string strreturnValue = "";
            using (DLWorkUpload objWork = new DLWorkUpload(oTenant))
            {
                strreturnValue = objWork.BulkUpload(dt, sWorkTable);
            }
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    using (BLWorkItemsQueue oQueue = new BLWorkItemsQueue(oTenant))
                    {
                        oQueue.RemoveCache(iCampaignID);
                    }
                }
            }
            return strreturnValue;

        }

    }
}
