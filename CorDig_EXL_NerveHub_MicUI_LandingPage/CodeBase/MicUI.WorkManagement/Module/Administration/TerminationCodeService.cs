using MicUI.WorkManagement.Services.Configuration;
using MicUI.WorkManagement.Services.ServiceModel;

namespace MicUI.WorkManagement.Module.Administration
{
    public class TerminationCodeService:ITerminationCodeService
    {
        private readonly IConfigApiService _configService;
        public TerminationCodeService(IConfigApiService configService)
        {
            _configService = configService;
        }
        public TerminationCodeModel GetTerminationCodeById(int terminationCodeID)
        {
            var result = _configService.GetTerminationCodeByIdAsync(terminationCodeID).GetAwaiter().GetResult();
            return result.data;
        }
        public List<TerminationCodeModel> GetTerminationCodeList(string? searchTerminationCode)
        {
            var result = _configService.GetTerminationCodeListAsync(searchTerminationCode).GetAwaiter().GetResult();
            return result.data;
        }
        public string UpdateTerminationCode(TerminationCodeModel TerminationCodeModel)
        {
            var result = _configService.UpdateTerminationCodeAsync(TerminationCodeModel).GetAwaiter().GetResult();
            return result.data;
        }
        public string AddTerminationCode(TerminationCodeModel TerminationCodeModel)
        {
            var result = _configService.AddTerminationCodeAsync(TerminationCodeModel).GetAwaiter().GetResult();
            return result.data;
        }
    }
}
