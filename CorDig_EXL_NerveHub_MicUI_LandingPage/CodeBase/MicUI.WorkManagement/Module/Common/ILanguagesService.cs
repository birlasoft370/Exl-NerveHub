using MicUI.WorkManagement.Services.Configuration;
using MicUI.WorkManagement.Services.ServiceModel;

namespace MicUI.WorkManagement.Module.Common
{
    public interface ILanguagesService
    {
        IList<BELanguages> GetLanguageList(bool IsActiveLanguages);
        IList<BEMailTranslatorConfiguration> GetCampaignWiseLangList(int iStoreID,int iFormId = 1);
    }
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
        public IList<BEMailTranslatorConfiguration> GetCampaignWiseLangList(int iStoreID, int iFormId = 1)
        {
            var result = _configService.GetCampaignWiseLangList(iStoreID, iFormId).GetAwaiter().GetResult();
            return result.data ?? new List<BEMailTranslatorConfiguration>();
        }
    }
}
