using BPA.Security.BusinessEntity.ExtrernalRefre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BPA.Security.ServiceContract.ExternalRef
{
    [ServiceContract(Name = "FacilityServiceContract")]
    public interface IFacilityService : IDisposable
    {
        //[OperationContract(Name = "GetFacilityListActive")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEFacility> GetFacilityList(bool bActiveFacility, BETenant oTenant);

        //[OperationContract(Name = "GetFacilityListActiveWithFacility")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEFacility> GetFacilityList(string FacilityName, bool bActiveFacility, BETenant oTenant);

        //[OperationContract(Name = "GetFacilityListWithFacilityID")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEFacility> GetFacilityList(int iFacilityID, BETenant oTenant);

        //[OperationContract(Name = "GetLocationWiseFacilityList")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEFacility> GetLocationWiseFacilityList(int iLocationID, BETenant oTenant);

        //[OperationContract(Name = "InsData")]
        //[FaultContract(typeof(ServiceFault))]
        void InsertData(BEFacility oFacility, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "UpdData")]
        //[FaultContract(typeof(ServiceFault))]
        void UpdateData(BEFacility oFacility, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "DelData")]
        //[FaultContract(typeof(ServiceFault))]
        void DeleteData(BEFacility oFacility, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "GetFacilityList")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEFacility> GetFacilityList(BETenant oTenant);

    }
}
