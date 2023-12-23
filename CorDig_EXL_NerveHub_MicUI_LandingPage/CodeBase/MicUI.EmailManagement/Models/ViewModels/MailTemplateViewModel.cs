using BPA.GlobalResources.UI;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Xml.Linq;

namespace MicUI.EmailManagement.Models.ViewModels
{
    public class MailTemplateViewModel : SearchViewModel
    {
        public MailTemplateViewModel()
            : base(new string[] { Resources_common.display_Client, Resources_common.display_Process, Resources_common.display_Campaign_Dropdown })
        {
            SearchViewList = new List<MailTemplateViewModel>();
        }

        public int MailTemplateId { get; set; }
        //[Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.EmailManagement.MailTemplate_Resources), ErrorMessageResourceName = "required_MailTemplate")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.EmailManagement.MailTemplate_Resources), Name = "display_MailTemplate")]
        //[DataType(DataType.Html)]
        // [AllowHtml]
        public string MailTemplate { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.EmailManagement.MailTemplate_Resources), Name = "display_Disable")]
        public bool? Disable { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.EmailManagement.MailTemplate_Resources), Name = "display_IsAutoReplay")]
        public bool? IsAutoReplay { get; set; }
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.EmailManagement.MailTemplate_Resources), ErrorMessageResourceName = "required_MailTemplateName")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.EmailManagement.MailTemplate_Resources), Name = "display_MailTemplateName")]
        public string MailTemplateName { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.EmailManagement.MailTemplate_Resources), Name = "display_SearchTemplateName")]
        public string SearchTemplateName { get; set; }

        public List<MailTemplateViewModel> SearchViewList { get; set; }
    }
}
