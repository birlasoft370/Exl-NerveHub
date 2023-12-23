using MicUI.Configuration.Services.Configuration;

namespace MicUI.Configuration.Module.Configuration.CampaignInfoSetup
{
    public interface IWorkObjectService
    {
        int CheckUserIsSuperOrFunctionalAdmin();
    }
    public class WorkObjectService : IWorkObjectService
    {
        private readonly IConfigApiService _configService;
        public WorkObjectService(IConfigApiService configService)
        {
            _configService = configService;
        }
        public int CheckUserIsSuperOrFunctionalAdmin()
        {
            var result = _configService.GetCheckUserIsSuperOrFunctionalAdminAsync().GetAwaiter().GetResult();
            return result.data;
        }
    }
}
