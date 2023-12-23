using BPA.GlobalResources.UI.WorkManagement;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicUI.WorkManagement.Helper.CustomValidationAttributes;
using MicUI.WorkManagement.Services.ServiceModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MicUI.WorkManagement.Models.ViewModels
{
    #region  Work Definition Model
    [Serializable]
    public class WorkDefinitionViewModel
    {


        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), ErrorMessageResourceName = "required_Client")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "dispaly_Client")]
        public string ClientName
        {
            get;
            set;
        }

        private string _ProcessName;
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), ErrorMessageResourceName = "required_Process")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_Process")]
        public string ProcessName
        {
            get { return _ProcessName; }
            set { _ProcessName = value; }
        }
        public string ProcessName1
        {
            get { return _ProcessName; }
            set { _ProcessName = value; }
        }
        public string ProcessName2
        {
            get { return _ProcessName; }
            set { _ProcessName = value; }
        }
        public string ProcessName3
        {
            get { return _ProcessName; }
            set { _ProcessName = value; }
        }
        public string ProcessName4
        {
            get { return _ProcessName; }
            set { _ProcessName = value; }
        }
        public string ProcessName5
        {
            get { return _ProcessName; }
            set { _ProcessName = value; }
        }
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), ErrorMessageResourceName = "required_Campaign")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_Campaign")]
        public string CampaignName
        {
            get;
            set;
        }
        public string CampaignName1
        {
            get;
            set;
        }
        public string CampaignName2
        {
            get;
            set;
        }
        public string CampaignName3
        {
            get;
            set;
        }
        public string CampaignName4
        {
            get;
            set;
        }
        public string CampaignName5
        {
            get;
            set;
        }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), ErrorMessageResourceName = "required_WorkDefinationName")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_WorkDefinitionName")]
        public string WorkDefinitionName
        {
            get;
            set;
        }


        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_Description")]
        public string Description
        {
            get;
            set;
        }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), ErrorMessageResourceName = "required_NoOfRows")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_NoRows")]
        public int NoOfRows
        {
            get;
            set;
        }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), ErrorMessageResourceName = "required_Columns")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_NoColumns")]
        public string NoOfColumns
        {
            get;
            set;
        }


        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_Disable")]
        public bool? DisableWork
        {
            get;
            set;
        }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_GenerateLetter")]
        public bool GenerateLetter
        {
            get;
            set;
        }

        public bool IsDistributionBot
        {
            get;
            set;
        }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_IscontrolInTAB")]
        public bool TABMapping
        {
            get;
            set;
        }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_IscontrolInGrid")]
        public bool IsGridConfiguration
        {
            get;
            set;
        }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_LetterMaping")]
        public bool LetterMaping
        {
            get;
            set;
        }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_ErrorSnapshot")]
        public bool? ErrorSnapshot
        {
            get;
            set;
        }
        public string ErrorDuration
        {
            get;
            set;
        }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_ErrorDurationInDay")]
        public string ErrorDurationInDay
        {
            get;
            set;
        }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_Translatetxt")]
        public string sObjectTranslatetxt { get; set; }


        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_Translatelanguage")]
        public string sTranslatelanguage
        {
            get;
            set;
        }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_Translatelanguage")]
        public string sTranslatelanguageChoice { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_Translatetxt")]
        public string sObjectTranslateChoicetxt { get; set; }

        List<WorkObject> _WorkDefinition = new List<WorkObject>();
        public List<WorkObject> WorkDefinition { get { return _WorkDefinition; } set { _WorkDefinition = value; } }

        List<WorkObject> _WorkDefinitionGRD = new List<WorkObject>();
        public List<WorkObject> WorkDefinitionGRD { get { return _WorkDefinitionGRD; } set { _WorkDefinitionGRD = value; } }



        List<TreeViewGrd> _TreeViewGRD = new List<TreeViewGrd>();
        public List<TreeViewGrd> TreeViewGRD { get { return _TreeViewGRD; } set { _TreeViewGRD = value; } }
        public string text { get; set; }
        public bool? DisableGridObject
        {
            get;
            set;
        }
        private List<BEWorkObjectChoice> _Choice = new List<BEWorkObjectChoice>();
        public List<BEWorkObjectChoice> Choice
        {
            get { return _Choice; }
            set { _Choice = value; }
        }
        private List<BEWorkObjectTAB> _AddTAB = new List<BEWorkObjectTAB>();
        public List<BEWorkObjectTAB> AddTAB
        {
            get { return _AddTAB; }
            set { _AddTAB = value; }
        }
        private List<BEWorkObjectTAB> _AddGrid = new List<BEWorkObjectTAB>();
        public List<BEWorkObjectTAB> AddGrid
        {
            get { return _AddGrid; }
            set { _AddGrid = value; }
        }
        public string IncreaseSearch { get; set; }
        private List<BEWorkObjectTranslateList> _TranslateList = new List<BEWorkObjectTranslateList>();
        public List<BEWorkObjectTranslateList> TranslateList
        {
            get { return _TranslateList; }
            set { _TranslateList = value; }
        }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), ErrorMessageResourceName = "required_Location")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_Location")]
        public string Location { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), ErrorMessageResourceName = "required_ShiftWindow")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_ShiftWindow")]
        public string ShiftWindow { get; set; }


        List<string> _PurposeofcreationofWork = new List<string>();
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), ErrorMessageResourceName = "required_Purposeofcreationofcampaign")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "required_WorkDefinationName")]
        public List<string> PurposeofcreationofWork { get { return _PurposeofcreationofWork; } set { _PurposeofcreationofWork = value; } }
        public bool bIsRunTimeUploadRequired { get; set; }

        public bool WorkManagement { get; set; }
        public bool TimeTracking { get; set; }
        public bool TransactionsMonitoring { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), ErrorMessageResourceName = "required_BusinessJustifications")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_BusinessJustifications")]
        public string BusinessJustifications { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_TargetUsers")]
        public string TargetUsers { get; set; }

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
        public string KeyBenefits { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), ErrorMessageResourceName = "required_BusinessApprover")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_BusinessApprover")]
        public string BusinessApprover { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_IsemailCampaign")]
        public bool IsEmail { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), ErrorMessageResourceName = "required_TechnologyApprover")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign), Name = "display_TechnologyApprover")]
        public string TechnologyApprover { get; set; }

        public string ApiKey { get; set; }

        public string ApiUrl { get; set; }
        public BEWorkObjectApprover ObjBusinessJustificationsData { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "dispaly_Changerequest")]
        public string ChangeRequest { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_ApproveRequester")]
        public string RequestCreator { get; set; }
        public Int32 IsLevel { get; set; }
        public Int32 iApproverId { get; set; }

        public Int32? iStoreId { get; set; }
        public string ChangeRequestStatus { get; set; }
        public BETenantInfo oTenant { get; set; }
        #region Page PreView

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_StoreWorkDefinitionName")]
        public string PreViewWorkDefinitionName
        {
            get;
            set;
        }

        List<PreView> _oPreView = new List<PreView>();
        public List<PreView> oPreView { get { return _oPreView; } set { _oPreView = value; } }



        #endregion
        #region Search
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_Name")]
        public string Name { get; set; }

        public List<BEStoreInfo> oStore
        {
            get;
            set;
        }


        #endregion

        // Object Formulae
        #region Object Formulae
        //[Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_ApproveRequester")]
        // Target Object  
        [DropDownRequired(true, ErrorMessageResourceName = "required_TargetObject", ErrorMessageResourceType = typeof(Resource_WorkDefinition), sControlKeyCollection = "btnSubmit")]
        [Display(ResourceType = typeof(Resource_WorkDefinition), Name = "dispaly_TargetObject")]
        public string TargetlstObject { get; set; }

        // Object

        [Display(ResourceType = typeof(Resource_WorkDefinition), Name = "display_Object")]
        public string lstObject { get; set; }

        //Operator

        [DropDownRequired(true, ErrorMessageResourceName = "required_object", ErrorMessageResourceType = typeof(Resource_WorkDefinition), sControlKeyCollection = "btnSubmit")]
        [Display(ResourceType = typeof(Resource_WorkDefinition), Name = "display_Operator")]
        public string Operator { get; set; }


        //Constant
        [Display(ResourceType = typeof(Resource_WorkDefinition), Name = "display_Constant")]
        public string Constant { get; set; }

        //Disable
        [Display(ResourceType = typeof(Resource_WorkDefinition), Name = "display_Disable")]
        public string Disable { get; set; }

        //Events
        [Display(ResourceType = typeof(Resource_WorkDefinition), Name = "display_Event")]
        public string Events { get; set; }

        //remarks

        public string remarks { get; set; }

        //Dropdown Property
        [Display(ResourceType = typeof(Resource_WorkDefinition), Name = "display_Property")]
        public string ddlproperty { get; set; }

        //TextBox Property
        [Display(ResourceType = typeof(Resource_WorkDefinition), Name = "display_Property")]
        public string txtproperty { get; set; }

        //Formula
        [Display(ResourceType = typeof(Resource_WorkDefinition), Name = "display_Formula")]
        public string Formula { get; set; }

        public string HiddenFormula { get; set; }
        public string ObjFormulaID { get; set; }
        public string EventName { get; set; }

        public IList<BEFormulaData> ListFormula;
        #endregion

        public string HTranslateTxt { get; set; }

        List<BEWorkObjectChoiceLanguage> _oLanguage_Choice = new List<BEWorkObjectChoiceLanguage>();
        public List<BEWorkObjectChoiceLanguage> oLanguage_Choice { get { return _oLanguage_Choice; } set { _oLanguage_Choice = value; } }
        public List<TranslateChoiceList> Choicelng
        {
            get { return _Choicelng; }
            set { _Choicelng = value; }
        }
        private List<TranslateChoiceList> _Choicelng = new List<TranslateChoiceList>();

        public string GridObjectName { get; set; }
        public string GridObjectNameID { get; set; }
        public string GridObjectRowNum { get; set; }
        public string GridControlF { get; set; }
        public bool? IsGrdEditable
        {
            get; set;
        }
        public bool? IsForStaging
        {
            get;
            set;
        }
    }
    #endregion

    public class TranslateList
    {
        public Int32 iSDOBJLanID { get; set; }
        public Int32 iLanguageID { get; set; }
        public string sCulture { get; set; }
        public string sLanguage { get; set; }
        public string sConvertText { get; set; }
    }
    public class TranslateChoiceList
    {
        public Int32 iCSDOBJLanID { get; set; }
        public Int32 DSObjChoiceLanguageID { get; set; }

        public Int32 DSObjChoiceID { get; set; }

        public Int32 DSObjID { get; set; }
        public Int32 iCLanguageID { get; set; }

        public Int32 iCdisplayorder { get; set; }
        public string sCCulture { get; set; }
        public string sCLanguage { get; set; }
        public string sCConvertText { get; set; }
        public string icGroupID { get; set; }


        public bool? cbDisabled { get; set; }
    }
    #region Work Object Grid Model
    [Serializable]
    public class WorkObject
    {
        public string ISExistingRow { get; set; }
        public bool? bIsMailCmapaign { get; set; }
        public bool? bVisible { get; set; }
        public int iColSpan { get; set; }
        public int iValidationID { get; set; }
        public int iObjectID { get; set; }
        public int iStoreID { get; set; }
        public string sObjectName { get; set; }
        public string sObjectDescription { get; set; }
        public string sObjectLabel { get; set; }

        public string sObjectTranslateLabel { get; set; }

        public string itranslateID { get; set; }
        public string UID { get; set; }
        public string LanguageClick { get; set; }

        public int iObjectType { get; set; }
        public bool? bEditable { get; set; }
        public bool? bRequired { get; set; }
        public bool? bReadOnly { get; set; }
        public bool? bWorkID { get; set; }
        public string sDataType { get; set; }
        public string sDataTypeValidation { get; set; }
        public string oChoiceSet { get; set; }
        public int iLength { get; set; }
        public int iLengthReadonly { get; set; }

        public DataTable dtChoiceTable { get; set; }
        List<BEWorkObjectChoice> _oChoice = new List<BEWorkObjectChoice>();
        public List<BEWorkObjectChoice> oChoice { get { return _oChoice; } set { _oChoice = value; } }

        private List<BEWorkObjectTranslateList> _TranslateList = new List<BEWorkObjectTranslateList>();
        public List<BEWorkObjectTranslateList> oTranslateList
        {
            get { return _TranslateList; }
            set { _TranslateList = value; }
        }
        //List<BEWorkObjectTranslateList> _TranslateList = new List<BEWorkObjectTranslateList>();
        //public List<BEWorkObjectTranslateList> TranslateList { get { return _TranslateList; } set { _TranslateList = value; } }
        public bool? bSearch { get; set; }
        public bool? bChangeVersion { get; set; }
        public string ObjectType_IsEnabled { get; set; }
        public string ObjectDataType_IsEnabled { get; set; }
        public string Lenght_IsEnabled { get; set; }
        public string Validation_IsEnabled { get; set; }
        public bool? bDisabled { get; set; }
        public bool? bTATComparison { get; set; }
        public string sTATType { get; set; }
        public bool? bTargetTAT { get; set; }
        public bool? bUniqueID { get; set; }
        public bool? bTransactionType { get; set; }
        public bool? bLANID { get; set; }
        public bool? bIsUpload { get; set; }

        public bool? bIsTranslate { get; set; }

        public bool? bIsReport { get; set; }
        public bool? bCustomerIdentifier { get; set; }
        public Int32 iIsReportOrder { get; set; }



        public int irow_No { get; set; }
        public int iRowNumber { get; set; }

        public int icolumn_No { get; set; }
        public int iColumnNumber { get; set; }
        public int icolumn_Span { get; set; }

        public string iTAB_ID { get; set; }

        public string sGridControlID { get; set; }
        public bool? isSelected { get; set; }
        public int iProcessStepId { get; set; }
        public string sProcessStepName { get; set; }
        public bool? bFreshTransaction { get; set; }
        public int iCreatedBy { get; set; }
        public DateTime? _dCreateDate = DateTime.Today;

        public DateTime? dCreateDate
        {
            get { return _dCreateDate; }
            set { _dCreateDate = value; }
        }

        public int iModifiedBy { get; set; }
        //public DateTime dModifyDate{ get; set; }
        private DateTime? _dModifyDate = DateTime.Today;
        public DateTime? dModifyDate
        {
            get { return _dModifyDate; }
            set { _dModifyDate = value; }
        }


        BEControlTypeInfo _selectControlType = new BEControlTypeInfo() { sControlType = @BPA.GlobalResources.UI.Resources_common.display_Select, iControlTypeID = 0 };
        public BEControlTypeInfo selectControlType { get { return _selectControlType; } set { _selectControlType = value; } }

        SelectListItem _selectedsDataType = new SelectListItem() { Text = @BPA.GlobalResources.UI.Resources_common.display_Select, Value = "0" };
        public SelectListItem selectedDataType { get { return _selectedsDataType; } set { _selectedsDataType = value; } }


        BEValidations _selectedValidation = new BEValidations() { ValidationType = @BPA.GlobalResources.UI.Resources_common.display_Select, ValidationId = 0 };
        public BEValidations selectedValidation { get { return _selectedValidation; } set { _selectedValidation = value; } }

        SelectListItem _selectedRow = new SelectListItem() { Text = @BPA.GlobalResources.UI.Resources_common.display_Select, Value = "0" };
        public SelectListItem selectedRow { get { return _selectedRow; } set { _selectedRow = value; } }

        SelectListItem _selectedcolumn = new SelectListItem() { Text = @BPA.GlobalResources.UI.Resources_common.display_Select, Value = "0" };
        public SelectListItem selectedcolumn { get { return _selectedcolumn; } set { _selectedcolumn = value; } }

        SelectListItem _selectedcolumnSpan = new SelectListItem() { Text = @BPA.GlobalResources.UI.Resources_common.display_Select, Value = "0" };
        public SelectListItem selectedcolumnSpan { get { return _selectedcolumnSpan; } set { _selectedcolumnSpan = value; } }


        BEWorkObjectTAB _selectedRowTAB = new BEWorkObjectTAB() { sTABNameValue = @BPA.GlobalResources.UI.Resources_common.display_Select, sChoiceValue = "0" };
        public BEWorkObjectTAB selectedRowTAB { get { return _selectedRowTAB; } set { _selectedRowTAB = value; } }


        BEWorkObjectGrid _selectedGridControlObj = new BEWorkObjectGrid() { sGridChoiceValue = @BPA.GlobalResources.UI.Resources_common.display_Select, iObjectGridChoiceID = "0" };
        public BEWorkObjectGrid selectedGridControlObj { get { return _selectedGridControlObj; } set { _selectedGridControlObj = value; } }
        public string GridObjectName { get; set; }

        public bool? bSearchableSearch { get; set; }
        public string iReportsOrderSearch { get; set; }

    }

    #endregion

    #region Page PreView
    [Serializable]
    public class PreView
    {


        public int iObjectID { get; set; }
        public string sObjectName { get; set; }
        public string sObjectLabel { get; set; }
        public int irow_No { get; set; }
        public int icolumn_No { get; set; }
        public int icolumn_Span { get; set; }

        public string iTAB_ID { get; set; }

        SelectListItem _selectedRow = new SelectListItem() { Text = @BPA.GlobalResources.UI.Resources_common.display_Select, Value = "0" };
        public SelectListItem PreViewselectedRow { get { return _selectedRow; } set { _selectedRow = value; } }

        SelectListItem _selectedcolumn = new SelectListItem() { Text = @BPA.GlobalResources.UI.Resources_common.display_Select, Value = "0" };
        public SelectListItem PreViewselectedcolumn { get { return _selectedcolumn; } set { _selectedcolumn = value; } }

        SelectListItem _selectedcolumnSpan = new SelectListItem() { Text = @BPA.GlobalResources.UI.Resources_common.display_Select, Value = "0" };
        public SelectListItem PreViewselectedcolumnSpan { get { return _selectedcolumnSpan; } set { _selectedcolumnSpan = value; } }

        SelectListItem _selectedRowTAB = new SelectListItem() { Text = @BPA.GlobalResources.UI.Resources_common.display_Select, Value = "0" };
        public SelectListItem PreViewselectedRowTAB { get { return _selectedRowTAB; } set { _selectedRowTAB = value; } }
    }


    #endregion

    #region WorkApproval
    [Serializable]
    public class WorkApproval
    {
        #region Fields
        public int ApprovalId { get; set; }
        public int BusinessApproverId { get; set; }
        public int TechnologyApproverId { get; set; }
        public int CreatedBy { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_ApproveClient")]
        public string ClientName { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_ApproveProcess")]
        public string ProcessName { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_ApproveCampaign")]
        public string CampaignName { get; set; }

        public string KeyBenfits { get; set; }
        public string StoreName { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_ApproveRequester")]
        public string RequestCreator { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_ApproveRequestedon")]
        public string RequestedOn { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_ApproveBussinesApprover")]
        public string BusinessApprover { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_ApproveTechApprover")]
        public string TechApprover { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_ApproveStatus")]
        public string Status { get; set; }
        public string StatusToShowHideButtons { get; set; }
        public string ChangeRequest { get; set; }

        public bool? IsChecked { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_FromDate")]
        public DateTime FromDate { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition), Name = "display_ToDate")]
        public DateTime ToDate { get; set; }
        public BETenantInfo oTenant { get; set; }
        #endregion
    }
    #endregion

    [Serializable]
    public class ObjectList : IDisposable
    {

        public void Dispose()
        {

        }
        public string ObjName { get; set; }
        public string ObjValue { get; set; }

    }

    [Serializable]
    public class TreeViewGrd
    {
        public string text { get; set; }

        public List<TreeViewGrd> items { get; set; }
    }

    public class GRDWorkObject
    {
        public string GridObjectName { get; set; }
        public string GridObjectNameID { get; set; }
        public bool DisableGridObject { get; set; }
        public bool IsGrdEditable { get; set; }
        public string GridObjectRowNum { get; set; }

        public string gClientName { get; set; }
        public string gProcessName { get; set; }
        public string gCampaignName { get; set; }
        public List<GrdListData> Grid_LstValues
        {
            get { return _Grid_LstValues; }
            set { _Grid_LstValues = value; }
        }
        private List<GrdListData> _Grid_LstValues = new List<GrdListData>();


    }
    public class GrdListData
    {
        public int iObjectID { get; set; }
        public int iStoreID { get; set; }
        public string sObjectName { get; set; }
        public string sObjectDescription { get; set; }
        public string sObjectLabel { get; set; }
        public int iObjectType { get; set; }
        public string sDataType { get; set; }
        public int iLength { get; set; }

        public int iValidationID { get; set; }

        public string iGrdColumeOrder { get; set; }

        public bool bVisible { get; set; }
        public bool bSearch { get; set; }
        public bool bEditable { get; set; }
        public bool bRequired { get; set; }
        public bool bDisabled { get; set; }

        public string UID { get; set; }
    }
}
