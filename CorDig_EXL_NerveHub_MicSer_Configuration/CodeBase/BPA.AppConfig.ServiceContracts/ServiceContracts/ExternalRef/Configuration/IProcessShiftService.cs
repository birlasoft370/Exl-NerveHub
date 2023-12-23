using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.Configuration
{
    [ServiceContract(Name = "ProcessShiftServiceContract")]
    public interface IProcessShiftService : IDisposable
    {
        IList<BEProcessShiftInfo> GetProcessShift(int ProcessID, BETenant oTenant);
        void UpdateData(BEProcessShiftInfo oShift, int iFormID, BETenant oTenant);
        void InsertData(BEProcessShiftInfo oShift, int iFormID, BETenant oTenant);
    }
}
