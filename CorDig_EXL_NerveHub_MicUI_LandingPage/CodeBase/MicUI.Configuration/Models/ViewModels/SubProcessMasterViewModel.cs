using BPA.GlobalResources.UI;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MicUI.Configuration.Models.ViewModels
{
    [Serializable]
    public class SubProcessMasterViewModel : SearchViewModel
    {

        public SubProcessMasterViewModel()
            : base(new string[] { Resources_common.display_Client, Resources_common.display_Process })
        {
            SubProcessList = new List<SubProcessMasterViewModel>();
        }
        [ScaffoldColumn(false)]
        public int SubProcessID { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster), Name = "display_Description")]
        public string Description { get; set; }

        //[Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_ClientInfo), ErrorMessageResourceName = "required_Client")]
        //[Display(ResourceType = typeof(Resources_ClientInfo), Name = "display_Client_Name")]
        //public string ClientName { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources_Process), ErrorMessageResourceName = "Req_ProcessName")]
        //[Display(ResourceType = typeof(Resources_Process), Name = "Disp_ProcessName")]
        //public string ProcessName { get; set; }


        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster), ErrorMessageResourceName = "required_SubProcessName")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster), Name = "display_SubProcessName")]
        public string SubProcessName { get; set; }
        [Display(Name = "PD Sub Process Name:")]
        public string PDSubProcessName { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster), Name = "display_ProductionStartDate")]
        public string ProductionStartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster), Name = "display_ProductionEndDate")]
        public string ProductionEndDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster), ErrorMessageResourceName = "required_SubProcessStartDate")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster), Name = "display_SubProcessStartDate")]
        public string SubProcessStartDate { get; set; }


        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster), ErrorMessageResourceName = "required_SubProcessEndDate")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster), Name = "display_SubProcessEndDate")]
        public string SubProcessEndDate { get; set; }


        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster), Name = "display_StabilizationStartDate")]
        public string StabilizationStartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster), Name = "display_StabilizationEndDate")]
        public string StabilizationEndDate { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster), Name = "display_GoLiveDate")]
        public string GoLiveDate { get; set; }


        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster), Name = "display_Disable")]
        public bool? Disable { get; set; }


        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster), Name = "display_SubProcessSearchName")]
        public string SubProcessSearchName { get; set; }

        public List<SubProcessMasterViewModel> SubProcessList { get; set; }

        SelectList _lstPDName = new SelectList(new List<KeyValuePair<int, string>>(), "Key", "Value");
        public SelectList lstPDName { get { return _lstPDName; } }
        public string HFPDSubProcessId { get; set; }

    }
}
