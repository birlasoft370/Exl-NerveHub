using MicUI.Configuration.Models.Security;
using MicUI.Configuration.Services.ServiceModel;
using System.ComponentModel.DataAnnotations;

namespace MicUI.Configuration.Models.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ERPJobRoleMapViewModel
    {

    

        BEJobCodeInfo bEJobCodeInfo = new BEJobCodeInfo();
        /// <summary>
        /// JobCode
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resource_ERPJobRoleMapping), ErrorMessageResourceName = "required_JobCode")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resource_ERPJobRoleMapping), Name = "display_JobCode")]
        public int iJobCode
        {
            get
            {
                return bEJobCodeInfo.iJobCode;
            }
            set
            {
                bEJobCodeInfo.iJobCode = value;
            }
        }

        /// <summary>
        /// sJobDesc
        /// </summary>
        public string sJobDesc
        {
            get
            {
                return bEJobCodeInfo.sJobDesc;
            }
            set
            {
                bEJobCodeInfo.sJobDesc = value;
            }
        }




        BERoleInfo bERoleInfo = new BERoleInfo();
        /// <summary>
        /// JobCode
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resource_ERPJobRoleMapping), ErrorMessageResourceName = "required_RoleName")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resource_ERPJobRoleMapping), Name = "display_RoleName")]
        public string sRoleName
        {
            get
            {
                return bERoleInfo.sRoleName;
            }
            set
            {
                bERoleInfo.sRoleName = value;
            }
        }


        /// <summary>
        /// iRoleID
        /// </summary>
        public int iRoleID
        {
            get
            {
                return bERoleInfo.iRoleID;
            }
            set
            {
                bERoleInfo.iRoleID = value;
            }
        }

        BEErpJobRoleMap bEErpJobRoleMap = new BEErpJobRoleMap();
        /// <summary>
        /// MappedOn
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resource_ERPJobRoleMapping), ErrorMessageResourceName = "required_MappedOn")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resource_ERPJobRoleMapping), Name = "display_MappedOn")]
        public int iMappedOn
        {
            get
            {
                return bEErpJobRoleMap.iMappedOn;
            }
            set
            {
                bEErpJobRoleMap.iMappedOn = value;
            }
        }

        /// <summary>
        /// Default Role
        /// </summary>     
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resource_ERPJobRoleMapping), Name = "display_DefaultRole")]
        public bool bDefaultRole
        {
            get
            {
                return bEErpJobRoleMap.bDefaultRole;
            }
            set
            {
                bEErpJobRoleMap.bDefaultRole = value;
            }
        }


        /// <summary>
        /// Desable
        /// </summary>     
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resource_ERPJobRoleMapping), Name = "display_Disable")]
        public bool bDisable
        {
            get
            {
                return bEErpJobRoleMap.bDisable;
            }
            set
            {
                bEErpJobRoleMap.bDisable = value;
            }
        }



        /// <summary>
        /// MappedOn
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resource_ERPJobRoleMapping), ErrorMessageResourceName = "required_Approver")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resource_ERPJobRoleMapping), Name = "display_Approver")]
        public int iApprover
        {
            get
            {
                return bEErpJobRoleMap.iApprover;
            }
            set
            {
                bEErpJobRoleMap.iApprover = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resource_ERPJobRoleMapping), ErrorMessageResourceName = "required_Fromdate")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resource_ERPJobRoleMapping), Name = "display_FromDate")]
        public DateTime FromDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resource_ERPJobRoleMapping), ErrorMessageResourceName = "required_Todate")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resource_ERPJobRoleMapping), Name = "display_ToDate")]
        public DateTime ToDate { get; set; }



        /// <summary>
        /// 
        /// </summary>
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resource_ERPJobRoleMapping), Name = "display_SearchJobRoleMapping")]
        public string SearchJobRoleMapping
        {
            get;
            set;

        }
        /// <summary>
        /// 
        /// </summary>
        public List<ERPJobRoleMapViewModel> lstERPJobRoleMapViewModel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RoleJobID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EncryptedRoleJobID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RoleJob { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string iMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BETenantInfo oTenant { get; set; }



    }
}