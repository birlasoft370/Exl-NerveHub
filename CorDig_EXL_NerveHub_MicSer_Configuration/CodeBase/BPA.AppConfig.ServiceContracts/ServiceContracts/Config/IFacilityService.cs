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
    [ServiceContract(Name = "FacilityServiceContract")]
    public interface IFacilityService : IDisposable
    {
        List<BEFacility> GetFacilityList(string FacilityName, bool bActiveFacility, BETenant oTenant);
        List<BEFacility> GetFacilityList(int iFacilityID, BETenant oTenant);
        void InsertData(BEFacility oFacility, int iFormID, BETenant oTenant);
        void UpdateData(BEFacility oFacility, int iFormID, BETenant oTenant);
    }
}
