namespace BPA.AppConfiguration.Models
{
    public class LoggerInfo
    {
        public string actionName { get; set; }
        public string message { get; set; }
        public string requestId { get; set; }
        public LogLevel level { get; set; }
    }
}
