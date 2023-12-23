using MicUI.Configuration.Services.Configuration;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Configuration.ProcessInfoSetup
{
    public interface ICalenderService
    {
        IList<BECalendarInfo> GetCalendarList();
    }

    public class CalenderService : ICalenderService
    {
        private readonly IConfigApiService _configService;

        public CalenderService(IConfigApiService configService)
        {
            _configService = configService;
        }

        public IList<BECalendarInfo> GetCalendarList()
        {
            var result = _configService.GetCalendarListAsync().GetAwaiter().GetResult();
            return result.data;
        }
    }
}
