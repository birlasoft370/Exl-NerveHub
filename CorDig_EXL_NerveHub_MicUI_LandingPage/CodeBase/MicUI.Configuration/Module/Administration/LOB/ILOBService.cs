using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Administration.LOB
{
    public interface ILOBService
    {
        List<BELOBInfo> GetLOBList(string lobName, bool IsActiveLOB);
        string UpdateData(LOBModel lOBModel);
        string InsertData(LOBModel lOBModel);
        LOBModel GetLOBById(int LOBId);
    }
}
