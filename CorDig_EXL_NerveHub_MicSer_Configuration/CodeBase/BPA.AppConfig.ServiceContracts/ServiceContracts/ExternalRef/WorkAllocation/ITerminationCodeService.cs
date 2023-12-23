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
    [ServiceContract(Name = "TerminationCodeServiceContract")]
    public interface ITerminationCodeService
    {
        List<BETerminationCodeInfo> GetTermCodeListByCamp(string sTermName, int iCampID, BETenant oTenant);
        List<BETerminationCodeInfo> GetTermCodeList(string sTermcodeName, bool bGetActive, BETenant oTenant);
        List<BETerminationCodeInfo> GetTermCodeList(int iTermCodeID, BETenant oTenant);
        string InsertData(BETerminationCodeInfo oTermCode, int iFormID, BETenant oTenant);
        void UpdateData(BETerminationCodeInfo oTermCode, int iFormID, BETenant oTenant);
        List<BETerminationCodeInfo> GetTermCodeListByCamp(int iCampID, BETenant oTenant);       
    }
}
