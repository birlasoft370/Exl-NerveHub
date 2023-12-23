using BPA.AppConfig.BusinessEntity.Application;
using System.ServiceModel;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.WorkAllocation
{
    [ServiceContract(Name = "WorkObjectServiceContract")]
    public interface IWorkObjectService : IDisposable
    {
        int CheckUserIsSuperOrFunctionalAdmin(int UserId, BETenant oTenant);
    }
}
