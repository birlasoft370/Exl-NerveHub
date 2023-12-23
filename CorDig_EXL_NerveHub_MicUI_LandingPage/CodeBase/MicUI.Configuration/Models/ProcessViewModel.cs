using BPA.GlobalResources.UI.AppConfiguration;
using MicUI.Configuration.Services.ServiceModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MicUI.Configuration.Models
{
    [Serializable]
    public class ProcessViewModel
    {
        #region "Properties"

        public ProcessViewModel()
        {
            if (lProcessGroup == null)
                lProcessGroup = new List<BEProcessGroup>();
            if (lProcessFTE == null)
                lProcessFTE = new List<BEProcessFTE>();
            if (lBEProcessInfo == null)
                lBEProcessInfo = new List<BEProcessInfo>();
            lErpProcess = new List<BEERPProcess>();
        }

        [ScaffoldColumn(false)]
        public int ProcessID { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources_ClientInfo), ErrorMessageResourceName = "required_Client")]
        public int ClientId { get; set; }

        // [Required(ErrorMessageResourceType = typeof(Resources_ClientInfo), ErrorMessageResourceName = "required_Client")]
        [Display(ResourceType = typeof(Resources_ClientInfo), Name = "display_Client_Name")]
        public string ClientName { get; set; }

        // [Required(ErrorMessageResourceType = typeof(Resources_Process), ErrorMessageResourceName = "Req_ProcessName")]
        [Display(ResourceType = typeof(Resources_Process), Name = "Disp_ProcessName")]
        public string ProcessName { get; set; }

        [Display(ResourceType = typeof(Resources_Process), Name = "Disp_Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        //[Required(ErrorMessage = "Please specify Calender.")]
        [Display(ResourceType = typeof(Resources_Process), Name = "Disp_Calender")]
        public string Calendar { get; set; }

        public string sCalendarId { get; set; }

        [Display(ResourceType = typeof(Resources_Process), Name = "Req_ProcessType")]
        public string ProcessType { get; set; }
        public int ProcessTypeId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources_Process), ErrorMessageResourceName = "required_Voice_NonVoice_Blended")]
        [Display(ResourceType = typeof(Resources_Process), Name = "display_Voice_NonVoice_Blended")]
        public string ProcessWorkType { get; set; }
        public int ProcessWorkTypeId { get; set; }

        //[Required(ErrorMessage = "Please specify SBU Name")]
        //[Display(Name = "SBU Name:")]
        public int SBUID { get; set; }
        public string SBUName { get; set; }

        [Display(ResourceType = typeof(Resources_Process), Name = "Disp_Scope")]
        [DataType(DataType.MultilineText)]
        public string Scope { get; set; }


        [Display(ResourceType = typeof(Resources_Process), Name = "Disp_StabilizationStartDate")]
        public DateTime StabilizationStartDate { get; set; }

        [Display(ResourceType = typeof(Resources_Process), Name = "Disp_StabilizationEndDate")]
        public DateTime StabilizationEndDate { get; set; }

        [Display(ResourceType = typeof(Resources_Process), Name = "Disp_PilotStartDate")]
        public DateTime PilotStartDate { get; set; }

        [Display(ResourceType = typeof(Resources_Process), Name = "Disp_PilotEndDate")]
        public DateTime PilotEndDate { get; set; }

        [Display(ResourceType = typeof(Resources_Process), Name = "Disp_ProductionEndDate")]
        public DateTime ProductionStartDate { get; set; }

        [Display(ResourceType = typeof(Resources_Process), Name = "Disp_ProductionStartDate")]
        public DateTime ProductionEndDate { get; set; }

        [Display(ResourceType = typeof(Resources_Process), Name = "Disp_FrequencyFeedbackSupervisor")]
        public int FrequencyFeedbackSupervisor { get; set; }

        public int FrequencyFeedbackSupervisorId { get; set; }

        [Display(ResourceType = typeof(Resources_Process), Name = "Disp_FrequencyFeedbackSupervisor")]
        public string FrequencyFeedbacksupervisor1 { get; set; }

        public int SupervisorFeebackTragetPerWeek { get; set; }

        [Display(ResourceType = typeof(Resources_Process), Name = "Disp_FrequencyQCA")]
        public int FrequencyQCA { get; set; }

        [Display(ResourceType = typeof(Resources_Process), Name = "display_TargetSidebySideAuditPerMonth")]
        public int TargetSidePerMonth { get; set; }

        [Display(ResourceType = typeof(Resources_Process), Name = "display_Agent")]
        public string Agent { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources_Process), ErrorMessageResourceName = "required_PleaseSpecifyTargetQCAHrs")]
        [Display(ResourceType = typeof(Resources_Process), Name = "Disp_TargetQCAPerMonth")]
        public int TargetQCAPerMonth { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources_Process), ErrorMessageResourceName = "Req_ProcessComplexity")]
        [Display(ResourceType = typeof(Resources_Process), Name = "Disp_ProcessComplexity")]
        public string ProcessComplexity { get; set; }

        public int ProcessComplexityId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources_Process), ErrorMessageResourceName = "required_Please_Specify_ERS_CAP_Type")]
        [Display(ResourceType = typeof(Resources_Process), Name = "Disp_ERSCAPType")]
        public string ERSCAPType { get; set; }

        public int ERSCAPTypeId { get; set; }

        [Display(ResourceType = typeof(Resources_Process), Name = "Disp_Disable")]
        public bool? Disable { get; set; }

        public IList<BEProcessGroup> lProcessGroup { get; set; }

        public IList<BEProcessFTE> lProcessFTE { get; set; }

        public List<BEProcessInfo> lBEProcessInfo { get; set; }

        [Display(ResourceType = typeof(Resources_Process), Name = "Disp_SLAStartDate")]
        public DateTime SLAStartDate { get; set; }

        [Display(ResourceType = typeof(Resources_Process), Name = "Disp_SLAEndDate")]
        public DateTime SLAEndDate { get; set; }

        [Display(ResourceType = typeof(Resources_Process), Name = "Disp_ProcessSLA")]
        public string ProcessSLA { get; set; }

        public int ProcessSLAId { get; set; }

        //For Erp Control
        [Display(ResourceType = typeof(Resources_Process), Name = "Disp_ERPProcess")]
        public string ERPProcess { get; set; }

        [Display(ResourceType = typeof(Resources_Process), Name = "Disp_Location")]
        public string Location { get; set; }

        public IList<BEERPProcess> lErpProcess { get; set; }

        public IList<BELocation> lLocation { get; set; }
        #endregion

        public int TargetAuditPerMonth { get; set; }


        public string sPASProcessMonth { get; set; }

        public string sTanatName { get; set; }
        public string sTanatNameWeb { get; set; }

        public string sPASProcessFlagActionType { get; set; }
        public string sPASProcess_U_ActionType { get; set; }

        public string PASProcessType { get; set; }
    }

    [Serializable]
    [DataContract]
    public class BELocation : ObjectBase
    {
        #region Field
        private int _iLocationID;
        private string _sLocationName;
        private string _sLocationDescription;

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="oLocation"/> class.
        /// </summary>
        public BELocation()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="oLocationInfo"/> class.
        /// </summary>
        /// <param name="iLocationID">The Location ID.</param>
        /// <param name="sLocationName">Name of the Location.</param>
        /// <param name="sLocationDescription">The Location description.</param>
        /// <param name="bDisabled">if set to <c>true</c> [disabled].</param>
        /// <param name="iCreatedBy">The i created by.</param>
        public BELocation(int iLocationID, string sLocationName, string sLocationDescription, bool bDisabled, int iCreatedBy)
        {
            _iLocationID = iLocationID;
            _sLocationName = sLocationName;
            _sLocationDescription = sLocationDescription;
            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the Location ID.
        /// </summary>
        /// <value>The Location ID.</value>
        [DataMember]
        public int iLocationID
        {
            get { return _iLocationID; }
            set { _iLocationID = value; }
        }

        /// <summary>
        /// Gets or sets the name of the Location.
        /// </summary>
        /// <value>The name of the Location.</value>
        [DataMember]
        public string sLocationName
        {
            get { return _sLocationName; }
            set { _sLocationName = value; }
        }

        /// <summary>
        /// Gets or sets the Location description.
        /// </summary>
        /// <value>The Location description.</value>
        [DataMember]
        public string sLocationDescription
        {
            get { return _sLocationDescription; }
            set { _sLocationDescription = value; }
        }
        #endregion
    }

    [Serializable]
    public class BEProcessFTE : ObjectBase
    {
        #region Member Fields
        private int _iProcessFTEID;
        //private int _iFTE;
        //private int _iQCACount;
        private float _iFTE;
        private float _iQCACount;
        private DateTime _dtEffectiveEndDate;
        private DateTime _dtEffectiveStartDate;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BEProcessFTEMaster"/> class.
        /// </summary>
        public BEProcessFTE()
        { oLocation = new BELocation(); }

        /// <summary>
        /// Initializes a new instance of the <see cref="BEProcessFTEMaster"/> class.
        /// </summary>
        /// <param name="iProcessFTEID">The ERPProcessID.</param>
        /// <param name="iProcessID">ProcessID</param>
        /// <param name="iLocationId">LocationId</param>
        /// <param name="iFTE">FTE.</param>
        /// <param name="iQCACount">QCA count.</param>
        /// <param name="bDisabled">if set to <c>true</c> [disabled].</param>
        /// <param name="iCreatedBy">created by.</param>
        public BEProcessFTE(int iProcessFTEID, int iProcessID, int iLocationId, float iFTE, float iQCACount, bool bDisabled, int iCreatedBy)
        {
            _iProcessFTEID = iProcessFTEID;
            _iFTE = iFTE;
            _iQCACount = iQCACount;
            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the ERPProcessID.
        /// </summary>
        /// <value>The ERPProcessID.</value>
        public int iProcessFTEID
        {
            get
            {
                return _iProcessFTEID;
            }
            set
            {
                _iProcessFTEID = value;
            }
        }


        /// <summary>
        /// Gets or sets the i FTE.
        /// </summary>
        /// <value>The i FTE.</value>
        public float iFTE
        {
            get
            {
                return _iFTE;
            }
            set
            {
                _iFTE = value;
            }
        }


        /// <summary>
        /// Gets or sets the i QCA count.
        /// </summary>
        /// <value>The i QCA count.</value>
        public float iQCACount
        {
            get
            {
                return _iQCACount;
            }
            set
            {
                _iQCACount = value;
            }
        }

        /// <summary>
        /// Gets or sets the location object
        /// </summary>
        /// <value>location object.</value>
        public BELocation oLocation { get; set; }

        /// <summary>
        /// Gets or sets the state of the row.
        /// </summary>
        /// <value>The state of the row.</value>
        public RowState oRowState { get; set; }

        /// <summary>
        /// Gets or sets the dt effective start date.
        /// </summary>
        /// <value>The dt effective start date.</value>
        public DateTime dtEffectiveStartDate
        {
            get { return _dtEffectiveStartDate; }
            set { _dtEffectiveStartDate = value; }
        }
        /// <summary>
        /// Gets or sets the dt effective end date.
        /// </summary>
        /// <value>The dt effective end date.</value>
        public DateTime dtEffectiveEndDate
        {
            get { return _dtEffectiveEndDate; }
            set { _dtEffectiveEndDate = value; }
        }
        #endregion
    }
}
