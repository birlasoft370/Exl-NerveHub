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
    [ServiceContract(Name = "MasterTableServiceContract")]
    public interface IMasterTableService : IDisposable
    {
        List<BEMasterTable> GetMasterList(int iFieldID, BETenant oTenant);
        List<BEMasterType> GetMasterList(string sFieldDesc, bool bGetAll, BETenant oTenant);
        void InsertData(BEMasterType oMasterType, int iFormID, BETenant oTenant);
        void UpdateData(BEMasterType oMasterType, int iFormID, BETenant oTenant);
        BEMasterType GetMasterDetails(int iFieldID, BETenant oTenant);
    }
}
