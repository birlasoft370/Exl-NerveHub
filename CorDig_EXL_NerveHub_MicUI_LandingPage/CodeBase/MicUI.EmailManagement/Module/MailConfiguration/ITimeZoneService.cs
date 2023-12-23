using MicUI.EmailManagement.Services.Configuration;
using MicUI.EmailManagement.Services.ServiceModel;

namespace MicUI.EmailManagement.Module.MailConfiguration
{
    public interface ITimeZoneService
    {
        List<BETimeZoneInfo> GetTimeZoneList();
    }
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
            return result.data ?? new List<BETimeZoneInfo>();
        }
    }
}
