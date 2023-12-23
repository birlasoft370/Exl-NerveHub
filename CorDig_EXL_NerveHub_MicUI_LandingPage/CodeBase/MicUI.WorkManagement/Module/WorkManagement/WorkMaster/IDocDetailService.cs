using MicUI.WorkManagement.Services.ServiceModel;
using MicUI.WorkManagement.Services.WorkManagement;

namespace MicUI.WorkManagement.Module.WorkManagement.WorkMaster
{
    public interface IDocDetailService
    {
        IList<BEDocDetail> GetTemplateList(bool iActive);
    }

    public class DocDetailService : IDocDetailService
    {
        private readonly IWorkManagementApiService _workManagementApiService;

        public DocDetailService(IWorkManagementApiService workManagementApiService)
        {
            _workManagementApiService = workManagementApiService;
        }
       public IList<BEDocDetail> GetTemplateList(bool iActive)
        {
            var result = _workManagementApiService.GetTemplateListAsync(iActive).GetAwaiter().GetResult();
            return result.data ?? new List<BEDocDetail>();
        }
    }
}
