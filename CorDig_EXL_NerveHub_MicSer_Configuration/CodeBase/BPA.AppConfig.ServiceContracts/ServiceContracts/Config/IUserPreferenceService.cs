using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.ServiceContracts.ServiceContracts.FaultContracts;
using System.ServiceModel;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.Config
{
    [ServiceContract(Name = "UserPreference")]
    public interface IUserPreferenceService
    {
        [OperationContract(Name = "SaveUpdateUserPreference")]
        [FaultContract(typeof(ServiceFault))]
        int SaveUpdateUserPreference(BEUserPreference objBEUserPreference, BETenant oTenant);

        [OperationContract(Name = "GetUserPerefernceDetail")]
        [FaultContract(typeof(ServiceFault))]
        BEUserPreference GetUserPerefernceDetail(int UserId, BETenant bETenant);
    }
}
