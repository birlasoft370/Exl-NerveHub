using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Security;
using BPA.EmailManagement.ServiceContract.ExternalRef.FaultContracts;
using System.ServiceModel;

namespace BPA.EmailManagement.ServiceContract.ExternalRef.Security
{
    [ServiceContract(Name = "AuthenticateServiceContract")]
    public interface IAuthenticateService : IDisposable
    {
        [OperationContract(Name = "IsLADPUser")]
        [FaultContract(typeof(ServiceFault))]
        List<BEUserInfo> IsLADPUser(string LoginName, BESession oBESession, out int iSessionID, out bool bProcessMap, BETenant oTenant, bool isWindowsAuthentication = true, int cachDuration = 24);
    }
}
