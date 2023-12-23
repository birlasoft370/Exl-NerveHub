using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef.BIReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.BIReports
{
    [ServiceContract(Name = "ProcessOffServiceContract")]
    public interface IProcessOffService
    {
        IList<BEProcessOff> GetProcessOffList(int iProcessId, int iYear, int iMonth, bool bGetActive, BETenant oTenant);
        IList<BEProcessOff> GetProcessOffListProcess(int iProcessID, int month, int Year, BETenant oTenant);
        string GetFirstLastDayOfCalender(int iProcessID, int Month, int Year, BETenant oTenant);
        IList<BEProcessOff> GetProcessOffListProcessDayWise(int iProcessID, int month, int Year, BETenant oTenant);
        void InsertData(BEProcessOff oProcessOff, int iFormID, BETenant oTenant);
        void UpdateData(BEProcessOff oProcessOff, int iFormID, BETenant oTenant);
    }
}
