using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BPA.EmailManagement.ServiceContract.ServiceContracts.WorkAllocation
{
    [ServiceContract(Name = "WorkUploadServiceContract")]
    public interface IWorkUploadService : IDisposable
    {
        void SendErrorMail(string sWorkTable, string strErr, int totalRec, string StartDate, string Subject, BETenant oTenant);

        int GetBatchIdValue(int CampaignId, string BatchCode, BETenant oTenant);

        DataTable GetUserOffSet(BETenant oTenant);

        string BulkUpload(int iCampaignID, DataTable dt, string sWorkTable, BETenant oTenant);
    }
}
