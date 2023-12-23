using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.Config
{
    [ServiceContract(Name = "LocationServiceContract")]
    public interface ILocationService : IDisposable
    {
        List<BELocation> GetLocationList(bool bActiveLocation, BETenant oTenant);
        List<BELocation> GetLocationList(string sLocationName, bool bActiveLocation, BETenant oTenant);
        List<BELocation> GetLocationList(int iLocationID, BETenant oTenant);
        void InsertData(BELocation oLocation, int iFormID, BETenant oTenant);
        void UpdateData(BELocation oLocation, int iFormID, BETenant oTenant);
    }
}
