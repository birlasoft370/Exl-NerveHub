using BPA.AppConfig.BusinessEntity.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.Security
{
    [Serializable]
    [DataContract]
    public class BEUserInfo : ObjectBase
    {
        private DateTime _dDOJ;
        public BEUserInfo()
        {
            oRoles = new List<BERoleInfo>();
            oLocalizationInfo = new BELocalization();
        }

        /// <summary>
        /// Gets or sets the s role campaign.
        /// </summary>
        /// <value>The s role campaign.</value>
        [DataMember]
        public string sRoleCampaign { get; set; }

        /// <summary>
        /// Gets or sets the i user facility.
        /// </summary>
        /// <value>The i user facility.</value>
        [DataMember]
        public int iUserFacility { get; set; }

        //Vipul
        [DataMember]
        public IList<BELanguages> oLanguage { get; set; }
        /// <summary>
        /// Gets or sets the DST role campaign.
        /// </summary>
        /// <value>The DST role campaign.</value>
        [DataMember]
        public DataSet dstRoleCampaign { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>The user ID.</value>
        [DataMember]
        public Int32 iUserID { get; set; }
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        /// <value>The user Name.</value>
        [DataMember]
        public string sUserName { get; set; }

        /// <summary>
        /// Gets or sets the employee ID.
        /// </summary>
        /// <value>The employee ID.</value>
        [DataMember]
        public int iEmployeeID { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>        
        public string Name
        {
            get { return bDisabled ? sFirstName + " " + sMiddleName + " " + sLastName + " ( " + sLoginName + " )(Disabled)" : sFirstName + (String.IsNullOrEmpty(sMiddleName) ? "" : " " + sMiddleName) + " " + sLastName + " ( " + sLoginName + " )"; }
        }

        /// <summary>
        /// Gets or sets the name of the first.
        /// </summary>
        /// <value>The name of the first.</value>
        [DataMember]
        public string sFirstName { get; set; }

        /// <summary>
        /// Gets or sets the name of the middle.
        /// </summary>
        /// <value>The name of the middle.</value>
        [DataMember]
        public string sMiddleName { get; set; }

        /// <summary>
        /// Gets or sets the name of the last.
        /// </summary>
        /// <value>The name of the last.</value>
        [DataMember]
        public string sLastName { get; set; }

        /// <summary>
        /// Gets or sets the name of the login.
        /// </summary>
        /// <value>The name of the login.</value>
        [DataMember]
        public string sLoginName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>

        [DataMember]
        public string sPassword { get; set; }

        /// <summary>
        /// Gets or sets the user level.
        /// </summary>
        /// <value>The user level.</value>
        [DataMember]
        public int iUserLevel { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>The roles.</value>
        [DataMember]
        public IList<BERoleInfo> oRoles { get; set; }

        /// <summary>
        /// Gets or sets the campaign.
        /// </summary>
        /// <value>The campaign.</value>
        [DataMember]
        public IList<BECampaignInfo> oCampaign { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [lan ID].
        /// </summary>
        /// <value><c>true</c> if [lan ID]; otherwise, <c>false</c>.</value>

        [DataMember]
        public bool bLanID { get; set; }

        [DataMember]
        public bool bClientUser { get; set; }

        /// <summary>
        /// Gets or sets the job desc.
        /// </summary>
        /// <value>The job desc.</value>
        [DataMember]
        public string sJobDesc { get; set; }

        /// <summary>
        /// Gets or sets the job ID.
        /// </summary>
        /// <value>The job ID.</value>
        [DataMember]
        public int iJobID { get; set; }

        /// <summary>
        /// Gets or sets the supervisor ID.
        /// </summary>
        /// <value>The supervisor ID.</value>
        [DataMember]
        public int iSupervisorID { get; set; }

        /// <summary>
        /// Gets or sets the name of the supervisor.
        /// </summary>
        /// <value>The name of the supervisor.</value>
        [DataMember]
        public string sSupervisorName { get; set; }

        /// <summary>
        /// Gets or sets the s email.
        /// </summary>
        /// <value>The s email.</value>
        [DataMember]
        public string sEmail { get; set; }
        /// <summary>
        /// Gets or sets the i client ID.
        /// </summary>
        /// <value>The i client ID.</value>
        [DataMember]
        public int iClientID { get; set; }
        /// <summary>
        /// Gets or sets the s process.
        /// </summary>
        /// <value>The s process.</value>
        [DataMember]
        public string sProcess { get; set; }
        [DataMember]
        public string sDeletedProcess { get; set; }

        [DataMember]
        public string sInvalidReason { get; set; }

        /// <summary>
        /// Gets or sets the i role approver.
        /// </summary>
        /// <value>The i role approver.</value>
        [DataMember]
        public int iRoleApprover { get; set; }

        [DataMember]
        public DateTime dDOJ
        {
            get { return _dDOJ; }
            set { _dDOJ = value; }
        }

        [DataMember]
        public int iFacilityId { get; set; }

        [DataMember]
        public int iLOBID { get; set; } //added by Omkar

        [DataMember]
        public int iSBUID { get; set; } //added by Omkar
        [DataMember]
        public int iJobCodeID { get; set; }
        /// <summary>
        /// Gets or sets the job ID.
        /// </summary>
        /// <value>The job ID.</value>
        [DataMember]
        public string sName { get; set; }


        [DataMember]
        public bool bIsBot { get; set; }

        [DataMember]
        public string sTime { get; set; }

        [DataMember]
        public bool sDisabled { get; set; }

        [DataMember]
        public bool bChecked
        {
            get;
            set;
        }



        [DataMember]
        public int iApprover
        {
            get;
            set;

        }
        [DataMember]
        public string sAgentName
        {
            get;
            set;

        }
        [DataMember]
        public BELocalization oLocalizationInfo
        {
            get;
            set;
        }

        /// <summary>
        /// sTimeZone
        /// </summary>
        [DataMember]
        public string sUserTimeZone { get; set; }


        /// <summary>
        /// sTimeZone
        /// </summary>
        [DataMember]
        public string sServerTimeZone { get; set; }

        /// <summary>
        /// iTimeZoneID
        /// </summary>
        [DataMember]
        public int iTimeZoneID { get; set; }

        /// <summary>
        /// sLanguage
        /// </summary>
        [DataMember]
        public string sLanguage { get; set; }


        [DataMember]
        public IList<BERoleInfo> lstJobDes { get; set; }
    }
}
