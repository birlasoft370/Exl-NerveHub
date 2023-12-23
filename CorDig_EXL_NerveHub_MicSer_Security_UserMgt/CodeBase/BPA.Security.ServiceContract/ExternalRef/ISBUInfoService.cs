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
    [ServiceContract(Name = "SBUInfoServiceContract")]
    public interface ISBUInfoService : IDisposable
    {
        //[OperationContract(Name = "DelData")]
        //[FaultContract(typeof(ServiceFault))]
        void DeleteData(BESBUInfo oSBU, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "InsData")]
        //[FaultContract(typeof(ServiceFault))]
        string InsertData(BESBUInfo oSBU, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "InsertDataSBU")]
        //[FaultContract(typeof(ServiceFault))]
        void InsertDataSBU(BESBUInfo oSBU, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "UpdData")]
        //[FaultContract(typeof(ServiceFault))]
        void UpdateData(BESBUInfo oSBU, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "UpdateDataSBU")]
        //[FaultContract(typeof(ServiceFault))]
        void UpdateDataSBU(BESBUInfo oSBU, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "GetSBUListAllByCheck")]
        //[FaultContract(typeof(ServiceFault))]
        List<BESBUInfo> GetSBUList(bool bGetAll, BETenant oTenant);

        //[OperationContract(Name = "GetSBUListWithSBUID")]
        //[FaultContract(typeof(ServiceFault))]
        List<BESBUInfo> GetSBUList(int iSBUID, BETenant oTenant);

        //[OperationContract(Name = "GetSBUListWithName")]
        //[FaultContract(typeof(ServiceFault))]
        List<BESBUInfo> GetSBUList(string sName, bool bGetAll, BETenant oTenant);

        //[OperationContract(Name = "GetSBUListBsedOnClient")]
        //[FaultContract(typeof(ServiceFault))]
        //List<BESBUInfo> GetSBUListBsedOnClient(int iClientId);

        //[OperationContract(Name = "GetSBUListbasedONClient")]
        //[FaultContract(typeof(ServiceFault))]
        List<BESBUInfo> GetSBUListbasedONClient(int iclientId, BETenant oTenant);

        //[OperationContract(Name = "GetSBUListall")]
        //[FaultContract(typeof(ServiceFault))]
        List<BESBUInfo> GetSBUListall(BETenant oTenant);

        //[OperationContract(Name = "GetMaxClientId")]
        //[FaultContract(typeof(ServiceFault))]
        int GetMaxClientId(BETenant oTenant);

    }
}