using System.Globalization;

namespace MicUI.Configuration.Services.ServiceModel
{
    public class LoggerInfo
    {
        public string ActionName { get; set; } = string.Empty;
        public string Title { get; set; } = "UserInterfacePolicy";
        public string Message { get; set; } = string.Empty;
        public string RequestId { get; set; } = DateTime.Now.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);
        public string MachineName { get; set; } = Environment.MachineName;
        public int Priority { get; set; } = 1;
        public LogLevel Severity { get; set; }
        public string FormattedMessage { get; set; } = string.Empty;
    }
}
