using MicSer.Security.UserMgt.Models;
using System.Reflection;

namespace MicSer.Security.UserMgt.Helper;

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
}