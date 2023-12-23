using MicUI.Configuration.Models;
using MicUI.Configuration.Services.Configuration;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Configuration.TimeZone
{
    public class TimeZoneService : ITimeZoneService
    {
        private readonly IConfigApiService _configService;

        public TimeZoneService(IConfigApiService configService)
        {
            _configService = configService;
        }

        public List<BETimeZoneInfo> GetTimeZoneList()
        {
            var result = _configService.GetTimeZoneListAsync().GetAwaiter().GetResult();
            return result.data;
        }
        public List<BETimeZoneInfo> GetTimeZoneList(string searchTimeZone)
        {
            var result = _configService.GetTimeZoneListAsync(searchTimeZone).GetAwaiter().GetResult();
            return result.data;
        }
        public TimeZoneModel GetTimeZoneById(int timeZoneId)
        {
            var result = _configService.GetTimeZoneByIdAsync(timeZoneId).GetAwaiter().GetResult();
            return result.data;
        }
    }
}
