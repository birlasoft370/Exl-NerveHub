using BPA.AppConfiguration.Constants;

namespace BPA.AppConfiguration.Helper;

public static class GlobalObject
{
    internal static IConfiguration IConfigurationObject { get; set; }

    private static string? _SSOAuth = null;
    private static string? _ValidateSSO = null;
    private static string? _ApplicationName = null;
    private static string? _Logging = null;

    internal static string URLSSOAuth
    {
        get
        {
            if (_SSOAuth == null)
                return _SSOAuth = GlobalObject.IConfigurationObject.GetSection(GlobalConst.URLSSOAuth).Value ?? "";
            else
                return _SSOAuth;
        }
    }

    internal static string URLValidateSSO
    {
        get
        {
            if (_ValidateSSO == null)
                return _ValidateSSO = GlobalObject.IConfigurationObject.GetSection(GlobalConst.URLValidateSSO).Value ?? "";
            else
                return _ValidateSSO;
        }
    }

    internal static string URLLogging
    {
        get
        {
            if (_Logging == null)
                return _Logging = GlobalObject.IConfigurationObject.GetSection(GlobalConst.URLLogging).Value ?? "";
            else
                return _Logging;
        }
    }

    internal static string ApplicationName
    {
        get
        {
            if (_ApplicationName == null)
                return _ApplicationName = GlobalObject.IConfigurationObject.GetSection(GlobalConst.ApplicationName).Value ?? "";
            else
                return _ApplicationName;
        }
    }
}
