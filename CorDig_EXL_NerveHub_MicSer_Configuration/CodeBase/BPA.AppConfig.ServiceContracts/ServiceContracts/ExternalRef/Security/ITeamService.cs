using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef.Security;
using BPA.AppConfig.ServiceContracts.ServiceContracts.FaultContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.Security
{
    [ServiceContract(Name = "TeamServiceContract")]
    public interface ITeamService : IDisposable
    {
        [OperationContract(Name = "GetTeamListWithTeamName")]
        [FaultContract(typeof(ServiceFault))]
        List<BETeamInfo> GetTeamList(string sTeamName, int iLoggedinUserID, bool bActiveTeam, BETenant oTenant);
       
        [OperationContract(Name = "GetTeamListWithTeamID")]
        [FaultContract(typeof(ServiceFault))]
        List<BETeamInfo> GetTeamList(int iTeamID, BETenant oTenant);

        [OperationContract(Name = "InsData")]
        [FaultContract(typeof(ServiceFault))]
        void InsertData(BETeamInfo oTeam, int iFormID, BETenant oTenant);

        [OperationContract(Name = "UpdData")]
        [FaultContract(typeof(ServiceFault))]
        void UpdateData(BETeamInfo oTeam, int iFormID, BETenant oTenant);

        [OperationContract(Name = "GetProcessWiseTeamList")]
        [FaultContract(typeof(ServiceFault))]
        List<BETeamInfo> GetProcessWiseTeamList(int iProcess, BETenant oTenant);
    }
}
