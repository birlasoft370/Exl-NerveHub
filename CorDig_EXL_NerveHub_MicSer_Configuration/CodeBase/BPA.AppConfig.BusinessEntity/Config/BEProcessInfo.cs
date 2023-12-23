using BPA.AppConfig.BusinessEntity.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.Config
{
    [Serializable]
    [DataContract]
    public class BEProcessInfo : ObjectBase
    {
        #region Process Fields

        private int _iProcessID;
        private int _iFamilyProcessID;
        private int _iClientID;
        private string _sProcessName;
        private string _sProcessDescription;
        private int _iCalendarID;
        private int _iProcessType;
        private int _iProcessWorkType;
        private int _iSBUID;
        private string _sSBUName;
        private string _sScope;
        private DateTime _dProcessStartDate;
        private DateTime _dGoLiveDate;
        private DateTime _dProcessEndDate;
        private string _sCalenderName;
        private DateTime _dStabilizationStartDate;
        private DateTime _dStabilizationEndDate;
        private DateTime _dProductionStartDate;
        private DateTime _dProductionEndDate;
        #endregion Process Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="oProcessInfo"/> class.
        /// </summary>
        public BEProcessInfo()
        {
            oProcessSLA = new BEProcessSLA();
            lProcessFTE = new List<BEProcessFTE>();
            lProcessGroup = new List<BEProcessGroup>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="oProcessInfo"/> class.
        /// </summary>
        /// <param name="iProcessID">The process ID.</param>
        /// <param name="iClientID">The i client ID.</param>
        /// <param name="sProcessName">Name of the process.</param>
        /// <param name="sProcessDescription">The process description.</param>
        /// <param name="bDisabled">if set to <c>true</c> [ disabled].</param>
        /// <param name="iCreatedBy">The i created by.</param>
        public BEProcessInfo(int iProcessID, int iClientID, string sProcessName, string sProcessDescription, bool bDisabled, int iCreatedBy)
        {
            _iProcessID = iProcessID;
            _iClientID = iClientID;
            _sProcessName = sProcessName;
            _sProcessDescription = sProcessDescription;
            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;
        }

        #endregion Constructors

        #region Public Properties
        [DataMember]
        public string sCalenderName
        {
            get
            {
                return _sCalenderName;
            }
            set
            {
                _sCalenderName = value;
            }
        }

        /// <summary>
        /// Gets or sets Unique process ID
        /// </summary>
        /// <value>The process ID.</value>
        /// 
        [DataMember]
        public int iProcessID
        {
            get
            {
                return _iProcessID;
            }
            set
            {
                _iProcessID = value;
            }
        }

        /// <summary>
        /// Gets or sets the client ID.
        /// </summary>
        /// <value>client ID.</value>
        /// 
        [DataMember]
        public int iClientID
        {
            get
            {
                return _iClientID;
            }
            set
            {
                _iClientID = value;
            }
        }

        /// <summary>
        /// Gets or sets Process Name
        /// </summary>
        /// <value>The name of the process.</value>
        /// 
        [DataMember]
        public string sProcessName
        {
            get
            {
                return _sProcessName;
            }
            set
            {
                _sProcessName = value;
            }
        }

        /// <summary>
        /// Gets or sets process description.
        /// </summary>
        /// <value>The process description.</value>
        /// 
        [DataMember]
        public string sProcessDescription
        {
            get
            {
                return _sProcessDescription;
            }
            set
            {
                _sProcessDescription = value;
            }
        }

        /// <summary>
        /// Gets or sets Scope
        /// </summary>
        /// <value>Scope.</value>
        /// 
        [DataMember]
        public string sScope
        {
            get
            {
                return _sScope;
            }
            set
            {
                _sScope = value;
            }
        }

        /// <summary>
        /// Gets or sets the Calender ID.
        /// </summary>
        /// <value>CalendarID </value>
        /// 
        [DataMember]
        public int iCalendarID
        {
            get
            {
                return _iCalendarID;
            }
            set
            {
                _iCalendarID = value;
            }
        }

        /// <summary>
        /// Gets or sets the  SBU ID.
        /// </summary>
        /// <value>The  SBUID.</value>
        /// 
        [DataMember]
        public int iSBUID
        {
            get
            {
                return _iSBUID;
            }
            set
            {
                _iSBUID = value;
            }
        }

        /// <summary>
        /// Gets or sets the i family process ID.
        /// </summary>
        /// <value>The i family process ID.</value>
        /// 
        [DataMember]
        public int iFamilyProcessID
        {
            get
            {
                return _iFamilyProcessID;
            }
            set
            {
                _iFamilyProcessID = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the SBU.
        /// </summary>
        /// <value>The name of the SBU.</value>
        /// 
        [DataMember]
        public string sSBUName
        {
            get
            {
                return _sSBUName;
            }
            set
            {
                _sSBUName = value;
            }
        }

        /// <summary>
        /// Gets or sets the  Process Type.
        /// </summary>
        /// <value>The  Process Type.</value>
        /// 
        [DataMember]
        public int iProcessType
        {
            get
            {
                return _iProcessType;
            }
            set
            {
                _iProcessType = value;
            }
        }

        /// <summary>
        /// Gets or sets the  Process Work Type.
        /// </summary>
        /// <value>The  Process WorkType.</value>
        /// 
        [DataMember]
        public int iProcessWorkType
        {
            get
            {
                return _iProcessWorkType;
            }
            set
            {
                _iProcessWorkType = value;
            }
        }

        /// <summary>
        /// Gets or sets Process Start Date
        /// </summary>
        /// <value>The Process Start Date.</value>
        /// 
        [DataMember]
        public DateTime dProcessStartDate
        {
            get
            {
                return _dProcessStartDate;
            }
            set
            {
                _dProcessStartDate = value;
            }
        }

        /// <summary>
        /// Gets or sets Process End Date
        /// </summary>
        /// <value>The Process End Date.</value>
        /// 
        [DataMember]
        public DateTime dProcessEndDate
        {
            get
            {
                return _dProcessEndDate;
            }
            set
            {
                _dProcessEndDate = value;
            }
        }

        /// <summary>
        /// Gets or sets Go Live Date
        /// </summary>
        /// <value>The Go Live Date.</value>
        /// 
        [DataMember]
        public DateTime dGoLiveDate
        {
            get
            {
                return _dGoLiveDate;
            }
            set
            {
                _dGoLiveDate = value;
            }
        }

        /// <summary>
        /// Gets or sets the object process SLA.
        /// </summary>
        /// <value>The oobject process SLA.</value>
        /// 
        [DataMember]
        public BEProcessSLA oProcessSLA
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the process Group list.
        /// </summary>
        /// <value>The process Group list.</value>
        /// 
        [DataMember]
        public List<BEProcessGroup> lProcessGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the process FTE list.
        /// </summary>
        /// <value>The process FTE list.</value>
        /// 
        [DataMember]
        public List<BEProcessFTE> lProcessFTE
        {
            get;
            set;
        }

        #endregion Public Properties

        /// <summary>
        /// Gets or sets the d stabilization start date.
        /// </summary>
        /// <value>The d stabilization start date.</value>
        [DataMember]
        public DateTime dStabilizationStartDate
        {
            get { return _dStabilizationStartDate; }
            set { _dStabilizationStartDate = value; }
        }
        /// <summary>
        /// Gets or sets the d stabilization end date.
        /// </summary>
        /// <value>The d stabilization end date.</value>
        [DataMember]
        public DateTime dStabilizationEndDate
        {
            get { return _dStabilizationEndDate; }
            set { _dStabilizationEndDate = value; }
        }
        /// <summary>
        /// Gets or sets the d production start date.
        /// </summary>
        /// <value>The d production start date.</value>
        [DataMember]
        public DateTime dProductionStartDate
        {
            get { return _dProductionStartDate; }
            set { _dProductionStartDate = value; }
        }

        /// <summary>
        /// Gets or sets the d production end date.
        /// </summary>
        /// <value>The d production end date.</value>
        [DataMember]
        public DateTime dProductionEndDate
        {
            get { return _dProductionEndDate; }
            set { _dProductionEndDate = value; }
        }

        /// <summary>
        /// Gets or sets the i supervisor feeback traget per week.
        /// </summary>
        /// <value>The i supervisor feeback traget per week.</value>
        [DataMember]
        public int iSupervisorFeebackTragetPerWeek { get; set; }

        /// <summary>
        /// Gets or sets the i supervisor feeback traget frequency.
        /// </summary>
        /// <value>The i supervisor feeback traget frequency.</value>
        [DataMember]
        public int iSupervisorFeedbackTargetFrequency { get; set; }

        /// <summary>
        /// Gets or sets the i QCA feeback traget per week.
        /// </summary>
        /// <value>The i QCA feeback traget per week.</value>
        [DataMember]
        public int iQCAFeebackTragetPerWeek { get; set; }

        /// <summary>
        /// Gets or sets the i target audit per month.
        /// </summary>
        /// <value>The i target audit per month.</value>
        [DataMember]
        public int iTargetAuditPerMonth { get; set; }

        /// <summary>
        /// Gets or sets the i target QCA HRS.
        /// </summary>
        /// <value>The i target QCA HRS.</value>
        [DataMember]
        public int iTargetQCAHrs { get; set; }

        /// <summary>
        /// Gets or sets the i process complexity.
        /// </summary>
        /// <value>The i process complexity.</value>
        [DataMember]
        public int iProcessComplexity { get; set; }

        /// <summary>
        /// Gets or sets the process complexity.
        /// </summary>
        /// <value>The process complexity.</value>
        [DataMember]
        public string sProcessComplexity { get; set; }

        /// <summary>
        /// Gets or sets the type of the s process work.
        /// </summary>
        /// <value>The type of the s process work.</value>
        [DataMember]
        public string sProcessWorkType { get; set; }

        /// <summary>
        /// Gets or sets the state of the row.
        /// </summary>
        /// <value>The state of the row.</value>
        [DataMember]
        public RowState oRowState { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [b item checked].
        /// </summary>
        /// <value><c>true</c> if [b item checked]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bItemChecked { get; set; }

        /// <summary>
        /// Gets or sets the i approved access.
        /// </summary>
        /// <value>The i approved access.</value>
        [DataMember]
        public int iApprovedAccess { get; set; }

        /// <summary>
        /// Gets or sets the type of the i CAP.
        /// </summary>
        /// <value>The type of the i CAP.</value>
        [DataMember]
        public int iCAPType { get; set; }

        /// <summary>
        /// Gets or sets the s process owner.
        /// </summary>
        /// <value>The s process owner.</value>
        [DataMember]
        public string sProcessOwner { get; set; }

        /// <summary>
        /// Gets or sets the i approver.
        /// </summary>
        /// <value>The i approver.</value>
        [DataMember]
        public Int32 iApprover { get; set; }

        [DataMember]
        public bool bChecked { get; set; }

        [DataMember]
        public List<BEUserInfo> lstAppGrid { get; set; }

        [DataMember]
        public int iProcessMapID { get; set; }

        [DataMember]
        public string sPASProcessMonth { get; set; }
        [DataMember]
        public string sPASProcessType { get; set; }

        [DataMember]
        public string sPASProcessFlagActionType { get; set; }
        [DataMember]
        public string sPASProcessU_ActionType { get; set; }

    }
}
