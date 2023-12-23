using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MicUI.Configuration.Models
{
    [Serializable]
    public class SearchViewModel : IDisposable
    {

        private string _ClientName;
        private string _ProcessName;
        private string _CampaignName;
        private string _WorkObjName;
        private string _SearchName;
        private string _SearchApprover;

        public List<SelectListItem> ClientSelList = null; //added by Nitin
        public List<SelectListItem> ProcessSelList = null;
        public List<SelectListItem> SubProcessSelList = null;
        public List<SelectListItem> MonitoringSelList = null;
        public void Dispose()
        {

        }

        public SearchViewModel(string[] controlName)
        {
            GetControl = new List<string>();
            GetControl.AddRange(controlName);
        }
        public SearchViewModel()
        {

        }
        public object Tt { get; set; }
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.Resources_common), ErrorMessageResourceName = "required_Client")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.Resources_common), Name = "display_Client")]
        [MaxLength(200)]
        public string ClientName
        {
            get { return _ClientName; }
            set
            {
                _ClientName = value;
            }
        }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.Resources_common), ErrorMessageResourceName = "required_Process")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.Resources_common), Name = "display_Process")]
        [MaxLength(100)]
        public string ProcessName
        {
            get { return _ProcessName; }
            set { _ProcessName = value; }
        }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.Resources_common), ErrorMessageResourceName = "required_Campaign")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.Resources_common), Name = "display_Campaign")]
        [MaxLength(100)]
        public string CampaignName { get { return _CampaignName; } set { _CampaignName = value; } }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.Resources_common), ErrorMessageResourceName = "required_WorkObjectName")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.Resources_common), Name = "display_WorkObjectName")]
        public string WorkObjName { get { return _WorkObjName; } set { _WorkObjName = value; } }

        //[Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.Resources_common), ErrorMessageResourceName = "required_Search")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.Resources_common), Name = "display_Search")]
        [MaxLength(100)]
        public string SearchName { get { return _SearchName; } set { _SearchName = value; } }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.Resources_common), ErrorMessageResourceName = "required_StartDate")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.Resources_common), Name = "display_StartDate")]
        [DataType(DataType.Date)]
        public string StartDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.Resources_common), ErrorMessageResourceName = "required_EndDate")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.Resources_common), Name = "display_EndDate")]
        [DataType(DataType.Date)]
        public string EndDate { get; set; }

        public List<string> GetControl { get; set; }

        /// <summary>
        /// For Role
        /// </summary>
        //public List<object> SearchViewList { get; set; }
        [Display(Name = "Approver")]
        [MaxLength(100)]
        public string SearchApprover { get { return _SearchApprover; } set { _SearchApprover = value; } }
        // For Role

    }

}

