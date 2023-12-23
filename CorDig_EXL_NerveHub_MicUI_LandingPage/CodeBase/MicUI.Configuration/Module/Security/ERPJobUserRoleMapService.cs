using MicUI.Configuration.Services.Security;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Security
{
    public class ERPJobUserRoleMapService : IERPJobUserRoleMapService
    {
        private readonly ISecurityApiService _securityApiService;
        public ERPJobUserRoleMapService(ISecurityApiService securityApiService)
        {
            _securityApiService = securityApiService;
        }
        public List<BEErpJobRoleMap> GetJobRoleMap(int iFilterId)
        {
            var result = _securityApiService.GetJobRoleMapAsync(iFilterId).GetAwaiter().GetResult();
            return result.data;
        }
        public List<BEJobCodeInfo> GetJob()
        {
            var result = _securityApiService.GetJobAsync().GetAwaiter().GetResult();
            return result.data;
        }
        public List<BERoleInfo> GetRoleList()
        {
            var result = _securityApiService.GetRoleListAsync().GetAwaiter().GetResult();
            return result.data;
        }
        public List<BEErpJobRoleMap> GetJobRoleMapByName(string JobDesc)
        {
            var result = _securityApiService.GetJobRoleMapByNameAsync(JobDesc).GetAwaiter().GetResult();
            return result.data;
        }
        public string AddERPJobData(ErpJobRoleMap oJobRole, int FormID)
        { 
        var result= _securityApiService.AddERPJobDataAsync(oJobRole, FormID).GetAwaiter().GetResult();
            return result.data;
        }
    }
}
