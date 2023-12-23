using MicUI.EmailManagement.Module.Logger;
using MicUI.EmailManagement.Services.ServiceModel;

namespace MicUI.EmailManagement.Helper.Extensions
{
    public static class LoggerExtension
    {
        //Reference :- https://stackoverflow.com/questions/40611683/accessing-asp-net-core-di-container-from-static-factory-class
        private static ILoggerService _loggerService;

        public static void InitLogger(ILoggerService logger)
        {
            _loggerService = logger;
        }

        private static string GetMessage(LoggerInfo loggerInfo)
        {
            return $"Application: {GlobalConstant.AreaEmailManagenent}, RequestId: {loggerInfo.RequestId}, Date: {DateTime.UtcNow}, Method Name: {loggerInfo.ActionName}, Info: {loggerInfo.Message}";
        }

        public static string Error(this ILogger logger, LoggerInfo loggerInfo)
        {
            loggerInfo.Severity = LogLevel.Error;
            var message = GetMessage(loggerInfo);
            logger.Log(loggerInfo.Severity, message);
            var apiResponse = _loggerService.SaveLogDetailsAsync(loggerInfo);
            return apiResponse;
        }
        public static string Information(this ILogger logger, LoggerInfo loggerInfo)
        {
            loggerInfo.Severity = LogLevel.Information;
            var message = GetMessage(loggerInfo);
            logger.Log(loggerInfo.Severity, message);
            var apiResponse = _loggerService.SaveLogDetailsAsync(loggerInfo);
            return apiResponse;
        }
    }
}
