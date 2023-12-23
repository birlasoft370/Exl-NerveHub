using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Config;
using BPA.EmailManagement.DataLayer.ExternalRef;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace BPA.EmailManagement.DataLayer
{
    /// <summary>
    /// Mail Configuration Class for Database Related Operation 
    /// </summary>
    public sealed class DLMailConfiguration : IDisposable
    {
        #region Declare & Initialize Constants
        private BETenant _oTenant = null;

        private const string SQL_SP_BY_CAMPAIGNID = @"EMS.USP_GetMailConfiguration";

        private const string SQL_SELECT_MAILFOLDERS = @" SELECT	[MailFolderDetailID],	[MailConfigID],	[MailFolderID],	[MailFolderName],	[Ingestion],	[MoveFolder],	[SearchFolder],	[CreatedBy],	[CreatedOn],	[ModifiedBy],	[ModifiedOn],	[Disabled]	FROM EMS.tblMailFolderDetails(NOLOCK) WHERE	MailConfigID=@MailConfigID and Disabled=0";

        private const string SQL_INSERT_MAILRECIEVED = @"INSERT INTO [EMS].[tblMailSchedule] (campaignID,MailConfigID,MailFolderDetailID,Subject,MailUploadTime,MailRecievedTime,MAilUniqueID) VALUES (@campaignID,@MailConfigID,@MailFolderDetailID,@Subject,GEtdate(),@MailRecievedTime,@MailUniqueID)";

        private const string SP_UPDATE_PDEXPIRE = @"[EMS].[Usp_DisableEMSConfiguration]";

        private const string SP_INSERT_MAILRECIEVEDETAILS = @"[EMS].[USP_GetMailRecieveDetails]";
        private const string SP_INSERT_MAILCONFIGURATION = @"[EMS].[USP_InsertUpdateMailConfiguration]";

        private const string SP_MANAGE_MAILCONFIGDETAILS = @"[EMS].[USP_InsertUpdateMailConfigDetails]";

        private const string SP_GET_MAILCONFIGALLDATA = @"[EMS].[USP_GetMailConfigAllData]";
        private const string SP_GET_WORKADDITIONFIELD = @"[EMS].[Usp_GetWorkAdditionField]";

        //private const string SQL_DELETE_MAILCONFIG = @"DELETE FROM EMS.tblMailConfiguration WHERE MailConfigID=@MailConfigID";

        private const string SQL_INSERTUPDATE_ADVANCEDCONFIGURATION = @"[EMS].[USP_InsertUpdateAdvancedConfiguration]";

        private const string PARAM_DSTOREID = "@DStoreID";
        private const string PARAM_MAILCONFIGID = "@MailConfigID";
        private const string PARAM_EXCEPTIONTYPE = "@ExceptionType";
        private const string PARAM_CAMPAIGNID = "@CampaignID";
        private const string PARAM_MAILBOXNAME = "@MailBoxName";
        private const string PARAM_MAILFOLDERDETAILID = "@MailFolderDetailID";
        private const string PARAM_EMAILID = "@EmailID";
        private const string PARAM_USERID = "@UserID";
        private const string PARAM_UPD = "@uPassword";
        private const string PARAM_USESERVICECREDENTIALPULL = "@UseServiceCredentialToPull";
        private const string PARAM_USESERVICECREDENTIALSEND = "@UseUserCredentialToSend";
        private const string PARAM_MAILSERVERTYPEID = "@MailServerTypeID";
        private const string PARAM_SCHEDULEINTERVAL = "@ScheduleInterval";
        private const string PARAM_AUTODISCOVERYPATH = "@AutoDiscoveryPath";
        private const string PARAM_LOTUSSERVERPATH = "@LotusServerPath";
        private const string PARAM_NFSFILEPATH = "@NFSFilePath";
        private const string PARAM_WEBENABLED = "@WebEnabled";
        private const string PARAM_EMSWEBSERVERHOSTING = "@EMSWebServerHostingAtClient";
        private const string PARAM_EMSWEBSERVERURL = "@EMSWebServerURL";
        private const string PARAM_ISPDEXPIRE = "@isPasswordExpire";
        private const string PARAM_AUTOREPLY = "@AutoReply";
        private const string PARAM_POOLINGVALUE = "@PoolingValue";
        private const string PARAM_MAILTEMPLATEID = "@MailTemplateID";
        private const string PARAM_ISREADMAIL = "@IsReadMail";


        private const string PARAM_LOTUSDOMAINNAME = "@LotusDomainName";
        private const string PARAM_LOTUSDOMAINPREFIX = "@LotusDomainPrefix";


        private const string PARAM_OUTOFOFFICE = "@OutofOffice";
        private const string PARAM_OUTLOOK = "@OutLook";
        private const string PARAM_TRANSLATIONENABLED = "@TranslationEnabled";

        private const string PARAM_OUTOFOFFICETEXT = "@OutofOfficeText";

        private const string PARAM_IMPERSONATION = "@Impersonation";
        private const string PARAM_IMPERSONATIONIDTYPE = "@ImpersonationIDType";
        private const string PARAM_IMPERSONATIONID = "@ImpersonationID";

        private const string PARAM_FOLDERTYPE = "@FolderType";
        private const string PARAM_CREATEDBY = "@CreatedBy";
        //private const string PARAM_MODIFIEDBY = "@ModifiedBy";
        //private const string PARAM_ModifiedOn = "@ModifiedOn";
        private const string PARAM_DISABLED = "@Disabled";
        private const string PARAM_BATCHFREQUENCY = "@BatchFrequency";
        //private const string PARAM_ID = "@id";

        private const string PARAM_MAILRECIEVEDTIME = "@MailRecievedTime";
        private const string PARAM_MAILUNIQUEID = "@MailUniqueID";
        //private const string PARAM_MAILFOLDERID = "@MailFolderID";
        //private const string PARAM_MAILFOLDERNAME = "@MailFolderName";
        //private const string PARAM_INGESTION = "@Ingestion";
        //private const string PARAM_MOVEFOLDER = "@MoveFolder";
        //private const string PARAM_SEARCHFOLDER = "@SearchFolder";
        private const string PARAM_SUBJECT = "@Subject";
        private const string PARAM_USERXML = "@User_XML";
        private const string PARAM_STOREID = "@StoreID";
        private const string PARAM_ISACTIVE = "@IsActive";
        private const string PARAM_SENDMAILQUIQUEIDENTIFIED = "@SENDMAILQUIQUEIDENTIFIED";
        private const string PARAM_SCHEDULETOSAMEUSER = "@SCHEDULETOSAMEUSER";
        private const string PARAM_KEY = "@KEY";
        private const string PARAM_TimeZoneID = "@TimeZoneID";
        private const string PARAM_InlineEditing = "@InlineEditing";

        private const string PARAM_NeedeFile = "@NeedeFile";
        private const string PARAM_bOutLookMailEnabled = "@bOutLookMailEnabled";
        private const string PARAM_NeedPrint = "@NeedPrint";
        private const string PARAM_ReadMailBody = "@ReadMailBody";
        private const string PARAM_CFX = "@CFX";
        private const string PARAM_DuringUpload = "@DuringUpload";



        private const string PARAM_FreshRequired = "@IsFreshRequired";
        private const string PARAM_Sensitivity = "@IsSensitivity";
        private const string PARAM_NeedTicketLenth = "@NeedTicketLenth";
        private const string PARAM_CEXlauncherPath = "@CEXlauncherPath";

        private const string PARAM_EfilePath = "@EFilePath";
        private const string PARAM_SubmitDisplay = "@IsSubmitDisplay";
        private const string PARAM_StrSubmitDisplay = "@sSubmitDisplay";


        private const string PARAM_TicketName = "@TicketName";
        private const string PARAM_NeedTicket = "@NeedTicket";
        private const string PARAM_LastAssignType = "@LastAssignType";
        private const string PARAM_IsAssignLast = "@IsAssignLast";


        private const string PARAM_UploadBy = "@UploadBy";


        // added by manishdwivedi
        private const string PARAM_ClinetID = "@ClinetID";
        private const string PARAM_TenentID = "@TenentID";
        private const string PARAM_Scope = "@Scope";
        private const string PARAM_RedirectUrl = "@RedirectUrl";
        private const string PARAM_Instance = "@Instance";
        private const string PARAM_IsForSWMIntegration = "@IsForSWMIntegration";
        private const string PARAM_IsSWMEMSIntegration = "@IsSWMEMSIntegration";

        //end




        #endregion

        #region Constructor and Dispose
        /// <summary>
        /// Initializes a new instance of the <see cref="DLClient"/> class.
        /// </summary>
        public DLMailConfiguration(BETenant oTenant) { _oTenant = oTenant; }



        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { _oTenant = null; }
        #endregion

        public IList<BEMailReceivedDateTime> getMailReceiveDateTime(int iCampaignID, int iMailConfigID, int iMailFolderDetailID, BETenant oTenant)
        {
            IList<BEMailReceivedDateTime> lReceiveDate = new List<BEMailReceivedDateTime>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_INSERT_MAILRECIEVEDETAILS);
            db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, iCampaignID);
            db.AddInParameter(dbCommand, PARAM_MAILCONFIGID, DbType.Int32, iMailConfigID);
            db.AddInParameter(dbCommand, PARAM_MAILFOLDERDETAILID, DbType.Int32, iMailFolderDetailID);
            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEMailReceivedDateTime BEMailRec = new BEMailReceivedDateTime
                    {
                        iDSOBJChoiceID = int.Parse(rdr["DSOBJChoiceID"].ToString()),
                        iMailFolderDetailID = int.Parse(rdr["MailFolderDetailID"].ToString()),
                        dReceivedDateTime = DateTime.Parse(rdr["ReceivedDateTime"].ToString()),
                        sChoiceValue = rdr["ChoiceValue"].ToString(),
                        sDSOBJName = rdr["DSOBJName"].ToString()
                    };

                    lReceiveDate.Add(BEMailRec);
                    BEMailRec = null;
                }
            }
            return lReceiveDate;
        }

        public void InsertRecieveDateTime(int iCampaignID, int iMailConfigID, int iMailFolderDetailID, string sSubject,
           DateTime dMailRecievedTime, string sMailUniqueID)
        {

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbInsertUserCommand = db.GetSqlStringCommand(SQL_INSERT_MAILRECIEVED);

            db.AddInParameter(dbInsertUserCommand, PARAM_CAMPAIGNID, DbType.Int32, iCampaignID);
            db.AddInParameter(dbInsertUserCommand, PARAM_MAILCONFIGID, DbType.Int32, iMailConfigID);
            db.AddInParameter(dbInsertUserCommand, PARAM_MAILFOLDERDETAILID, DbType.Int32, iMailFolderDetailID);
            db.AddInParameter(dbInsertUserCommand, PARAM_MAILRECIEVEDTIME, DbType.DateTime, dMailRecievedTime);
            db.AddInParameter(dbInsertUserCommand, PARAM_MAILUNIQUEID, DbType.String, sMailUniqueID);
            db.AddInParameter(dbInsertUserCommand, PARAM_SUBJECT, DbType.String, sSubject);
            db.ExecuteNonQuery(dbInsertUserCommand);

        }
        /// <summary>
        /// Gets the LIST
        /// </summary>
        /// <param name="ClientID">The client ID.</param>
        /// <returns></returns>
        public IList<BEMailConfiguration> GetCampaignWiseList(int iStoreID, bool isActive)
        {
            List<BEMailConfiguration> lMailConfiguration = new List<BEMailConfiguration>();

            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = null;
                dbCommand = db.GetStoredProcCommand(SQL_SP_BY_CAMPAIGNID);
                db.AddInParameter(dbCommand, PARAM_STOREID, DbType.Int32, iStoreID);
                db.AddInParameter(dbCommand, PARAM_ISACTIVE, DbType.Int32, isActive);
                using (IDataReader rdr = db.ExecuteReader(dbCommand))
                {
                    while (rdr.Read())
                    {
                        BEMailConfiguration BEMailConfig = new BEMailConfiguration
                        {
                            iMailConfigID = string.IsNullOrEmpty(rdr["MailConfigID"].ToString()) ? 0 : int.Parse(rdr["MailConfigID"].ToString()),
                            iCampaignID = int.Parse(rdr["CampaignID"].ToString()),
                            MailBoxType = string.IsNullOrEmpty(rdr["FolderType"].ToString()) ? 0 : (emailType)int.Parse(rdr["FolderType"].ToString()),
                            sMailBoxName = rdr["MailBoxName"].ToString(),
                            sEmailID = rdr["EmailID"].ToString(),
                            sUserID = rdr["UserID"].ToString(),
                            sPassword = rdr["uPassword"].ToString(),
                            bUseServiceCredentialToPull = string.IsNullOrEmpty(rdr["UseServiceCredentialToPull"].ToString()) ? false : bool.Parse(rdr["UseServiceCredentialToPull"].ToString()),
                            bUseUserCredentialToSend = string.IsNullOrEmpty(rdr["UseUserCredentialToSend"].ToString()) ? false : bool.Parse(rdr["UseUserCredentialToSend"].ToString()),
                            iMailServerTypeID = string.IsNullOrEmpty(rdr["MailServerTypeID"].ToString()) ? 0 : (EmailServerType)int.Parse(rdr["MailServerTypeID"].ToString()),
                            iScheduleInterval = string.IsNullOrEmpty(rdr["ScheduleInterval"].ToString()) ? 0 : int.Parse(rdr["ScheduleInterval"].ToString()),
                            sAutoDiscoveryPath = rdr["AutoDiscoveryPath"].ToString(),
                            sLotusServerPath = rdr["LotusServerPath"].ToString(),
                            sNFSFilePath = rdr["NFSFilePath"].ToString(),
                            bWebEnabled = string.IsNullOrEmpty(rdr["WebEnabled"].ToString()) ? false : bool.Parse(rdr["WebEnabled"].ToString()),
                            bEMSWebServerHosting = string.IsNullOrEmpty(rdr["EMSWebServerHostingAtClient"].ToString()) ? false : bool.Parse(rdr["EMSWebServerHostingAtClient"].ToString()),
                            EMSWebServerURL = rdr["EMSWebServerURL"].ToString(),
                            isPasswordExpire = string.IsNullOrEmpty(rdr["isPasswordExpire"].ToString()) ? false : bool.Parse(rdr["isPasswordExpire"].ToString()),
                            AutoReply = string.IsNullOrEmpty(rdr["AutoReply"].ToString()) ? false : bool.Parse(rdr["AutoReply"].ToString()),
                            bInlineEditing = string.IsNullOrEmpty(rdr["InlineEditing"].ToString()) ? false : bool.Parse(rdr["InlineEditing"].ToString()),

                            bOutLookMailEnabled = string.IsNullOrEmpty(rdr["bOutLookMailEnabled"].ToString()) ? false : bool.Parse(rdr["bOutLookMailEnabled"].ToString()),
                            bNeedPrintEnabled = string.IsNullOrEmpty(rdr["NeedPrint"].ToString()) ? false : bool.Parse(rdr["NeedPrint"].ToString()),
                            beFileEnabled = string.IsNullOrEmpty(rdr["NeedeFile"].ToString()) ? false : bool.Parse(rdr["NeedeFile"].ToString()),
                            bCFXEnabled = string.IsNullOrEmpty(rdr["CFX"].ToString()) ? false : bool.Parse(rdr["CFX"].ToString()),
                            bReadMailBodyEnabled = string.IsNullOrEmpty(rdr["ReadBodyContent"].ToString()) ? false : bool.Parse(rdr["ReadBodyContent"].ToString()),
                            bDuringUploadEnabled = string.IsNullOrEmpty(rdr["DuringUploadeTicket"].ToString()) ? false : bool.Parse(rdr["DuringUploadeTicket"].ToString()),

                            bFreshRequiredEnabled = string.IsNullOrEmpty(rdr["IsFreshRequired"].ToString()) ? false : bool.Parse(rdr["IsFreshRequired"].ToString()),
                            bSensitivityEnabled = string.IsNullOrEmpty(rdr["IsSensitivity"].ToString()) ? false : bool.Parse(rdr["IsSensitivity"].ToString()),
                            strCEXlauncherPath = rdr["CEXlauncherPath"].ToString(),
                            strEFilePath = rdr["EFilePath"].ToString(),
                            bSubmitDisplayEnabled = string.IsNullOrEmpty(rdr["IsSubmitDisplay"].ToString()) ? false : bool.Parse(rdr["IsSubmitDisplay"].ToString()),
                            strTicketName = rdr["TicketName"].ToString(),
                            iNeedTicketLenth = string.IsNullOrEmpty(rdr["NeedTicketLenth"].ToString()) ? 0 : Convert.ToInt32(rdr["NeedTicketLenth"].ToString()),
                            bNeedTicketEnabled = string.IsNullOrEmpty(rdr["NeedTicket"].ToString()) ? false : bool.Parse(rdr["NeedTicket"].ToString()),
                            iUploadBy = Convert.ToInt32(rdr["UploadBy"].ToString()),
                            strSubmitDisplay = rdr["sSubmitDisplay"].ToString(),

                            PoolingValue = string.IsNullOrEmpty(rdr["PoolingValue"].ToString()) ? false : bool.Parse(rdr["PoolingValue"].ToString()),
                            IsReadMail = string.IsNullOrEmpty(rdr["IsReadMail"].ToString()) ? false : bool.Parse(rdr["IsReadMail"].ToString()),
                            iFolderType = int.Parse(rdr["FolderType"].ToString()),
                            bDisabled = string.IsNullOrEmpty(rdr["Disabled"].ToString()) ? false : bool.Parse(rdr["Disabled"].ToString()),
                            oMailfolderdetails = getFolderList(int.Parse(rdr["MailConfigID"].ToString())),
                            iCreatedBy = string.IsNullOrEmpty(rdr["Createdby"].ToString()) ? 0 : int.Parse(rdr["Createdby"].ToString()),
                            bSendmailQuiqueIdentified = string.IsNullOrEmpty(rdr["SENDMAILQUIQUEIDENTIFIED"].ToString()) ? false : bool.Parse(rdr["SENDMAILQUIQUEIDENTIFIED"].ToString()),
                            bScheduletoSameUser = string.IsNullOrEmpty(rdr["SCHEDULETOSAMEUSER"].ToString()) ? false : bool.Parse(rdr["SCHEDULETOSAMEUSER"].ToString()),
                            BatchFrequency = (BatchFrequencyType)rdr["BatchFrequency"],
                            iStoreID = int.Parse(rdr["DStoreID"].ToString()),
                            bOutofOfficeEnabled = string.IsNullOrEmpty(rdr["OutofOffice"].ToString()) ? false : bool.Parse(rdr["OutofOffice"].ToString()),
                            sOutofOffice = rdr["OutofOfficeText"].ToString(),
                            bImpersonation = string.IsNullOrEmpty(rdr["Impersonation"].ToString()) ? false : bool.Parse(rdr["Impersonation"].ToString()),
                            sImpersonationIDType = string.IsNullOrEmpty(rdr["ImpersonationIDType"].ToString()) ? 0 : (ImpersonationIDType)int.Parse(rdr["ImpersonationIDType"].ToString()),
                            sImpersonationID = rdr["ImpersonationID"].ToString(),
                            bOutLookEnabled = string.IsNullOrEmpty(rdr["OutLook"].ToString()) ? false : bool.Parse(rdr["OutLook"].ToString()),
                            bTranslationEnabled = string.IsNullOrEmpty(rdr["TranslationEnabled"].ToString()) ? false : bool.Parse(rdr["TranslationEnabled"].ToString()),
                            ClinetID = rdr["ClinetID"].ToString(),
                            TenentID = rdr["TenentID"].ToString(),
                            Scope = rdr["Scope"].ToString(),
                            RedirectUrl = rdr["RedirectUrl"].ToString(),
                            Instance = rdr["Instance"].ToString(),

                            oTimeZone = new BETimeZoneInfo()
                            {
                                iTimeZoneID = int.Parse(rdr["TimeZoneID"].ToString()),
                                sTimeZoneID = rdr["TimeZoneName"].ToString()
                            },

                            sServerTimeZone = rdr["ServerTimeZone"].ToString()
                        };
                        BEMailConfig.oCampaignAdditionFields = GetMailCamapignFields(BEMailConfig.iStoreID);
                        using (DLMailTemplate omailtempl = new DLMailTemplate(_oTenant))
                        {
                            BEMailConfig.oMailTemplate = omailtempl.GetMailTemplateList(0, BEMailConfig.iCampaignID);
                        }
                        lMailConfiguration.Add(BEMailConfig);
                        BEMailConfig = null;
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return lMailConfiguration;
        }

        public IList<BEMailfolderdetails> getFolderList(int iMailconfigID)
        {
            IList<BEMailfolderdetails> oBEMailfolderdetails = new List<BEMailfolderdetails>();
            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_MAILFOLDERS);
            db.AddInParameter(dbCommand, PARAM_MAILCONFIGID, DbType.Int32, iMailconfigID);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEMailfolderdetails objMailFolder = new BEMailfolderdetails();
                    objMailFolder.MailFolderDetailID = int.Parse(rdr["MailFolderDetailID"].ToString());
                    objMailFolder.sMailFolderID = rdr["MailFolderID"].ToString();
                    objMailFolder.sMailFolderName = rdr["MailFolderName"].ToString();
                    objMailFolder.bSearchFolder = Convert.ToBoolean(rdr["SearchFolder"]);
                    objMailFolder.bMoveFolder = Convert.ToBoolean(rdr["MoveFolder"]);
                    objMailFolder.bIngestion = Convert.ToBoolean(rdr["Ingestion"]);
                    objMailFolder.bDisabled = Convert.ToBoolean(rdr["Disabled"]);
                    oBEMailfolderdetails.Add(objMailFolder);
                    objMailFolder = null;
                }
            }

            return oBEMailfolderdetails;
        }
        public IList<BEMailCampaignField> GetMailCamapignFields(int iDStoreID)
        {
            IList<BEMailCampaignField> oBEFiled = new List<BEMailCampaignField>();

            Database db = DL_Shared.dbFactory(_oTenant);
            DbCommand dbCommand = db.GetStoredProcCommand(SP_GET_WORKADDITIONFIELD);
            db.AddInParameter(dbCommand, PARAM_DSTOREID, DbType.Int32, iDStoreID);

            using (IDataReader rdr = db.ExecuteReader(dbCommand))
            {
                while (rdr.Read())
                {
                    BEMailCampaignField objMailFolder = new BEMailCampaignField();
                    objMailFolder.ObjName = rdr["DSObjname"].ToString();
                    objMailFolder.DataType = rdr["ObjDatatype"].ToString();
                    oBEFiled.Add(objMailFolder);
                    objMailFolder = null;
                }
            }
            return oBEFiled;

        }
        public IList<BEMailConfiguration> GetMailConfigAllData(int MailConfigID)
        {
            List<BEMailConfiguration> lMailConfiguration = new List<BEMailConfiguration>();
            IList<BEMailfolderdetails> oBEMailfolderdetails = new List<BEMailfolderdetails>();
            //BEMailfolderdetails oBEMailfolderdetails = new BEMailfolderdetails();
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = null;
                if (MailConfigID != 0)
                {
                    dbCommand = db.GetStoredProcCommand(SP_GET_MAILCONFIGALLDATA);
                }

                db.AddInParameter(dbCommand, PARAM_MAILCONFIGID, DbType.Int32, MailConfigID);
                using (DataSet ds = db.ExecuteDataSet(dbCommand))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            BEMailConfiguration BEMailConfig = new BEMailConfiguration
                            {
                                iMailConfigID = int.Parse(ds.Tables[0].Rows[i]["MailConfigID"].ToString()),
                                iCampaignID = int.Parse(ds.Tables[0].Rows[i]["CampaignID"].ToString()),
                                sMailBoxName = ds.Tables[0].Rows[i]["MailBoxName"].ToString(),
                                sEmailID = ds.Tables[0].Rows[i]["EmailID"].ToString(),
                                sUserID = ds.Tables[0].Rows[i]["UserID"].ToString(),
                                sPassword = ds.Tables[0].Rows[i]["uPassword"].ToString(),
                                bUseServiceCredentialToPull = bool.Parse(ds.Tables[0].Rows[i]["UseServiceCredentialToPull"].ToString()),
                                bUseUserCredentialToSend = bool.Parse(ds.Tables[0].Rows[i]["UseUserCredentialToSend"].ToString()),
                                iMailServerTypeID = (EmailServerType)int.Parse(ds.Tables[0].Rows[i]["MailServerTypeID"].ToString()),
                                iScheduleInterval = int.Parse(ds.Tables[0].Rows[i]["ScheduleInterval"].ToString()),
                                sAutoDiscoveryPath = ds.Tables[0].Rows[i]["AutoDiscoveryPath"].ToString(),
                                sLotusServerPath = ds.Tables[0].Rows[i]["LotusServerPath"].ToString(),
                                sNFSFilePath = ds.Tables[0].Rows[i]["NFSFilePath"].ToString(),
                                bWebEnabled = bool.Parse(ds.Tables[0].Rows[i]["WebEnabled"].ToString()),
                                bEMSWebServerHosting = bool.Parse(ds.Tables[0].Rows[i]["EMSWebServerHostingAtClient"].ToString()),
                                EMSWebServerURL = ds.Tables[0].Rows[i]["EMSWebServerURL"].ToString(),
                                isPasswordExpire = bool.Parse(ds.Tables[0].Rows[i]["isPasswordExpire"].ToString()),
                                AutoReply = bool.Parse(ds.Tables[0].Rows[i]["AutoReply"].ToString()),
                                MailTemplateID = int.Parse(ds.Tables[0].Rows[i]["MailTemplateID"].ToString()),
                                PoolingValue = bool.Parse(ds.Tables[0].Rows[i]["PoolingValue"].ToString()),
                                IsReadMail = bool.Parse(ds.Tables[0].Rows[i]["IsReadMail"].ToString()),
                                iFolderType = int.Parse(ds.Tables[0].Rows[i]["FolderType"].ToString()),
                                bDisabled = bool.Parse(ds.Tables[0].Rows[i]["Disabled"].ToString()),
                                sLotusDomainName = ds.Tables[0].Rows[i]["LotusDomainName"].ToString(),
                                sLotusDomainPrefix = ds.Tables[0].Rows[i]["LotusDomainPrefix"].ToString(),
                                bOutofOfficeEnabled = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["OutofOffice"].ToString()) ? false : bool.Parse(ds.Tables[0].Rows[i]["OutofOffice"].ToString()),
                                bOutLookEnabled = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["OutLook"].ToString()) ? false : bool.Parse(ds.Tables[0].Rows[i]["OutLook"].ToString()),
                                bTranslationEnabled = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["TranslationEnabled"].ToString()) ? false : bool.Parse(ds.Tables[0].Rows[i]["TranslationEnabled"].ToString()),
                                sOutofOffice = ds.Tables[0].Rows[i]["OutofOfficeText"].ToString(),
                                bImpersonation = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Impersonation"].ToString()) ? false : bool.Parse(ds.Tables[0].Rows[i]["Impersonation"].ToString()),
                                sImpersonationIDType = (ImpersonationIDType)int.Parse(ds.Tables[0].Rows[i]["ImpersonationIDType"].ToString()),
                                sImpersonationID = ds.Tables[0].Rows[i]["ImpersonationID"].ToString(),

                                ClinetID = ds.Tables[0].Rows[i]["ClinetID"].ToString(),
                                TenentID = ds.Tables[0].Rows[i]["TenentID"].ToString(),
                                Scope = ds.Tables[0].Rows[i]["Scope"].ToString(),
                                RedirectUrl = ds.Tables[0].Rows[i]["RedirectUrl"].ToString(),
                                Instance = ds.Tables[0].Rows[i]["Instance"].ToString(),
                                IsForSWMIntegration = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["IsForSWMIntegration"].ToString()) ? false : bool.Parse(ds.Tables[0].Rows[i]["IsForSWMIntegration"].ToString()),
                                bSWMEMSIntegration = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["IsSWMEMSIntegration"].ToString()) ? false : bool.Parse(ds.Tables[0].Rows[i]["IsSWMEMSIntegration"].ToString())
                            };


                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                                {
                                    BEMailfolderdetails obj = new BEMailfolderdetails();
                                    obj.MailFolderDetailID = int.Parse(ds.Tables[1].Rows[j]["MailFolderDetailID"].ToString());
                                    obj.sMailFolderID = ds.Tables[1].Rows[j]["MailFolderID"].ToString();
                                    obj.sMailFolderName = ds.Tables[1].Rows[j]["MailFolderName"].ToString();
                                    obj.bIngestion = bool.Parse(ds.Tables[1].Rows[j]["Ingestion"].ToString());
                                    obj.bMoveFolder = bool.Parse(ds.Tables[1].Rows[j]["MoveFolder"].ToString().ToLower());
                                    obj.bSearchFolder = bool.Parse(ds.Tables[1].Rows[j]["SearchFolder"].ToString().ToLower());
                                    obj.bDisabled = bool.Parse(ds.Tables[1].Rows[j]["Disabled"].ToString().ToLower());
                                    obj.sMailFolderPath = ds.Tables[1].Rows[j]["MailFolderPath"].ToString();
                                    oBEMailfolderdetails.Add(obj);
                                    obj = null;

                                }
                            }


                            BEMailConfig.oMailfolderdetails = (oBEMailfolderdetails);

                            lMailConfiguration.Add(BEMailConfig);
                            BEMailConfig = null;
                        }
                    }


                }

            }


            catch (Exception ex)
            {
                throw ex;
            }
            return lMailConfiguration;
        }

        //#endregion

        #region Insert Mail Configuration
        /// <summary>
        /// Inserts Mail Configuration.
        /// <param name="oMailConfiguration">client.</param>
        public void InsertData(BEMailConfiguration oMailConfiguration)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(oMailConfiguration.dtMailfolderdetails);
                System.IO.StringWriter sw = new System.IO.StringWriter();
                ds.WriteXml(sw);
                string strXml = sw.ToString();


                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetStoredProcCommand(SP_INSERT_MAILCONFIGURATION);

                db.AddInParameter(dbCommand, PARAM_STOREID, DbType.Int32, oMailConfiguration.iStoreID);
                db.AddInParameter(dbCommand, PARAM_MAILCONFIGID, DbType.Int32, oMailConfiguration.iMailConfigID);
                db.AddInParameter(dbCommand, PARAM_MAILTEMPLATEID, DbType.Int32, oMailConfiguration.MailTemplateID);
                db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, oMailConfiguration.iCampaignID);
                db.AddInParameter(dbCommand, PARAM_MAILBOXNAME, DbType.String, oMailConfiguration.sMailBoxName);
                db.AddInParameter(dbCommand, PARAM_EMAILID, DbType.String, oMailConfiguration.sEmailID);
                db.AddInParameter(dbCommand, PARAM_USERID, DbType.String, oMailConfiguration.sUserID);
                db.AddInParameter(dbCommand, PARAM_UPD, DbType.String, oMailConfiguration.sPassword);
                db.AddInParameter(dbCommand, PARAM_USESERVICECREDENTIALPULL, DbType.Boolean, oMailConfiguration.bUseServiceCredentialToPull);
                db.AddInParameter(dbCommand, PARAM_USESERVICECREDENTIALSEND, DbType.Boolean, oMailConfiguration.bUseUserCredentialToSend);
                db.AddInParameter(dbCommand, PARAM_MAILSERVERTYPEID, DbType.Int32, (EmailServerType)oMailConfiguration.iMailServerTypeID);
                db.AddInParameter(dbCommand, PARAM_SCHEDULEINTERVAL, DbType.Int32, oMailConfiguration.iScheduleInterval);
                db.AddInParameter(dbCommand, PARAM_AUTODISCOVERYPATH, DbType.String, oMailConfiguration.sAutoDiscoveryPath);
                db.AddInParameter(dbCommand, PARAM_LOTUSSERVERPATH, DbType.String, oMailConfiguration.sLotusServerPath);
                db.AddInParameter(dbCommand, PARAM_NFSFILEPATH, DbType.String, oMailConfiguration.sNFSFilePath);
                db.AddInParameter(dbCommand, PARAM_WEBENABLED, DbType.Boolean, oMailConfiguration.bWebEnabled);
                db.AddInParameter(dbCommand, PARAM_EMSWEBSERVERHOSTING, DbType.Boolean, oMailConfiguration.bEMSWebServerHosting);
                db.AddInParameter(dbCommand, PARAM_EMSWEBSERVERURL, DbType.String, oMailConfiguration.EMSWebServerURL);
                db.AddInParameter(dbCommand, PARAM_ISPDEXPIRE, DbType.Boolean, oMailConfiguration.isPasswordExpire);
                db.AddInParameter(dbCommand, PARAM_AUTOREPLY, DbType.Boolean, oMailConfiguration.AutoReply);
                db.AddInParameter(dbCommand, PARAM_POOLINGVALUE, DbType.Boolean, oMailConfiguration.PoolingValue);
                db.AddInParameter(dbCommand, PARAM_ISREADMAIL, DbType.Boolean, oMailConfiguration.IsReadMail);
                db.AddInParameter(dbCommand, PARAM_FOLDERTYPE, DbType.Int32, oMailConfiguration.iFolderType);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oMailConfiguration.iCreatedBy);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oMailConfiguration.bDisabled);

                db.AddInParameter(dbCommand, PARAM_LOTUSDOMAINNAME, DbType.String, oMailConfiguration.sLotusDomainName);
                db.AddInParameter(dbCommand, PARAM_LOTUSDOMAINPREFIX, DbType.String, oMailConfiguration.sLotusDomainPrefix);

                db.AddInParameter(dbCommand, PARAM_OUTOFOFFICE, DbType.Boolean, oMailConfiguration.bOutofOfficeEnabled);
                db.AddInParameter(dbCommand, PARAM_OUTOFOFFICETEXT, DbType.String, oMailConfiguration.sOutofOffice);

                db.AddInParameter(dbCommand, PARAM_IMPERSONATION, DbType.Boolean, oMailConfiguration.bImpersonation);
                db.AddInParameter(dbCommand, PARAM_IMPERSONATIONID, DbType.String, oMailConfiguration.sImpersonationID);
                db.AddInParameter(dbCommand, PARAM_IMPERSONATIONIDTYPE, DbType.Int32, (ImpersonationIDType)oMailConfiguration.sImpersonationIDType);
                db.AddInParameter(dbCommand, PARAM_OUTLOOK, DbType.Boolean, oMailConfiguration.bOutLookEnabled);
                db.AddInParameter(dbCommand, PARAM_TRANSLATIONENABLED, DbType.Boolean, oMailConfiguration.bTranslationEnabled);

                db.AddInParameter(dbCommand, PARAM_USERXML, DbType.Xml, strXml);

                db.AddInParameter(dbCommand, PARAM_ClinetID, DbType.String, oMailConfiguration.ClinetID);
                db.AddInParameter(dbCommand, PARAM_TenentID, DbType.String, oMailConfiguration.TenentID);
                db.AddInParameter(dbCommand, PARAM_Scope, DbType.String, oMailConfiguration.Scope);
                db.AddInParameter(dbCommand, PARAM_RedirectUrl, DbType.String, oMailConfiguration.RedirectUrl);
                db.AddInParameter(dbCommand, PARAM_Instance, DbType.String, oMailConfiguration.Instance);
                db.AddInParameter(dbCommand, PARAM_IsForSWMIntegration, DbType.Boolean, oMailConfiguration.IsForSWMIntegration);
                db.AddInParameter(dbCommand, PARAM_IsSWMEMSIntegration, DbType.Boolean, oMailConfiguration.bSWMEMSIntegration);
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open

                    StringBuilder sb = new StringBuilder();
                    foreach (SqlParameter item in dbCommand.Parameters)
                    {
                        sb = sb.Append($"{item.ParameterName}='{item.SqlValue}',");
                    }
                    var input = sb.ToString().TrimEnd(',');
                    try
                    {
                        int iMailConfigID = Convert.ToInt32(db.ExecuteScalar(dbCommand));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            /*
            catch (System.Data.SqlClient.SqlException ex)
            {
                
                if (ex.Number == 547)
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_MailConfigAlreadyExist))
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_MailConfigAlreadyExist);
                }
                throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }*/
            catch (Exception)
            {
                throw;//new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }

        }


        // add by alankar
        public void InsertUpdateAdvancedConfiguration(BEMailConfiguration oMailConfiguration)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetStoredProcCommand(SQL_INSERTUPDATE_ADVANCEDCONFIGURATION);
                db.AddInParameter(dbCommand, PARAM_KEY, DbType.Int32, 1);
                db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, oMailConfiguration.iCampaignID);
                db.AddInParameter(dbCommand, PARAM_SENDMAILQUIQUEIDENTIFIED, DbType.Boolean, oMailConfiguration.bSendmailQuiqueIdentified);
                db.AddInParameter(dbCommand, PARAM_SCHEDULETOSAMEUSER, DbType.Boolean, oMailConfiguration.bScheduletoSameUser);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oMailConfiguration.iCreatedBy);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oMailConfiguration.bDisabled);
                db.AddInParameter(dbCommand, PARAM_BATCHFREQUENCY, DbType.Int32, (int)oMailConfiguration.BatchFrequency);
                db.AddInParameter(dbCommand, PARAM_TimeZoneID, DbType.Int32, oMailConfiguration.oTimeZone.iTimeZoneID);
                db.AddInParameter(dbCommand, PARAM_InlineEditing, DbType.Boolean, oMailConfiguration.bInlineEditing);
                db.AddInParameter(dbCommand, PARAM_DuringUpload, DbType.Boolean, oMailConfiguration.bDuringUploadEnabled);
                db.AddInParameter(dbCommand, PARAM_NeedeFile, DbType.Boolean, oMailConfiguration.beFileEnabled);
                db.AddInParameter(dbCommand, PARAM_bOutLookMailEnabled, DbType.Boolean, oMailConfiguration.bOutLookMailEnabled);
                db.AddInParameter(dbCommand, PARAM_NeedPrint, DbType.Boolean, oMailConfiguration.bNeedPrintEnabled);
                db.AddInParameter(dbCommand, PARAM_CFX, DbType.Boolean, oMailConfiguration.bCFXEnabled);
                db.AddInParameter(dbCommand, PARAM_ReadMailBody, DbType.Boolean, oMailConfiguration.bReadMailBodyEnabled);

                db.AddInParameter(dbCommand, PARAM_FreshRequired, DbType.Boolean, oMailConfiguration.bFreshRequiredEnabled);
                db.AddInParameter(dbCommand, PARAM_Sensitivity, DbType.Boolean, oMailConfiguration.bSensitivityEnabled);
                db.AddInParameter(dbCommand, PARAM_NeedTicketLenth, DbType.Int32, oMailConfiguration.iNeedTicketLenth);
                db.AddInParameter(dbCommand, PARAM_CEXlauncherPath, DbType.String, oMailConfiguration.strCEXlauncherPath);
                db.AddInParameter(dbCommand, PARAM_TicketName, DbType.String, oMailConfiguration.strTicketName);
                db.AddInParameter(dbCommand, PARAM_NeedTicket, DbType.Boolean, oMailConfiguration.bNeedTicketEnabled);
                db.AddInParameter(dbCommand, PARAM_UploadBy, DbType.Int32, oMailConfiguration.iUploadBy);
                db.AddInParameter(dbCommand, PARAM_LastAssignType, DbType.String, oMailConfiguration.sAssignType);
                db.AddInParameter(dbCommand, PARAM_IsAssignLast, DbType.Boolean, oMailConfiguration.IsAssignLast);

                db.AddInParameter(dbCommand, PARAM_EfilePath, DbType.String, oMailConfiguration.strEFilePath);
                db.AddInParameter(dbCommand, PARAM_SubmitDisplay, DbType.Boolean, oMailConfiguration.bSubmitDisplayEnabled);
                db.AddInParameter(dbCommand, PARAM_StrSubmitDisplay, DbType.String, oMailConfiguration.strSubmitDisplay);

                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open
                    try
                    {
                        int iMailConfigID = Convert.ToInt32(db.ExecuteScalar(dbCommand));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            /*
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }

        }


        public IList<BEMailConfiguration> GetAdvancedConfiguration(int iCampaignID)
        {
            List<BEMailConfiguration> lMailConfiguration = new List<BEMailConfiguration>();
            BETimeZoneInfo objBETimeZoneInfo = new BETimeZoneInfo();
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = null;


                dbCommand = db.GetStoredProcCommand(SQL_INSERTUPDATE_ADVANCEDCONFIGURATION);
                db.AddInParameter(dbCommand, PARAM_KEY, DbType.Int32, 2);

                db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, iCampaignID);
                //db.AddInParameter(dbCommand, PARAM_SENDMAILQUIQUEIDENTIFIED, DbType.Boolean, false);
                //db.AddInParameter(dbCommand, PARAM_SCHEDULETOSAMEUSER, DbType.Boolean, false);
                //db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, 1);
                //db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, false);





                using (IDataReader rdr = db.ExecuteReader(dbCommand))
                {
                    while (rdr.Read())
                    {
                        objBETimeZoneInfo.iTimeZoneID = int.Parse(rdr["TimeZoneID"].ToString());
                        BEMailConfiguration BEMailConfig = new BEMailConfiguration();
                        // {


                        BEMailConfig.bSendmailQuiqueIdentified = string.IsNullOrEmpty(rdr["SENDMAILQUIQUEIDENTIFIED"].ToString()) ? false : bool.Parse(rdr["SENDMAILQUIQUEIDENTIFIED"].ToString());
                        BEMailConfig.bScheduletoSameUser = string.IsNullOrEmpty(rdr["SCHEDULETOSAMEUSER"].ToString()) ? false : bool.Parse(rdr["SCHEDULETOSAMEUSER"].ToString());
                        BEMailConfig.BatchFrequency = (BatchFrequencyType)Enum.Parse(typeof(BatchFrequencyType), rdr["BatchFrequency"].ToString());
                        BEMailConfig.oTimeZone = objBETimeZoneInfo;
                        BEMailConfig.bDisabled = string.IsNullOrEmpty(rdr["Disabled"].ToString()) ? false : bool.Parse(rdr["Disabled"].ToString());
                        BEMailConfig.iCreatedBy = int.Parse(rdr["Createdby"].ToString());
                        BEMailConfig.bInlineEditing = string.IsNullOrEmpty(rdr["InlineEditing"].ToString()) ? false : bool.Parse(rdr["InlineEditing"].ToString());
                        BEMailConfig.bNeedPrintEnabled = string.IsNullOrEmpty(rdr["NeedPrint"].ToString()) ? false : bool.Parse(rdr["NeedPrint"].ToString());
                        BEMailConfig.beFileEnabled = string.IsNullOrEmpty(rdr["NeedeFile"].ToString()) ? false : bool.Parse(rdr["NeedeFile"].ToString());
                        BEMailConfig.bOutLookMailEnabled = string.IsNullOrEmpty(rdr["bOutLookMailEnabled"].ToString()) ? false : bool.Parse(rdr["bOutLookMailEnabled"].ToString());
                        BEMailConfig.bDuringUploadEnabled = string.IsNullOrEmpty(rdr["DuringUploadeTicket"].ToString()) ? false : bool.Parse(rdr["DuringUploadeTicket"].ToString());
                        BEMailConfig.bCFXEnabled = string.IsNullOrEmpty(rdr["CFX"].ToString()) ? false : bool.Parse(rdr["CFX"].ToString());
                        BEMailConfig.bReadMailBodyEnabled = string.IsNullOrEmpty(rdr["ReadBodyContent"].ToString()) ? false : bool.Parse(rdr["ReadBodyContent"].ToString());

                        BEMailConfig.bFreshRequiredEnabled = string.IsNullOrEmpty(rdr["IsFreshRequired"].ToString()) ? false : bool.Parse(rdr["IsFreshRequired"].ToString());
                        BEMailConfig.bSensitivityEnabled = string.IsNullOrEmpty(rdr["IsSensitivity"].ToString()) ? false : bool.Parse(rdr["IsSensitivity"].ToString());
                        BEMailConfig.strCEXlauncherPath = rdr["CEXlauncherPath"].ToString();

                        BEMailConfig.strEFilePath = rdr["EFilePath"].ToString();
                        BEMailConfig.bSubmitDisplayEnabled = string.IsNullOrEmpty(rdr["IsSubmitDisplay"].ToString()) ? false : bool.Parse(rdr["IsSubmitDisplay"].ToString());
                        BEMailConfig.strSubmitDisplay = rdr["sSubmitDisplay"].ToString();
                        BEMailConfig.strTicketName = string.IsNullOrEmpty(rdr["TicketName"].ToString()) ? "" : rdr["TicketName"].ToString();
                        BEMailConfig.iNeedTicketLenth = string.IsNullOrEmpty(rdr["NeedTicketLenth"].ToString()) ? 0 : Convert.ToInt32(rdr["NeedTicketLenth"].ToString());
                        BEMailConfig.bNeedTicketEnabled = string.IsNullOrEmpty(rdr["NeedTicket"].ToString()) ? false : bool.Parse(rdr["NeedTicket"].ToString());
                        BEMailConfig.iUploadBy = string.IsNullOrEmpty(rdr["UploadBy"].ToString()) ? 0 : int.Parse(rdr["UploadBy"].ToString());
                        BEMailConfig.sAssignType = string.IsNullOrEmpty(rdr["LastAssignType"].ToString()) ? "" : rdr["LastAssignType"].ToString();
                        BEMailConfig.IsAssignLast = string.IsNullOrEmpty(rdr["IsAssignLast"].ToString()) ? false : bool.Parse(rdr["IsAssignLast"].ToString());
                        //};

                        lMailConfiguration.Add(BEMailConfig);
                        BEMailConfig = null;
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return lMailConfiguration;
        }

        //end alankar

        public void ManageMailConfigDetails(int MailConfigID, Database db, DbTransaction trans, BEMailConfiguration oMailConfiguration)
        {
            DataSet ds = new DataSet();
            for (int i = 0; i < oMailConfiguration.dtMailfolderdetails.Rows.Count; i++)
            {
                oMailConfiguration.dtMailfolderdetails.Rows[i]["MailConfigID"] = MailConfigID;
                oMailConfiguration.dtMailfolderdetails.Rows[i]["CreatedBy"] = oMailConfiguration.iCreatedBy;
                oMailConfiguration.dtMailfolderdetails.Rows[i]["CreatedOn"] = DateTime.UtcNow;

                oMailConfiguration.dtMailfolderdetails.AcceptChanges();
            }
            ds.Tables.Add(oMailConfiguration.dtMailfolderdetails);
            System.IO.StringWriter sw = new System.IO.StringWriter();
            ds.WriteXml(sw);
            string strXml = sw.ToString();

            DbCommand dbCommand = db.GetStoredProcCommand(SP_MANAGE_MAILCONFIGDETAILS);

            db.AddInParameter(dbCommand, PARAM_MAILCONFIGID, DbType.Int32, MailConfigID);
            db.AddInParameter(dbCommand, PARAM_USERXML, DbType.Xml, strXml);

            try
            {
                db.ExecuteScalar(dbCommand, trans);
            }


            catch (Exception ex)
            {
                trans.Rollback();//Transaction RollBack
                throw ex;
            }
        }



        #endregion

        //#region Update Mail  Configuration Data
        ///// <summary>
        ///// Update Mail Configuration Data
        ///// </summary>
        ///// <param name="oMailConfiguration">client.</param>
        public void UpdateData(BEMailConfiguration oMailConfiguration)
        {

            try
            {
                DataSet ds = new DataSet();
                for (int i = 0; i < oMailConfiguration.dtMailfolderdetails.Rows.Count; i++)
                {
                    oMailConfiguration.dtMailfolderdetails.Rows[i]["MailConfigID"] = oMailConfiguration.iMailConfigID;
                    oMailConfiguration.dtMailfolderdetails.Rows[i]["CreatedBy"] = oMailConfiguration.iCreatedBy;
                    oMailConfiguration.dtMailfolderdetails.Rows[i]["CreatedOn"] = DateTime.UtcNow;

                    oMailConfiguration.dtMailfolderdetails.AcceptChanges();
                }
                ds.Tables.Add(oMailConfiguration.dtMailfolderdetails);
                System.IO.StringWriter sw = new System.IO.StringWriter();
                ds.WriteXml(sw);
                string strXml = sw.ToString();
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetStoredProcCommand(SP_INSERT_MAILCONFIGURATION);
                db.AddInParameter(dbCommand, PARAM_STOREID, DbType.Int32, oMailConfiguration.iStoreID);
                db.AddInParameter(dbCommand, PARAM_MAILCONFIGID, DbType.Int32, oMailConfiguration.iMailConfigID);
                db.AddInParameter(dbCommand, PARAM_MAILTEMPLATEID, DbType.Int32, oMailConfiguration.MailTemplateID);
                db.AddInParameter(dbCommand, PARAM_CAMPAIGNID, DbType.Int32, oMailConfiguration.iCampaignID);
                db.AddInParameter(dbCommand, PARAM_MAILBOXNAME, DbType.String, oMailConfiguration.sMailBoxName);
                db.AddInParameter(dbCommand, PARAM_EMAILID, DbType.String, oMailConfiguration.sEmailID);
                db.AddInParameter(dbCommand, PARAM_USERID, DbType.String, oMailConfiguration.sUserID);
                db.AddInParameter(dbCommand, PARAM_UPD, DbType.String, oMailConfiguration.sPassword);
                db.AddInParameter(dbCommand, PARAM_USESERVICECREDENTIALPULL, DbType.Boolean, oMailConfiguration.bUseServiceCredentialToPull);
                db.AddInParameter(dbCommand, PARAM_USESERVICECREDENTIALSEND, DbType.Boolean, oMailConfiguration.bUseUserCredentialToSend);
                db.AddInParameter(dbCommand, PARAM_MAILSERVERTYPEID, DbType.Int32, (EmailServerType)oMailConfiguration.iMailServerTypeID);
                db.AddInParameter(dbCommand, PARAM_SCHEDULEINTERVAL, DbType.Int32, oMailConfiguration.iScheduleInterval);
                db.AddInParameter(dbCommand, PARAM_AUTODISCOVERYPATH, DbType.String, oMailConfiguration.sAutoDiscoveryPath);
                db.AddInParameter(dbCommand, PARAM_LOTUSSERVERPATH, DbType.String, oMailConfiguration.sLotusServerPath);
                db.AddInParameter(dbCommand, PARAM_NFSFILEPATH, DbType.String, oMailConfiguration.sNFSFilePath);
                db.AddInParameter(dbCommand, PARAM_WEBENABLED, DbType.Boolean, oMailConfiguration.bWebEnabled);
                db.AddInParameter(dbCommand, PARAM_EMSWEBSERVERHOSTING, DbType.Boolean, oMailConfiguration.bEMSWebServerHosting);
                db.AddInParameter(dbCommand, PARAM_EMSWEBSERVERURL, DbType.String, oMailConfiguration.EMSWebServerURL);
                db.AddInParameter(dbCommand, PARAM_ISPDEXPIRE, DbType.Boolean, oMailConfiguration.isPasswordExpire);
                db.AddInParameter(dbCommand, PARAM_AUTOREPLY, DbType.Boolean, oMailConfiguration.AutoReply);
                db.AddInParameter(dbCommand, PARAM_POOLINGVALUE, DbType.Boolean, oMailConfiguration.PoolingValue);
                db.AddInParameter(dbCommand, PARAM_ISREADMAIL, DbType.Boolean, oMailConfiguration.IsReadMail);
                db.AddInParameter(dbCommand, PARAM_FOLDERTYPE, DbType.Int32, oMailConfiguration.iFolderType);
                db.AddInParameter(dbCommand, PARAM_CREATEDBY, DbType.Int32, oMailConfiguration.iCreatedBy);
                db.AddInParameter(dbCommand, PARAM_DISABLED, DbType.Boolean, oMailConfiguration.bDisabled);
                db.AddInParameter(dbCommand, PARAM_LOTUSDOMAINNAME, DbType.String, oMailConfiguration.sLotusDomainName);
                db.AddInParameter(dbCommand, PARAM_LOTUSDOMAINPREFIX, DbType.String, oMailConfiguration.sLotusDomainPrefix);

                db.AddInParameter(dbCommand, PARAM_OUTOFOFFICE, DbType.Boolean, oMailConfiguration.bOutofOfficeEnabled);
                db.AddInParameter(dbCommand, PARAM_OUTOFOFFICETEXT, DbType.String, oMailConfiguration.sOutofOffice);
                db.AddInParameter(dbCommand, PARAM_OUTLOOK, DbType.Boolean, oMailConfiguration.bOutLookEnabled);
                db.AddInParameter(dbCommand, PARAM_TRANSLATIONENABLED, DbType.Boolean, oMailConfiguration.bTranslationEnabled);
                db.AddInParameter(dbCommand, PARAM_IMPERSONATION, DbType.Boolean, oMailConfiguration.bImpersonation);
                db.AddInParameter(dbCommand, PARAM_IMPERSONATIONID, DbType.String, oMailConfiguration.sImpersonationID);
                db.AddInParameter(dbCommand, PARAM_IMPERSONATIONIDTYPE, DbType.Int32, (ImpersonationIDType)oMailConfiguration.sImpersonationIDType);

                db.AddInParameter(dbCommand, PARAM_USERXML, DbType.Xml, strXml);


                db.AddInParameter(dbCommand, PARAM_ClinetID, DbType.String, oMailConfiguration.ClinetID);
                db.AddInParameter(dbCommand, PARAM_TenentID, DbType.String, oMailConfiguration.TenentID);
                db.AddInParameter(dbCommand, PARAM_Scope, DbType.String, oMailConfiguration.Scope);
                db.AddInParameter(dbCommand, PARAM_RedirectUrl, DbType.String, oMailConfiguration.RedirectUrl);
                db.AddInParameter(dbCommand, PARAM_Instance, DbType.String, oMailConfiguration.Instance);
                db.AddInParameter(dbCommand, PARAM_IsForSWMIntegration, DbType.Boolean, oMailConfiguration.IsForSWMIntegration);
                db.AddInParameter(dbCommand, PARAM_IsSWMEMSIntegration, DbType.Boolean, oMailConfiguration.bSWMEMSIntegration);
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();// Connection Open
                    try
                    {

                        //string s = "";
                        //foreach (SqlParameter item in dbCommand.Parameters)
                        //{
                        //    s = s + $"{item.ParameterName}='{item.SqlValue}',";
                        //}

                        int iMailConfigID = Convert.ToInt32(db.ExecuteScalar(dbCommand));
                    }

                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            /*
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                if (ex.Number == 2627)
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                if (ex.Message.Contains(BPA.GlobalResources.DataLayer.Resources.msg_MailConfigAlreadyExist))
                {
                    throw new ExceptionHandler.ExceptionType.DataAccessCustomException(BPA.GlobalResources.DataLayer.Resources.msg_MailConfigAlreadyExist);
                }
                throw new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }*/
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }
        }
        //#endregion

        public void DisableMailConfig(BEMailConfiguration oMailConfig, int iExceptionType)
        {
            try
            {
                Database db = DL_Shared.dbFactory(_oTenant);
                DbCommand dbCommand = db.GetStoredProcCommand(SP_UPDATE_PDEXPIRE);
                db.AddInParameter(dbCommand, PARAM_MAILCONFIGID, DbType.Int32, oMailConfig.iMailConfigID);
                db.AddInParameter(dbCommand, PARAM_EXCEPTIONTYPE, DbType.Int32, iExceptionType);
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                throw;// new ExceptionHandler.ExceptionType.DataAccessException(ex.Message);
            }

        }
    }
}