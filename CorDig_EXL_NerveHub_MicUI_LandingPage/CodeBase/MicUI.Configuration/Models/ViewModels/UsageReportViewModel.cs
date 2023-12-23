using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MicUI.Configuration.Models.ViewModels
{
    public class UsageReportViewModel
    {
        [DataType(DataType.Date)]
        public string FromDate { get; set; }
        [DataType(DataType.Date)]
        public string ToDate { get; set; }
        public int iClientId { get; set; }
        public int iProcessId { get; set; }
        public int iCampaignId { get; set; }
        public string BatchCode { get; set; }
        public string ResolutionCode { get; set; }
        // [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_Reports), Name = "display_BSR_IsBackOffice")]
        public bool? IsBackOffice { get; set; }
        public bool? AsOnCurrentDate { get; set; }

        [DataType(DataType.Date)]
        public string ChoosedDateTime { get; set; }

        public IList<dynamic> ReportData { get; set; }
        public DataTable dttbl { get; set; }
        List<string> _lstResolutionCode = new List<string>();
        public List<string> lstResolutionCode { get { return _lstResolutionCode; } set { _lstResolutionCode = value; } }
    }
}
