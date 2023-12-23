using BPA.AppConfiguration.Models;

namespace BPA.AppConfiguration.Helper;

public static class Logger
{
    public static string Warning(this ILogger logger, LoggerInfo loggerInfo)
    {
        loggerInfo.level = LogLevel.Warning;
        var message = GetMessage(loggerInfo);
        logger.Log(loggerInfo.level, message);
        CallDBLogger(loggerInfo);
        return message;
    }

    private static string GetMessage(LoggerInfo loggerInfo)
    {
        return $"Application: {GlobalObject.ApplicationName}, RequestId: {loggerInfo.requestId}, Date: {DateTime.UtcNow}, Method Name: {loggerInfo.actionName}, Info: {loggerInfo.message}";
    }

    private static void CallDBLogger(LoggerInfo loggerInfo)
    {
        var log = GlobalObject.URLLogging;
    }

    public static string Information(this ILogger logger, LoggerInfo loggerInfo)
    {
        loggerInfo.level = LogLevel.Information;
        var message = GetMessage(loggerInfo);
        logger.Log(loggerInfo.level, message);
        CallDBLogger(loggerInfo);
        return message;
    }

    public static string Error(this ILogger logger, LoggerInfo loggerInfo)
    {
        loggerInfo.level = LogLevel.Error;
        var message = GetMessage(loggerInfo);
        logger.Log(loggerInfo.level, message);
        CallDBLogger(loggerInfo);
        return message;
    }

    public static string Trace(this ILogger logger, LoggerInfo loggerInfo)
    {
        loggerInfo.level = LogLevel.Trace;
        var message = GetMessage(loggerInfo);
        logger.Log(loggerInfo.level, message);
        CallDBLogger(loggerInfo);
        return message;
    }

    private static void CallLoggerApi(LoggerInfo loggerInfo, IHttpClientFactory _httpClientFactory)
    {
        var client = _httpClientFactory.CreateClient("loggerApiClient");
        var response = client.PostAsJsonAsync($"WeatherForecast/InsertLogger", loggerInfo);
        /*
        using (HttpClient httpClient = new())
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "Url");
            request.Content = new StringContent(JsonConvert.SerializeObject(result));
            var response = await httpClient.SendAsync(request);
        }*/
    }
}