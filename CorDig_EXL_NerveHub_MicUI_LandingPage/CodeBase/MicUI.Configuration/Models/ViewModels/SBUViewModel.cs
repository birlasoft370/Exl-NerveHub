using MicUI.Configuration.Helper.CustomValidationAttributes;
using MicUI.Configuration.Services.ServiceModel;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Xml.Linq;

namespace MicUI.Configuration.Models.ViewModels
{
    [Serializable]
    public class SBUViewModel : ViewModelValidationHelper
    {

        public SBUViewModel()
        {
            SBUList = new List<BESBUInfo>();
        }
        //Get and Set SBU Id For Edit,Update and Delete 
        public int SBUID
        {
            get;
            set;
        }

        //Get and Set value for ERPID

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.ResourceSBU), ErrorMessageResourceName = "requiredERPID")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.ResourceSBU), Name = "DisplayERPID")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "The field ERP ID :must be number.")]
        public int ERPID
        { get; set; }

        //Get and Set value for SBUName
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.ResourceSBU), ErrorMessageResourceName = "requiredSBUName")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.ResourceSBU), Name = "DisplaySBUNAME")]
        public string SBUName
        {
            get;
            set;
        }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.ResourceSBU), Name = "DisplaySBUNAME")]
        public string SBUSeachName
        {
            get;
            set;
        }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.ResourceSBU), Name = "displayDescription")]
        //Get and Set value for Description
        public string Description
        {
            get;
            set;
        }

        //Get and Set value for IsClientSBU
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.ResourceSBU), Name = "displayIsClientSBU")]
        public Boolean IsClientSBU
        {
            get;
            set;
        }

        //Get and Set value for Disable
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.ResourceSBU), Name = "displayDisable")]
        public Boolean Disable
        {
            get;
            set;
        }
        //     Gets or sets SBU List.
        public List<BESBUInfo> SBUList { get; set; }
    }
}

