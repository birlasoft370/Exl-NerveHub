using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Security;
using BPA.AppConfig.ServiceContracts.ServiceContracts.FaultContracts;
using System.ServiceModel;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.Security
{
    [ServiceContract(Name = "AuthenticateServiceContract")]
    public interface IAuthenticateService : IDisposable
    {
        [OperationContract(Name = "IsLADPUser")]
        [FaultContract(typeof(ServiceFault))]
        List<BEUserInfo> IsLADPUser(string LoginName, BESession oBESession, out int iSessionID, out bool bProcessMap, BETenant oTenant, bool isWindowsAuthentication = true, int cachDuration = 24);
    }
}
