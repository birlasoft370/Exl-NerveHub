using MicUI.Configuration.Services.Reports;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Reports
{
    public interface IERPJobRoleMapService
    {
        List<BEJobCodeInfo> GetJob();
        List<BEErpJobRoleMap> GetJobRoleMap(int roleJobID);
        List<RoleFormAccessModel> GetRoleFormMap(string RoleJobID);
        List<BERoleInfo> GetRoleList(bool bActiveRoles);
    }

    public class ERPJobRoleMapService : IERPJobRoleMapService
    {
        private readonly IReportsApiService _reportsService;

        public ERPJobRoleMapService(IReportsApiService reportsService)
        {
            _reportsService = reportsService;
        }

        public List<BEJobCodeInfo> GetJob()
        {
            var result = _reportsService.GetJobAsync().GetAwaiter().GetResult();
            return result.data ?? new List<BEJobCodeInfo>();
        }

        public List<BEErpJobRoleMap> GetJobRoleMap(int roleJobID)
        {
            var result = _reportsService.GetERPJobRoleMappingReportAsync(roleJobID).GetAwaiter().GetResult();
            return result.data ?? new List<BEErpJobRoleMap>();
        }
        /// <summary>
        /// Gets the Multi job role map.
        /// </summary>
        /// <param name="RoleJobID">The role job ID.</param>
        /// <param name="oTenant">The Tenant.</param>
        /// <returns></returns>
        public List<RoleFormAccessModel> GetRoleFormMap(string RoleJobID)
        {
            var result = _reportsService.GetRoleFormMapAsync(RoleJobID).GetAwaiter().GetResult();
            return result.data ?? new List<RoleFormAccessModel>();
        }

        public List<BERoleInfo> GetRoleList(bool bActiveRoles)
        {
            var result = _reportsService.GetRoleListAsync(bActiveRoles).GetAwaiter().GetResult();
            return result.data ?? new List<BERoleInfo>();
        }
    }
}
