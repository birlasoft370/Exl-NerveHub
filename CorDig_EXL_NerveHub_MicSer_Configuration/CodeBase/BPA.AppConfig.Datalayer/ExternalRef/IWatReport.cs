using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BPA.AppConfig.Datalayer.ExternalRef
{
    [ServiceContract(Name = "WATProcessFamily")]
    public interface IWatReport : IDisposable
    {
        void SaveProcessFamily(List<BEProcessFamilyMap> lstProcessFamily, BETenant oTenant);
        void UpdateProcessFamily(List<BEProcessFamilyMap> lstProcessFamily, int iProcessFamilyID, BETenant oTenant);
        void DisableProcessFamily(int iProcessFamilyID, int iUserID, BETenant oTenant);
        List<BEProcessFamilyMap> GetProcessFamilyList(int iProcessFamilyID, BETenant oTenant);
        List<BEProcessFamilyMap> GetProcessFamilyList(string sProcessFamilyName, BETenant oTenant);
    }
}
