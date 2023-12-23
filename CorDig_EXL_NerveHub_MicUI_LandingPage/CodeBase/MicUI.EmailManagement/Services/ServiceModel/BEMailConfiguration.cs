using System.Data;
using System.Runtime.Serialization;

namespace MicUI.EmailManagement.Services.ServiceModel
{
    [Serializable]
    [DataContract]
    public class BEMailConfiguration : ObjectBase
    {
        /// <summary>
        /// Gets or sets the store ID.
        /// </summary>
        /// <value>The store ID.</value>
        [DataMember]
        public int iStoreID { get; set; }

        /// <summary>
        /// Gets or sets the mail config ID.
        /// </summary>
        /// <value>The mail config ID.</value>
        [DataMember]
        public int iMailConfigID { get; set; }

        /// <summary>
        /// Gets or sets the Campaign ID.
        /// </summary>
        /// <value>The Campaign ID.</value>
        [DataMember]
        public int iCampaignID { get; set; }

        /// <summary>
        /// Gets or sets the name of the mail box.
        /// </summary>
        /// <value>The name of the mail box.</value>
        [DataMember]
        public string sMailBoxName { get; set; }

        [DataMember]
        public string sLotusDomainName { get; set; }

        [DataMember]
        public string sLotusDomainPrefix { get; set; }

        /// <summary>
        /// Gets or sets the use service credential to pull Mail.
        /// </summary>
        /// <value>The use service credential to pull Mail.</value>
        [DataMember]
        public bool bUseServiceCredentialToPull { get; set; }

