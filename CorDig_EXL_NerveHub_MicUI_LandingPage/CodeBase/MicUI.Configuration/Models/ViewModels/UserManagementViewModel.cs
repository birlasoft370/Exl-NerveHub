using BPA.GlobalResources.UI.AppConfiguration;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicUI.Configuration.Services.ServiceModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MicUI.Configuration.Models.ViewModels
{
    // [Serializable]
    public class UserManagementViewModel : ViewModelValidationHelper
    {

        public BEUserInfo UserInfo = new BEUserInfo();
        public BERoleInfo RoleInfo = new BERoleInfo();

        private IList<BERoleInfo> _lstRoleInfo = new List<BERoleInfo>();
        public IList<BERoleInfo> lstRoleInfo { get { return _lstRoleInfo; } set { _lstRoleInfo = value; } }

        public int iUserID
        {
            get { return UserInfo.iUserID; }
            set { UserInfo.iUserID = value; }
        }

        public bool bLanID
        {
            get { return UserInfo.bLanID; }
            set { UserInfo.bLanID = value; }
        }



        private List<UserApproval> _UserApproval = new List<UserApproval>();
        public List<UserApproval> lstUserApproval { get { return _UserApproval; } set { _UserApproval = value; } }


        //[Required(ErrorMessageResourceType = typeof(Resources_UserManagement), ErrorMessageResourceName = "required_LanID")]
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_LoginName")]
        public string sLoginName
        {
            get { return UserInfo.sLoginName; }
            set { UserInfo.sLoginName = value; }
        }
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_EmployeeID")]
        // [Required(ErrorMessageResourceType = typeof(Resources_UserManagement), ErrorMessageResourceName = "required_EmployeeID")]
        // [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType =typeof(Resources_UserManagement), ErrorMessageResourceName = "required_EmployeeIDNumeric") ]
        public string iEmployeeID
        {
            get { return UserInfo.iEmployeeID.ToString(); }
            set { UserInfo.iEmployeeID = int.Parse(value); }
        }
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_Password")]
        public string sPassword
        {
            get { return UserInfo.sPassword; }
            set { UserInfo.sPassword = value; }
        }
        [Required(ErrorMessageResourceType = typeof(Resources_UserManagement), ErrorMessageResourceName = "required_FirstName")]
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_FirstName")]
        public string sFirstName
        {
            get { return UserInfo.sFirstName; }
            set { UserInfo.sFirstName = value; }
        }
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_MiddleName")]
        public string sMiddleName
        {
            get { return UserInfo.sMiddleName; }
            set { UserInfo.sMiddleName = value; }
        }
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_LastName")]
        public string sLastName
        {
            get { return UserInfo.sLastName; }
            set { UserInfo.sLastName = value; }
        }
        //[Formate("email", ErrorMessage = "Enter valid Email.")]required_mail
        [Required(ErrorMessageResourceType = typeof(Resources_UserManagement), ErrorMessageResourceName = "required_mail")]
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
     ErrorMessageResourceType = typeof(Resources_UserManagement), ErrorMessageResourceName = "required_CorrectMailAdress")]
        public string sEmail
        {
            get { return UserInfo.sEmail; }
            set { UserInfo.sEmail = value; }
        }
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_DOJ")]
        public string dDOJ { get; set; }
        //[DisplayFormat(DataFormatString = "{0:d}")]
        //public DateTime dDOJ
        //{
        //    get { return UserInfo.dDOJ; }
        //    set { UserInfo.dDOJ = value; }
        //}
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_UserLevel")]
        public int iUserLevel
        {
            get { return UserInfo.iUserLevel; }
            set { UserInfo.iUserLevel = value; }
        }
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_LocationName")]
        public int iLocation
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceType = typeof(Resources_UserManagement), ErrorMessageResourceName = "required_Facility")]
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_Facility")]
        public int iFacilityId
        {
            get { return UserInfo.iFacilityId; }
            set { UserInfo.iFacilityId = value; }
        }
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_Supervisor")]
        public int iSupervisorID
        {
            get { return UserInfo.iSupervisorID; }
            set { UserInfo.iSupervisorID = value; }
        }


        [Required(ErrorMessageResourceType = typeof(Resources_UserManagement), ErrorMessageResourceName = "required_Supervisor")]
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_Supervisor")]
        public string sSupervisorName
        {
            get { return UserInfo.sSupervisorName; }
            set { UserInfo.sSupervisorName = value; }
        }
        [Required(ErrorMessageResourceType = typeof(Resources_UserManagement), ErrorMessageResourceName = "required_LOB")]
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_LOB")]
        public int iLOBID
        {
            get { return UserInfo.iLOBID; }
            set { UserInfo.iLOBID = value; }
        }
        [Required(ErrorMessageResourceType = typeof(Resources_UserManagement), ErrorMessageResourceName = "required_designation")]
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_designation")]
        public int iJobID
        {
            get { return UserInfo.iJobID; }
            set { UserInfo.iJobID = value; }
        }
        [Required(ErrorMessageResourceType = typeof(Resources_UserManagement), ErrorMessageResourceName = "required_SBU")]
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_SBU")]
        public int iSBUID
        {
            get { return UserInfo.iSBUID; }
            set { UserInfo.iSBUID = value; }
        }
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_Disable")]
        public bool? bDisabled
        {
            get { return UserInfo.bDisabled; }
            set { UserInfo.bDisabled = Convert.ToBoolean(value); }
        }

        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_Isbot")]
        public bool? bIsBot
        {
            get { return UserInfo.bIsBot; }
            set { UserInfo.bIsBot = Convert.ToBoolean(value); }
        }
        [Required(ErrorMessageResourceType = typeof(Resources_UserManagement), ErrorMessageResourceName = "required_Role")]
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_Role")]
        public string iRoleID
        {
            get
            {
                if (UserInfo.oRoles == null || UserInfo.oRoles.Count == 0)
                { return "0"; }
                else
                    return UserInfo.oRoles.First().iRoleID.ToString();
            }
            set
            {
                if (UserInfo.oRoles == null)
                {
                    UserInfo.oRoles = new List<BERoleInfo>();
                }
                UserInfo.oRoles.Add(new BERoleInfo() { iRoleID = Convert.ToInt32(value) });
                //RoleInfo.iRoleID = value;
            }
        }
        public int iExistingRoleID
        {
            get;
            set;
        }
        //[Required(ErrorMessageResourceType = typeof(Resources_UserManagement), ErrorMessageResourceName = "required_RoleApprover")]
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_Approver")]
        public string iRoleApprover
        {
            get { return UserInfo.iRoleApprover.ToString(); }
            set { UserInfo.iRoleApprover = Convert.ToInt32(value); }
        }
        //  [Required(ErrorMessageResourceType = typeof(Resources_UserManagement), ErrorMessageResourceName = "required_Client")]
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_Client")]
        public int iClientID
        {
            get { return UserInfo.iClientID; }
            set { UserInfo.iClientID = value; }
        }
        private List<UserManagementViewModel> _lstUserCreateData = new List<UserManagementViewModel>();
        public List<UserManagementViewModel> lstUserCreateData { get { return _lstUserCreateData; } set { _lstUserCreateData = value; } }

        IList<ProcessAndApprover> _lstProcessToMap = new List<ProcessAndApprover>();
        public IList<ProcessAndApprover> lstProcessToMap { get { return _lstProcessToMap; } set { _lstProcessToMap = value; } }
        SelectList _lstSuperwisers = new SelectList(new List<KeyValuePair<int, string>>(), "Key", "Value");
        public SelectList lstSuperwisers { get { return _lstSuperwisers; } }

        // [Required(ErrorMessageResourceType = typeof(Resources_UserManagement), ErrorMessageResourceName = "required_Search")]
        public string sSearchText { get; set; }

        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_Team")]
        public string sTeamName { get; set; }
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_ERP")]
        public string sErpJob { get; set; }
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_DOL")]
        public string DOL { get; set; }
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_LocationName")]
        public string sLocationName { get; set; }
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_Facility")]
        public string sFacilityName { get; set; }
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_ClientUser")]
        public bool bClientUser { get; set; }

        DataTable _dtShift = new DataTable();
        public DataTable dtShift { get { return _dtShift; } set { _dtShift = value; } }

        //  DataTable _dtClientProcess = new DataTable();
        public DataTable dtClientProcess { get; set; }
        //public DataTable dtClientProcess
        //{
        //    get
        //    //{
        //    //    return _dtClientProcess;
        //    //}
        //    //set
        //    //{
        //    //    _dtClientProcess = value;
        //    //}
        //}
        DataTable _dtQuota = new DataTable();
        public DataTable dtQuota { get { return _dtQuota; } set { _dtQuota = value; } }
        DataTable _dtERPChanges = new DataTable();
        public DataTable dtERPChanges { get { return _dtERPChanges; } set { _dtERPChanges = value; } }

        private DateTime _date;

        [Required(ErrorMessageResourceType = typeof(Resources_UserManagement), ErrorMessageResourceName = "required_FromDate")]
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_FromDate")]
        public string FromDate
        {
            get;
            set;
        }
        //public DateTime FromDate
        //{
        //    get { return _date; }
        //    set { _date = value; }
        //}
        [Required(ErrorMessageResourceType = typeof(Resources_UserManagement), ErrorMessageResourceName = "required_ToDate")]
        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_ToDate")]
        public string ToDate
        {
            get;
            set;
        }
        //public DateTime ToDate
        //{
        //    get { return _date; }
        //    set { _date = value; }
        //}
        DataTable _dtRequestStatus = new DataTable();
        public DataTable dtRequestStatus { get { return _dtRequestStatus; } set { _dtRequestStatus = value; } }
        DataTable _dtRequestApproval = new DataTable();
        public DataTable dtRequestApproval { get { return _dtRequestApproval; } set { _dtRequestApproval = value; } }

        private List<BEUserInfo> _UserEditList = new List<BEUserInfo>();
        public List<BEUserInfo> UserEditList { get { return _UserEditList; } set { _UserEditList = value; } }

        //public BEUserMapping objUserMapping = new BEUserMapping();

        [Display(ResourceType = typeof(Resources_UserManagement), Name = "display_EffectiveDate")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime dEffectiveDate
        {
            get { return _date; }
            set { _date = value; }
        }


        private List<TreeStructure> _treeData = new List<TreeStructure>();
        public List<TreeStructure> treeData { get { return _treeData; } set { _treeData = value; } }
        DataTable _dtUserApproval = new DataTable();
        public DataTable dtUserApproval { get { return _dtUserApproval; } set { _dtUserApproval = value; } }
    }

    [Serializable]
    public class UserApproval
    {
        private int _RequestId;
        public int RequestId { get { return _RequestId; } set { _RequestId = value; } }

        private string _RequestTypeDetails;
        public string RequestTypeDetails { get { return _RequestTypeDetails; } set { _RequestTypeDetails = value; } }

        private int _RequestType;
        public int RequestType { get { return _RequestType; } set { _RequestType = value; } }

        private int _Id;
        public int Id { get { return _Id; } set { _Id = value; } }

        private string _RequestName;
        public string RequestName { get { return _RequestName; } set { _RequestName = value; } }

        private int _RequestTypeId;
        public int RequestTypeId { get { return _RequestTypeId; } set { _RequestTypeId = value; } }
        ApproverDetail _SelectedApproverL1 = new ApproverDetail() { sApproverName = BPA.GlobalResources.UI.Resources_common.display_Select, iApproverId = 0 };
        public ApproverDetail SelectedApproverL1 { get { return _SelectedApproverL1; } set { _SelectedApproverL1 = value; } }

        private List<ApproverDetail> _lstApprovalLaveL1 = new List<ApproverDetail>();
        public List<ApproverDetail> lstApprovalLaveL1 { get { return _lstApprovalLaveL1; } set { _lstApprovalLaveL1 = value; } }

        ApproverDetail _SelectedApproverL2 = new ApproverDetail() { sApproverName = BPA.GlobalResources.UI.Resources_common.display_Select, iApproverId = 0 };
        public ApproverDetail SelectedApproverL2 { get { return _SelectedApproverL2; } set { _SelectedApproverL2 = value; } }

        private List<ApproverDetail> _lstApprovalLaveL2 = new List<ApproverDetail>();
        public List<ApproverDetail> lstApprovalLaveL2 { get { return _lstApprovalLaveL2; } set { _lstApprovalLaveL2 = value; } }

        private int _ProcessId;
        public int ProcessId { get { return _ProcessId; } set { _ProcessId = value; } }

    }

    [Serializable]
    public class ApproverDetail
    {
        private int _iApproverId;
        public int iApproverId { get { return _iApproverId; } set { _iApproverId = value; } }
        private string _sApproverName;
        public string sApproverName { get { return _sApproverName; } set { _sApproverName = value; } }
    }

    [Serializable]
    public class TreeStructure
    {
        public string text { get; set; }
        public string id { get; set; }

        private List<TreeStructure> _items = new List<TreeStructure>();
        public List<TreeStructure> items { get { return _items; } set { _items = value; } }
        public bool Checked { get; set; }
        public bool AllReadyChecked { get; set; }
        public bool expanded { get; set; }
        public string NodeIdentity { get; set; }
        public int MappedOn { get; set; }
    }
    [Serializable]
    public class ProcessAndApprover
    {

        public int iApproverId { get { return _SelectedApprover == null ? 0 : _SelectedApprover.iApproverId; } }
        public BEProcessInfo objProcessInfo = new BEProcessInfo();
        public bool bSelected { get; set; }
        public int iProcessID { get { return objProcessInfo.iProcessID; } set { objProcessInfo.iProcessID = value; } }
        public string sProcessName { get { return objProcessInfo.sProcessName; } set { objProcessInfo.sProcessName = value; } }
        public int iProcessMapID { get { return objProcessInfo.iProcessID; } set { objProcessInfo.iProcessID = value; } }

        private List<ApproverDetail> _lstApprover = new List<ApproverDetail>();
        public List<ApproverDetail> lstApprover { get { return _lstApprover; } set { _lstApprover = value; } }
        ApproverDetail _SelectedApprover = new ApproverDetail() { iApproverId = 0, sApproverName = BPA.GlobalResources.UI.Resources_common.display_Select };
        public ApproverDetail SelectedApprover { get { return _SelectedApprover; } set { _SelectedApprover = value; } }
    }
    [Serializable]
    public class ProcessMap
    {
        public int ClientID { get; set; }
        public int ProcessID { get; set; }
    }
}
