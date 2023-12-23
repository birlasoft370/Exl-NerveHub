using BPA.AppConfig.BusinessEntity.Application;
using System.Data;
using System.ServiceModel;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.Security
{
    [ServiceContract(Name = "PermissionServiceContract")]
    public interface IPermissionService : IDisposable
    {
        DataSet GetUserListD(string sLoginName, bool bActiveUser, int iLoggedinUserID, int iClientUser, string SearchCondition, BETenant oTenant);
    }
}
