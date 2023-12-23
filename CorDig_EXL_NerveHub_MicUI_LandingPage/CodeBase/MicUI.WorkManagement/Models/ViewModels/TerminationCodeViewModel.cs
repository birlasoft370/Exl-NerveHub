using BPA.GlobalResources.UI.WorkManagement;
using MicUI.WorkManagement.Helper.CustomValidationAttributes;
using MicUI.WorkManagement.Services.ServiceModel;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Xml.Linq;

namespace MicUI.WorkManagement.Models.ViewModels
{
    [Serializable]
    public class TerminationCodeViewModel : ViewModelValidationHelper
    {


        public IList<BETerminationCodeInfo> BETerminationCodeList = new List<BETerminationCodeInfo>();
        [ScaffoldColumn(false)]
        public int mTerminationCodeID
        {
            get;
            set;
        }


        [Display(ResourceType = typeof(Resources_TerminationCode), Name = "display_Termination_Code_Name")]
        public string mTerminationCodeSearchName
        {
            get;
            set;
        }

        [Required(ErrorMessageResourceType = typeof(Resources_TerminationCode), ErrorMessageResourceName = "required_TermCodeName")]
        [Display(ResourceType = typeof(Resources_TerminationCode), Name = "display_Termination_Code_Name")]
        public string mTerminationCodeName
        { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources_TerminationCode), ErrorMessageResourceName = "required_Description")]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resources_TerminationCode), Name = "display_Description")]
        public string mDescription
        { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resources_TerminationCode), Name = "display_Disable")]
        public bool mIsdisable
        {
            get;
            set;
        }

    }
}