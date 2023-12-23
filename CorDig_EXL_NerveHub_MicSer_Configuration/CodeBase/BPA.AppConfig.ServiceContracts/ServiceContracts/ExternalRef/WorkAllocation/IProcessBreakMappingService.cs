using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
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
    [ServiceContract(Name = "ProcessBreakMappingServiceContract")]
    public interface IProcessBreakMappingService
    {
        List<BEProcessInfo> GetProcessBreakMappedList(int ClientID, string ProcessName, int UserId, BETenant oTenant);
        BEProcessBreakMapping GetProcessBreakMappedDetails(int iProcessId, BETenant oTenant);
        void InsertData(BEProcessBreakMapping oProcess, int FormID, BETenant oTenant);
        void UpdateData(BEProcessBreakMapping oProcess, int FormID, BETenant oTenant);
    }
}
