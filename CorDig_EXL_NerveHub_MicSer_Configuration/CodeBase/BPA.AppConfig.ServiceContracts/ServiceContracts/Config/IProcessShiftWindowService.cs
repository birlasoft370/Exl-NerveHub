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
    [ServiceContract(Name = "ProcessShiftWindowServiceContract")]
    public interface IProcessShiftWindowService : IDisposable
    {
        int InsertProcessSiftConfig(string strBreakXml, BETenant oTenant);
        int UpdateProcessSiftConfig(string strBreakXml, BETenant oTenant);
        List<BEProcessConfigData> GetProcessShiftConfig(string sProcessName, BETenant oTenant);
        BEProcessWindow GetProcessShiftConfigDetail(int iProcessshiftConfigId, BETenant oTenant);
    }
}
