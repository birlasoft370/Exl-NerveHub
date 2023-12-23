using MicUI.Configuration.Models.Security;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Xml.Linq;

namespace MicUI.Configuration.Models.ViewModels
{
    [Serializable]
    public class ProcessOwnerViewModel
    {
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.Resources_common), ErrorMessageResourceName = "required_Client")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.Resources_common), Name = "display_Client")]
        public string ClientName { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_ProcessOwner), ErrorMessageResourceName = "required_Approver")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_ProcessOwner), Name = "display_Approver")]
        public string Approver { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.Resources_common), ErrorMessageResourceName = "required_Process")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.Resources_common), Name = "display_Process")]
        public string ProcessName { get; set; }


        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_ProcessOwner), ErrorMessageResourceName = "required_ProcessOwnerName")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_ProcessOwner), Name = "display_ProcessOwnerName")]
        public List<string> ProcessOwnerName { get { return _ProcessOwnerName; } set { _ProcessOwnerName = value; } }

        List<string> _ProcessOwnerName = new List<string>();
         public string UserId { get; set; }

    }

    #region ProcessOwnerApproval
    [Serializable]
    public class ProcessOwnerApproval
    {
        #region Fields
        public int ForApprover { get; set; }

        public int ForCancel { get; set; }
        public int RequestId { get; set; }
        public int CreatedBy { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_ProcessOwner), Name = "display_ApproveClient")]
        public string ClientName { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_ProcessOwner), Name = "display_ApproveProcess")]
        public string ProcessName { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_ProcessOwner), Name = "display_ApproveRequester")]
        public string Creater { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_ProcessOwner), Name = "display_ApproveRequestedon")]
        public string Approver { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_ProcessOwner), Name = "display_ApproveCreateDate")]
        public DateTime CreateDate { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_ProcessOwner), Name = "display_ApproveForUser")]
        public string ForUser { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_ProcessOwner), Name = "display_ApproveStatus")]
        public string TransStatus { get; set; }

        public string StatusToShowHideButtons { get; set; }
        public string ChangeRequest { get; set; }

        public bool IsChecked { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_ProcessOwner), Name = "display_FromDate")]
        public DateTime FromDate { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_ProcessOwner), Name = "display_ToDate")]
        public DateTime ToDate { get; set; }

        public BETenantInfo oTenant { get; set; }
        #endregion
    }

    public class ProcessApproval
    {
        public int RequestId { get; set; }
        public string ClientName { get; set; }
        public string ProcessName { get; set; }
        public string Creater { get; set; }
        public string Approver { get; set; }
        public string CreateDate { get; set; }
        public string ForUser { get; set; }
        public string ForApprover { get; set; }
        public string ForCancel { get; set; }
        public string TransStatus { get; set; }

    }

    #endregion
}