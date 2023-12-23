using MicUI.WorkManagement.Services.ServiceModel;

namespace MicUI.WorkManagement.Module.Administration
{
    public interface IBreakService
    {
        List<BreakMasterModel> GetBreakInfoList(string? breakName);
        BreakMasterModel GetBreakMasterById(int breakId);
        string AddBreakMaster(BreakMasterModel BreakMasterModel);
        string UpdateBreakMaster(BreakMasterModel BreakMasterModel);
    }
}
