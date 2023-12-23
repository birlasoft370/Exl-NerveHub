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
    public class BECampaignInfo : ObjectBase
    {
        private DateTime _dtEndDate;

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="oClientInfo"/> class.
        /// </summary>
        public BECampaignInfo()
        { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the client ID.
        /// </summary>
        /// <value>The client ID.</value>
        [DataMember]
        public int iCampaignID { get; set; }

        /// <summary>
        /// Gets or sets the process ID.
        /// </summary>
        /// <value>process ID.</value>
        [DataMember]
        public int iProcessID { get; set; }

        /// <summary>
        /// Gets or sets the TimeZone ID.
        /// </summary>
        /// <value>TimeZone ID.</value>
        [DataMember]
        public int iTimeZoneID { get; set; }

        /// <summary>
        /// Gets or sets the name of the client.
        /// </summary>
        /// <value>The name of the client.</value>
        [DataMember]
        public string sCampaignName { get; set; }

        /// <summary>
        /// Gets or sets the clinet description.
        /// </summary>
        /// <value>The clinet description.</value>
        [DataMember]
        public string sCampaignDescription { get; set; }

        /// <summary>
        /// Gets or sets the dt end date.
        /// </summary>
        /// <value>The dt end date.</value>
        [DataMember]
        public DateTime dtEndDate
        {
            get { return _dtEndDate; }
            set { _dtEndDate = value; }
        }

        /// <summary>
        /// Gets or sets the skill Data.
        /// </summary>
        /// <value>The skill.</value>
        [DataMember]
        public IList<BESkillInfo> iSkillID { get; set; }

        /// <summary>
        /// Gets or sets the i client ID.
        /// </summary>
        /// <value>The i client ID.</value>
        [DataMember]
        public int iClientID { get; set; }

        /// <summary>
        /// Gets or sets the i approved access.
        /// </summary>
        /// <value>The i approved access.</value>
        [DataMember]
        public int iApprovedAccess { get; set; }
        /// <summary>
        /// Gest or sets the free field entry
        /// </summary>
        [DataMember]
        public bool bFreeField { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int iModeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string sModeName { get; set; }

        [DataMember]
        public string sModeIds { get; set; }

        [DataMember]
        public string sLocations { get; set; }

        [DataMember]
        public string sShiftwindows { get; set; }

        [DataMember]
        public bool bPurposesWM { get; set; }

        [DataMember]
        public bool bPurposeTime { get; set; }

        [DataMember]
        public bool bPurposeTrans { get; set; }

        [DataMember]
        public string sBusinessJustifications { get; set; }

        [DataMember]
        public string sTargetq1 { get; set; }

        [DataMember]
        public string sTargetq2 { get; set; }
        [DataMember]
        public string sTargetq3 { get; set; }

        [DataMember]
        public string sTargety1 { get; set; }
        [DataMember]
        public string sTargety2 { get; set; }
        [DataMember]
        public string sTargety3 { get; set; }

        [DataMember]
        public string sKeyBenefits { get; set; }

        [DataMember]
        public int iBuisnessID { get; set; }

        [DataMember]
        public int iTechID { get; set; }

        [DataMember]
        public string sStatus { get; set; }

        [DataMember]
        public int iApprovalId { get; set; }


        [DataMember]
        public string sEmail { get; set; }

        [DataMember]
        public int iThresholdForCompletion { get; set; }

        [DataMember]
        public int iThresholdForToOpen { get; set; }

        [DataMember]
        public Double dTargetEfficiency { get; set; }
        [DataMember]
        public bool bBillingSystem { get; set; }
        #endregion
    }
    [Serializable]
    [DataContract]
    public class BEBOTDowntimeInfo : ObjectBase
    {
        [DataMember]
        public int iDowntimeReasonId { get; set; }
        [DataMember]
        public string sDowntimeReason { get; set; }

        [DataMember]
        public int iCaptureId { get; set; }
        [DataMember]
        public string sCampaignName { get; set; }
        [DataMember]
        public int iCampaignID { get; set; }

        [DataMember]
        public string sComment { get; set; }
        [DataMember]
        public int iProcessID { get; set; }

        [DataMember]
        public int iClientID { get; set; }

        [DataMember]
        public string sModeIds { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime EndDate { get; set; }
    }
}
