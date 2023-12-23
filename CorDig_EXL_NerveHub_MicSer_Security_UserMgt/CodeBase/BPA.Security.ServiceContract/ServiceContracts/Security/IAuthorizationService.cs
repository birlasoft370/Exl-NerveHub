using BPA.Security.BusinessEntity.ExtrernalRefre;
using System.Security;
using System.ServiceModel;


namespace BPA.Security.Contract.ServiceContracts.Security
{
   
    [ServiceContract(Name = "AuthorizationServiceContract")]
    public interface IAuthorizationService
    {
       //[OperationContract(Name = "CheckPermission")]
       //[FaultContract(typeof(ServiceFault))]
        bool CheckPermission(int FormID, int UserID, PermissionSet Action, BETenant oTenant);
    }
}
