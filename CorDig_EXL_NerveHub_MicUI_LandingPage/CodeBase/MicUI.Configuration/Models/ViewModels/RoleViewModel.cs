using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Text;
using System.Xml.Linq;

namespace MicUI.Configuration.Models.ViewModels
{
    [Serializable]
    public class RoleViewModel : SearchViewModel
    {

        public RoleViewModel()
        {
            lstFromActionMap = new List<GetFromAction>();
            SearchViewList = new List<RoleViewModel>();
            lstFromModuleActionMap = new List<GetFromModuleAction>();
            RoleApprovalList = new List<RoleViewModel>();
            RoleRequestList = new List<RoleViewModel>();
        }
        #region Role Index View Property
        public int RoleId { get; set; }
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Roles), ErrorMessageResourceName = "required_RoleName")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Roles), Name = "display_RoleName")]
        public string RoleName { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Roles), Name = "display_Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Roles), Name = "display_IsClientUserRole")]
        public bool IsClientUserRole { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Roles), ErrorMessageResourceName = "required_SecurityGroup")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Roles), Name = "display_SecurityGroup")]
        public string SecurityGroup { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Roles), ErrorMessageResourceName = "required_RoleLevel")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Roles), Name = "display_RoleLevel")]
        public string RoleLevel { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Roles), ErrorMessageResourceName = "required_Approver")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Roles), Name = "display_Approver")]
        public string Approver { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Roles), Name = "display_Disable")]
        public bool Disable { get; set; }
        #endregion

        #region Role Serach View Property
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Roles), Name = "display_RoleName")]
        public string SearchRoleName { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Roles), Name = "display_Approver")]
        public string SearchApprover { get; set; }
        //---------------------
        public List<GetFromAction> lstFromActionMap { get; set; }



        public List<RoleViewModel> SearchViewList { get; set; }
        public List<GetFromModuleAction> lstFromModuleActionMap { get; set; }
        #endregion

        #region Roles Approval View and Request Status View Property
        public int RequestId { get; set; }
        public string RequestBy { get; set; }
        public string RequestDesc { get; set; }
        public DateTime RequestDate { get; set; }
        public string RequestApprover { get; set; }
        public string RequestStatus { get; set; }
        public List<RoleViewModel> RoleApprovalList { get; set; }
        public List<RoleViewModel> RoleRequestList { get; set; }
        public StringBuilder sbMailBody = new StringBuilder();

        public string JsonStringAllowAction { get; set; }

        #endregion


        //public string FormName { get; set; }
        //public string ModuleName { get; set; }
        //public int? SelectedView { get; set; }
        //public int? SelectedAdd { get; set; }
        //public int? SelectedModify { get; set; }
        //public int? SelectedDelete { get; set; }
        //public int? SelectedApprove { get; set; }
        //public int FormId { get; set; }

    }

    /// <summary>
    /// This class is used for Form Action property value like check boxes
    /// </summary>
    [Serializable]
    public class GetFromAction
    {
        public string FormName { get; set; }
        public string ModuleName { get; set; }
        public int? SelectedView { get; set; }
        public int? SelectedAdd { get; set; }
        public int? SelectedModify { get; set; }
        public int? SelectedDelete { get; set; }
        public int? SelectedApprove { get; set; }
        public int FormId { get; set; }
        public string MenuID { get; set; }
        public string ParentID { get; set; }

    }
    [Serializable]
    public class GetFromModuleAction
    {

        public string ModuleName { get; set; }
        public int? SelectedModelView { get; set; }
        public int? SelectedModelAdd { get; set; }
    }
}