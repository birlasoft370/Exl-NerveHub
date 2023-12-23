using MicUI.Configuration.Services.Configuration;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Configuration.Languages
{
    public class LanguagesService : ILanguagesService
    {
        private readonly IConfigApiService _configService;

        public LanguagesService(IConfigApiService configService)
        {
            _configService = configService;
        }

        public IList<BELanguages> GetLanguageList(bool IsActiveLanguages)
        {
            var result = _configService.GetLanguageListAsync(IsActiveLanguages).GetAwaiter().GetResult();
            return result.data ?? new List<BELanguages>();
        }
    }
}
