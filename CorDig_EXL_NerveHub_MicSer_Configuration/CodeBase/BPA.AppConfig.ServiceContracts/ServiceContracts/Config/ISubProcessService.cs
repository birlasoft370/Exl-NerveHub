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
    [ServiceContract(Name = "SubProcessServiceContract")]
    public interface ISubProcessService : IDisposable
    {
        List<BESubProcess> GetSubProcessListProcessWise(int iProcessId, BETenant oTenant);
        List<BESubProcess> GetSubProcessList(int iProcessID, string sSubProcessName, bool bActiveSubProcess, BETenant oTenant);
        BESubProcess GetSubProcessList(int iSubProcess, BETenant oTenant);
        void InsertData(BESubProcess oSubprocess, int iFormID, BETenant oTenant);
        void UpdateData(BESubProcess oSubprocess, int iFormID, BETenant oTenant);
    }
}
