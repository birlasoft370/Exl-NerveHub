using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.Config
{
    [Serializable]
    public class BEProcessShiftWindow : ObjectBase
    {
        private int _iProcessShiftID;
        private int _iClientID;
        private int _iProcessID;
        private int _iLocationID;
        private string _sStartTime;
        private string _sEndTime;
        private string _AdProdHrs;
        private string _sProcessName;

        /// <summary>
        /// Initializes a new instance of the <see cref="BEProcessShiftWindow"/> class.
        /// </summary>
        public BEProcessShiftWindow()
        { }


        /// <summary>
        /// Initializes a new instance of the <see cref="BEProcessShiftWindow"/> class.
        /// </summary>
        /// <param name="ClientID">The client ID.</param>
        /// <param name="ProcessShiftID">The process shift ID.</param>
        /// <param name="ProcessID">The process ID.</param>
        /// <param name="LocationID">The location ID.</param>
        /// <param name="sStartTime">The s start time.</param>
        /// <param name="sEndTime">The s end time.</param>
        /// <param name="AddProdHrs">The add prod HRS.</param>
        /// <param name="iCreatedBy">The i created by.</param>
        /// <param name="Disabled">if set to <c>true</c> [disabled].</param>
        public BEProcessShiftWindow(int ClientID, int ProcessShiftID, int ProcessID, int LocationID, string sStartTime, string sEndTime, string AddProdHrs, int iCreatedBy, Boolean Disabled)
        {
            _iClientID = ClientID;
            _iProcessShiftID = ProcessShiftID;
            _iProcessID = ProcessID;
            _iLocationID = LocationID;
            _sStartTime = sStartTime;
            _sEndTime = sEndTime;
            _AdProdHrs = AddProdHrs;
            base.bDisabled = Disabled;
            base.iCreatedBy = iCreatedBy;
        }


        /// <summary>
        /// Gets or sets  shift ID.
        /// </summary>
        /// <value> shift ID.</value>
        public int iProcessShiftID
        {
            get
            {
                return _iProcessShiftID;
            }
            set
            {
                _iProcessShiftID = value;
            }
        }

        /// <summary>
        /// Gets or sets the i client ID.
        /// </summary>
        /// <value>The i client ID.</value>
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
        /// Gets or sets the i process ID.
        /// </summary>
        /// <value>The i process ID.</value>
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
        /// Gets or sets the i location ID.
        /// </summary>
        /// <value>The i location ID.</value>
        public int iLocationID
        {
            get
            {
                return _iLocationID;
            }
            set
            {
                _iLocationID = value;
            }
        }

        /// <summary>
        /// Gets or sets Process Name
        /// </summary>
        /// <value>The name of the process.</value>
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
        /// Gets or sets  start time.
        /// </summary>
        /// <value> start time.</value>
        public string sStartTime
        {
            get
            {
                return _sStartTime;
            }
            set
            {
                _sStartTime = value;
            }
        }
        /// <summary>
        /// Gets or sets  end time.
        /// </summary>
        /// <value> end time.</value>
        public string sEndTime
        {
            get
            {
                return _sEndTime;
            }
            set
            {
                _sEndTime = value;
            }
        }

        /// <summary>
        /// Gets or sets the ad prod HRS.
        /// </summary>
        /// <value>The ad prod HRS.</value>
        public string AdProdHrs
        {
            get { return _AdProdHrs; }
            set { _AdProdHrs = value; }
        }

        public double dLastBreakTime
        {
            get;
            set;
        }

        public double dTimeForBreakPolicyTotalSec
        {
            get;
            set;
        }
    }

    [Serializable]
    [DataContract]
    public class BEProcessWindow
    {
        [DataMember]
        public List<BEProcessConfigData> lstProcessConfigData { get; set; }
        [DataMember]
        public List<BEProcessBreakData> lstProcessBreakData { get; set; }
    }

    [Serializable]
    [DataContract]
    public class BEProcessConfigData
    {
        [DataMember]
        public int ProcessShiftConfigId { get; set; }

        [DataMember]
        public int ProcessId { get; set; }

        [DataMember]
        public string ProcessName { get; set; }

        [DataMember]
        public int LocationId { get; set; }

        [DataMember]
        public string StartTime { get; set; }

        [DataMember]
        public string EndTime { get; set; }

        [DataMember]
        public string MaxTimeForBreak { get; set; }

        [DataMember]
        public int BreakInterval { get; set; }

        [DataMember]
        public int TotalNoOfBreak { get; set; }

        [DataMember]
        public int TotalTimeOfAllBreak { get; set; }

        [DataMember]
        public bool Disabled { get; set; }

        [DataMember]
        public int ClientId { get; set; }

        [DataMember]
        public bool isforcebreak { get; set; }

        [DataMember]
        public int isforcebreakID { get; set; }

    }

    [Serializable]
    [DataContract]
    public class BEProcessBreakData
    {
        [DataMember]
        public int ProcessShiftConfigId { get; set; }

        [DataMember]
        public string BreakID { get; set; }

        [DataMember]
        public int BreakTime { get; set; }
    }
}
