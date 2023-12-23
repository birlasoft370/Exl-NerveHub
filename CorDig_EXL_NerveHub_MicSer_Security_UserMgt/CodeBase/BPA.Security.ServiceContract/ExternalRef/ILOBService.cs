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
    [ServiceContract(Name = "LOBServiceContract")]
    public interface ILOBService : IDisposable
    {
        //[OperationContract(Name = "GetLOBListActive")]
        //[FaultContract(typeof(ServiceFault))]
        List<BELOBInfo> GetLOBList(bool IsActiveLOB, BETenant oTenant);

        //[OperationContract(Name = "GetLOBListActiveWithLOB")]
        //[FaultContract(typeof(ServiceFault))]
        List<BELOBInfo> GetLOBList(string sLOBName, bool IsActiveLOB, BETenant oTenant);

        //[OperationContract(Name = "GetLOBListWithLOBID")]
        //[FaultContract(typeof(ServiceFault))]
        List<BELOBInfo> GetLOBList(int iLOBID, BETenant oTenant);

        //[OperationContract(Name = "InsData")]
        //[FaultContract(typeof(ServiceFault))]
        void InsertData(BELOBInfo oLOB, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "UpdData")]
        //[FaultContract(typeof(ServiceFault))]
        void UpdateData(BELOBInfo oLOB, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "DelData")]
        //[FaultContract(typeof(ServiceFault))]
        void DeleteData(BELOBInfo oLOB, int iFormID, BETenant oTenant);

    }
}
