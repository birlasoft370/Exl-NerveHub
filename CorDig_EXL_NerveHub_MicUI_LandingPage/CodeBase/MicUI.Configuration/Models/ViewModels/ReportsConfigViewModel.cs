using BPA.GlobalResources.UI.AppConfiguration;
using System.ComponentModel.DataAnnotations;

namespace MicUI.Configuration.Models.ViewModels
{
    [Serializable]
    public class ReportsConfigViewModel
    {


        List<string> _ClientName = new List<string>();


        [Required(ErrorMessageResourceType = typeof(Resources_ReportsConfig), ErrorMessageResourceName = "req_Client")]
        [Display(ResourceType = typeof(Resources_ReportsConfig), Name = "dispClient")]
        public List<string> ClientName { get { return _ClientName; } set { _ClientName = value; } }

        List<string> _ProcessName = new List<string>();


        [Required(ErrorMessageResourceType = typeof(Resources_ReportsConfig), ErrorMessageResourceName = "req_Process")]
        [Display(ResourceType = typeof(Resources_ReportsConfig), Name = "dispProcess")]
        public List<string> ProcessName { get { return _ProcessName; } set { _ProcessName = value; } }

        List<string> _RoleName = new List<string>();

        [Required(ErrorMessageResourceType = typeof(Resources_ReportsConfig), ErrorMessageResourceName = "req_Role")]
        [Display(ResourceType = typeof(Resources_ReportsConfig), Name = "dispRole")]
        public List<string> RoleName { get { return _RoleName; } set { _RoleName = value; } }


        List<string> _JobName = new List<string>();

        [Required(ErrorMessageResourceType = typeof(Resources_ReportsConfig), ErrorMessageResourceName = "req_Job")]
        [Display(ResourceType = typeof(Resources_ReportsConfig), Name = "dispJob")]
        public List<string> JobName { get { return _JobName; } set { _JobName = value; } }


        // [Required(ErrorMessageResourceType = typeof(Resources_ReportsConfig), ErrorMessageResourceName = "required_Status")]
        [Display(ResourceType = typeof(Resources_ReportsConfig), Name = "display_Status")]
        public string Status { get; set; }

        // [Required(ErrorMessageResourceType = typeof(Resources_ReportsConfig), ErrorMessageResourceName = "required_RequestedFor")]
        [Display(ResourceType = typeof(Resources_ReportsConfig), Name = "display_RequestedFor")]
        public string RequestedFor { get; set; }


        // [Required(ErrorMessageResourceType = typeof(Resources_ReportsConfig), ErrorMessageResourceName = "required_RequestedBy")]
        [Display(ResourceType = typeof(Resources_ReportsConfig), Name = "display_RequestedBy")]
        public string RequestedBy { get; set; }

        public string LogID { get; set; }
        public string Severity { get; set; }
        public string Timestamp { get; set; }
        public string MachineName { get; set; }
        public string Message_cut { get; set; }
        public string AppDomainName { get; set; }
        public string ProcessID { get; set; }
        // public string ProcessName { get; set; }
        public string ThreadName { get; set; }
        public string Win32ThreadId { get; set; }
        public string FormattedMessage { get; set; }


        public string SeverityFlag { get; set; }
    }
}
