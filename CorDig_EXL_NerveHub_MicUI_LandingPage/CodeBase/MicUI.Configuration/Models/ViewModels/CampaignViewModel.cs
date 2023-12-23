using BPA.GlobalResources.UI;
using MicUI.Configuration.Models.Security;
using System.ComponentModel.DataAnnotations;

namespace MicUI.Configuration.Models.ViewModels
{
    /// <summary>
    /// Campaign View Model Class
    /// </summary>
    [Serializable]
    public class CampaignViewModel : SearchViewModel
    {
        #region Private Variable
        private int _CampaignID;
        private string _CampSearchName;
        private string _Description;
        private string _TimeZone;
        private string _Location;
        private string _ShiftWindow;
        private string _BusinessJustifications;
        private string _TargetUsers;
        private string _KeyBenefits;
        private string _BusinessApprover;
        private string _WorkDefinitionName;
        private string _ChangeRequest;
        private string _RequestCreator;
        private string _ChangeRequestStatus;
        private string _sEmail;
        List<string> _Mode = new List<string>();
        #endregion
        /// <summary>
        /// Campaign constructor
        /// </summary>
        public CampaignViewModel()
            : base(new string[] { Resources_common.display_Client, Resources_common.display_Process, Resources_common.display_Campaign })
        {
            SearchViewList = new List<CampaignViewModel>();
        }
        /// <summary>
        /// Campaign ID
        /// </summary>
        [ScaffoldColumn(false)]
        public int CampaignID { get { return _CampaignID; } set { _CampaignID = value; } }

        /// <summary>
        /// This property is use to Search Campaign
        /// </summary>
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_CampaignName")]
        public string CampSearchName { get { return _CampSearchName; } set { _CampSearchName = value; } }

