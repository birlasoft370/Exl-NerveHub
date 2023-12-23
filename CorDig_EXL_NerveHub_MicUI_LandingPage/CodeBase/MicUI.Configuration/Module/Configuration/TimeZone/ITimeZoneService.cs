using MicUI.Configuration.Models;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Configuration.TimeZone
{
    public interface ITimeZoneService
    {
        List<BETimeZoneInfo> GetTimeZoneList();
        List<BETimeZoneInfo> GetTimeZoneList(string searchTimeZone);
        TimeZoneModel GetTimeZoneById(int timeZoneId);
    }
}
