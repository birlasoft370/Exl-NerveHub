using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.Config
{
    public interface ILOBService
    {
        List<BELOBInfo> GetLOBList(bool IsActiveLOB, BETenant oTenant);
        List<BELOBInfo> GetLOBList(string sLOBName, bool IsActiveLOB, BETenant oTenant);
        List<BELOBInfo> GetLOBList(int iLOBID, BETenant oTenant);
        void InsertData(BELOBInfo oLOB, int iFormID, BETenant oTenant);
        void UpdateData(BELOBInfo oLOB, int iFormID, BETenant oTenant);
    }
}