        /// <summary>
        /// This Property is used for mode
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), ErrorMessageResourceName = "required_Mode")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_Mode")]
        public List<string> Mode { get { return _Mode; } set { _Mode = value; } }


        /// <summary>
        /// This is used to backoffice
        /// </summary>
        public bool? BackOffice { get; set; }

        /// <summary>
        /// For Inbound 
        /// </summary>
        public bool? InBound { get; set; }

        /// <summary>
        /// For Outbound
        /// </summary>
        public bool? OutBound { get; set; }

        /// <summary>
        /// This Property is used to campaign Description
        /// </summary>
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_Description")]
        public string Description { get { return _Description; } set { _Description = value; } }


        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_Email")]
        public string sEmail { get { return _sEmail; } set { _sEmail = value; } }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_ThresholdForCompletion")]
        public int iThresholdForCompletion { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_ThresholdForToOpen")]
        public int iThresholdForToOpen { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_TargetEfficiency")]
        public Double dTargetEfficiency { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_BillingSystem")]
        public bool? bBillingSystem { get; set; }
        //public int IsLevel { get; set; }
        //public int IsLevel { get; set; }
        /// <summary>
        /// For Time Zone
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), ErrorMessageResourceName = "required_TimeZone")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_TimeZone")]
        public string TimeZone { get { return _TimeZone; } set { _TimeZone = value; } }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), ErrorMessageResourceName = "required_EndDate")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_EndDate")]
        [DataType(DataType.Date)]
        public DateTime? EndDate1 { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_NoFieldDataEntry")]
        public bool? NoFieldDataEntry { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), ErrorMessageResourceName = "required_Location")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_Location")]
        public string Location { get { return _Location; } set { _Location = value; } }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), ErrorMessageResourceName = "required_ShiftWindow")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_ShiftWindow")]
        public string ShiftWindow { get { return _ShiftWindow; } set { _ShiftWindow = value; } }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), ErrorMessageResourceName = "required_Purposeofcreationofcampaign")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_Purposeofcreationofcampaign")]
        public List<string> Purposeofcreationofcampaign { get { return _Purposeofcreationofcampaign; } set { _Purposeofcreationofcampaign = value; } }

        List<string> _Purposeofcreationofcampaign = new List<string>();
        //public List<string> Purposeofcreationofcampaign { get; set; }

        public bool? WorkManagement { get; set; }
        public bool? TimeTracking { get; set; }
        public bool? TransactionsMonitoring { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), ErrorMessageResourceName = "required_BusinessJustifications")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_BusinessJustifications")]
        public string BusinessJustifications { get { return _BusinessJustifications; } set { _BusinessJustifications = value; } }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_TargetUsers")]
        public string TargetUsers { get { return _TargetUsers; } set { _TargetUsers = value; } }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), ErrorMessageResourceName = "required_Q1Q2Q3")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_Q1")]
        public int? Q1 { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), ErrorMessageResourceName = "required_Q1Q2Q3")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_Q2")]
        public int? Q2 { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), ErrorMessageResourceName = "required_Q1Q2Q3")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_Q3")]
        public int? Q3 { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), ErrorMessageResourceName = "required_Y1Y2Y3")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_Y1")]
        public int? Y1 { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), ErrorMessageResourceName = "required_Y1Y2Y3")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_Y2")]
        public int? Y2 { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), ErrorMessageResourceName = "required_Y1Y2Y3")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_Y3")]
        public int? Y3 { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), ErrorMessageResourceName = "required_KeyBenefits")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_KeyBenefits")]
        public string KeyBenefits { get { return _KeyBenefits; } set { _KeyBenefits = value; } }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), ErrorMessageResourceName = "required_BusinessApprover")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_BusinessApprover")]
        public string BusinessApprover { get { return _BusinessApprover; } set { _BusinessApprover = value; } }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_Disable")]
        public bool? Disable { get; set; }
        public List<CampaignViewModel> SearchViewList { get; set; }

        List<string> _PurposeofcreationofWork = new List<string>();
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), ErrorMessageResourceName = "required_Purposeofcreationofcampaign")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "required_WorkDefinationName")]
        public List<string> PurposeofcreationofWork { get { return _PurposeofcreationofWork; } set { _PurposeofcreationofWork = value; } }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_WorkDefinitionName")]
        public string WorkDefinitionName
        { get { return _WorkDefinitionName; } set { _WorkDefinitionName = value; } }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "dispaly_Changerequest")]
        public string ChangeRequest { get { return _ChangeRequest; } set { _ChangeRequest = value; } }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_ApproveRequester")]
        public string RequestCreator { get { return _RequestCreator; } set { _RequestCreator = value; } }
        public int IsLevel { get; set; }
        public int iApproverId { get; set; }

        public string ChangeRequestStatus { get { return _ChangeRequestStatus; } set { _ChangeRequestStatus = value; } }

        public BETenantInfo oTenant { get; set; }
    }
    #region CampaignApproval
    [Serializable]
    public class CampaignApproval
    {
        private int _ApprovalId;
        private int _BusinessApproverId;
        private int _TechnologyApproverId;
        private int _CreatedBy;
        private string _ClientName;
        private string _ProcessName;
        private string _CampaignName;
        private string _KeyBenfits;
        private string _StoreName;
        private string _RequestCreater;
        private string _RequestedOn;
        private string _BusinessApprover;
        private string _TechApprover;
        private string _Status;
        private string _StatusToShowHideButtons;
        private string _ChangeRequest;
        #region Fields
        public int ApprovalId { get { return _ApprovalId; } set { _ApprovalId = value; } }
        public int BusinessApproverId { get { return _BusinessApproverId; } set { _BusinessApproverId = value; } }
        public int TechnologyApproverId { get { return _TechnologyApproverId; } set { _TechnologyApproverId = value; } }
        public int CreatedBy { get { return _CreatedBy; } set { _CreatedBy = value; } }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_Client")]
        public string ClientName { get { return _ClientName; } set { _ClientName = value; } }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_Process")]
        public string ProcessName { get { return _ProcessName; } set { _ProcessName = value; } }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_Campaign")]
        public string CampaignName { get { return _CampaignName; } set { _CampaignName = value; } }
        public string KeyBenfits { get { return _KeyBenfits; } set { _KeyBenfits = value; } }
        public string StoreName { get { return _StoreName; } set { _StoreName = value; } }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_Requester")]
        public string RequestCreater { get { return _RequestCreater; } set { _RequestCreater = value; } }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_RequestedOn")]
        public string RequestedOn { get { return _RequestedOn; } set { _RequestedOn = value; } }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_BussinessAppprover")]
        public string BusinessApprover { get { return _BusinessApprover; } set { _BusinessApprover = value; } }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_TechApprover")]
        public string TechApprover { get { return _TechApprover; } set { _TechApprover = value; } }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_Status")]
        public string Status { get { return _Status; } set { _Status = value; } }
        public string StatusToShowHideButtons { get { return _StatusToShowHideButtons; } set { _StatusToShowHideButtons = value; } }
        public string ChangeRequest { get { return _ChangeRequest; } set { _ChangeRequest = value; } }
        public bool? IsChecked { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_FromDate")]
        public DateTime FromDate { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_ToDate")]
        public DateTime ToDate { get; set; }
        public BETenantInfo oTenant { get; set; }
        #endregion
    }
    #endregion
}
