using BPA.GlobalResources.UI.WorkManagement;
using MicUI.WorkManagement.Helper.CustomValidationAttributes;
using MicUI.WorkManagement.Services.ServiceModel;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Xml.Linq;

namespace MicUI.WorkManagement.Models.ViewModels
{
    [Serializable]
    public class BreakMasterViewModel : ViewModelValidationHelper
    {
        public List<BEBreakInfo> BEBreakInfoList = new List<BEBreakInfo>();

        public int mBreakID
        {
            get;
            set;
        }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resources_Break), Name = "display_Break_Name")]
        public string mBreakSearchName
        { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources_Break), ErrorMessageResourceName = "reqired_BreakName")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resources_Break), Name = "display_Break_Name")]
        public string mBreakName
        { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources_Break), ErrorMessageResourceName = "requiredDescription")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resources_Break), Name = "display_Description")]
        [DataType(DataType.MultilineText)]
        public string mDescription
        { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resources_Break), Name = "display_Disable")]
        public bool mIsdisable
        {
            get;
            set;
        }
    }
}