using MicUI.Configuration.Services.ServiceModel;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Xml.Linq;

namespace MicUI.Configuration.Models.ViewModels
{
    [Serializable]
    public class SkillMasterViewModel
    {
        public SkillMasterViewModel()
        {
            SkillList = new List<BESkillInfo>();
        }
        [ScaffoldColumn(false)]
        public int SkillID { get; set; }


        [Display(ResourceType = typeof(BPA.GlobalResources.UI.Resources_common), Name = "display_Disable")]
        public bool IsDisable { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_SkillMaster), Name = "display_Description")]
        public string SkillDescription { get; set; }


        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_SkillMaster), ErrorMessageResourceName = "required_SkillName")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_SkillMaster), Name = "display_SkillName")]
        public string SkillName { get; set; }


        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_SkillMaster), Name = "display_SkillName")]
        public string SearchSkillName { get; set; }

        public List<BESkillInfo> SkillList { get; set; }
    }
}