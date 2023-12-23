using MicUI.WorkManagement.Services.ServiceModel;

namespace MicUI.WorkManagement.Module.Administration
{
    public interface ITerminationCodeService
    {
        TerminationCodeModel GetTerminationCodeById(int terminationCodeID);
        List<TerminationCodeModel> GetTerminationCodeList(string? searchTerminationCode);
        string UpdateTerminationCode(TerminationCodeModel TerminationCodeModel);
        string AddTerminationCode(TerminationCodeModel TerminationCodeModel);
    }
}
