namespace MicUI.Configuration.Services.ServiceModel
{
    public class LogDetailsReport
    {
        public int LogID { get; set; }
        public int EventID { get; set; }
        public int Priority { get; set; }
        public string Severity { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string MachineName { get; set; }
        public string AppDomainName { get; set; }
        public int ProcessID { get; set; }
        public string ProcessName { get; set; }
        public string? ThreadName { get; set; }
        public int Win32ThreadId { get; set; }
        public string Message_cut { get; set; }
        public string Message { get; set; }
        public string FormattedMessage { get; set; }

    }
}
