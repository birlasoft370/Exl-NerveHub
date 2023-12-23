using MicUI.WorkManagement.Services.ServiceModel;
using MicUI.WorkManagement.Services.WorkManagement;

namespace MicUI.WorkManagement.Module.WorkManagement.WorkMaster
{
    public interface IControlTypeService
    {
        List<BEControlTypeInfo> GetControlTypeList(string sControlName, bool bGetAll);
    }
    public class ControlTypeService : IControlTypeService
    {
        private readonly IWorkManagementApiService _workManagementApiService;

        public ControlTypeService(IWorkManagementApiService workManagementApiService)
        {
            _workManagementApiService = workManagementApiService;
        }
        public List<BEControlTypeInfo> GetControlTypeList(string sControlName, bool bGetAll)
        {
            var result = _workManagementApiService.GetControlTypeListAsync(sControlName, bGetAll).GetAwaiter().GetResult();
            return result.data ?? new List<BEControlTypeInfo>();
        }
    }
}
