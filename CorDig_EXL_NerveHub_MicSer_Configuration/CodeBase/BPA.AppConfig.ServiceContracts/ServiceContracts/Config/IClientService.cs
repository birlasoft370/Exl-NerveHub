using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.BusinessEntity.ExternalRef.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.Config
{
    [ServiceContract(Name = "ClientServiceContract")]
    public interface IClientService : IDisposable
    {
        List<BEClientInfo> GetClientList(int iLoggedinUserID, string ClientName, bool bActiveClient, BETenant oTenant);
        List<BEClientInfo> GetClientList(int ClientID, BETenant oTenant);
        List<BEClientInfo> GetClientList(int iLoggedinUserID, bool bActiveClient, BETenant oTenant);
        void InsertData(BEClientInfo oClient, int iFormID, BETenant oTenant);
        void UpdateData(BEClientInfo oClient, int iFormID, BETenant oTenant);

    }
}
