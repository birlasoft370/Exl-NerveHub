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
    [ServiceContract(Name = "TimeZoneServiceContract")]
    public interface ITimeZoneService
    {
        List<BETimeZoneInfo> GetTimeZoneList(bool bGetAll, BETenant oTenant);
        List<BETimeZoneInfo> GetTimeZoneList(int iTimeZoneID, BETenant oTenant);
        List<BETimeZoneInfo> GetTimeZoneList(string sTimeZoneName, bool bGetAll, BETenant oTenant);
        void InsertData(BETimeZoneInfo oTimeZone, int iFormID, BETenant oTenant);
        void UpdateData(BETimeZoneInfo oTimeZone, int iFormID, BETenant oTenant);
    }
}
