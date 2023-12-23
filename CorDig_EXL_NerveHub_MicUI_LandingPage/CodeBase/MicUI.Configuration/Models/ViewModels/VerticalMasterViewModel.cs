using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Xml.Linq;

namespace MicUI.Configuration.Models.ViewModels
{
    [Serializable]
    public class VerticalMasterViewModel
    {
        public VerticalMasterViewModel()
        {
            VerticaMasterList = new List<VerticalMasterViewModel>();
        }
        public int VerticalID { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Vertical), ErrorMessageResourceName = "required_ERPID")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Vertical), Name = "display_ERPName")]
        public int? ERPID { get; set; }


        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Vertical), ErrorMessageResourceName = "required_Verticalname")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Vertical), Name = "display_Verticalname")]
        public string VerticaName { get; set; }



        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Vertical), Name = "display_description")]
        public string VerticaDescription { get; set; }


        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Vertical), Name = "display_disable")]
        public bool? Disable { get; set; }



        public List<VerticalMasterViewModel> VerticaMasterList { get; set; }

        [ScaffoldColumn(false)]
        public string EncryptVerticaMasterID { get; set; }
    }
}