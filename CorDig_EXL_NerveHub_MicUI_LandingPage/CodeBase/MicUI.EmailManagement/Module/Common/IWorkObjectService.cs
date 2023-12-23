using MicUI.EmailManagement.Services.Configuration;

namespace MicUI.EmailManagement.Module.Common
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
