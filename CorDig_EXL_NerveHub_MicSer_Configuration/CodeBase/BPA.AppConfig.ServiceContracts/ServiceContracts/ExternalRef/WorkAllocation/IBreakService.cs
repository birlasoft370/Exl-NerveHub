using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.WorkAllocation
{
    [ServiceContract(Name = "BreakServiceContract")]
    public interface IBreakService : IDisposable
    {
        List<BEBreakInfo> GetBreakList(bool bGetActive, BETenant oTenant);
        List<BEBreakInfo> GetBreakListByProcessID(int iProcessID, BETenant oTenant);
        List<BEBreakInfo> GetBreakListBySearch(string sBreakName, int iProcessID, BETenant oTenant);
        List<BEBreakInfo> GetBreakList(int iBreakID, BETenant oTenant);
        List<BEBreakInfo> GetBreakList(string sBreakName, bool bGetActive, BETenant oTenant);
        void InsertData(BEBreakInfo oBreak, int iFormID, BETenant oTenant);
        void UpdateData(BEBreakInfo oBreak, int iFormID, BETenant oTenant);

    }
}
