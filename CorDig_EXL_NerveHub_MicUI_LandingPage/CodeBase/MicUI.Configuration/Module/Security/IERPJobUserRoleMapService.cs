using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Security
{
    public interface IERPJobUserRoleMapService
    {
        List<BEErpJobRoleMap> GetJobRoleMap(int iFilterId);
        List<BEJobCodeInfo> GetJob();
        List<BERoleInfo> GetRoleList();
        List<BEErpJobRoleMap> GetJobRoleMapByName(string JobDesc);
        string AddERPJobData(ErpJobRoleMap oJobRole, int FormID);
    }
}
