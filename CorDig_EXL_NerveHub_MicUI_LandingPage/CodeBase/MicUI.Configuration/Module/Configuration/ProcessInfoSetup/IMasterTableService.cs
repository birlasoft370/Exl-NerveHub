using MicUI.Configuration.Services.Configuration;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Configuration.ProcessInfoSetup
{
    public interface IMasterTableService
    {
        List<BEMasterTable> GetMasterList();
        List<BEMasterTable> GetProcessComplexityList();
    }
    public class MasterTableService : IMasterTableService
    {
        private readonly IConfigApiService _configService;

        public MasterTableService(IConfigApiService configService)
        {
            _configService = configService;
        }
        public List<BEMasterTable> GetMasterList()
        {
            var result = _configService.GetProcessWorkTypeListAsync().GetAwaiter().GetResult();
            return result.data;
        }

        public List<BEMasterTable> GetProcessComplexityList()
        {
            var result = _configService.GetProcessComplexityListAsync().GetAwaiter().GetResult();
            return result.data;
        }
    }
}
