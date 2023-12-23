using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.Configuration
{
    [ServiceContract(Name = "SBUInfoServiceContract")]
    public interface ISBUInfoService : IDisposable
    {
        List<BESBUInfo> GetSBUList(string sName, bool bGetAll, BETenant oTenant);
        List<BESBUInfo> GetSBUList(bool bGetAll, BETenant oTenant);
        List<BESBUInfo> GetSBUList(int iSBUID, BETenant oTenant);
        List<BESBUInfo> GetSBUListbasedONClient(int iclientId, BETenant oTenant);
        int GetMaxClientId(BETenant oTenant);
        void InsertDataSBU(BESBUInfo oSBU, int iFormID, BETenant oTenant);
        void UpdateDataSBU(BESBUInfo oSBU, int iFormID, BETenant oTenant);
        string InsertData(BESBUInfo oSBU, int iFormID, BETenant oTenant);
        void UpdateData(BESBUInfo oSBU, int iFormID, BETenant oTenant);
    }
}
