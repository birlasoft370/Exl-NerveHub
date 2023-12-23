using BPA.Security.BusinessEntity.ExtrernalRefre;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BPA.Security.ServiceContract.ExternalRef
{
    [ServiceContract(Name = "ClientServiceContract")]
    public interface IClientService : IDisposable
    {
        //[OperationContract(Name = "GetClientListLoggedActive")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEClientInfo> GetClientList(int iLoggedinUserID, bool bActiveClient, BETenant oTenant);

        //[OperationContract(Name = "GetClientListLoggedWIthClient")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEClientInfo> GetClientList(int iLoggedinUserID, string ClientName, bool bActiveClient, BETenant oTenant);

        //[OperationContract(Name = "GetClientListLoggedWithMonthYear")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEClientInfo> GetClientList(int iLoggedinUserID, string Month, string Year, bool bActiveClient, BETenant oTenant);

        //[OperationContract(Name = "GetClientListLoggedWithClientMonthYear")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEClientInfo> GetClientList(int iLoggedinUserID, string ClientName, string Month, string Year, bool bActiveClient, BETenant oTenant);

        //[OperationContract(Name = "GetClientListWithMonthYear")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEClientInfo> GetClientList(string Month, string Year, bool bActiveClient, BETenant oTenant);

        //[OperationContract(Name = "GetClientListWithClient")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEClientInfo> GetClientList(int ClientID, BETenant oTenant);

        //[OperationContract(Name = "GetClientAccessList")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEClientInfo> GetClientAccessList(int iLoggedinUserID, int iAgentID, bool bActiveClient, BETenant oTenant);

        //[OperationContract(Name = "GetERPClient")]
        //[FaultContract(typeof(ServiceFault))]
        DataTable GetERPClient(int iClientID, BETenant oTenant);

        //[OperationContract(Name = "InsData")]
        //[FaultContract(typeof(ServiceFault))]
        void InsertData(BEClientInfo oClient, int iFormID, BETenant oTenant);


        //[OperationContract(Name = "UpdData")]
        //[FaultContract(typeof(ServiceFault))]
        void UpdateData(BEClientInfo oClient, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "DelData")]
        //[FaultContract(typeof(ServiceFault))]
        void DeleteData(BEClientInfo oClient, int iFormID, BETenant oTenant);
        List<BEClientInfo> GetClientListDataUtility(int iLoggedinUserID, string ClientName, bool bActiveClient, BETenant oTenant);

    }
}