        /// <summary>
        /// Gets or sets the use user credential to send Mail.
        /// </summary>
        /// <value>The use user credential to send Mail.</value>
        [DataMember]
        public bool bUseUserCredentialToSend { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>The user ID.</value>
        [DataMember]
        public string sUserID { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [DataMember]
        public string sPassword { get; set; }

        /// <summary>
        /// Gets or sets the email ID.
        /// </summary>
        /// <value>The email ID.</value>
        [DataMember]
        public string sEmailID { get; set; }

        /// <summary>
        /// Gets or sets the user credential.
        /// </summary>
        /// <value>The user credential.</value>
        [DataMember]
        public UserCredential oUserCredential { get; set; }

        /// <summary>
        /// Gets or sets the mail server type ID.
        /// </summary>
        /// <value>The mail server type ID.</value>
        [DataMember]
        public EmailServerType iMailServerTypeID { get; set; }

        /// <summary>
        /// Gets or sets the schedule interval.
        /// </summary>
        /// <value>The schedule interval.</value>
        [DataMember]
        public int iScheduleInterval { get; set; }

        /// <summary>
        /// Gets or sets the is running.
        /// </summary>
        /// <value>The is running.</value>
        [DataMember]
        public bool isRunning { get; set; }

        /// <summary>
        /// Gets or sets the type of the mail box.
        /// </summary>
        /// <value>The type of the mail box.</value>
        [DataMember]
        public emailType MailBoxType { get; set; }

        /// <summary>
        /// Gets or sets the s auto discovery path for Exchange server.
        /// </summary>
        /// <value>The auto discovery path for Exchange server.</value>
        [DataMember]
        public string sAutoDiscoveryPath { get; set; }

        /// <summary>
        /// Gets or sets the Dominos server path.
        /// </summary>
        /// <value>The Dominos server path.</value>
        [DataMember]
        public string sLotusServerPath { get; set; }

        /// <summary>
        /// Gets or sets the Dominos NFS file path.
        /// </summary>
        /// <value>The Dominos NFS file path.</value>
        [DataMember]
        public string sNFSFilePath { get; set; }

        /// <summary>
        /// Gets or sets the web enabled.
        /// </summary>
        /// <value>The web enabled.</value>
        [DataMember]
        public bool bWebEnabled { get; set; }

        /// <summary>
        /// Gets or sets the EMS web server hosting.
        /// </summary>
        /// <value>The EMS web server hosting.</value>
        [DataMember]
        public bool bEMSWebServerHosting { get; set; }

        /// <summary>
        /// Gets or sets the EMS web server URL.
        /// </summary>
        /// <value>The EMS web server URL.</value>
        [DataMember]
        public string EMSWebServerURL { get; set; }

        /// <summary>
        /// Gets or sets the is password expire.
        /// </summary>
        /// <value>The is password expire.</value>
        [DataMember]
        public bool isPasswordExpire { get; set; }

        /// <summary>
        /// Gets or sets the auto reply.
        /// </summary>
        /// <value>The auto reply.</value>
        [DataMember]
        public bool AutoReply { get; set; }


        /// <summary>
        /// Gets or sets the mail template ID.
        /// </summary>
        /// <value>The mail template ID.</value>
        [DataMember]
        public int MailTemplateID { get; set; }

        /// <summary>
        /// Gets or sets the mail template ID.
        /// </summary>
        /// <value>The mail template ID.</value>
        [DataMember]
        public IList<BEMailTemplate> oMailTemplate { get; set; }

        [DataMember]
        public IList<MailTemplateImage> oMailTemplateImage { get; set; }
        /// <summary>
        /// Gets or sets the pooling value.
        /// </summary>
        /// <value>The pooling value.</value>
        [DataMember]
        public bool PoolingValue { get; set; }

        [DataMember]
        public bool IsReadMail { get; set; }

        /// <summary>
        /// Gets or sets the type of the folder.
        /// </summary>
        /// <value>The type of the  folder.</value>
        [DataMember]
        public int iFolderType { get; set; }

        /// <summary>
        /// Gets or sets the current timer.
        /// </summary>
        /// <value>The current timer.</value>
        [DataMember]
        public int icurrentTimer { get; set; }

        /// <summary>
        /// Gets or sets the mailfolderdetails Object.
        /// </summary>
        /// <value>The mailfolderdetails Object.</value>
        [DataMember]
        public IList<BEMailfolderdetails> oMailfolderdetails { get; set; }



        /// <summary>
        /// Gets or sets the  mailfolderdetails.
        /// </summary>
        /// <value>The  mailfolderdetails.</value>
        [DataMember]
        public DataTable dtMailfolderdetails { get; set; }

        /// <summary>
        /// Gets or sets the tenant Object.
        /// </summary>
        /// <value>The tenant Object.</value>
        [DataMember]
        public BETenantInfo oTenant { get; set; }

        /// <summary>
        /// Gets or sets the time zone Object.
        /// </summary>
        /// <value>The time zone Object.</value>
        [DataMember]
        public BETimeZoneInfo oTimeZone { get; set; }

        /// <summary>
        /// Gets or sets the server time zone.
        /// </summary>
        /// <value>The server time zone.</value>
        [DataMember]
        public string sServerTimeZone { get; set; }

        /// <summary>
        /// Gets or sets the sendmail quique identified.
        /// </summary>
        /// <value>The sendmail quique identified.</value>
        [DataMember]
        public bool bSendmailQuiqueIdentified { get; set; }

        /// <summary>
        /// Gets or sets the b scheduleto same user.
        /// </summary>
        /// <value>The b scheduleto same user.</value>
        [DataMember]
        public bool bScheduletoSameUser { get; set; }

        /// <summary>
        /// Gets or sets the inline editing.
        /// </summary>
        /// <value>The inline editing.</value>
        [DataMember]
        public bool bInlineEditing { get; set; }

        /// <summary>
        /// Gets or sets the batch frequency.
        /// </summary>
        /// <value>The batch frequency.</value>
        [DataMember]
        public BatchFrequencyType BatchFrequency { get; set; }

        /// <summary>
        /// Gets or sets the exception count.
        /// </summary>
        /// <value>The exception count.</value>
        [DataMember]
        public int ExceptionCount { get; set; }

        /// <summary>
        /// Gets or sets the Campaign Additional fields.
        /// </summary>
        /// <value>The Campaign Additional fields.</value>
        [DataMember]
        public IList<BEMailCampaignField> oCampaignAdditionFields { get; set; }

        //Vipul Changes
        /// <summary>
        /// Gets or Sets the BCC value
        /// </summary>
        [DataMember]
        public bool bBCCEnabled { get; set; }

        [DataMember]
        public bool bOutLookEnabled { get; set; }

        [DataMember]
        public bool bTranslationEnabled { get; set; }

        [DataMember]
        public bool beFileEnabled { get; set; }

        [DataMember]
        public bool bOutLookMailEnabled { get; set; }


        [DataMember]
        public bool bNeedPrintEnabled { get; set; }
        [DataMember]
        public bool bReadMailBodyEnabled { get; set; }
        [DataMember]
        public bool bCFXEnabled { get; set; }
        [DataMember]
        public bool bDuringUploadEnabled { get; set; }

        [DataMember]
        public string sAssignType { get; set; }

        [DataMember]
        public bool IsAssignLast { get; set; }

        [DataMember]
        public bool bNeedTicketEnabled { get; set; }


        [DataMember]
        public int iUploadBy { get; set; }
        [DataMember]
        public bool bSensitivityEnabled { get; set; }
        [DataMember]
        public string strCEXlauncherPath { get; set; }

        [DataMember]
        public bool bSubmitDisplayEnabled { get; set; }

        [DataMember]
        public string strEFilePath { get; set; }

        [DataMember]
        public string strSubmitDisplay { get; set; }
        [DataMember]
        public int iNeedTicketLenth { get; set; }
        [DataMember]
        public string strTicketName { get; set; }
        [DataMember]
        public bool bFreshRequiredEnabled { get; set; }


        [DataMember]
        public bool bOutofOfficeEnabled { get; set; }

        [DataMember]
        public string sOutofOffice { get; set; }

        [DataMember]
        public bool bImpersonation { get; set; }

        [DataMember]
        public ImpersonationIDType sImpersonationIDType { get; set; }

        [DataMember]
        public string sImpersonationID { get; set; }


        [DataMember]
        public string ClinetID { get; set; }
        [DataMember]
        public string TenentID { get; set; }
        [DataMember]
        public string Scope { get; set; }
        [DataMember]
        public string RedirectUrl { get; set; }
        [DataMember]
        public string Instance { get; set; }
        [DataMember]
        public bool IsForSWMIntegration { get; set; }

        public bool bSWMEMSIntegration { get; set; }


        [DataMember]
        public string Authority { get; set; }




    }

    [Serializable]
    [DataContract]
    public class UserCredential
    {
        /// <summary>
        /// Gets or sets the user LANID.
        /// </summary>
        /// <value>The user LANID.</value>
        [DataMember]
        public string sUserLANID { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [DataMember]
        public string sPassword { get; set; }

        /// <summary>
        /// Gets or sets the email ID.
        /// </summary>
        /// <value>The email ID.</value>
        [DataMember]
        public string sEmailID { get; set; }

    }
}
