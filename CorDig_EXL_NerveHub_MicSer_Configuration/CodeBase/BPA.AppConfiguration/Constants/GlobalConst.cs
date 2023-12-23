namespace BPA.AppConfiguration.Constants
{
    internal static class GlobalConst
    {
        internal static readonly string ApplicationName = "ApplicationSection:ApplicationName";
        internal static readonly string URLLogging = "APIDetails:Logging";
        internal static readonly string URLValidateSSO = "APIDetails:ValidateSSO";
        internal static readonly string URLSSOAuth = "APIDetails:SSOAuth";

        internal static readonly string WelcomeMessage = "Application is working fine.";
        internal static readonly string TokenKey = "X-Token-Id";
        internal static readonly string TokenInfo = "X-Token-In";
        internal static readonly string CorrelationIdHeader = "X-Correlation-Id";
        internal static readonly string ConnectionString;

    }
}
