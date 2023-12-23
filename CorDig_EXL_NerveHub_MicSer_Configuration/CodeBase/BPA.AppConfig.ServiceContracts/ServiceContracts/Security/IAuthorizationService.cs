using BPA.AppConfig.BusinessEntity;
using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.ServiceContracts.ServiceContracts.FaultContracts;
using System.ServiceModel;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.Security
{
    public interface IAuthorizationUserService
    {
        [OperationContract(Name = "CheckPermission")]
        [FaultContract(typeof(ServiceFault))]
        bool CheckPermission(int FormID, int UserID, PermissionSet Action, BETenant oTenant);
    }
}
