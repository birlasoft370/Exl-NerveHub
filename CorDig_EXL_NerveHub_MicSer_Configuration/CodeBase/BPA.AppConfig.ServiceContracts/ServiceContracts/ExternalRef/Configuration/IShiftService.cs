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
    [ServiceContract(Name = "ShiftServiceContract")]
    public interface IShiftService
    {
        List<BEShiftInfo> GetShiftList(bool bGetAll, BETenant oTenant);
        List<BEShiftInfo> GetShiftList(string sShiftName, bool bGetAll, BETenant oTenant);
        List<BEShiftInfo> GetShiftList(int iShiftID, BETenant oTenant);
        string InsertData(BEShiftInfo oShift, int iFormID, BETenant oTenant);
        void UpdateData(BEShiftInfo oShift, int iFormID, BETenant oTenant);
    }
}
