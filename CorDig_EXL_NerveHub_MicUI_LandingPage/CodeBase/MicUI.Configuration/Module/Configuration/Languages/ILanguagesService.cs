using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Configuration.Languages
{
    public interface ILanguagesService
    {
        IList<BELanguages> GetLanguageList(bool IsActiveLanguages);
    }
}
