using MicUI.Configuration.Helper.CustomValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace MicUI.Configuration.Models.ViewModels
{
    [Serializable]
    public class LOBViewModel : ViewModelValidationHelper
    {
        //     Gets or sets LOB ID.
        [ScaffoldColumn(false)]
        public int LOBID { get; set; }
        //     Gets or sets ERP ID.
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.ResourceLOB), ErrorMessageResourceName = "required_ERPID")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.ResourceLOB), Name = "display_ERPID")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "The field ERP ID :must be number.")]
        public int ERPID { get; set; }

        //     Gets or sets disable.        
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.ResourceLOB), Name = "display_Disable")]
        public Boolean IsDisable { get; set; }

        //     Gets or sets LOB Decription.       
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.ResourceLOB), Name = "display_Description")]
        public string Description { get; set; }

        //     Gets or sets LOB Name.
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.ResourceLOB), ErrorMessageResourceName = "required_LOBName")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.ResourceLOB), Name = "display_LOBName")]
        public string LOBName { get; set; }


        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.ResourceLOB), Name = "display_LOBName")]
        public string LOBSeachName { get; set; }
    }
}
