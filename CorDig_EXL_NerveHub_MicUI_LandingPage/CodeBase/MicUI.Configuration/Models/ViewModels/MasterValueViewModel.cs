using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Xml.Linq;

namespace MicUI.Configuration.Models.ViewModels
{
    [Serializable]
    public class MasterValueViewModel : MasterValueList
    {
        public MasterValueViewModel()
        {
            MasterValueList = new List<MasterValueViewModel>();
        }
        public int MasterValueID
        {
            get;
            set;
        }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_MasterValue), AllowEmptyStrings = false, ErrorMessageResourceName = "required_MasterValue")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_MasterValue), Name = "display_MasterTypeMain")]
        public string MasterType
        {
            get;
            set;
        }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_MasterValue), Name = "display_Disable")]
        public bool? bDisable
        {
            get;
            set;
        }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_MasterValue), Name = "display_MasterName")]
        public string MasterValueSearchName
        {
            get;
            set;
        }

        public IList<MasterValueList> ValueList = new List<MasterValueList>();

        public List<MasterValueViewModel> MasterValueList { get; set; }
    }
    [Serializable]
    public class MasterValueList
    {
        public int FieldID
        {
            get;
            set;
        }
        [RegularExpression(@"(?!.*\s{2,}.*|^\s.*|.*\s$)^.*$", ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_MasterValue), ErrorMessageResourceName = "required_SpaceValues")]
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_MasterValue), ErrorMessageResourceName = "required_Values")]
        public string Values
        {
            get;
            set;
        }

        public bool? Disable
        {
            get;
            set;
        }
    }
}
