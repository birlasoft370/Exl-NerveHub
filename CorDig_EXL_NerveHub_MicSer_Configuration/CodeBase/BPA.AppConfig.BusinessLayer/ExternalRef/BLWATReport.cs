using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef;
using BPA.AppConfig.Datalayer.ExternalRef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessLayer.ExternalRef
{
    public class BLWATReport : IDisposable, IWatReport
    {
        public void Dispose()
        { }

        public void SaveProcessFamily(List<BEProcessFamilyMap> lstProcessFamily, BETenant oTenant)
        {
            using (DLWATReport objWATReport = new DLWATReport(oTenant))
            {
                objWATReport.SaveProcessFamily(lstProcessFamily);
            }
        }

        public void UpdateProcessFamily(List<BEProcessFamilyMap> lstProcessFamily, int iProcessFamilyID, BETenant oTenant)
        {
            using (DLWATReport objWATReport = new DLWATReport(oTenant))
            {
                objWATReport.UpdateProcessFamily(lstProcessFamily, iProcessFamilyID);
            }
        }

        public void DisableProcessFamily(int iProcessFamilyID, int iUserID, BETenant oTenant)
        {
            using (DLWATReport objWATReport = new DLWATReport(oTenant))
            {
                objWATReport.DisableProcessFamily(iProcessFamilyID, iUserID);
            }
        }
        public List<BEProcessFamilyMap> GetProcessFamilyList(int iProcessFamilyID, BETenant oTenant)
        {
            using (DLWATReport objWATReport = new DLWATReport(oTenant))
            {
                return objWATReport.GetProcessFamilyList(iProcessFamilyID);
            }
        }

        public List<BEProcessFamilyMap> GetProcessFamilyList(string sProcessFamilyName, BETenant oTenant)
        {
            using (DLWATReport objWATReport = new DLWATReport(oTenant))
            {
                return objWATReport.GetProcessFamilyList(sProcessFamilyName);
            }
        }
    }
}
