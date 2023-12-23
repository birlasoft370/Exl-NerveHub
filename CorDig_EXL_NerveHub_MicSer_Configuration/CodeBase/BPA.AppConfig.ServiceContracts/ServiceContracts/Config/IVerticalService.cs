using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.ServiceModel;
using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.Config
{

    [ServiceContract(Name = "VerticalServiceContract")]
    public interface IVerticalService : IDisposable
    {
        List<BEVerticalInfo> GetVerticalList(bool IsActiveVertical, BETenant oTenant);
        List<BEVerticalInfo> GetVerticalList(string Name, bool IsActiveVertical, BETenant oTenant);
        List<BEVerticalInfo> GetVerticalList(int VerticalID, BETenant oTenant);
        void InsertData(BEVerticalInfo oVertical, int FormID, BETenant oTenant);
        void UpdateData(BEVerticalInfo oVertical, int FormID, BETenant oTenant);
    }
}
