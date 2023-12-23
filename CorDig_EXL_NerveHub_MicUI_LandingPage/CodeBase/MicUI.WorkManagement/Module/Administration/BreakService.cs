using MicUI.WorkManagement.Services.Configuration;
using MicUI.WorkManagement.Services.ServiceModel;

namespace MicUI.WorkManagement.Module.Administration
{
    public class BreakService:IBreakService
    {
        private readonly IConfigApiService _configService;
        public BreakService(IConfigApiService configService)
        {
            _configService = configService;
        }
        public List<BreakMasterModel> GetBreakInfoList(string? breakName)
        {
            var result = _configService.GetBreakInfoListAsync(breakName).GetAwaiter().GetResult();
            return result.data;

        }
        public BreakMasterModel GetBreakMasterById(int breakId)
        {
            var result = _configService.GetBreakMasterByIdAsync(breakId).GetAwaiter().GetResult();
            return result.data;

        }
        public string AddBreakMaster(BreakMasterModel BreakMasterModel)
        {
            var result = _configService.AddBreakMasterAsync(BreakMasterModel).GetAwaiter().GetResult();
            return result.data;
        }
        public string UpdateBreakMaster(BreakMasterModel BreakMasterModel)
        {
            var result = _configService.UpdateBreakMasterAsync(BreakMasterModel).GetAwaiter().GetResult();
            return result.data;
        }

    }
}
