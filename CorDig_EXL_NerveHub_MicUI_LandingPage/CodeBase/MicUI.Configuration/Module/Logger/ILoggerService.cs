using MicUI.Configuration.Services.Logger;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Logger
{
    public interface ILoggerService
    {
        string SaveLogDetailsAsync(LoggerInfo loggerInfo);
    }

    public class LoggerService : ILoggerService
    {
        private readonly ILoggerApiService _loggerApiService;

        public LoggerService(ILoggerApiService loggerApiService)
        {
            this._loggerApiService = loggerApiService;
        }

        public string SaveLogDetailsAsync(LoggerInfo loggerInfo)
        {
            return  _loggerApiService.SaveLogDetailsAsync(loggerInfo).GetAwaiter().GetResult();
        }
    }
}
