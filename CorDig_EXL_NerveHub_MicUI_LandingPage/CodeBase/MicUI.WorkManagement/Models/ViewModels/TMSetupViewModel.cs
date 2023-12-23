using BPA.GlobalResources.UI;
using Microsoft.AspNetCore.Mvc;
using MicUI.WorkManagement.Helper.CustomValidationAttributes;
using MicUI.WorkManagement.Services.ServiceModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MicUI.WorkManagement.Models.ViewModels
{
    [Serializable]
    public class TMSetupViewModel : SearchViewModel
    {
        public TMSetupViewModel()
            : base(new string[] { Resources_common.display_Client, Resources_common.display_Process })
        { }

        #region COMMON VARIABLE

        public int TMTabIndex { get; set; }
        public int TMSetupId { get; set; }
        public bool bTabStrip { get; set; }
        public string EncryptTMSetupId { get; set; }

        public int WizardDirection { get; set; } //0>> Prev Direction,1>> Next Direction

        #endregion

        #region INDEX & TM DEFINITION

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), ErrorMessageResourceName = "required_SubProcess")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_SubProcess")]
        public string SubProcess { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_DefinitionName")]
        public string DefinitionName { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), ErrorMessageResourceName = "required_TMSetupName")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_TMSetupName")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), ErrorMessageResourceName = "required_MonitoringType")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_MonitoringType")]
        public string[] MonitoringType { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_RandomSample")]
        public bool RandomSample { get; set; } //changed by Nitin bool? to bool

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_AuditEmail")]
        public bool AuditEmail { get; set; } //changed by Nitin bool? to bool

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_Disable")]
        public bool Disable { get; set; } //changed by Nitin bool? to bool
        public bool AuditsCompleted { get; set; } //changed by Nitin bool? to bool
        public bool MTDAuditsCompleted { get; set; } //changed by Nitin bool? to bool
        public bool PendingAudits { get; set; } //changed by Nitin bool? to bool

        public bool RCAConfiguration { get; set; } //changed by Nitin bool? to bool
        public bool CustomEmail { get; set; } //changed by Nitin bool? to bool
        public List<TMSetupViewModel> TMDefinitionList { get; set; }
        public bool hfRCAConfiguration { get; set; } //changed by Nitin bool? to bool
        public int RCAID { get; set; }
        public int iRCAID { get; set; }

        public string sRCAConfigQuestionID { get; set; }
        #endregion

        #region TM FORMULA

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_Fatal")]
        public string Fatal { get; set; }
        public bool bFatal { get; set; }
        public string HFFatal { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_NonFatal")]
        public string NonFatal { get; set; }

        public string HFNonFatal { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_Overall")]
        public string Overall { get; set; }

        public string HFOverall { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_DPU")]
        public string DPU { get; set; }

        public string HFDPU { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_DPO")]
        public string DPO { get; set; }

        public string HFDPO { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_Compliance")]
        public string Compliance { get; set; }

        public string HFCompliance { get; set; }

        public string ddlParameter { get; set; }
        public string Opertions { get; set; }
        public string ddlNo { get; set; }
        public bool rdoButtonList { get; set; }

        public string TOUseFormula { get; set; }

        public string EnterFormula { get; set; }

        public string ParameterWiseDefects { get; set; }
        public string Miscellaneous { get; set; }
        public string FreeFields { get; set; }

        public string ViewFormula { get; set; }


        #endregion

        #region TM QUESTION
        public string TMAnchorFlag { get; set; }

        public bool TMOptionStatus { get; set; }
        public string sTempOptionID { get; set; }
        public int TMQuestionId { get; set; }
        public string sTMQTempQuestionId { get; set; }
        public List<BETMQOption> lstTMQQuestionOptions { get; set; }
        public int iQuestionCreatedBy { get; set; }
        public int SRN { get; set; }
        public string Info { get; set; }

        [RequiredField(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), sControlKeyCollection = "setuppage", ErrorMessageResourceName = "required_TMQuestionLabel")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_TMQuestionLabel")]
        public string TMQuestionLabel { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), ErrorMessageResourceName = "required_TMQuestionWeightage")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_TMQuestionWeightage")]
        public double TMQuestionWeightage { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_TMQuestionFatal")]
        public bool TMQuestionFatal { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_TMQuestionNonFatal")]
        public bool TMQuestionNonFatal { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_TMQuestionCTC")]
        public bool TMQuestionCTC { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_TMQuestionCompliance")]
        public bool TMQuestionCompliance { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_TMQuestionDisqualifier")]
        public bool TMQuestionDisqualifier { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_TMQuestionNonScoring")]
        public bool TMQuestionNonScoring { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_TMQuestionERSAble")]
        public bool TMQuestionERSAble { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_TMQuestionIsAuditEMail")]
        public bool TMQuestionIsAuditEMail { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_TMQuestionDisable")]
        public bool TMQuestionDisable { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_Category")]
        public string TMQuestionCategory { get; set; }

        public List<TMSetupViewModel> GetTMQuestionChoice { get; set; }

        public List<TMSetupViewModel> GetTMQuestionList = new List<TMSetupViewModel>();

        public string OptionValue { get; set; }

        public string TempTMOptionId { get; set; }
        public int OptionTMID { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), ErrorMessageResourceName = "required_Options")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_Options")]
        public string Options { get; set; }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), ErrorMessageResourceName = "required_OptionScore")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_OptionScore")]
        public float OptionScore { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_OptionNoScore")]
        public bool OptionNoScore { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_OptionDefect")]
        public bool OptionDefect { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_OptionCTC")]
        public bool OptionCTC { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_OptionIsAudit")]
        public bool OptionIsAudit { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_OptionDisable")]
        public bool OptionDisable { get; set; }
        public bool Optionbutton { get; set; }// added by chandan

        #endregion

        public string HFFlagePage { get; set; }
        public int CategoryNameID { get; set; }
        public string CategoryName { get; set; }
        public bool CategoryDisable { get; set; }
        public bool FormulaEnabled { get; set; }
        public string CategoryFormula { get; set; }
        public string CategoryDisplayFormula { get; set; }
        public List<TMSetupViewModel> FeedbackTypeLIST { get; set; }
        public List<TMSetupViewModel> CategoryList = new List<TMSetupViewModel>();
        public List<TMSetupViewModel> PreferredContactMode { get; set; }
        public int CategoryID { set; get; }
        public int Id { get; set; }
        public int FreeFieldID { get; set; }
        public string FreeName { get; set; }
        public string FreeDescription { get; set; }
        public string iControlTypeID { get; set; }
        public string FreeDataType { get; set; }
        public string FreeValidation { get; set; }
        public string modelId { get; set; }
        public string DataTypeText { get; set; }
        public string DataTypeValue { get; set; }
        public string ValidateText { get; set; }
        public string ValidateId { get; set; }
        public int MaxLength { get; set; }
        public string rowNumber { get; set; }
        public bool IsRequired { get; set; }//Step1
        public bool IsVasible { get; set; }//Step2
        public bool IsCTC { get; set; }//Step3
        public bool IsAuditEmail { get; set; }//Step4
        public bool Disabled { get; set; }//Step5
        public int ChoiceID { get; set; }
        public string ChoiceValue { get; set; }
        public string ChoiceCreatedBy { get; set; }
        public bool ChoiceDisabled { get; set; }
        public List<TMSetupViewModel> ListBindValidation { get; set; }
        public List<TMSetupViewModel> ListBind { get; set; }
        public List<TMSetupViewModel> FreeFieldList { get; set; }
        public List<TMSetupViewModel> Choice { get; set; }
        public List<TMSetupViewModel> FinalChoice { get; set; }
        public RowState RowState { get; set; }
        public string p_gRD_id { get; set; }
        public int ObjectChoiceID { get; set; }
        public int Order { get; set; }
        public bool IsRejectOptionRequired { get; set; }
        public List<string> RejectOptions { get; set; }

        #region TM PREVIEW
        public string PSheetName { get; set; }
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_Previewddlname")]
        public string PTMDefinitionID { get; set; }
        public string QHeading { get; set; }
        public string QoptionNum { get; set; }
        public List<TMSetupViewModel> FillTMSheet { get; set; }
        public int iTMQOptionID { get; set; }
        public string sOptionText { get; set; }
        public string MultipleText { get; set; }
        public List<TMSetupViewModel> ListShowSheet { get; set; }
        public List<TMSetupViewModel> lTMQOption { get; set; }
        #endregion

        #region TM MATRICMAP

        public int MetricDefinitionID { get; set; }//hfmetricdefID
        public string MetricName { get; set; } // sName
        public bool MetricDisabled { get; set; }//chkdisabled
        public string hfMetricTMID { get; set; }
        public bool hfItem { get; set; }
        public bool chkItem { get; set; }
        public bool hfdisabled { get; set; }
        public string RowStateMAP { get; set; }
        public List<TMSetupViewModel> MetricList { get; set; }


        #endregion

        #region TM FIELDLEVEL FORMULA

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), ErrorMessageResourceName = "required_TargetObject")]
        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_TargetObject")]
        public string TargetObject { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_TMFieldFormulaRadioQuestion")]
        public bool TMFieldFormulaRadioQuestion { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_TMFieldLevelFormulaFreeField")]
        public bool TMFieldLevelFormulaFreeField { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_TMFieldFormulaDrpDownQuestion")]
        public string TMFieldFormulaDrpDownQuestion { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_TMFreefieldDrpDownFormula")]
        public string TMFreefieldDrpDownFormula { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_TMFreeFieldFormulaChoice")]
        public string TMFreeFieldFormulaChoice { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_TMFreeFieldFormulaOperator")]
        public string TMFreeFieldFormulaOperator { get; set; }

        [Display(ResourceType = typeof(BPA.GlobalResources.UI.QualityManagement.Resources_TMSetup), Name = "display_TMFreeFieldFormulaProperty")]
        public string TMFreeFieldFormulaProperty { get; set; }


        public int FreeFieldFormulaID { get; set; }

        public string FreeFieldFormulaHidden { get; set; }

        [Required]
        public string FreeFieldEnterFormula { get; set; }

        public string SearchFreeFieldName { get; set; }

        public string ControlEvent { get; set; }

        public List<TMSetupViewModel> GetTMFreeFieldFormulaLst = new List<TMSetupViewModel>();

        #endregion

        #region TM MAPPING OF  FREE FIELD WITH SAMPLING


        #endregion
    }

    /// <summary>
    /// Shift Info Data Class
    /// </summary>
    [Serializable]
    [DataContract]
    public class BETMQOption : ObjectBase, IDisposable
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the BETMQuestion object.
        /// </summary>
        /// <value>The BETMQuestion</value>
        [DataMember]
        public BETMQuestion oTMQuestion
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the TMQOptionID.
        /// </summary>
        /// <value>The TMQOptionID.</value>
        [DataMember]
        public int iTMQOptionID
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the sTempOptionID.
        /// </summary>
        /// <value>The sTempOptionID</value>
        [DataMember]
        public string sTempOptionID
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the OptionText.
        /// </summary>
        /// <value>The OptionText</value>
        [DataMember]
        public string sOptionText
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the Score.
        /// </summary>
        /// <value>The Score.</value>
        [DataMember]
        public float fScore
        {
            get;
            set;
        }
        [DataMember]
        public bool bAuditEmail
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [Defect].
        /// </summary>
        /// <value><c>true</c> if [Defect]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bDefect
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [b CTC].
        /// </summary>
        /// <value><c>true</c> if [b CTC]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bCTC
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [b no score].
        /// </summary>
        /// <value><c>true</c> if [b no score]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bNoScore { get; set; }


        /// <summary>
        /// Gets or sets the state of the o row.
        /// </summary>
        /// <value>The state of the o row.</value>
        [DataMember]
        public RowState oRowState
        {
            get;
            set;
        }

        [DataMember]
        public bool Score_IsEnabled { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="BETMQOption"/> class.
        /// </summary>

        public BETMQOption()
        { oTMQuestion = new BETMQuestion(); }

        #endregion

    }

    /// <summary>
    /// Shift Info Data Class
    /// </summary>
    [Serializable]
    [DataContract]
    public class BETMQuestion : ObjectBase, IDisposable
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets the TMCategory object.
        /// </summary>
        /// <value>The TMCategory</value>
        [DataMember]
        public BETMCategory oTMCategory { get; set; }

        /// <summary>
        /// Gets or sets the TMQuestionID.
        /// </summary>
        /// <value>The TMQuestionID.</value>
        [DataMember]
        public int iTMQuestionID
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the TMQTypeID.
        /// </summary>
        /// <value>The TMQTypeID.</value>
        [DataMember]
        public int iTMQTypeID
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the TempQID.
        /// </summary>
        /// <value>The sTempQID</value>
        [DataMember]
        public string sTempQID
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the Label.
        /// </summary>
        /// <value>The Label</value>
        [DataMember]
        public string sLabel
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the weightage.
        /// </summary>
        /// <value>The weightage.</value>
        [DataMember]
        public double dbWeightage
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets a value indicating whether [fatal].
        /// </summary>
        /// <value><c>true</c> if [fatal]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bFatal
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets a value indicating whether [IsAudit].
        /// </summary>
        /// <value><c>true</c> if [IsAudit]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bIsAudit
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the TMQOption List.
        /// </summary>
        /// <value>The TMQOption List.</value>
        [DataMember]
        public List<BETMQOption> lTMQOption
        {
            get;
            set;
        }
        [DataMember]
        public List<BETMQOption> lTMRCAOption
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the state of the o row.
        /// </summary>
        /// <value>The state of the o row.</value>
        [DataMember]
        public RowState oRowState
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets a value indicating whether [b CTC].
        /// </summary>
        /// <value><c>true</c> if [b CTC]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bCTC
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [b compliance].
        /// </summary>
        /// <value><c>true</c> if [b compliance]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bCompliance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [b dis qualifier].
        /// </summary>
        /// <value><c>true</c> if [b dis qualifier]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bDisQualifier { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [b non scoring].
        /// </summary>
        /// <value><c>true</c> if [b non scoring]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bNonScoring { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [b ER sable].
        /// </summary>
        /// <value><c>true</c> if [b ER sable]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bERSable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [b non fatal].
        /// </summary>
        /// <value><c>true</c> if [b non fatal]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bNonFatal { get; set; }

        /// <summary>
        /// Weightage IsEnabled
        /// </summary>
        [DataMember]
        public bool Weightage_IsEnabled { get; set; }

        /// <summary>
        /// Serial Number
        /// </summary>
        [DataMember]
        public int SRN { get; set; }

        /// <summary>
        /// Question Info
        /// </summary>
        [DataMember]
        public string Info { get; set; }
        [DataMember]
        public string sRCAConfigurationID { get; set; }

        [DataMember]
        public int iRCAConfigurationID { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            oTMCategory = null;
            lTMQOption = null;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BETMQuestion"/> class.
        /// </summary>

        public BETMQuestion()
        { oTMCategory = new BETMCategory(); lTMQOption = new List<BETMQOption>(); }

        #endregion

    }
    /// <summary>
    /// Shift Info Data Class
    /// </summary>
    [Serializable]
    [DataContract]
    public class BETMCategory : ObjectBase, IDisposable
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the TMdefinition object.
        /// </summary>
        /// <value>The TMdefinition object.</value>
        [DataMember]
        public BETMDefinition oTMDefinition { get; set; }

        /// <summary>
        /// Gets or sets the question List.
        /// </summary>
        /// <value>The question List.</value>
        [DataMember]
        public List<BETMQuestion> lQuestion { get; set; }
        [DataMember]
        public List<BETMQuestion> lRCAQuestion { get; set; }
        [DataMember]
        public List<BEPACDetails> lPACQuestion { get; set; }
        /// <summary>
        /// Gets or sets the TMCID.
        /// </summary>
        /// <value>The TMCID.</value>
        [DataMember]
        public int iTMCID { get; set; }

        /// <summary>
        /// Gets or sets the TempTMCID.
        /// </summary>
        /// <value>The TempTMCID</value>
        [DataMember]
        public string sTempTMCID { get; set; }

        /// <summary>
        /// Gets or sets the name of Category.
        /// </summary>
        /// <value>The name</value>
        [DataMember]
        public string sCatName { get; set; }

        /// <summary>
        /// Gets or sets the weightage.
        /// </summary>
        /// <value>The weightage.</value>
        [DataMember]
        public double dbWeightage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [fatal].
        /// </summary>
        /// <value><c>true</c> if [fatal]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bFatal { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [FormulaEnabled].
        /// </summary>
        /// <value><c>true</c> if [fatal]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bFormulaEnabled { get; set; }

        /// <summary>
        /// Gets or sets the formula.
        /// </summary>
        /// <value>The formula.</value>
        [DataMember]
        public string sFormula { get; set; }

        /// <summary>
        /// Gets or sets the Displayformula.
        /// </summary>
        /// <value>The formula.</value>
        [DataMember]
        public string sDisplayFormula { get; set; }

        /// <summary>
        /// Gets or sets the state of the o row.
        /// </summary>
        /// <value>The state of the row.</value>
        [DataMember]
        public RowState oRowState { get; set; }

        [DataMember]
        public bool iRCAConfiguration { get; set; }

        [DataMember]
        public bool iCEAAuditConfiguration { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            oTMDefinition = null;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BETMCategory"/> class.
        /// </summary>

        public BETMCategory()
        {
            oTMDefinition = new BETMDefinition();
        }

        #endregion

    }

    [Serializable]
    [DataContract]
    public class BETMDefinition : ObjectBase, IDisposable
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets the Tmdefinition ID.
        /// </summary>
        /// <value>The Tmdefinition ID.</value>
        [DataMember]
        public int iTMDefinitionID { get; set; }


        /// <summary>
        /// Gets or sets the i process ID.
        /// </summary>
        /// <value>The i process ID.</value>
        [DataMember]
        public int iProcessID { get; set; }

        /// <summary>
        /// Gets or sets the i SubProcess ID.
        /// </summary>
        /// <value>The i SubProcess ID.</value>
        [DataMember]
        public int iSubProcessID { get; set; }

        /// <summary>
        /// Gets or sets the i client ID.
        /// </summary>
        /// <value>The i client ID.</value>
        [DataMember]
        public int iClientID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The Name.</value>
        [DataMember]
        public string sName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [DataMember]
        public string sDescription { get; set; }

        /// <summary>
        /// Gets or sets the fatal.
        /// </summary>
        /// <value>The fatal.</value>
        [DataMember]
        public string sFatal { get; set; }

        /// <summary>
        /// Gets or sets the fatal Display.
        /// </summary>
        /// <value>The fatal Display.</value>
        [DataMember]
        public string sFatalDisplay { get; set; }

        /// <summary>
        /// Gets or sets the non fatal.
        /// </summary>
        /// <value>The non fatal.</value>
        [DataMember]
        public string sNonFatal { get; set; }

        /// <summary>
        /// Gets or sets the non fatal display.
        /// </summary>
        /// <value>The non fatal display.</value>
        [DataMember]
        public string sNonFatalDisplay { get; set; }

        /// <summary>
        /// Gets or sets the overall.
        /// </summary>
        /// <value>The overall.</value>
        [DataMember]
        public string sOverall { get; set; }

        /// <summary>
        /// Gets or sets the overall display.
        /// </summary>
        /// <value>The overall display.</value>
        [DataMember]
        public string sOverallDisplay { get; set; }

        /// <summary>
        /// Gets or sets the DPU.
        /// </summary>
        /// <value>The DPU.</value>
        [DataMember]
        public string sDPU { get; set; }

        /// <summary>
        /// Gets or sets the DPU Display.
        /// </summary>
        /// <value>The DPU Display.</value>
        [DataMember]
        public string sDPUDisplay { get; set; }
        /// <summary>
        /// Gets or sets the DPO.
        /// </summary>
        /// <value>The DPO.</value>
        [DataMember]
        public string sDPO { get; set; }

        /// <summary>
        /// Gets or sets the DPO display.
        /// </summary>
        /// <value>The DPO display.</value>
        [DataMember]
        public string sDPODisplay { get; set; }

        /// <summary>
        /// Gets or sets the base.
        /// </summary>
        /// <value>The base.</value>
        [DataMember]
        public string sBase { get; set; }

        /// <summary>
        /// Gets or sets the base display.
        /// </summary>
        /// <value>The base display.</value>
        [DataMember]
        public string sBaseDisplay { get; set; }

        /// <summary>
        /// Gets or sets the additional.
        /// </summary>
        /// <value>The additional.</value>
        [DataMember]
        public string sAdditional { get; set; }

        /// <summary>
        /// Gets or sets the additional display.
        /// </summary>
        /// <value>The additional display.</value>
        [DataMember]
        public string sAdditionalDisplay { get; set; }

        /// <summary>
        /// Gets or sets the s compliance.
        /// </summary>
        /// <value>The s compliance.</value>
        [DataMember]
        public string sCompliance { get; set; }

        /// <summary>
        /// Gets or sets the s compliance display.
        /// </summary>
        /// <value>The s compliance display.</value>
        [DataMember]
        public string sComplianceDisplay { get; set; }

        /// <summary>
        /// Gets or sets the TMcategory list.
        /// </summary>
        /// <value>The TMcategory list.</value>
        [DataMember]
        IList<BETMCategory> lTMCategory { get; set; }

        /// <summary>
        /// Gets or sets the i cat exist.
        /// </summary>
        /// <value>The i cat exist.</value>
        [DataMember]
        public int iCatExist { get; set; }

        /// <summary>
        /// Gets or sets the i Question exist.
        /// </summary>
        /// <value>The iQExist.</value>
        [DataMember]
        public int iQExist { get; set; }


        /// <summary>
        /// Gets or sets the i FF exist.
        /// </summary>
        /// <value>The i FF exist.</value>
        [DataMember]
        public int iFFExist { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [b transaction void].
        /// </summary>
        /// <value><c>true</c> if [b transaction void]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bTransactionVoid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string sMonitoringTypeID { get; set; }

        //--New for C# formula--------------

        /// <summary>
        /// get or set Csharp formula string
        /// </summary>
        [DataMember]
        public string sFatalCSharp { get; set; }

        /// <summary>
        ///  get or set Csharp formula string
        /// </summary>
        [DataMember]
        public string sNonFatalCSharp { get; set; }

        /// <summary>
        ///  get or set Csharp formula string
        /// </summary>
        [DataMember]
        public string sOverallCSharp { get; set; }

        /// <summary>
        ///  get or set Csharp formula string
        /// </summary>
        [DataMember]
        public string sDPUCSharp { get; set; }

        /// <summary>
        ///  get or set Csharp formula string
        /// </summary>
        [DataMember]
        public string sDPOCSharp { get; set; }

        /// <summary>
        ///  get or set Csharp formula string
        /// </summary>
        [DataMember]
        public string sComplianceCSharp { get; set; }

        /// <summary>
        /// MonitoringTypeID
        /// </summary>
        [DataMember]
        public int iMonitoringTypeID { get; set; }

        /// <summary>
        /// MonitoringTypeTxt
        /// </summary>
        [DataMember]
        public string sMonitoringTypeTxt { get; set; }

        /// <summary>
        /// Randon Sample
        /// </summary>
        [DataMember]
        public bool bRandomSample { get; set; }

        /// <summary>
        /// Audit Email
        /// </summary>
        [DataMember]
        public bool bAuditEmail { get; set; }

        /// <summary>
        /// for predefined Reject Option Required
        /// </summary>
        public bool bIsRejectOptionRequired { get; set; }

        /// <summary>
        /// Reject Options
        /// </summary>
        public List<string> RejectOptions { get; set; }
        public bool IsAuditsCompleted { get; set; } //changed by Nitin bool? to bool
        public bool IsMTDAuditsCompleted { get; set; } //changed by Nitin bool? to bool
        public bool IsPendingAudits { get; set; } //changed by Nitin bool? to bool

        public bool IsRCAConfiguration { get; set; } //changed by Deepak bool? to bool
        public bool IsCustomEmail { get; set; } //changed by Deepak bool? to bool

        public bool IsCEAAudit { get; set; } //changed by Deepak bool? to bool
        #endregion

        #region Constructors
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="BETMDefinition"/> class.
        /// </summary>
        public BETMDefinition()
        { lTMCategory = new List<BETMCategory>(); }
        #endregion

    }

    public class BEPACDetails : ObjectBase, IDisposable
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets the TMCategory object.
        /// </summary>
        /// <value>The TMCategory</value>
        [DataMember]
        public BETMCategory oTMCategory { get; set; }

        /// <summary>
        /// Gets or sets the TMQuestionID.
        /// </summary>
        /// <value>The TMQuestionID.</value>
        [DataMember]
        public int iTMQuestionID
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the TMQTypeID.
        /// </summary>
        /// <value>The TMQTypeID.</value>
        [DataMember]
        public int iTMQTypeID
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the TempQID.
        /// </summary>
        /// <value>The sTempQID</value>
        [DataMember]
        public string sTempQID
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the Label.
        /// </summary>
        /// <value>The Label</value>
        [DataMember]
        public string sLabel
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the weightage.
        /// </summary>
        /// <value>The weightage.</value>
        [DataMember]
        public string dbWeightage
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets a value indicating whether [fatal].
        /// </summary>
        /// <value><c>true</c> if [fatal]; otherwise, <c>false</c>.</value>
        [DataMember]
        public string bFatal
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets a value indicating whether [IsAudit].
        /// </summary>
        /// <value><c>true</c> if [IsAudit]; otherwise, <c>false</c>.</value>
        [DataMember]
        public string bIsAudit
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the TMQOption List.
        /// </summary>
        /// <value>The TMQOption List.</value>
        [DataMember]
        public List<BETMQOption> lTMQOption
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the state of the o row.
        /// </summary>
        /// <value>The state of the o row.</value>
        [DataMember]
        public RowState oRowState
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets a value indicating whether [b CTC].
        /// </summary>
        /// <value><c>true</c> if [b CTC]; otherwise, <c>false</c>.</value>
        [DataMember]
        public string bCTC
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [b compliance].
        /// </summary>
        /// <value><c>true</c> if [b compliance]; otherwise, <c>false</c>.</value>
        [DataMember]
        public string bCompliance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [b dis qualifier].
        /// </summary>
        /// <value><c>true</c> if [b dis qualifier]; otherwise, <c>false</c>.</value>
        [DataMember]
        public string bDisQualifier { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [b non scoring].
        /// </summary>
        /// <value><c>true</c> if [b non scoring]; otherwise, <c>false</c>.</value>
        [DataMember]
        public string bNonScoring { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [b ER sable].
        /// </summary>
        /// <value><c>true</c> if [b ER sable]; otherwise, <c>false</c>.</value>
        [DataMember]
        public string bERSable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [b non fatal].
        /// </summary>
        /// <value><c>true</c> if [b non fatal]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bNonFatal { get; set; }

        /// <summary>
        /// Weightage IsEnabled
        /// </summary>
        [DataMember]
        public string Weightage_IsEnabled { get; set; }

        /// <summary>
        /// Serial Number
        /// </summary>
        [DataMember]
        public int SRN { get; set; }

        /// <summary>
        /// Question Info
        /// </summary>
        [DataMember]
        public string Info { get; set; }

        [DataMember]
        public string TXT2 { get; set; }
        [DataMember]
        public string TXT3 { get; set; }
        [DataMember]
        public string TXT4 { get; set; }
        [DataMember]
        public string TXT5 { get; set; }

        [DataMember]
        public string TXT6 { get; set; }
        [DataMember]
        public string TXT7 { get; set; }
        [DataMember]
        public string TXT8 { get; set; }
        [DataMember]
        public string TXT9 { get; set; }
        [DataMember]
        public string TXT10 { get; set; }
        [DataMember]
        public string TXT11 { get; set; }
        [DataMember]
        public string TXT12 { get; set; }
        [DataMember]
        public string TXT13 { get; set; }
        [DataMember]
        public string TXT14 { get; set; }
        [DataMember]
        public string TXT15 { get; set; }
        [DataMember]
        public string TXT1 { get; set; }
        [DataMember]
        public string UActionFlag { get; set; }
        [DataMember]
        public string UserContactID { get; set; }

        public string UserContactName { get; set; }

        public string UserRoleID { get; set; }
        public string UserActionSaveID { get; set; }

        public int CheckID { get; set; }
        public int ClientID { get; set; }
        public int ProcessID { get; set; }

        public int CheckListID { get; set; }
        public string MonthYear { get; set; }

        public int ConsolidatedMasterID { get; set; }
        public string UserStatusID { get; set; }

        public string ST1ID { get; set; }

        public string CloseComment { get; set; }

        public string sDueDate { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            oTMCategory = null;
            lTMQOption = null;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BETMQuestion"/> class.
        /// </summary>

        public BEPACDetails()
        { oTMCategory = new BETMCategory(); lTMQOption = new List<BETMQOption>(); }


        #endregion

    }

}
