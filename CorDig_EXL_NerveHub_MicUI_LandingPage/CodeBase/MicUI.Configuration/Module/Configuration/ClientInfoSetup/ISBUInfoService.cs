using MicUI.Configuration.Services.ServiceModel;
using System.Runtime.InteropServices;

namespace MicUI.Configuration.Module.Configuration.ClientInfoSetup
{
    public interface ISBUInfoService
    {
        List<BESBUInfo> GetSBUListbasedONClient(int iclientId);
        List<BESBUInfo> GetSBUList(bool isActive, [Optional] string? sbuName);
        List<BESBUInfo> GetSBUList(string sbuName);
        string UpdateData(SBUModel sBUModelModel);
        string InsertData(SBUModel sBUModelModel);
        SBUModel GetSBUById(int sbuName);
    }
}
