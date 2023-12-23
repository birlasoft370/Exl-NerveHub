using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessLayer.CacheData;
using BPA.EmailManagement.BusinessLayer.ExternalRef.WorkAllocation;
using BPA.EmailManagement.ExchangeAdapter;
using BPA.EmailManagement.ServiceContract.ServiceContracts;
using BPA.EmailManagement.ServiceContract.ServiceContracts.WorkAllocation;
using BPA.Utility;
using Microsoft.Exchange.WebServices.Data;
using System.Collections;
using System.Data;

namespace BPA.EmailManagement.BusinessLayer.ExchangeData
{
    /// <summary>
    /// EWS Exchange Abstract Class
    /// </summary>
   // [ExceptionShielding("WCF Exception Shielding")]
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public abstract class AbstractOffice : IMailService
    {
        private string _extendedPropertyName = "BPAExtendedProperty";
        private Guid _myPropertySetId = new Guid("{20B5C09F-7CAD-44c6-BDBF-8FCBEEA08544}"); // Guid.NewGuid();
        private string _extendedeEssageID = "--";
        bool isenableTrace = false;
        IList<VendorDetail> venderdetailList = new List<VendorDetail>();

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public void Dispose()
        {

        }

        public Hashtable GetMailFolderList(BEMailConfiguration oMailConfig, BETenant oTenant)
        {
            Hashtable hshTable = new Hashtable();
            ExchangeService service = null;
            FindFoldersResults fdr = null;
            try
            {

                using (EmailObjectCache omail = new EmailObjectCache())
                {
                    service = omail.ConnectExchnageServer(oMailConfig, oTenant);
                }
                string logstr = "  GetMailFolderList Start:  Campaign : " + oMailConfig.iCampaignID;

               // EHLogger.WriteLog(logstr, EHLogger.ApplicationLog.EMSServiceLog);
                switch (oMailConfig.MailBoxType)
                {


                    case emailType.PublicFolder:
                        fdr = MailExchangeAdapter.GetFoldersList(service, WellKnownFolderName.PublicFoldersRoot);
                        break;
                    case emailType.Primary:

                        fdr = MailExchangeAdapter.GetFoldersList(service, WellKnownFolderName.MsgFolderRoot);
                        break;
                    case emailType.SharedMailbox:
                        Mailbox mb = new Mailbox(oMailConfig.sEmailID.TrimEnd());
                        FolderId fid2 = new FolderId(WellKnownFolderName.MsgFolderRoot, mb);
                        fdr = MailExchangeAdapter.GetFoldersList(service, fid2);
                        break;
                    default:
                        break;
                }

                foreach (var strval in fdr)
                {
                    if (strval.ChildFolderCount > 0)
                    {
                        hshTable.Add(strval.Id.ToString(), strval);
                    }
                    else
                    {
                        hshTable.Add(strval.Id.ToString(), strval);
                    }
                }

                string logstr1 = "  GetMailFolderList End:  Campaign : " + oMailConfig.iCampaignID;

                //EHLogger.WriteLog(logstr1, EHLogger.ApplicationLog.EMSServiceLog);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                service = null;
                fdr = null;
            }
            return hshTable;
        }

        public Hashtable GetFolderList(BEMailConfiguration oMailConfig, string folderID, string rootFolderName, BETenant oTenant)
        {
            ExchangeService service = null;
            FolderId fid = null;
            FindFoldersResults fdr = null;
            Hashtable hshTable = new Hashtable();

            try
            {
                using (EmailObjectCache omail = new EmailObjectCache())
                {
                    service = omail.ConnectExchnageServer(oMailConfig, oTenant);
                }

                if (oMailConfig.MailBoxType == emailType.SharedMailbox)
                {
                    if (ValidationRegex.IsEmail(folderID))
                    {
                        Mailbox mb = new Mailbox(oMailConfig.sEmailID.TrimEnd());
                        FolderId fid2 = new FolderId(WellKnownFolderName.MsgFolderRoot, mb);
                        fdr = MailExchangeAdapter.GetFoldersList(service, fid2);
                    }
                    else
                    {
                        fdr = MailExchangeAdapter.GetFoldersList(service, folderID);
                    }
                }
                else
                {
                    fid = new FolderId(folderID);
                    fdr = MailExchangeAdapter.GetFoldersList(service, fid);
                }

                foreach (var strval in fdr)
                {
                    hshTable.Add(strval.Id.ToString(), strval);
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }

            service = null;
            return hshTable;
        }

        public virtual bool DownloadBulkMail(BEMailConfiguration mailConfig, out int ExceptionValidation, BETenant oTenant)
        {

            //SWM.ServiceContract.ServiceContracts.Unbilled.IUnbilledService
            //EHLogger.WriteLog("DownLoad Started CampaignID:" +  mailConfig.iCampaignID.ToString() + @"\n MailConfig: " + mailConfig.iMailConfigID +" MailBoxName:" + mailConfig.sMailBoxName, EHLogger.ApplicationLog.EMSServiceLog);
            //BPA.Utility.BPALogEvents.WriteLog("Memory Usage:" + Convert.ToString(GC.GetTotalMemory(false)));
            ExchangeService service = null;
            IList<BEMailReceivedDateTime> lMailReceive = null;
            SearchFilter sf = null;
            ItemView view = null;
            FindItemsResults<Item> findResults = null;
            ExceptionValidation = 0;
            ExtendedPropertyDefinition myExtendedPropertyDefinition = null;
            PropertySet requestedPropertySet = null;
            int iMailFolderChoice = 0;
            try
            {
                using (EmailObjectCache ObjEOC = new EmailObjectCache())
                {
                    service = ObjEOC.ConnectExchnageServer(mailConfig, oTenant);
                }

                if (service != null)
                {
                    if (mailConfig.oMailfolderdetails != null && mailConfig.oMailfolderdetails.Count > 0)
                    {

                        foreach (BEMailfolderdetails row in mailConfig.oMailfolderdetails)
                        {

                            if (row.bIngestion)
                            {
                                using (MailfolderCache oMailConfig = new MailfolderCache())
                                {
                                    lMailReceive = oMailConfig.GetCacheItems(mailConfig, row.MailFolderDetailID, oTenant);
                                }
                                TimeZoneInfo tz = TimeZoneInfo.Local;
                                if (mailConfig.oTimeZone != null)
                                {
                                    if (!string.IsNullOrEmpty(mailConfig.oTimeZone.sTimeZoneID))
                                    {
                                        tz = TimeZoneInfo.FindSystemTimeZoneById(mailConfig.oTimeZone.sTimeZoneID);
                                    }
                                }
                                int iReceiveCount = 0;
                                if (lMailReceive == null)
                                {
                                    string logstr = "  lMailReceive :  Null" + tz.DisplayName +
                                   "  Campaign : " + mailConfig.iCampaignID;

                                   // EHLogger.WriteLog(logstr, EHLogger.ApplicationLog.EMSServiceLog); //issue
                                }
                                else
                                {
                                    iReceiveCount = lMailReceive.Count;
                                    string logstr = "  lMailReceive : " + lMailReceive[0].dReceivedDateTime.ToString() +
                                  "  Campaign : " + mailConfig.iCampaignID;

                                   // EHLogger.WriteLog(logstr, EHLogger.ApplicationLog.EMSServiceLog);
                                }


                                DateTime dRecievedate = new DateTime();
                                for (int i = 0; i < iReceiveCount; i++)
                                {
                                    DateTime dt = lMailReceive[i].dReceivedDateTime;
                                    if (lMailReceive[i].iMailFolderDetailID == 0)
                                    {
                                        dRecievedate = DateTimeTimeZoneConversion.AdjustTimeZone(dt, mailConfig.sServerTimeZone, tz.StandardName);
                                    }
                                    else
                                    {
                                        iMailFolderChoice = lMailReceive[i].iDSOBJChoiceID;
                                    }
                                    //isenableTrace=bool.Parse(AppSettingsValues.GetSetting("enableTrace"));
                                    //if (isenableTrace)
                                    //{
                                    //}
                                }
                                tz = null;
                                sf = new SearchFilter.SearchFilterCollection(LogicalOperator.And, new SearchFilter.IsGreaterThan(EmailMessageSchema.DateTimeReceived, dRecievedate));
                                view = new ItemView(int.MaxValue);
                                view.OrderBy.Add(ItemSchema.DateTimeReceived, SortDirection.Ascending);
                                // findResultsExt = service.FindItems(row.sMailFolderID, view);

                                myExtendedPropertyDefinition = new ExtendedPropertyDefinition(_myPropertySetId, _extendedPropertyName, MapiPropertyType.String);
                                requestedPropertySet = new PropertySet(BasePropertySet.FirstClassProperties, myExtendedPropertyDefinition);
                                view.PropertySet = requestedPropertySet;

                                if (string.IsNullOrEmpty(row.sMailFolderID))
                                {
                                    string logstr = "  Mail folder ID is not configured" +
                                   "  Campaign : " + mailConfig.iCampaignID;

                                    // EHLogger.WriteLog(logstr, EHLogger.ApplicationLog.EMSServiceLog);
                                    // throw new BPA.ExceptionHandler.ExceptionType.BusinessLogicCustomException("Mail folder ID is not configured");/issue
                                }
                                findResults = MailExchangeAdapter.FindItems(service, row.sMailFolderID, sf, view);
                                //switch (mailConfig.MailBoxType)
                                //   {
                                //       case emailType.Primary:
                                //               findResults = MailExchangeAdapter.FindItems(service, row.sMailFolderID, sf, view);

                                //           break;
                                //       case emailType.PublicFolder:
                                //               findResults = MailExchangeAdapter.FindItems(service, row.sMailFolderID, sf, view);
                                //           break;
                                //       case emailType.SharedMailbox:
                                //               Mailbox mb = new Mailbox(mailConfig.sEmailID.TrimEnd());
                                //               ItemView itemView = new ItemView(10000);
                                //               FolderId sharedMailbox = new FolderId(WellKnownFolderName.Inbox, mb);
                                //               findResults = service.FindItems(sharedMailbox, sf, itemView);
                                //           break;
                                //       default:
                                //           break;
                                //   }


                                if (findResults != null)
                                {
                                    if (findResults.Items.Count > 0)
                                    {
                                        string TableName = "";
                                        string sWorkList = "";
                                        using (BLWorkItems IStore = new BLWorkItems(mailConfig.iCampaignID, oTenant))
                                        {
                                            if (IStore != null)
                                            {
                                                TableName = IStore.GetTableNameAndVersion.Split(',')[0];
                                            }
                                            else
                                            {
                                                string logstr3 = " IStore is null " + "  Campaign : " + mailConfig.iCampaignID;
                                               // EHLogger.WriteLog(logstr3, EHLogger.ApplicationLog.EMSServiceLog); // issue
                                            }
                                        }
                                        //string logstr = "  findResults Count : " + findResults.Items.Count.ToString() +
                                        //    "  Campaign : " + mailConfig.iCampaignID + " TableName :" + TableName;
                                        //EHLogger.WriteLog(logstr, EHLogger.ApplicationLog.EMSServiceLog);
                                        ReadBulkMails(mailConfig, service, findResults, lMailReceive, row, out dRecievedate, requestedPropertySet, TableName, out sWorkList, oTenant);

                                    }
                                }

                            }
                        }
                    }
                }
                else
                {
                    //When password is expired then this code is executed. 
                    //To update the flag status, so that account doesnt gets locked.
                    //Right now this code is commented, later on we need to uncomment it.

                    //EwsShared.Errormail(emailid, userid, spwd, "Mail can’t be uploaded Please change password".Trim(), campID);
                    //EMSService_Client.MailFlagUpdate(userid, campID);
                    //foreach (PoolingValue item in MailConfigurationShared.PoolValue)
                    //{
                    //    if (Convert.ToString(item.CampaignID) == campID && item.sUserID == userid)
                    //        item.bPwdExFlag = true;
                    //}
                    //throw new Exception("Need Password changed :___" + emailid);


                }
                // EHLogger.WriteLog("DownLoad Completed CampaignID:" + mailConfig.iCampaignID.ToString() + @"\n MailConfig: " + mailConfig.iMailConfigID + " MailBoxName:" + mailConfig.sMailBoxName, EHLogger.ApplicationLog.EMSServiceLog );

            }
            catch (Microsoft.Exchange.WebServices.Data.ServiceResponseException ex)
            {

                if (ex.Message.Contains("The remote server returned an error: (401) Unauthorized."))
                {
                    ExceptionValidation = 1;
                }
                else if (ex.Message.Contains("The specified object was not found in the store."))
                {
                    ExceptionValidation = 2;
                }
                else if (ex.Message.Contains("SMTP address has no mailbox associated with it"))
                {
                    ExceptionValidation = 3;
                }
                else if (ex.Message.Contains("Mailbox has exceeded maximum mailbox size"))
                {
                    ExceptionValidation = 4;
                }
                else
                {
                    string str = "Error: " + ex.Message + " DownLoad Error CampaignID:" + mailConfig.iCampaignID.ToString() + @" MailConfig: " + mailConfig.iMailConfigID + " MailBoxName:" + mailConfig.sMailBoxName;
                    //PolicyBasedExceptionHandler.HandleException(PolicyBasedExceptionHandler.PolicyName.BusinessLogicExceptionPolicy, ex, str); // issue

                }
                using (IWorkUploadService oSendErrorMail = new BLWorkUpload())
                {
                    oSendErrorMail.SendErrorMail("", "Error occurred during DownlaodBulk Mail99999" + ex.Message.ToString(), mailConfig.iCampaignID, DateTime.Now.ToString(), "", oTenant);
                }
            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("The remote server returned an error: (401) Unauthorized."))
                {
                    ExceptionValidation = 1;
                }
                else if (ex.Message.Contains("The specified object was not found in the store."))
                {
                    ExceptionValidation = 2;
                }
                else if (ex.Message.Contains("SMTP address has no mailbox associated with it"))
                {
                    ExceptionValidation = 3;
                }
                else if (ex.Message.Contains("Mailbox has exceeded maximum mailbox size"))
                {
                    ExceptionValidation = 4;
                }
                else
                {
                    string str = "Error: " + ex.Message + " DownLoad Error CampaignID:" + mailConfig.iCampaignID.ToString() + @" MailConfig: " + mailConfig.iMailConfigID + " MailBoxName:" + mailConfig.sMailBoxName;
                   // PolicyBasedExceptionHandler.HandleException(PolicyBasedExceptionHandler.PolicyName.BusinessLogicExceptionPolicy, ex, str); // issue
                }
                using (IWorkUploadService oSendErrorMail = new BLWorkUpload())
                {
                    oSendErrorMail.SendErrorMail("", "Error occurred during DownLoad Error99999" + ex.Message.ToString(), mailConfig.iCampaignID, DateTime.Now.ToString(), "", oTenant);
                }
            }

            finally
            {
                service = null;
                sf = null;
                view = null;
                findResults = null;
                requestedPropertySet = null;
                myExtendedPropertyDefinition = null;
                lMailReceive = null;

            }
            return true;
        }

        private void ReadBulkMails(BEMailConfiguration mailConfig, ExchangeService service, FindItemsResults<Item> findResults, IList<BEMailReceivedDateTime> lMailReceive, BEMailfolderdetails row, out DateTime dUpdateRecieveTime, PropertySet requestedPropertySet, string TableName, out string sWorkList, BETenant oTenant)
        {
            string mailBodyDraft = string.Empty;
            string display = "";
            int cntall = 0;
            int iMailChoice = 0;
            int iMailFolderChoice = 0;
            sWorkList = "";
            string sMailID = "";
            DataSet dsVendor = null;
            dUpdateRecieveTime = new DateTime();
            DateTime dMailTime = new DateTime();
            DataTable datatable = BLMailHelper.getUploadTable(mailConfig.oCampaignAdditionFields, mailConfig.bSensitivityEnabled, mailConfig.bNeedTicketEnabled, mailConfig.bDuringUploadEnabled);
            DataTable datatableWork = BLMailHelper.getUploadTable(mailConfig.oCampaignAdditionFields, mailConfig.bSensitivityEnabled, mailConfig.bNeedTicketEnabled, mailConfig.bDuringUploadEnabled);
            DataTable FinalDataTable = BLMailHelper.getUploadTable(mailConfig.oCampaignAdditionFields, mailConfig.bSensitivityEnabled, mailConfig.bNeedTicketEnabled, mailConfig.bDuringUploadEnabled);
            ExtendedPropertyDefinition htmlBodyProperty = null;
            IList<BusinessEntity.BEMailHelper> ListMailData = new List<BusinessEntity.BEMailHelper>();
            PropertySet propertySet = null;
            bool containsWorkID = false;
            try
            {
                if (mailConfig.iUploadBy > 0)
                {
                    mailConfig.iCreatedBy = mailConfig.iUploadBy;
                }
                TimeZoneInfo tz = TimeZoneInfo.Local;
                if (!string.IsNullOrEmpty(mailConfig.oTimeZone.sTimeZoneID))
                {
                    tz = TimeZoneInfo.FindSystemTimeZoneById(mailConfig.oTimeZone.sTimeZoneID);
                }
                //if (mailConfig.AutoReply)
                //{
                //    var draft = from oDraft in  mailConfig.oMailTemplate where oDraft.iMailTemplateId == mailConfig.MailTemplateID
                //                select new {draftBody = oDraft.sMailTemplate };
                //}
                int iReceiveCount = lMailReceive.Count;
                for (int i = 0; i < iReceiveCount; i++)
                {
                    if (lMailReceive[i].iMailFolderDetailID == 0)
                    {
                        iMailChoice = lMailReceive[i].iDSOBJChoiceID;
                    }
                    else
                    {
                        iMailFolderChoice = lMailReceive[i].iDSOBJChoiceID;
                        DateTime RecievedTime = lMailReceive[i].dReceivedDateTime;
                        dMailTime = DateTimeTimeZoneConversion.AdjustTimeZone(RecievedTime, mailConfig.sServerTimeZone, tz.StandardName);
                        dUpdateRecieveTime = DateTimeTimeZoneConversion.AdjustTimeZone(RecievedTime, mailConfig.sServerTimeZone, tz.StandardName);
                    }
                }
                if (dMailTime < DateTime.Now.AddYears(-10))
                {
                   // BPA.ExceptionHandler.Logging.EHLogger.WriteLog("Campaing ID: " + mailConfig.iCampaignID.ToString() + Environment.NewLine + "MailConfigID :" + mailConfig.iMailConfigID + Environment.NewLine + "Problem in Email Configrautiom", ExceptionHandler.Logging.EHLogger.ApplicationLog.Appfabric);//issue
                    return;
                }
                if (findResults != null)
                {
                    string BatchName = "";
                    int BatchID = 0;
                    using (IWorkUploadService oWorkUpload = new BLWorkUpload())
                    {
                        BatchName = BLMailHelper.GetBatchCodeName(mailConfig.BatchFrequency);
                        BatchID = oWorkUpload.GetBatchIdValue(mailConfig.iCampaignID, BatchName, oTenant);
                        string logstr = "  BatchID : " + BatchID.ToString() +
                                                                    "  Campaign : " + mailConfig.iCampaignID;
                      //  EHLogger.WriteLog(logstr, EHLogger.ApplicationLog.EMSServiceLog); //issue
                    }

                    foreach (EmailMessage msg in findResults)
                    {
                        if (msg == null)
                        {
                            //throw new BPA.ExceptionHandler.ExceptionType.BusinessLogicCustomException(String.Format(@"msg is null Found \n Subject: {0} \n Camp ID: {1} ", "Null", mailConfig.iCampaignID));//issue
                        }
                        if (mailConfig.bOutofOfficeEnabled)
                        {
                            string strm = msg.Subject;

                            if (msg.ItemClass != null)
                            {
                                if (msg.ItemClass.ToString().ToUpper() != "IPM.NOTE")
                                {
                                    continue;
                                }
                            }

                            char[] delimiterChars = { ',', '.', ':', '\t' };
                            if (mailConfig.sOutofOffice != null)
                            {
                                string[] strOutofOffice = mailConfig.sOutofOffice.ToUpper().Split(delimiterChars);
                                if (strOutofOffice != null)
                                    if (strOutofOffice.Count() > 0)
                                    {
                                        var fr = from c in strOutofOffice where msg.Subject.ToUpper().Contains(c.ToString()) select c;
                                        if (fr.Count() > 0) continue;
                                    }
                            }
                        }
                        if (msg.DateTimeReceived == null)
                        {
                            // EHLogger.WriteLog("DateTimeReceived field is empty " + mailConfig.sMailBoxName, EHLogger.ApplicationLog.EMSServiceLog);
                            continue;
                        }
                        if (msg.DateTimeReceived <= dMailTime) continue;
                        string strcc = "";
                        string strto = "";

                        if (msg.Subject == null)
                        {
                            display = "";
                        }
                        else
                        {
                            display = (msg.Subject.Length < 100) ? msg.Subject.Substring(0, msg.Subject.Length - 1) : msg.Subject.Substring(0, 100);
                        }

                        cntall++;

                        htmlBodyProperty = new ExtendedPropertyDefinition(0x1013, MapiPropertyType.Binary);
                        propertySet = new PropertySet(BasePropertySet.FirstClassProperties);
                        propertySet.RequestedBodyType = BodyType.HTML;
                        if (msg.Id == null)
                        {
                            //throw new BPA.ExceptionHandler.ExceptionType.BusinessLogicCustomException(String.Format(@"Message Id not Found \n Subject: {0} \n Mail Recieve Date: {1} ", msg.Subject, msg.DateTimeReceived));
                        }
                        EmailMessage message = MailExchangeAdapter.GetEmailMessage(service, msg.Id, propertySet);
                        if (message == null)
                        {
                            //throw new BPA.ExceptionHandler.ExceptionType.BusinessLogicCustomException(String.Format(@"message is null Found  \n Camp ID: {0} ", mailConfig.iCampaignID));
                        }
                        DataRow dr = null;
                        if (mailConfig.bSendmailQuiqueIdentified)
                        {
                            containsWorkID = BLMailHelper.isExistWorkDetails((message.Subject != null) ? message.Subject.Trim() : "");
                        }
                        if (containsWorkID)
                        {
                            emaildetail odetails = BLMailHelper.GetWorkIDFromSubject((message.Subject != null) ? message.Subject.Trim() : "");
                            if (mailConfig.iStoreID == odetails.iDStoreID)
                            {
                                dr = datatableWork.NewRow();
                                dr["ParentWorkID"] = odetails.iWorkID;
                                //if(!mailConfig.AutoReply)
                                // dr["EmployeeNumber"] = odetails.iEmployeeID;                             
                            }
                            else
                            {
                                containsWorkID = false;
                                dr = datatable.NewRow();
                            }
                            odetails = null;
                        }
                        else
                        {
                            dr = datatable.NewRow();
                        }
                        dr["MailConfigID"] = iMailChoice;
                        dr["MailFolderid"] = iMailFolderChoice;
                        dr["MoveMailFolderID"] = 0;
                        dr["MailReceivedDate"] = message.DateTimeReceived;
                        //get the update recieved datetime 
                        //if (message.DateTimeReceived > dUpdateRecieveTime)
                        //{
                        //    dUpdateRecieveTime = message.DateTimeReceived;
                        //}


                        foreach (var str in message.CcRecipients)
                        {
                            strcc += str.Address + "; ";
                        }

                        dr["MailCC"] = string.IsNullOrEmpty(strcc.Trim().TrimEnd(';')) ? null : strcc.Trim().TrimEnd(';').Trim();
                        foreach (var strt in message.ToRecipients)
                        {
                            strto += strt.Address + "; ";
                        }

                        if (message.ReceivedBy != null)
                            dr["ReceivedMail"] = message.ReceivedBy.Address.Trim();
                        else
                            dr["ReceivedMail"] = null;
                        dr["MailTo"] = strto.Trim().TrimEnd(';');
                        dr["MailFrom"] = (message.From != null) ? message.From.Address.Trim() : "";
                        dr["MailSubject"] = (message.Subject != null) ? message.Subject.Trim() : "";
                        //this code for zurich ticketing
                        if (mailConfig.bNeedTicketEnabled || mailConfig.bDuringUploadEnabled)
                        {
                            string[] getTop = message.Subject.Trim().ToString().Replace("“", "").Replace("”", "").Trim().Split('#');
                            if (getTop.Length > 2)
                            {
                                string stringToCheck = "CCT-";
                                int ZeroNo = 8;
                                if (!string.IsNullOrEmpty(mailConfig.strTicketName))
                                    stringToCheck = mailConfig.strTicketName.Trim() + "-";

                                if (mailConfig.iNeedTicketLenth > 0)
                                    ZeroNo = mailConfig.iNeedTicketLenth;
                                for (int i = 0; i < getTop.Length; i++)
                                {
                                    if (!string.IsNullOrEmpty(getTop[i].ToString()))
                                    {
                                        if (getTop[i].ToString().Contains(stringToCheck))
                                        {
                                            if (getTop[i].ToString().Trim().IndexOf("-") != -1)
                                            {
                                                string[] str = getTop[i].ToString().Trim().Split('-');
                                                if (str.Length > 1)
                                                {
                                                    if (!string.IsNullOrEmpty(str[1].ToString()))
                                                    {
                                                        if (str[1].ToString().Length == ZeroNo && str[0].ToString().Length == stringToCheck.Length - 1)
                                                        {
                                                            int j;
                                                            if (int.TryParse(str[1].ToString(), out j))
                                                            {
                                                                //dr = datatableWork.NewRow();
                                                                dr["TOPID"] = Convert.ToInt32(str[1].ToString());
                                                                dr["ReferenceNumber"] = getTop[i].ToString();

                                                                dr["ParentWorkID"] = Convert.ToInt32(str[1].ToString());
                                                                //dr["ParentWorkId"] = Convert.ToInt64(str[1].ToString());
                                                                // SetDataTableValue(mydt, dr, Convert.ToInt32(CampID), Convert.ToInt32(str[1].ToString()));
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        dr["AttachmentsCount"] = (message.Attachments != null) ? message.Attachments.Count : 0;

                        if (mailConfig.bSensitivityEnabled)
                        {
                            dr["Sensitivity"] = (message.Sensitivity != null) ? message.Sensitivity.ToString() : "";
                        }
                        dr["conversationid"] = (mailConfig.iMailServerTypeID == EmailServerType.Exchange2007SP1) ? "" : (message.ConversationId != null) ? message.ConversationId.UniqueId : "";

                        dr["Importance"] = (message.Importance != null) ? message.Importance.ToString().Trim() : "";
                        string logstr = "  message.Importance : " + message.Importance.ToString() +
                                                                   "  Campaign : " + mailConfig.iCampaignID;
                       // EHLogger.WriteLog(logstr, EHLogger.ApplicationLog.EMSServiceLog);
                        if (message.Categories != null)
                        {
                            if (message.Categories.Count > 0)
                                dr["CategoriesVal"] = message.Categories[0].ToString();
                            else
                                dr["CategoriesVal"] = "None";
                        }

                        if (mailConfig.bReadMailBodyEnabled)
                        {
                            PropertySet proSet = new PropertySet(BasePropertySet.FirstClassProperties, ItemSchema.TextBody, ItemSchema.MimeContent);
                            EmailMessage msg1 = message;
                            msg1.Load(proSet);
                            string textMessg = msg1.TextBody;
                            dsVendor = BLMailHelper.GetVendorColumnDetails(mailConfig.iCampaignID.ToString());
                            AddVendorRelatedData(datatable, dsVendor);
                            if (!datatable.Columns.Contains("TimeZone"))
                                datatable.Columns.Add("TimeZone");
                            if (!datatable.Columns.Contains("DateInitiated"))
                                datatable.Columns.Add("DateInitiated");
                            if (dsVendor != null)
                                PopulateDataRowForVendorDetails(dr, dsVendor.Tables[0].Columns, dsVendor.Tables[1].Columns, dsVendor.Tables[2].Columns, textMessg, message.Subject);

                            DataTable dtMailConfigUserOffSet = null;

                            using (IWorkUploadService obj = new BLWorkUpload())
                            {
                                dtMailConfigUserOffSet = obj.GetUserOffSet(oTenant);
                            }
                            string frmEmailId = (message.From != null) ? message.From.Address.Trim() : "";
                            string expression = "CampaignId =" + mailConfig.iCampaignID.ToString() + " AND EmailId=  '" + frmEmailId + "'";
                            DataRow[] selectedRows = dtMailConfigUserOffSet.Select(expression);
                            if (selectedRows.Count() > 0)
                            {
                                dr["TimeZone"] = selectedRows[0]["TimeZone"];
                                DateTime mailDate;
                                if (DateTime.TryParse(dr["MailReceivedDate"].ToString(), out mailDate))
                                {
                                    double offSet = 0;
                                    if (Double.TryParse(selectedRows[0]["OffSet"].ToString(), out offSet))
                                    {
                                        mailDate = mailDate.AddMinutes(offSet);
                                    }
                                    else
                                    {
                                        mailDate = new DateTime();
                                    }
                                }
                                dr["DateInitiated"] = mailDate;
                            }

                        }
                        Int32 attachCount = 0;
                        foreach (Microsoft.Exchange.WebServices.Data.Attachment attachment in message.Attachments)
                        {

                            if (mailConfig.iMailServerTypeID == EmailServerType.Exchange2007SP1)
                            {
                                if (string.IsNullOrEmpty(attachment.ContentId) && string.IsNullOrEmpty(attachment.ContentType)
                                    ||
                                    (!string.IsNullOrEmpty(attachment.ContentId) && string.IsNullOrEmpty(attachment.ContentType))
                                    )
                                {
                                    attachCount++;
                                }
                            }
                            else
                            {
                                if (!attachment.IsInline)
                                    attachCount++;
                            }



                        }
                        dr["AttachmentsCount"] = attachCount;
                        dr["Getmaildetails"] = msg.Id ?? "";
                        if (msg.Id != null)
                        {
                            if (containsWorkID)
                            {
                                datatableWork.Rows.Add(dr);
                            }
                            else
                            {
                                datatable.Rows.Add(dr.ItemArray);
                            }
                        }

                        if (mailConfig.AutoReply && !containsWorkID)
                        {
                           // BPA.ExceptionHandler.Logging.EHLogger.WriteLog("Campaing ID: " + mailConfig.iCampaignID.ToString() + Environment.NewLine + "AutoReply: " + mailConfig.AutoReply.ToString() + Environment.NewLine + "MailConfigID :" + mailConfig.iMailConfigID, ExceptionHandler.Logging.EHLogger.ApplicationLog.Appfabric);
                            //sMailID += "'"+message.Id + "'|";
                            sMailID += message.Id + "|";

                            BEMailHelper MailData = new BEMailHelper();
                            MailData.sMailUniqueID = message.Id.ToString();
                            MailData.MsgBody = message.Body;
                            MailData.Msg_DateReceived = message.DateTimeReceived;
                            MailData.sMailUniqueID = message.Id.ToString();
                            MailData.replyType = ReplyType.Reply;
                            MailData.FromAddress = (message.From != null) ? message.From.Address : "";
                            MailData.MsgSender = (message.Sender != null) ? message.Sender.Address : "";
                            MailData.MailToRecipients = new List<string>();
                            MailData.MailCCRecipients = new List<string>();
                            MailData.MailBCCRecipients = new List<string>();
                            if (message.Sender != null)
                            {
                                MailData.MsgSenderDisplayName = MailData.MsgSender;
                            }
                            if (message.From != null)
                            {
                                MailData.FromAddressWithDisplayName = MailData.FromAddress;
                            }
                            foreach (var receipent in message.ToRecipients)
                            {
                                if (receipent.Address.ToUpper().ToString().Trim().Equals(mailConfig.sEmailID.ToUpper().ToString().Trim())) continue;
                                MailData.MailToRecipients.Add(receipent.Address);
                            }

                            foreach (var receipent in message.CcRecipients)
                            {
                                if (receipent.Address.ToUpper().ToString().Trim().Equals(mailConfig.sEmailID.ToUpper().ToString().Trim())) continue;
                                MailData.MailCCRecipients.Add(receipent.Address);
                            }

                            foreach (var receipent in message.BccRecipients)
                            {
                                if (receipent.Address.ToUpper().ToString().Trim().Equals(mailConfig.sEmailID.ToUpper().ToString().Trim())) continue;
                                MailData.MailBCCRecipients.Add(receipent.Address);
                            }

                            MailData.OMailAttachment = new List<MailAttachment>();
                            foreach (var attachment in message.Attachments)
                            {
                                MailAttachment oMessage = new MailAttachment
                                {
                                    IsExistingAttachment = true
                                };
                                if (mailConfig.iMailServerTypeID == EmailServerType.Exchange2007SP1)
                                {
                                    if ((!string.IsNullOrEmpty(attachment.ContentId) && !string.IsNullOrEmpty(attachment.ContentType)) ||
                                        (!string.IsNullOrEmpty(attachment.ContentId) && string.IsNullOrEmpty(attachment.ContentType)))
                                    {
                                        oMessage.IsLineAttachment = true;
                                    }
                                    else
                                    {
                                        oMessage.IsLineAttachment = false;
                                    }
                                }
                                else
                                {
                                    oMessage.IsLineAttachment = attachment.IsInline;

                                }
                                if (oMessage.IsLineAttachment)
                                {
                                    if (attachment is FileAttachment)
                                    {
                                        FileAttachment FA = attachment as FileAttachment;
                                        using (MemoryStream ms = new MemoryStream())
                                        {
                                            FA.Load(ms);
                                            oMessage.AttachmentData = ms.ToArray();
                                            oMessage.AttachmentsName = attachment.Name;
                                            oMessage.ContentId = attachment.ContentId;
                                            oMessage.ContentType = attachment.ContentType;
                                            oMessage.AttachmentType = AttachementType.FileAttachment;
                                        }
                                        FA = null;
                                    }
                                    else
                                    {
                                        ItemAttachment itemAttachment = attachment as ItemAttachment;
                                        itemAttachment.Load(new PropertySet(EmailMessageSchema.MimeContent));
                                        byte[] ContentBytes = itemAttachment.Item.MimeContent.Content;
                                        using (MemoryStream ms = new MemoryStream())
                                        {
                                            oMessage.AttachmentData = ContentBytes;
                                            oMessage.AttachmentsName = attachment.Name;
                                            oMessage.ContentId = attachment.ContentId;
                                            oMessage.ContentType = attachment.ContentType;
                                            oMessage.AttachmentType = AttachementType.ItemAttachment;
                                        }
                                        itemAttachment = null;
                                        ContentBytes = null;
                                    }
                                    MailData.OMailAttachment.Add(oMessage);
                                }

                                oMessage = null;
                            }

                            ListMailData.Add(MailData);
                            MailData = null;
                        }
                        containsWorkID = false;
                        message = null;
                        propertySet = null;
                        htmlBodyProperty = null;


                        datatable = BLMailHelper.AddData(datatable, mailConfig.iCreatedBy, BatchID, BatchName, mailConfig.bFreshRequiredEnabled, mailConfig.bNeedTicketEnabled, mailConfig.bDuringUploadEnabled);
                        FinalDataTable = datatable.Copy();
                        datatableWork = BLMailHelper.AddData(datatableWork, mailConfig.iCreatedBy, BatchID, BatchName, mailConfig.bFreshRequiredEnabled, mailConfig.bNeedTicketEnabled, mailConfig.bDuringUploadEnabled);

                        if (mailConfig.bNeedTicketEnabled || mailConfig.bDuringUploadEnabled)
                        {
                            if (dr["ParentWorkID"] == DBNull.Value)
                                FinalDataTable.Merge(BLMailHelper.GetWorkID(datatableWork, TableName, mailConfig.oTenant, out sWorkList, mailConfig.oCampaignAdditionFields));
                            else
                                FinalDataTable = BLMailHelper.GetWorkID(datatable, TableName, mailConfig.oTenant, out sWorkList, mailConfig.oCampaignAdditionFields);
                        }
                        else
                        {
                            FinalDataTable.Merge(BLMailHelper.GetWorkID(datatableWork, TableName, mailConfig.oTenant, out sWorkList, mailConfig.oCampaignAdditionFields));
                        }
                    }

                    if (FinalDataTable.Rows.Count > 0)
                    {
                        string logstr = "  BulkCopyDataUpload call : FinalDataTable.Rows.Count : " + FinalDataTable.Rows.Count.ToString() +
                                                                  "  Campaign : " + mailConfig.iCampaignID;
                        //EHLogger.WriteLog(logstr, EHLogger.ApplicationLog.EMSServiceLog);
                        BLMailHelper.BulkCopyDataUpload(mailConfig, FinalDataTable, row.MailFolderDetailID, TableName, oTenant, sWorkList);

                        using (MailfolderCache oMailConfig = new MailfolderCache())
                        {
                            oMailConfig.RemoveCache(mailConfig, row.MailFolderDetailID, oTenant);
                        }
                        string logstr1 = "  BulkCopyDataUpload Done : " +
                                                                 "  Campaign : " + mailConfig.iCampaignID;
                       // EHLogger.WriteLog(logstr1, EHLogger.ApplicationLog.EMSServiceLog);
                        int exceptionValidation = 0;
                        if (mailConfig.AutoReply && ListMailData.Count > 0)
                        {
                            sMailID = sMailID.Substring(0, sMailID.Length - 1);
                            //sMailID = sMailID.Insert(0, "'").Insert(sMailID.Length + 1, "'");
                            if (mailConfig.oMailTemplate.Count > 0)
                            {
                                mailBodyDraft = System.Net.WebUtility.HtmlDecode(mailConfig.oMailTemplate.Select(x => x.sMailTemplate).ToList()[0]);
                            }

                            var getAutoMailDT = BLMailHelper.GetWorkAutoMailID(TableName, mailConfig.oTenant, sMailID);

                            if (getAutoMailDT != null && getAutoMailDT.Rows.Count > 0)
                            {
                               // BPA.ExceptionHandler.Logging.EHLogger.WriteLog("Campaing ID: " + mailConfig.iCampaignID.ToString() + Environment.NewLine + "getAutoMailDT Calling: " + getAutoMailDT.Rows.Count + Environment.NewLine + "MailConfigID :" + mailConfig.iMailConfigID, ExceptionHandler.Logging.EHLogger.ApplicationLog.Appfabric);
                                foreach (BEMailHelper objMail in ListMailData)
                                {
                                    var x = getAutoMailDT.AsEnumerable().Where(p => p["Getmaildetails"].ToString() == objMail.sMailUniqueID).AsDataView();
                                    objMail.MsgBody = BLMailHelper.AddHeadersToMailBody(objMail, objMail.MsgBody);
                                    objMail.DraftMsgBody = string.Format(mailBodyDraft, x[0]["TOPID"].ToString());
                                    objMail.MsgSubject = "[####" + x[0]["TOPID"].ToString() + "|" + 0 + "|" + mailConfig.iStoreID + "####]" + x[0]["MailSubject"];
                                    //List<string> _StrToAdd = new List<string>();
                                    //List<string> _StrCCAdd = new List<string>();
                                    //_StrToAdd = objMail.MailToRecipients;
                                    //_StrCCAdd = objMail.MailCCRecipients;
                                    //string address = string.Empty;
                                    //objMail.MailToRecipients = new List<string>();
                                    objMail.MailBCCRecipients = new List<string>();
                                    //for (int i = 0; i < _StrToAdd.Count; i++)
                                    //{
                                    //    try
                                    //    {

                                    //        if (_StrToAdd[i].Contains("<"))
                                    //        {
                                    //            string st = _StrToAdd[i].ToString().Split(new string[] { "<" }, StringSplitOptions.None)[1].Split('>')[0].Trim();
                                    //            objMail.MailToRecipients.Add(st);
                                    //        }
                                    //        else
                                    //        {
                                    //            objMail.MailToRecipients.Add(_StrToAdd[i]);
                                    //        }

                                    //    }
                                    //    catch (Exception ex)
                                    //    {
                                    //        BPA.ExceptionHandler.Logging.EHLogger.WriteLog("Error In objMail.MailToRecipients.Add: " + ex.Message.ToString() + Environment.NewLine + "To Mail Address: " + _StrToAdd[i] + Environment.NewLine + "MailConfigID :" + mailConfig.iMailConfigID, ExceptionHandler.Logging.EHLogger.ApplicationLog.Appfabric);
                                    //    }
                                    //}

                                    //for (int i = 0; i < _StrCCAdd.Count; i++)
                                    //{
                                    //    try
                                    //    {

                                    //        if (_StrCCAdd[i].Contains("<"))
                                    //        {
                                    //            string st = _StrCCAdd[i].ToString().Split(new string[] { "<" }, StringSplitOptions.None)[1].Split('>')[0].Trim();
                                    //            objMail.MailCCRecipients.Add(st);
                                    //        }
                                    //        else
                                    //        {
                                    //            objMail.MailCCRecipients.Add(_StrCCAdd[i]);
                                    //        }

                                    //    }
                                    //    catch (Exception ex)
                                    //    {
                                    //        BPA.ExceptionHandler.Logging.EHLogger.WriteLog("Error In objMail.MailCCRecipients.Add: " + ex.Message.ToString() + Environment.NewLine + "CC Mail Address: " + _StrCCAdd[i] + Environment.NewLine + "MailConfigID :" + mailConfig.iMailConfigID, ExceptionHandler.Logging.EHLogger.ApplicationLog.Appfabric);
                                    //    }
                                    //}

                                    objMail.MailToRecipients.Add(objMail.FromAddress.Trim());
                                    //BPA.ExceptionHandler.Logging.EHLogger.WriteLog("objMail.FromAddress: " + mailConfig.sEmailID.ToString().Trim() + Environment.NewLine + "MailConfigID :" + mailConfig.iMailConfigID, ExceptionHandler.Logging.EHLogger.ApplicationLog.Appfabric);
                                    objMail.FromAddress = mailConfig.sEmailID.ToString().Trim();
                                    try
                                    {
                                        MailSend(objMail, mailConfig, out exceptionValidation, oTenant);
                                        string strexc = "Auto mail Send failure: ";
                                        switch (exceptionValidation)
                                        {
                                            case 1:
                                                strexc += "Login Failure";
                                                break;
                                            case 2:
                                                strexc += "Mail was not found or you do not have permission to read mail from " + objMail.FromAddress.Trim();
                                                break;
                                            case 3:
                                                strexc += "SMTP address has no mailbox associated with it";
                                                break;
                                            case 4:
                                                strexc += objMail.FromAddress.Trim() + " Mailbox has exceeded maximum mailbox size";
                                                break;
                                            case 5:
                                                strexc += @"Mail was not send, message has exceeded maximum allocated size.";
                                                break;
                                            case 6:
                                                strexc += @"One or more recipients are invalid.";
                                                break;
                                        }
                                        if (exceptionValidation > 0)
                                        {
                                            //BPA.ExceptionHandler.Logging.EHLogger.WriteLog(strexc, ExceptionHandler.Logging.EHLogger.ApplicationLog.Appfabric);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                       // BPA.ExceptionHandler.Logging.EHLogger.WriteLog("Error In Sending: " + ex.Message.ToString() + Environment.NewLine + "MailConfigID :" + mailConfig.iMailConfigID, ExceptionHandler.Logging.EHLogger.ApplicationLog.Appfabric);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (IWorkUploadService oSendErrorMail = new BLWorkUpload())
                {
                    oSendErrorMail.SendErrorMail("", "Error occurred during ReadBulkMails99999" + ex.Message.ToString(), mailConfig.iCampaignID, DateTime.Now.ToString(), "", oTenant);
                }
                throw ex;
            }
            finally
            {
                ListMailData = null;
                htmlBodyProperty = null;
                propertySet = null;
                datatable = null;
                datatableWork = null;
                FinalDataTable = null;
            }

        }

        void AddVendorRelatedData(DataTable myDataTable, DataSet dsVendor)
        {
            DataColumnCollection Vendor1ExpectedColumns = dsVendor.Tables[0].Columns;
            DataColumnCollection Vendor2ExpectedColumns = dsVendor.Tables[1].Columns;
            DataColumnCollection Vendor3ExpectedColumns = dsVendor.Tables[2].Columns;

            foreach (DataColumn column in dsVendor.Tables[0].Columns)
            {
                if (!myDataTable.Columns.Contains(column.ColumnName))
                    myDataTable.Columns.Add(column.ColumnName);
            }
            foreach (DataColumn column in dsVendor.Tables[1].Columns)
            {
                if (!myDataTable.Columns.Contains(column.ColumnName))
                    myDataTable.Columns.Add(column.ColumnName);
            }
            foreach (DataColumn column in dsVendor.Tables[2].Columns)
            {
                if (!myDataTable.Columns.Contains(column.ColumnName))
                    myDataTable.Columns.Add(column.ColumnName);
            }

        }

        private void PopulateDataRowForVendorDetails(DataRow dr, DataColumnCollection Vendor1ExpectedColumns, DataColumnCollection Vendor2ExpectedColumns, DataColumnCollection Vendor3ExpectedColumns, string bodyMessage, string subject)
        {
            ExtractVendorDataFromBody(bodyMessage);
            string caseNumber = "";
            #region Vendor1
            if (venderdetailList != null && venderdetailList.Count >= 1)
            {
                if (!string.IsNullOrEmpty(subject))
                {
                    if (subject.Contains("Claim Number:"))
                    {
                        caseNumber = subject.Substring(subject.IndexOf("Claim Number:") + 13, subject.Length - (subject.IndexOf("Claim Number:") + 13));
                    }
                    else if (subject.Contains("Case Number:"))
                    {
                        caseNumber = subject.Substring(subject.IndexOf("Case Number:") + 12, subject.Length - (subject.IndexOf("Case Number:") + 12));
                    }

                    caseNumber = caseNumber.Trim();
                    if (caseNumber.Length > 20)
                    {
                        caseNumber = caseNumber.Substring(0, 20);
                    }
                }

                if (venderdetailList[0] != null)
                {
                    VendorDetail v1 = venderdetailList[0];
                    foreach (DataColumn column in Vendor1ExpectedColumns)
                    {
                        if (column.ColumnName.Contains("ClaimNumber"))
                        {
                            dr[column.ColumnName] = caseNumber;
                        }
                        if (column.ColumnName.Contains("ClaimantName"))
                        {
                            dr[column.ColumnName] = v1.ClaimantName;
                        }
                        if (column.ColumnName.Contains("VendorName"))
                        {
                            dr[column.ColumnName] = v1.VendorName;
                        }
                        if (column.ColumnName.Contains("MDName"))
                        {
                            dr[column.ColumnName] = v1.MDName;
                        }

                        if (column.ColumnName.Contains("MDPhoneNumber"))
                        {
                            dr[column.ColumnName] = v1.MDPhoneNumber;
                        }

                        if (column.ColumnName.Contains("Diagnosis"))
                        {
                            dr[column.ColumnName] = v1.Diagnosis;
                        }

                        if (column.ColumnName.Contains("Working"))
                        {
                            dr[column.ColumnName] = v1.IsWorking;
                        }

                        if (column.ColumnName.Contains("NextMDAppointmentDate"))
                        {
                            dr[column.ColumnName] = v1.NextMDAppointmentDate;
                        }

                        if (column.ColumnName.Contains("TaskRequiredOne"))
                        {
                            dr[column.ColumnName] = v1.TaskRequired1;
                        }

                        if (column.ColumnName.Contains("TaskDetailsOne"))
                        {
                            dr[column.ColumnName] = v1.TaskDetails1;
                        }

                        if (column.ColumnName.Contains("DateOfServiceOne"))
                        {
                            dr[column.ColumnName] = v1.DateofService1;
                        }
                        if (column.ColumnName.Contains("AdditionalCommentsOne"))
                        {
                            dr[column.ColumnName] = v1.AdditionalComments1;
                        }

                        if (column.ColumnName.Contains("TaskRequiredTwo"))
                        {
                            dr[column.ColumnName] = v1.TaskRequired2;
                        }

                        if (column.ColumnName.Contains("TaskDetailsTwo"))
                        {
                            dr[column.ColumnName] = v1.TaskDetails2;
                        }
                        if (column.ColumnName.Contains("DateOfServiceTwo"))
                        {
                            dr[column.ColumnName] = v1.DateofService2;
                        }
                        if (column.ColumnName.Contains("AdditionalCommentsTwo"))
                        {
                            dr[column.ColumnName] = v1.AdditionalComments2;
                        }


                        if (column.ColumnName.Contains("TaskRequiredThree"))
                        {
                            dr[column.ColumnName] = v1.TaskRequired3;
                        }
                        if (column.ColumnName.Contains("TaskDetailsThree"))
                        {
                            dr[column.ColumnName] = v1.TaskDetails3;
                        }
                        if (column.ColumnName.Contains("DateOfServiceThree"))
                        {
                            dr[column.ColumnName] = v1.DateofService3;
                        }
                        if (column.ColumnName.Contains("AdditionalCommentsThree"))
                        {
                            dr[column.ColumnName] = v1.AdditionalComments3;
                        }

                        if (column.ColumnName.Contains("TaskRequiredFour"))
                        {
                            dr[column.ColumnName] = v1.TaskRequired4;
                        }

                        if (column.ColumnName.Contains("TaskDetailsFour"))
                        {
                            dr[column.ColumnName] = v1.TaskDetails4;
                        }
                        if (column.ColumnName.Contains("DateOfServiceFour"))
                        {
                            dr[column.ColumnName] = v1.DateofService4;
                        }
                        if (column.ColumnName.Contains("AdditionalCommentsFour"))
                        {
                            dr[column.ColumnName] = v1.AdditionalComments4;
                        }

                        //Start NCM2 
                        if (column.ColumnName.Contains("RecurrentTaskOne"))
                        {
                            dr[column.ColumnName] = v1.RecurrentTask1;
                        }

                        if (column.ColumnName.Contains("FollowupParameterOne"))
                        {
                            dr[column.ColumnName] = v1.FollowupParameter1;
                        }
                        if (column.ColumnName.Contains("MMAOne"))
                        {
                            dr[column.ColumnName] = v1.MMA1;
                        }
                        if (column.ColumnName.Contains("NCMOne"))
                        {
                            dr[column.ColumnName] = v1.NCM1;
                        }
                        if (column.ColumnName.Contains("IndicateAllRecipientsOne"))
                        {
                            dr[column.ColumnName] = v1.IndicateAllRecipients1;
                        }
                        if (column.ColumnName.Contains("InjuredWorkerOne"))
                        {
                            dr[column.ColumnName] = v1.INJUREDWORKER1;
                        }
                        if (column.ColumnName.Contains("AttorneyOne"))
                        {
                            dr[column.ColumnName] = v1.ATTORNEY1;
                        }

                        if (column.ColumnName.Contains("EmployerOne"))
                        {
                            dr[column.ColumnName] = v1.EMPLOYER1;
                        }

                        if (column.ColumnName.Contains("MedicalProviderOne"))
                        {
                            dr[column.ColumnName] = v1.MEDICALPROVIDER1;
                        }
                        if (column.ColumnName.Contains("ProviderNameOne"))
                        {
                            dr[column.ColumnName] = v1.PROVIDERNAME1;
                        }

                        //2
                        if (column.ColumnName.Contains("RecurrentTaskTwo"))
                        {
                            dr[column.ColumnName] = v1.RecurrentTask2;
                        }

                        if (column.ColumnName.Contains("FollowupParameterTwo"))
                        {
                            dr[column.ColumnName] = v1.FollowupParameter2;
                        }
                        if (column.ColumnName.Contains("MMATwo"))
                        {
                            dr[column.ColumnName] = v1.MMA2;
                        }
                        if (column.ColumnName.Contains("NCMTwo"))
                        {
                            dr[column.ColumnName] = v1.NCM2;
                        }
                        if (column.ColumnName.Contains("IndicateAllRecipientsTwo"))
                        {
                            dr[column.ColumnName] = v1.IndicateAllRecipients2;
                        }
                        if (column.ColumnName.Contains("InjuredWorkerTwo"))
                        {
                            dr[column.ColumnName] = v1.INJUREDWORKER2;
                        }
                        if (column.ColumnName.Contains("AttorneyTwo"))
                        {
                            dr[column.ColumnName] = v1.ATTORNEY2;
                        }

                        if (column.ColumnName.Contains("EmployerTwo"))
                        {
                            dr[column.ColumnName] = v1.EMPLOYER2;
                        }

                        if (column.ColumnName.Contains("MedicalProviderTwo"))
                        {
                            dr[column.ColumnName] = v1.MEDICALPROVIDER2;
                        }
                        if (column.ColumnName.Contains("ProviderNameTwo"))
                        {
                            dr[column.ColumnName] = v1.PROVIDERNAME2;
                        }
                        //3

                        if (column.ColumnName.Contains("RecurrentTaskThree"))
                        {
                            dr[column.ColumnName] = v1.RecurrentTask3;
                        }

                        if (column.ColumnName.Contains("FollowupParameterThree"))
                        {
                            dr[column.ColumnName] = v1.FollowupParameter3;
                        }
                        if (column.ColumnName.Contains("MMAThree"))
                        {
                            dr[column.ColumnName] = v1.MMA3;
                        }
                        if (column.ColumnName.Contains("NCMThree"))
                        {
                            dr[column.ColumnName] = v1.NCM3;
                        }
                        if (column.ColumnName.Contains("IndicateAllRecipientsThree"))
                        {
                            dr[column.ColumnName] = v1.IndicateAllRecipients3;
                        }
                        if (column.ColumnName.Contains("InjuredWorkerThree"))
                        {
                            dr[column.ColumnName] = v1.INJUREDWORKER3;
                        }
                        if (column.ColumnName.Contains("AttorneyThree"))
                        {
                            dr[column.ColumnName] = v1.ATTORNEY3;
                        }

                        if (column.ColumnName.Contains("EmployerThree"))
                        {
                            dr[column.ColumnName] = v1.EMPLOYER3;
                        }

                        if (column.ColumnName.Contains("MedicalProviderThree"))
                        {
                            dr[column.ColumnName] = v1.MEDICALPROVIDER3;
                        }
                        if (column.ColumnName.Contains("ProviderNameThree"))
                        {
                            dr[column.ColumnName] = v1.PROVIDERNAME3;
                        }

                        //4

                        if (column.ColumnName.Contains("RecurrentTaskFour"))
                        {
                            dr[column.ColumnName] = v1.RecurrentTask4;
                        }

                        if (column.ColumnName.Contains("FollowupParameterFour"))
                        {
                            dr[column.ColumnName] = v1.FollowupParameter4;
                        }
                        if (column.ColumnName.Contains("MMAFour"))
                        {
                            dr[column.ColumnName] = v1.MMA4;
                        }
                        if (column.ColumnName.Contains("NCMFour"))
                        {
                            dr[column.ColumnName] = v1.NCM4;
                        }
                        if (column.ColumnName.Contains("IndicateAllRecipientsFour"))
                        {
                            dr[column.ColumnName] = v1.IndicateAllRecipients4;
                        }
                        if (column.ColumnName.Contains("InjuredWorkerFour"))
                        {
                            dr[column.ColumnName] = v1.INJUREDWORKER4;
                        }
                        if (column.ColumnName.Contains("AttorneyFour"))
                        {
                            dr[column.ColumnName] = v1.ATTORNEY4;
                        }

                        if (column.ColumnName.Contains("EmployerFour"))
                        {
                            dr[column.ColumnName] = v1.EMPLOYER4;
                        }

                        if (column.ColumnName.Contains("MedicalProviderFour"))
                        {
                            dr[column.ColumnName] = v1.MEDICALPROVIDER4;
                        }
                        if (column.ColumnName.Contains("ProviderNameFour"))
                        {
                            dr[column.ColumnName] = v1.PROVIDERNAME4;
                        }
                        // NCM2 
                    }
                }

            }
            #endregion

            #region vendor2
            if (venderdetailList != null && venderdetailList.Count >= 2)
            {
                if (venderdetailList[1] != null)
                {
                    VendorDetail v2 = venderdetailList[1];
                    foreach (DataColumn column in Vendor2ExpectedColumns)
                    {

                        if (column.ColumnName.Contains("VendorName"))
                        {
                            dr[column.ColumnName] = v2.VendorName;
                        }
                        if (column.ColumnName.Contains("MDName"))
                        {
                            dr[column.ColumnName] = v2.MDName;
                        }

                        if (column.ColumnName.Contains("MDPhoneNumber"))
                        {
                            dr[column.ColumnName] = v2.MDPhoneNumber;
                        }

                        if (column.ColumnName.Contains("Diagnosis"))
                        {
                            dr[column.ColumnName] = v2.Diagnosis;
                        }

                        if (column.ColumnName.Contains("Working"))
                        {
                            dr[column.ColumnName] = v2.IsWorking;
                        }

                        if (column.ColumnName.Contains("NextMDAppointmentDate"))
                        {
                            dr[column.ColumnName] = v2.NextMDAppointmentDate;
                        }

                        if (column.ColumnName.Contains("TaskRequiredOne"))
                        {
                            dr[column.ColumnName] = v2.TaskRequired1;
                        }

                        if (column.ColumnName.Contains("TaskDetailsOne"))
                        {
                            dr[column.ColumnName] = v2.TaskDetails1;
                        }

                        if (column.ColumnName.Contains("TaskRequiredTwo"))
                        {
                            dr[column.ColumnName] = v2.TaskRequired2;
                        }

                        if (column.ColumnName.Contains("TaskDetailsTwo"))
                        {
                            dr[column.ColumnName] = v2.TaskDetails2;
                        }
                        if (column.ColumnName.Contains("TaskRequiredThree"))
                        {
                            dr[column.ColumnName] = v2.TaskRequired3;
                        }
                        if (column.ColumnName.Contains("TaskDetailsThree"))
                        {
                            dr[column.ColumnName] = v2.TaskDetails3;
                        }

                        if (column.ColumnName.Contains("TaskRequiredFour"))
                        {
                            dr[column.ColumnName] = v2.TaskRequired4;
                        }

                        if (column.ColumnName.Contains("TaskDetailsFour"))
                        {
                            dr[column.ColumnName] = v2.TaskDetails4;
                        }

                        if (column.ColumnName.Contains("DateOfServiceOne"))
                        {
                            dr[column.ColumnName] = v2.DateofService1;
                        }
                        if (column.ColumnName.Contains("AdditionalCommentsOne"))
                        {
                            dr[column.ColumnName] = v2.AdditionalComments1;
                        }
                        if (column.ColumnName.Contains("DateOfServiceTwo"))
                        {
                            dr[column.ColumnName] = v2.DateofService2;
                        }
                        if (column.ColumnName.Contains("AdditionalCommentsTwo"))
                        {
                            dr[column.ColumnName] = v2.AdditionalComments2;
                        }
                        if (column.ColumnName.Contains("DateOfServiceThree"))
                        {
                            dr[column.ColumnName] = v2.DateofService3;
                        }
                        if (column.ColumnName.Contains("AdditionalCommentsThree"))
                        {
                            dr[column.ColumnName] = v2.AdditionalComments3;
                        }
                        if (column.ColumnName.Contains("DateOfServiceFour"))
                        {
                            dr[column.ColumnName] = v2.DateofService4;
                        }
                        if (column.ColumnName.Contains("AdditionalCommentsFour"))
                        {
                            dr[column.ColumnName] = v2.AdditionalComments4;
                        }

                        //Start NCM2 
                        if (column.ColumnName.Contains("RecurrentTaskOne"))
                        {
                            dr[column.ColumnName] = v2.RecurrentTask1;
                        }

                        if (column.ColumnName.Contains("FollowupParameterOne"))
                        {
                            dr[column.ColumnName] = v2.FollowupParameter1;
                        }
                        if (column.ColumnName.Contains("MMAOne"))
                        {
                            dr[column.ColumnName] = v2.MMA1;
                        }
                        if (column.ColumnName.Contains("NCMOne"))
                        {
                            dr[column.ColumnName] = v2.NCM1;
                        }
                        if (column.ColumnName.Contains("IndicateAllRecipientsOne"))
                        {
                            dr[column.ColumnName] = v2.IndicateAllRecipients1;
                        }
                        if (column.ColumnName.Contains("InjuredWorkerOne"))
                        {
                            dr[column.ColumnName] = v2.INJUREDWORKER1;
                        }
                        if (column.ColumnName.Contains("AttorneyOne"))
                        {
                            dr[column.ColumnName] = v2.ATTORNEY1;
                        }

                        if (column.ColumnName.Contains("EmployerOne"))
                        {
                            dr[column.ColumnName] = v2.EMPLOYER1;
                        }

                        if (column.ColumnName.Contains("MedicalProviderOne"))
                        {
                            dr[column.ColumnName] = v2.MEDICALPROVIDER1;
                        }
                        if (column.ColumnName.Contains("ProviderNameOne"))
                        {
                            dr[column.ColumnName] = v2.PROVIDERNAME1;
                        }

                        //2
                        if (column.ColumnName.Contains("RecurrentTaskTwo"))
                        {
                            dr[column.ColumnName] = v2.RecurrentTask2;
                        }

                        if (column.ColumnName.Contains("FollowupParameterTwo"))
                        {
                            dr[column.ColumnName] = v2.FollowupParameter2;
                        }
                        if (column.ColumnName.Contains("MMATwo"))
                        {
                            dr[column.ColumnName] = v2.MMA2;
                        }
                        if (column.ColumnName.Contains("NCMTwo"))
                        {
                            dr[column.ColumnName] = v2.NCM2;
                        }
                        if (column.ColumnName.Contains("IndicateAllRecipientsTwo"))
                        {
                            dr[column.ColumnName] = v2.IndicateAllRecipients2;
                        }
                        if (column.ColumnName.Contains("InjuredWorkerTwo"))
                        {
                            dr[column.ColumnName] = v2.INJUREDWORKER2;
                        }
                        if (column.ColumnName.Contains("AttorneyTwo"))
                        {
                            dr[column.ColumnName] = v2.ATTORNEY2;
                        }

                        if (column.ColumnName.Contains("EmployerTwo"))
                        {
                            dr[column.ColumnName] = v2.EMPLOYER2;
                        }

                        if (column.ColumnName.Contains("MedicalProviderTwo"))
                        {
                            dr[column.ColumnName] = v2.MEDICALPROVIDER2;
                        }
                        if (column.ColumnName.Contains("ProviderNameTwo"))
                        {
                            dr[column.ColumnName] = v2.PROVIDERNAME2;
                        }
                        //3

                        if (column.ColumnName.Contains("RecurrentTaskThree"))
                        {
                            dr[column.ColumnName] = v2.RecurrentTask3;
                        }

                        if (column.ColumnName.Contains("FollowupParameterThree"))
                        {
                            dr[column.ColumnName] = v2.FollowupParameter3;
                        }
                        if (column.ColumnName.Contains("MMAThree"))
                        {
                            dr[column.ColumnName] = v2.MMA3;
                        }
                        if (column.ColumnName.Contains("NCMThree"))
                        {
                            dr[column.ColumnName] = v2.NCM3;
                        }
                        if (column.ColumnName.Contains("IndicateAllRecipientsThree"))
                        {
                            dr[column.ColumnName] = v2.IndicateAllRecipients3;
                        }
                        if (column.ColumnName.Contains("InjuredWorkerThree"))
                        {
                            dr[column.ColumnName] = v2.INJUREDWORKER3;
                        }
                        if (column.ColumnName.Contains("AttorneyThree"))
                        {
                            dr[column.ColumnName] = v2.ATTORNEY3;
                        }

                        if (column.ColumnName.Contains("EmployerThree"))
                        {
                            dr[column.ColumnName] = v2.EMPLOYER3;
                        }

                        if (column.ColumnName.Contains("MedicalProviderThree"))
                        {
                            dr[column.ColumnName] = v2.MEDICALPROVIDER3;
                        }
                        if (column.ColumnName.Contains("ProviderNameThree"))
                        {
                            dr[column.ColumnName] = v2.PROVIDERNAME3;
                        }

                        //4

                        if (column.ColumnName.Contains("RecurrentTaskFour"))
                        {
                            dr[column.ColumnName] = v2.RecurrentTask4;
                        }

                        if (column.ColumnName.Contains("FollowupParameterFour"))
                        {
                            dr[column.ColumnName] = v2.FollowupParameter4;
                        }
                        if (column.ColumnName.Contains("MMAFour"))
                        {
                            dr[column.ColumnName] = v2.MMA4;
                        }
                        if (column.ColumnName.Contains("NCMFour"))
                        {
                            dr[column.ColumnName] = v2.NCM4;
                        }
                        if (column.ColumnName.Contains("IndicateAllRecipientsFour"))
                        {
                            dr[column.ColumnName] = v2.IndicateAllRecipients4;
                        }
                        if (column.ColumnName.Contains("InjuredWorkerFour"))
                        {
                            dr[column.ColumnName] = v2.INJUREDWORKER4;
                        }
                        if (column.ColumnName.Contains("AttorneyFour"))
                        {
                            dr[column.ColumnName] = v2.ATTORNEY4;
                        }

                        if (column.ColumnName.Contains("EmployerFour"))
                        {
                            dr[column.ColumnName] = v2.EMPLOYER4;
                        }

                        if (column.ColumnName.Contains("MedicalProviderFour"))
                        {
                            dr[column.ColumnName] = v2.MEDICALPROVIDER4;
                        }
                        if (column.ColumnName.Contains("ProviderNameFour"))
                        {
                            dr[column.ColumnName] = v2.PROVIDERNAME4;
                        }
                        // NCM2 
                    }
                }

            }
            #endregion

            #region vendor3
            if (venderdetailList != null && venderdetailList.Count == 3)
            {
                if (venderdetailList[2] != null)
                {
                    VendorDetail v3 = venderdetailList[2];
                    foreach (DataColumn column in Vendor3ExpectedColumns)
                    {

                        //if (column.ColumnName.Contains("CaseNumber"))
                        //{
                        //    dr[column.ColumnName] = caseNumber;
                        //}
                        //if (column.ColumnName.Contains("ClaimantName"))
                        //{
                        //    dr[column.ColumnName] = v3.ClaimantName;
                        //}
                        if (column.ColumnName.Contains("VendorName"))
                        {
                            dr[column.ColumnName] = v3.VendorName;
                        }
                        if (column.ColumnName.Contains("MDName"))
                        {
                            dr[column.ColumnName] = v3.MDName;
                        }

                        if (column.ColumnName.Contains("MDPhoneNumber"))
                        {
                            dr[column.ColumnName] = v3.MDPhoneNumber;
                        }

                        if (column.ColumnName.Contains("Diagnosis"))
                        {
                            dr[column.ColumnName] = v3.Diagnosis;
                        }

                        if (column.ColumnName.Contains("Working"))
                        {
                            dr[column.ColumnName] = v3.IsWorking;
                        }

                        if (column.ColumnName.Contains("NextMDAppointmentDate"))
                        {
                            dr[column.ColumnName] = v3.NextMDAppointmentDate;
                        }

                        if (column.ColumnName.Contains("TaskRequiredOne"))
                        {
                            dr[column.ColumnName] = v3.TaskRequired1;
                        }

                        if (column.ColumnName.Contains("TaskDetailsOne"))
                        {
                            dr[column.ColumnName] = v3.TaskDetails1;
                        }

                        if (column.ColumnName.Contains("TaskRequiredTwo"))
                        {
                            dr[column.ColumnName] = v3.TaskRequired2;
                        }

                        if (column.ColumnName.Contains("TaskDetailsTwo"))
                        {
                            dr[column.ColumnName] = v3.TaskDetails2;
                        }
                        if (column.ColumnName.Contains("TaskRequiredThree"))
                        {
                            dr[column.ColumnName] = v3.TaskRequired3;
                        }
                        if (column.ColumnName.Contains("TaskDetailsThree"))
                        {
                            dr[column.ColumnName] = v3.TaskDetails3;
                        }

                        if (column.ColumnName.Contains("TaskRequiredFour"))
                        {
                            dr[column.ColumnName] = v3.TaskRequired4;
                        }

                        if (column.ColumnName.Contains("TaskDetailsFour"))
                        {
                            dr[column.ColumnName] = v3.TaskDetails4;
                        }


                        if (column.ColumnName.Contains("DateOfServiceOne"))
                        {
                            dr[column.ColumnName] = v3.DateofService1;
                        }
                        if (column.ColumnName.Contains("AdditionalCommentsOne"))
                        {
                            dr[column.ColumnName] = v3.AdditionalComments1;
                        }
                        if (column.ColumnName.Contains("DateOfServiceTwo"))
                        {
                            dr[column.ColumnName] = v3.DateofService2;
                        }
                        if (column.ColumnName.Contains("AdditionalCommentsTwo"))
                        {
                            dr[column.ColumnName] = v3.AdditionalComments2;
                        }
                        if (column.ColumnName.Contains("DateOfServiceThree"))
                        {
                            dr[column.ColumnName] = v3.DateofService3;
                        }
                        if (column.ColumnName.Contains("AdditionalCommentsThree"))
                        {
                            dr[column.ColumnName] = v3.AdditionalComments3;
                        }
                        if (column.ColumnName.Contains("DateOfServiceFour"))
                        {
                            dr[column.ColumnName] = v3.DateofService4;
                        }
                        if (column.ColumnName.Contains("AdditionalCommentsFour"))
                        {
                            dr[column.ColumnName] = v3.AdditionalComments4;
                        }
                        //Start NCM2 RecurrentTaskOne
                        if (column.ColumnName.Contains("RecurrentTaskOne"))
                        {
                            dr[column.ColumnName] = v3.RecurrentTask1;
                        }
                        //FollowupParameterOne
                        if (column.ColumnName.Contains("FollowupParameterOne"))
                        {
                            dr[column.ColumnName] = v3.FollowupParameter1;
                        }
                        //MMAOne
                        if (column.ColumnName.Contains("MMAOne"))
                        {
                            dr[column.ColumnName] = v3.MMA1;
                        }
                        if (column.ColumnName.Contains("NCMOne"))
                        {
                            dr[column.ColumnName] = v3.NCM1;
                        }
                        //IndicateAllRecipientsOne
                        if (column.ColumnName.Contains("IndicateAllRecipientsOne"))
                        {
                            dr[column.ColumnName] = v3.IndicateAllRecipients1;
                        }
                        if (column.ColumnName.Contains("InjuredWorkerOne"))
                        {
                            dr[column.ColumnName] = v3.INJUREDWORKER1;
                        }
                        if (column.ColumnName.Contains("AttorneyOne"))
                        {
                            dr[column.ColumnName] = v3.ATTORNEY1;
                        }

                        if (column.ColumnName.Contains("EmployerOne"))
                        {
                            dr[column.ColumnName] = v3.EMPLOYER1;
                        }

                        if (column.ColumnName.Contains("MedicalProviderOne"))
                        {
                            dr[column.ColumnName] = v3.MEDICALPROVIDER1;
                        }
                        if (column.ColumnName.Contains("ProviderNameOne"))
                        {
                            dr[column.ColumnName] = v3.PROVIDERNAME1;
                        }

                        //2
                        if (column.ColumnName.Contains("RecurrentTaskTwo"))
                        {
                            dr[column.ColumnName] = v3.RecurrentTask2;
                        }

                        if (column.ColumnName.Contains("FollowupParameterTwo"))
                        {
                            dr[column.ColumnName] = v3.FollowupParameter2;
                        }
                        if (column.ColumnName.Contains("MMATwo"))
                        {
                            dr[column.ColumnName] = v3.MMA2;
                        }
                        if (column.ColumnName.Contains("NCMTwo"))
                        {
                            dr[column.ColumnName] = v3.NCM2;
                        }
                        if (column.ColumnName.Contains("IndicateAllRecipientsTwo"))
                        {
                            dr[column.ColumnName] = v3.IndicateAllRecipients2;
                        }
                        if (column.ColumnName.Contains("InjuredWorkerTwo"))
                        {
                            dr[column.ColumnName] = v3.INJUREDWORKER2;
                        }
                        if (column.ColumnName.Contains("AttorneyTwo"))
                        {
                            dr[column.ColumnName] = v3.ATTORNEY2;
                        }

                        if (column.ColumnName.Contains("EmployerTwo"))
                        {
                            dr[column.ColumnName] = v3.EMPLOYER2;
                        }

                        if (column.ColumnName.Contains("MedicalProviderTwo"))
                        {
                            dr[column.ColumnName] = v3.MEDICALPROVIDER2;
                        }
                        if (column.ColumnName.Contains("ProviderNameTwo"))
                        {
                            dr[column.ColumnName] = v3.PROVIDERNAME2;
                        }
                        //3

                        if (column.ColumnName.Contains("RecurrentTaskThree"))
                        {
                            dr[column.ColumnName] = v3.RecurrentTask3;
                        }

                        if (column.ColumnName.Contains("FollowupParameterThree"))
                        {
                            dr[column.ColumnName] = v3.FollowupParameter3;
                        }
                        if (column.ColumnName.Contains("MMAThree"))
                        {
                            dr[column.ColumnName] = v3.MMA3;
                        }
                        if (column.ColumnName.Contains("NCMThree"))
                        {
                            dr[column.ColumnName] = v3.NCM3;
                        }
                        if (column.ColumnName.Contains("IndicateAllRecipientsThree"))
                        {
                            dr[column.ColumnName] = v3.IndicateAllRecipients3;
                        }
                        if (column.ColumnName.Contains("InjuredWorkerThree"))
                        {
                            dr[column.ColumnName] = v3.INJUREDWORKER3;
                        }
                        if (column.ColumnName.Contains("AttorneyThree"))
                        {
                            dr[column.ColumnName] = v3.ATTORNEY3;
                        }

                        if (column.ColumnName.Contains("EmployerThree"))
                        {
                            dr[column.ColumnName] = v3.EMPLOYER3;
                        }

                        if (column.ColumnName.Contains("MedicalProviderThree"))
                        {
                            dr[column.ColumnName] = v3.MEDICALPROVIDER3;
                        }
                        if (column.ColumnName.Contains("ProviderNameThree"))
                        {
                            dr[column.ColumnName] = v3.PROVIDERNAME3;
                        }

                        //4

                        if (column.ColumnName.Contains("RecurrentTaskFour"))
                        {
                            dr[column.ColumnName] = v3.RecurrentTask4;
                        }

                        if (column.ColumnName.Contains("FollowupParameterFour"))
                        {
                            dr[column.ColumnName] = v3.FollowupParameter4;
                        }
                        if (column.ColumnName.Contains("MMAFour"))
                        {
                            dr[column.ColumnName] = v3.MMA4;
                        }
                        if (column.ColumnName.Contains("NCMFour"))
                        {
                            dr[column.ColumnName] = v3.NCM4;
                        }
                        if (column.ColumnName.Contains("IndicateAllRecipientsFour"))
                        {
                            dr[column.ColumnName] = v3.IndicateAllRecipients4;
                        }
                        if (column.ColumnName.Contains("InjuredWorkerFour"))
                        {
                            dr[column.ColumnName] = v3.INJUREDWORKER4;
                        }
                        if (column.ColumnName.Contains("AttorneyFour"))
                        {
                            dr[column.ColumnName] = v3.ATTORNEY4;
                        }

                        if (column.ColumnName.Contains("EmployerFour"))
                        {
                            dr[column.ColumnName] = v3.EMPLOYER4;
                        }

                        if (column.ColumnName.Contains("MedicalProviderFour"))
                        {
                            dr[column.ColumnName] = v3.MEDICALPROVIDER4;
                        }
                        if (column.ColumnName.Contains("ProviderNameFour"))
                        {
                            dr[column.ColumnName] = v3.PROVIDERNAME4;
                        }
                        // NCM2 
                    }


                }
            }
            #endregion

        }

        void ExtractVendorDataFromBody(string bodyMessage)
        {
            try
            {
                // int LineNoWhereVendorInfoEnd = 0;E
                int lineCount = 0;
                //string caseNumber = string.Empty;
                List<string> KeyWordToAvoidFromMailBody = new List<string> { "<*** Start of macro information *>", "<** Start Vendor info *>","<** Start Provider info *>" , "<**BOT*>",
            "<**EOT*>","<**End of Vendor *>","<**End of Vendor *>"};
                if (venderdetailList != null)
                    venderdetailList.Clear();

                System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
                string codeBase = System.IO.Path.GetDirectoryName(ass.CodeBase);
                System.Uri uri = new Uri(codeBase);
                var path = uri.LocalPath.Replace(Path.DirectorySeparatorChar.ToString() + "bin", "").Replace(Path.DirectorySeparatorChar.ToString() + "Debug", "");
                path = path + Path.DirectorySeparatorChar.ToString() + "mail.txt";

                if (File.Exists(path))
                    File.Delete(path);

                if (string.IsNullOrEmpty(bodyMessage) == false)
                    File.WriteAllText(path, bodyMessage);

                if (File.Exists(path))
                {

                    string line;
                    VendorDetail vd = null;
                    string[] vdn = new string[2];
                    string PreviousLineFor = string.Empty;
                    string ClaimantName = string.Empty;
                    System.IO.StreamReader file = new System.IO.StreamReader(path);
                    int vendorNumber = 0;
                    bool insideBOT = false;
                    int taskNumber = 0;
                    bool insideVendorDetails = false;

                    # region Begin of While
                    while ((line = file.ReadLine()) != null)
                    {
                        lineCount++;

                        if (line.ToUpper().Contains("END OF VENDOR") || line.ToUpper().Contains("END OF PROVIDER"))
                        {
                            insideVendorDetails = false;
                            venderdetailList.Add(vd);
                            vd = null;
                            continue;
                        }


                        if (line.ToUpper().Contains("START VENDOR INFO") || line.ToUpper().Contains("START PROVIDER INFO"))
                        {
                            insideVendorDetails = true;
                            vd = new VendorDetail();
                            taskNumber = 0;
                            vendorNumber++;
                            continue;
                        }

                        if (insideVendorDetails == true)
                        {
                            if (line.ToUpper().Contains("CLAIMANT NAME") && line.Split(':')[0].ToUpper().Contains("CLAIMANT NAME") && !KeyWordToAvoidFromMailBody.Contains(line))
                            {
                                // for First Time Only
                                vdn = line.Split(':');
                                if (vdn.Length > 1)
                                    vd.ClaimantName = String.IsNullOrEmpty(vdn[1]) ? "" : vdn[1];
                                else
                                    vd.ClaimantName = string.Empty;
                            }
                            if (line.ToUpper().Contains("VENDOR NAME") && line.Split(':')[0].ToUpper().Contains("VENDOR NAME") && !KeyWordToAvoidFromMailBody.Contains(line))
                            {
                                PreviousLineFor = "Vendor Name";
                                vdn = line.Split(':');
                                if (vdn.Length > 1)
                                    vd.VendorName = String.IsNullOrEmpty(vdn[1]) ? "" : vdn[1];
                                else
                                    vd.VendorName = string.Empty;
                            }
                            else if (line.ToUpper().Contains("MD NAME") && line.Split(':')[0].ToUpper().Contains("MD NAME") && !KeyWordToAvoidFromMailBody.Contains(line))
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');
                                if (vdn.Length > 1)
                                    vd.MDName = String.IsNullOrEmpty(vdn[1]) ? string.Empty : vdn[1];
                                else
                                    vd.MDName = string.Empty;

                            }
                            else if (line.ToUpper().Contains("MD PHONE NUMBER") && line.Split(':')[0].ToUpper().Contains("MD PHONE NUMBER") && !KeyWordToAvoidFromMailBody.Contains(line))
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');
                                if (vdn.Length > 1)
                                    vd.MDPhoneNumber = String.IsNullOrEmpty(vdn[1]) ? string.Empty : vdn[1];
                                else
                                    vd.MDPhoneNumber = string.Empty;


                            }


                            else if (line.ToUpper().Contains("DIAGNOSIS") && line.Split(':')[0].ToUpper().Contains("DIAGNOSIS") && !KeyWordToAvoidFromMailBody.Contains(line))
                            {

                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');
                                if (vdn.Length > 1)
                                    vd.Diagnosis = String.IsNullOrEmpty(vdn[1]) ? string.Empty : vdn[1];
                                else
                                    vd.Diagnosis = string.Empty;

                            }

                            else if (line.ToUpper().Contains("WORKING (IF APPLICABLE)") && line.Split(':')[0].ToUpper().Contains("WORKING (IF APPLICABLE)") && !KeyWordToAvoidFromMailBody.Contains(line))
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');
                                if (vdn.Length > 1)
                                    vd.IsWorking = String.IsNullOrEmpty(vdn[1]) ? string.Empty : vdn[1];
                                else
                                    vd.IsWorking = string.Empty;

                            }

                            else if (line.ToUpper().Contains("NEXT MD APPOINTMENT DATE (IF APPLICABLE)") && line.Split(':')[0].ToUpper().Contains("NEXT MD APPOINTMENT DATE (IF APPLICABLE)") && !KeyWordToAvoidFromMailBody.Contains(line))
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');
                                if (vdn.Length > 1)
                                {
                                    vd.NextMDAppointmentDate = String.IsNullOrEmpty(vdn[1]) ? string.Empty : vdn[1];
                                }
                                else
                                    vd.NextMDAppointmentDate = string.Empty;

                            }


                            else if (line.ToUpper().Contains("<**BOT*>"))
                            {
                                PreviousLineFor = string.Empty;
                                insideBOT = true;
                                taskNumber++;

                            }
                            else if (PreviousLineFor == "Vendor Name" && !KeyWordToAvoidFromMailBody.Contains(line))
                            {
                                PreviousLineFor = "Vendor Name";
                                if (line.Trim().Length > 0)
                                {
                                    vd.VendorName += Environment.NewLine + line;
                                }
                            }


                            else if (line.ToUpper().Contains("TASK REQUIRED") && line.Split(':')[0].ToUpper().Contains("TASK REQUIRED") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 1)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.TaskRequired1 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.TaskRequired1 = string.Empty;

                            }
                            else if (line.ToUpper().Contains("TASK DETAIL") && line.Split(':')[0].ToUpper().Contains("TASK DETAIL") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 1)
                            {
                                PreviousLineFor = "Task Detail";

                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.TaskDetails1 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.TaskDetails1 = string.Empty;

                            }


                            ////// DATE OF SERVICE
                            else if (line.ToUpper().Contains("DATE OF SERVICE") && line.Split(':')[0].ToUpper().Contains("DATE OF SERVICE") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 1)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.DateofService1 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.DateofService1 = string.Empty;
                            }

                            //   //add for NCM2 Start  For 1

                            else if (line.ToUpper().Contains("RECURRENT TASK") && line.Split(':')[0].ToUpper().Contains("RECURRENT TASK") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 1)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.RecurrentTask1 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.RecurrentTask1 = string.Empty;

                            }
                            else if (line.ToUpper().Contains("FOLLOW-UP PARAMETER (# OF BUSINESS DAYS)") && line.Split(':')[0].ToUpper().Contains("FOLLOW-UP PARAMETER (# OF BUSINESS DAYS)") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 1)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.FollowupParameter1 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.FollowupParameter1 = string.Empty;

                            }

                            else if (line.ToUpper().Contains("MMA") && line.Split(':')[0].ToUpper().Contains("MMA") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 1)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.MMA1 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.MMA1 = string.Empty;

                            }


                            else if (line.ToUpper().Contains("NCM") && line.Split(':')[0].ToUpper().Contains("NCM") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 1)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.NCM1 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.NCM1 = string.Empty;

                            }

                            else if (line.ToUpper().Contains("INDICATE ALL RECIPIENTS OF CLOSURE LETTERS") && line.Split(':')[0].ToUpper().Contains("INDICATE ALL RECIPIENTS OF CLOSURE LETTERS") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 1)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.IndicateAllRecipients1 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.IndicateAllRecipients1 = string.Empty;

                            }

                            else if (line.ToUpper().Contains("INJURED WORKER") && line.Split(':')[0].ToUpper().Contains("INJURED WORKER") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 1)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.INJUREDWORKER1 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.INJUREDWORKER1 = string.Empty;

                            }


                            else if (line.ToUpper().Contains("ATTORNEY") && line.Split(':')[0].ToUpper().Contains("ATTORNEY") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 1)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.ATTORNEY1 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.ATTORNEY1 = string.Empty;

                            }



                            else if (line.ToUpper().Contains("EMPLOYER") && line.Split(':')[0].ToUpper().Contains("EMPLOYER") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 1)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.EMPLOYER1 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.EMPLOYER1 = string.Empty;


                            }


                            else if (line.ToUpper().Contains("MEDICAL PROVIDER") && line.Split(':')[0].ToUpper().Contains("MEDICAL PROVIDER") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 1)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.MEDICALPROVIDER1 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.MEDICALPROVIDER1 = string.Empty;


                            }


                            else if (line.ToUpper().Contains("PROVIDER NAME") && line.Split(':')[0].ToUpper().Contains("PROVIDER NAME") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 1)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.PROVIDERNAME1 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.PROVIDERNAME1 = string.Empty;

                            }


                            ///////NCM2 ENd 1

                            ///// Additional Comments

                            else if (line.ToUpper().Contains("ADDITIONAL COMMENTS") && line.Split(':')[0].ToUpper().Contains("ADDITIONAL COMMENTS") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 1)
                            {
                                PreviousLineFor = "Additional Comments";
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.AdditionalComments1 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.AdditionalComments1 = string.Empty;

                            }


                            else if (PreviousLineFor == "Additional Comments" && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 1 && !line.ToUpper().Contains("<**EOT*>"))
                            {
                                PreviousLineFor = "Additional Comments";
                                if (line.Trim().Length > 0)
                                    vd.AdditionalComments1 += Environment.NewLine + line;

                            }

                            else if (PreviousLineFor == "Task Detail" && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 1 && !line.ToUpper().Contains("<**EOT*>"))
                            {

                                PreviousLineFor = "Task Detail";

                                if (line.Trim().Length > 0)
                                    vd.TaskDetails1 += Environment.NewLine + line;
                            }


                            else if (line.ToUpper().Contains("TASK REQUIRED") && line.Split(':')[0].ToUpper().Contains("TASK REQUIRED") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 2)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.TaskRequired2 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.TaskRequired2 = string.Empty;

                            }
                            else if (line.ToUpper().Contains("TASK DETAIL") && line.Split(':')[0].ToUpper().Contains("TASK DETAIL") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 2)
                            {
                                PreviousLineFor = "Task Detail";
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.TaskDetails2 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.TaskDetails2 = string.Empty;
                            }
                            //////DATE OF SERVICE
                            else if (line.ToUpper().Contains("DATE OF SERVICE") && line.Split(':')[0].ToUpper().Contains("DATE OF SERVICE") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 2)
                            {
                                PreviousLineFor = "Date of Service";
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.DateofService2 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.DateofService2 = string.Empty;

                            }

                            else if (line.ToUpper().Contains("RECURRENT TASK") && line.Split(':')[0].ToUpper().Contains("RECURRENT TASK") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 2)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.RecurrentTask2 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.RecurrentTask2 = string.Empty;

                            }
                            else if (line.ToUpper().Contains("FOLLOW-UP PARAMETER (# OF BUSINESS DAYS)") && line.Split(':')[0].ToUpper().Contains("FOLLOW-UP PARAMETER (# OF BUSINESS DAYS)") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 2)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.FollowupParameter2 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.FollowupParameter2 = string.Empty;

                            }

                            else if (line.ToUpper().Contains("MMA") && line.Split(':')[0].ToUpper().Contains("MMA") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 2)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.MMA2 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.MMA2 = string.Empty;

                            }


                            else if (line.ToUpper().Contains("NCM") && line.Split(':')[0].ToUpper().Contains("NCM") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 2)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.NCM2 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.NCM2 = string.Empty;

                            }

                            else if (line.ToUpper().Contains("INDICATE ALL RECIPIENTS OF CLOSURE LETTERS") && line.Split(':')[0].ToUpper().Contains("INDICATE ALL RECIPIENTS OF CLOSURE LETTERS") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 2)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.IndicateAllRecipients2 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.IndicateAllRecipients2 = string.Empty;

                            }

                            else if (line.ToUpper().Contains("INJURED WORKER") && line.Split(':')[0].ToUpper().Contains("INJURED WORKER") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 2)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.INJUREDWORKER2 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.INJUREDWORKER2 = string.Empty;

                            }

                            else if (line.ToUpper().Contains("ATTORNEY") && line.Split(':')[0].ToUpper().Contains("ATTORNEY") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 2)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.ATTORNEY2 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.ATTORNEY2 = string.Empty;

                            }

                            else if (line.ToUpper().Contains("EMPLOYER") && line.Split(':')[0].ToUpper().Contains("EMPLOYER") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 2)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.EMPLOYER2 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.EMPLOYER2 = string.Empty;


                            }

                            else if (line.ToUpper().Contains("MEDICAL PROVIDER") && line.Split(':')[0].ToUpper().Contains("MEDICAL PROVIDER") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 2)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.MEDICALPROVIDER2 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.MEDICALPROVIDER2 = string.Empty;

                            }

                            else if (line.ToUpper().Contains("PROVIDER NAME") && line.Split(':')[0].ToUpper().Contains("PROVIDER NAME") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 2)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.PROVIDERNAME2 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.PROVIDERNAME2 = string.Empty;

                            }

                            ///////NCM2 ENd 2

                            ///// Additional Comments

                            else if (line.ToUpper().Contains("ADDITIONAL COMMENTS") && line.Split(':')[0].ToUpper().Contains("ADDITIONAL COMMENTS") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 2)
                            {
                                PreviousLineFor = "Additional Comments";
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.AdditionalComments2 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.AdditionalComments2 = string.Empty;

                            }


                            else if (PreviousLineFor == "Additional Comments" && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 2 && !line.ToUpper().Contains("<**EOT*>"))
                            {
                                PreviousLineFor = "Additional Comments";
                                if (line.Trim().Length > 0)
                                    vd.AdditionalComments2 += Environment.NewLine + line;


                            }
                            else if (PreviousLineFor == "Task Detail" && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 2 && !line.ToUpper().Contains("<**EOT*>"))
                            {
                                PreviousLineFor = "Task Detail";
                                if (line.Trim().Length > 0)
                                    vd.TaskDetails2 += Environment.NewLine + line;

                            }

                            /////
                            //////

                            else if (line.ToUpper().Contains("TASK REQUIRED") && line.Split(':')[0].ToUpper().Contains("TASK REQUIRED") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 3)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.TaskRequired3 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.TaskRequired3 = string.Empty;

                            }
                            else if (line.ToUpper().Contains("TASK DETAIL") && line.Split(':')[0].ToUpper().Contains("TASK DETAIL") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 3)
                            {
                                PreviousLineFor = "Task Detail";
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.TaskDetails3 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.TaskDetails3 = string.Empty;

                            }



                            //////DATE OF SERVICE
                            else if (line.ToUpper().Contains("DATE OF SERVICE") && line.Split(':')[0].ToUpper().Contains("DATE OF SERVICE") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 3)
                            {
                                PreviousLineFor = "Date of Service";
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.DateofService3 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.DateofService3 = string.Empty;

                            }
                            //   //add for NCM2 Start  For 3

                            else if (line.ToUpper().Contains("RECURRENT TASK") && line.Split(':')[0].ToUpper().Contains("RECURRENT TASK") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 3)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.RecurrentTask3 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.RecurrentTask3 = string.Empty;

                            }
                            else if (line.ToUpper().Contains("FOLLOW-UP PARAMETER (# OF BUSINESS DAYS)") && line.Split(':')[0].ToUpper().Contains("FOLLOW-UP PARAMETER (# OF BUSINESS DAYS)") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 3)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.FollowupParameter3 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.FollowupParameter3 = string.Empty;

                            }

                            else if (line.ToUpper().Contains("MMA") && line.Split(':')[0].ToUpper().Contains("MMA") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 3)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.MMA3 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.MMA3 = string.Empty;

                            }

                            else if (line.ToUpper().Contains("NCM") && line.Split(':')[0].ToUpper().Contains("NCM") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 3)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.NCM3 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.NCM3 = string.Empty;

                            }

                            else if (line.ToUpper().Contains("INDICATE ALL RECIPIENTS OF CLOSURE LETTERS") && line.Split(':')[0].ToUpper().Contains("INDICATE ALL RECIPIENTS OF CLOSURE LETTERS") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 3)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.IndicateAllRecipients3 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.IndicateAllRecipients3 = string.Empty;

                            }

                            else if (line.ToUpper().Contains("INJURED WORKER") && line.Split(':')[0].ToUpper().Contains("INJURED WORKER") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 3)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.INJUREDWORKER3 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.INJUREDWORKER3 = string.Empty;


                            }
                            else if (line.ToUpper().Contains("ATTORNEY") && line.Split(':')[0].ToUpper().Contains("ATTORNEY") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 3)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.ATTORNEY3 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.ATTORNEY3 = string.Empty;

                            }

                            else if (line.ToUpper().Contains("EMPLOYER") && line.Split(':')[0].ToUpper().Contains("EMPLOYER") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 3)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.EMPLOYER3 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.EMPLOYER3 = string.Empty;

                            }

                            else if (line.ToUpper().Contains("MEDICAL PROVIDER") && line.Split(':')[0].ToUpper().Contains("MEDICAL PROVIDER") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 3)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.MEDICALPROVIDER3 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.MEDICALPROVIDER3 = string.Empty;

                            }

                            else if (line.ToUpper().Contains("PROVIDER NAME") && line.Split(':')[0].ToUpper().Contains("PROVIDER NAME") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 3)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.PROVIDERNAME3 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.PROVIDERNAME3 = string.Empty;

                            }

                            ///////NCM2 ENd 3

                            ///// Additional Comments

                            else if (line.ToUpper().Contains("ADDITIONAL COMMENTS") && line.Split(':')[0].ToUpper().Contains("ADDITIONAL COMMENTS") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 3)
                            {
                                PreviousLineFor = "Additional Comments";
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.AdditionalComments3 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.AdditionalComments3 = string.Empty;

                            }

                            else if (PreviousLineFor == "Additional Comments" && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 3 && !line.ToUpper().Contains("<**EOT*>"))
                            {
                                PreviousLineFor = "Additional Comments";
                                if (line.Trim().Length > 0)
                                    vd.AdditionalComments3 += Environment.NewLine + line;


                            }
                            else if (PreviousLineFor == "Task Detail" && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 3 && !line.ToUpper().Contains("<**EOT*>"))
                            {
                                PreviousLineFor = "Task Detail";
                                if (line.Trim().Length > 0)
                                    vd.TaskDetails3 += Environment.NewLine + line;

                            }
                            /////
                            //////

                            else if (line.ToUpper().Contains("TASK REQUIRED") && line.Split(':')[0].ToUpper().Contains("TASK REQUIRED") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 4)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.TaskRequired4 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.TaskRequired4 = string.Empty;


                            }
                            else if (line.ToUpper().Contains("TASK DETAIL") && line.Split(':')[0].ToUpper().Contains("TASK DETAIL") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 4)
                            {
                                PreviousLineFor = "Task Detail";
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.TaskDetails4 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.TaskDetails4 = string.Empty;


                            }

                            //////DATE OF SERVICE
                            else if (line.ToUpper().Contains("DATE OF SERVICE") && line.Split(':')[0].ToUpper().Contains("DATE OF SERVICE") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 4)
                            {
                                PreviousLineFor = "Date of Service";
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.DateofService4 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.DateofService4 = string.Empty;


                            }

                            //   //add for NCM2 Start  For 3

                            else if (line.ToUpper().Contains("RECURRENT TASK") && line.Split(':')[0].ToUpper().Contains("RECURRENT TASK") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 4)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.RecurrentTask4 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.RecurrentTask4 = string.Empty;


                            }
                            else if (line.ToUpper().Contains("FOLLOW-UP PARAMETER (# OF BUSINESS DAYS)") && line.Split(':')[0].ToUpper().Contains("FOLLOW-UP PARAMETER (# OF BUSINESS DAYS)") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 4)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.FollowupParameter4 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.FollowupParameter4 = string.Empty;

                                //Globals.WriteToErrorLogFile("@@@@@@@@ FOLLOW-UP PARAMETER (# OF BUSINESS DAYS)4: " + vd.FollowupParameter4 + " for vendor " + vendorNumber);
                            }

                            else if (line.ToUpper().Contains("MMA") && line.Split(':')[0].ToUpper().Contains("MMA") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 4)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.MMA4 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.MMA4 = string.Empty;

                                //Globals.WriteToErrorLogFile("@@@@@@@@ MMA4: " + vd.MMA4 + " for vendor " + vendorNumber);
                            }


                            else if (line.ToUpper().Contains("NCM") && line.Split(':')[0].ToUpper().Contains("NCM") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 4)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.NCM4 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.NCM4 = string.Empty;

                                //Globals.WriteToErrorLogFile("@@@@@@@@ NCM4: " + vd.NCM4 + " for vendor " + vendorNumber);
                            }

                            else if (line.ToUpper().Contains("INDICATE ALL RECIPIENTS OF CLOSURE LETTERS") && line.Split(':')[0].ToUpper().Contains("INDICATE ALL RECIPIENTS OF CLOSURE LETTERS") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 4)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.IndicateAllRecipients4 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.IndicateAllRecipients4 = string.Empty;

                                //Globals.WriteToErrorLogFile("@@@@@@@@ IndicateAllRecipients4: " + vd.IndicateAllRecipients4 + " for vendor " + vendorNumber);
                            }

                            else if (line.ToUpper().Contains("INJURED WORKER") && line.Split(':')[0].ToUpper().Contains("INJURED WORKER") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 4)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.INJUREDWORKER4 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.INJUREDWORKER4 = string.Empty;

                                //Globals.WriteToErrorLogFile("@@@@@@@@ INJUREDWORKER4: " + vd.INJUREDWORKER4 + " for vendor " + vendorNumber);
                            }


                            else if (line.ToUpper().Contains("ATTORNEY") && line.Split(':')[0].ToUpper().Contains("ATTORNEY") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 4)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.ATTORNEY4 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.ATTORNEY4 = string.Empty;

                                //Globals.WriteToErrorLogFile("@@@@@@@@ ATTORNEY4: " + vd.ATTORNEY4 + " for vendor " + vendorNumber);
                            }



                            else if (line.ToUpper().Contains("EMPLOYER") && line.Split(':')[0].ToUpper().Contains("EMPLOYER") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 4)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.EMPLOYER4 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.EMPLOYER4 = string.Empty;

                                //Globals.WriteToErrorLogFile("@@@@@@@@ EMPLOYER4: " + vd.EMPLOYER4 + " for vendor " + vendorNumber);
                            }


                            else if (line.ToUpper().Contains("MEDICAL PROVIDER") && line.Split(':')[0].ToUpper().Contains("MEDICAL PROVIDER") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 4)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.MEDICALPROVIDER4 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.MEDICALPROVIDER4 = string.Empty;

                                //Globals.WriteToErrorLogFile("@@@@@@@@ MEDICALPROVIDER4: " + vd.MEDICALPROVIDER4 + " for vendor " + vendorNumber);
                            }


                            else if (line.ToUpper().Contains("PROVIDER NAME") && line.Split(':')[0].ToUpper().Contains("PROVIDER NAME") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 4)
                            {
                                PreviousLineFor = string.Empty;
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.PROVIDERNAME4 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.PROVIDERNAME4 = string.Empty;

                                //Globals.WriteToErrorLogFile("@@@@@@@@ PROVIDERNAME4: " + vd.PROVIDERNAME4 + " for vendor " + vendorNumber);
                            }


                            ///////NCM2 ENd 4
                            ///// Additional Comments

                            else if (line.ToUpper().Contains("ADDITIONAL COMMENTS") && line.Split(':')[0].ToUpper().Contains("ADDITIONAL COMMENTS") && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 4)
                            {
                                PreviousLineFor = "Additional Comments";
                                vdn = line.Split(':');

                                if (vdn.Length > 1)
                                    vd.AdditionalComments4 = String.IsNullOrEmpty(vdn[1]) ? null : vdn[1];
                                else
                                    vd.AdditionalComments4 = string.Empty;

                                //Globals.WriteToErrorLogFile("@@@@@@@@ Additional Comments4: " + vd.AdditionalComments4 + " for vendor " + vendorNumber);
                            }


                            else if (PreviousLineFor == "Additional Comments" && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 4 && !line.ToUpper().Contains("<**EOT*>"))
                            {
                                PreviousLineFor = "Additional Comments";
                                if (line.Trim().Length > 0)
                                    vd.AdditionalComments4 += Environment.NewLine + line;

                                //Globals.WriteToErrorLogFile("@@@@@@@@ Additional Comments4: " + vd.AdditionalComments4 + " for vendor " + vendorNumber);

                            }
                            else if (PreviousLineFor == "Task Detail" && !KeyWordToAvoidFromMailBody.Contains(line) && insideBOT && taskNumber == 4 && !line.ToUpper().Contains("<**EOT*>"))
                            {
                                PreviousLineFor = "Task Detail";
                                if (line.Trim().Length > 0)
                                    vd.TaskDetails4 += Environment.NewLine + line;

                                //Globals.WriteToErrorLogFile("@@@@@@@@ TaskDetails4: " + vd.TaskDetails4 + " for vendor " + vendorNumber);
                            }
                            else if (line.ToUpper().Contains("<**EOT*>"))
                            {
                                insideBOT = false;
                                PreviousLineFor = string.Empty;
                                //Globals.WriteToErrorLogFile("@@@@@@@@EOT: " + taskNumber + " for vendor " + vendorNumber);
                            }



                        }

                    }
                    #endregion
                    file.Dispose();
                }
            }
            catch (Exception ex)
            {
            }

        }

        public string MailSend(BusinessEntity.BEMailHelper MailData, BEMailConfiguration oMailConfig, out int ExceptionValidation, BETenant oTenant)
        {
            string logstr122 = "Campaign : " + oMailConfig.iCampaignID + " : oMailConfig.sEmailID :  " + oMailConfig.sEmailID +
                                 " Service Mail Send Start Time: " + DateTime.Now.ToLongDateString() + " Mail Subject : " + MailData.MsgSubject;

            //EHLogger.WriteLog(logstr122, EHLogger.ApplicationLog.EMSServiceLog);

            string mailId = "";
            EmailMessage message = null;
            EmailMessage Sendmessage = null;
            ResponseMessage ResponseEmail = null;
            ExceptionValidation = 0;
            ExchangeService service = null;
            try
            {
                if (MailData.FromAddress == null)// incase user have not click on the mail submit button
                { return ""; }
                if (MailData.FromAddress == null && MailData.replyType != ReplyType.NoAction)// incase user have not click on the mail submit button
                { return ""; }
                using (EmailObjectCache omail = new EmailObjectCache())
                {
                    service = omail.ConnectExchnageServer(oMailConfig, oTenant, MailData.FromAddress);
                }

                PropertySet propertySet = new PropertySet(BasePropertySet.FirstClassProperties);
                //if (oMailConfig.sEmailID.Trim() != MailData.FromAddress.Trim())
                //{
                //    message = new EmailMessage(service);
                //    Sendmessage = message;
                //}
                //else // When a user is sending a mail from Service ID mailbox
                //{
                Mailbox mb = new Mailbox(MailData.FromAddress.Trim());
                message = (MailData.replyType == ReplyType.New) ? new EmailMessage(service) : MailExchangeAdapter.GetEmailMessage(service, MailData.sMailUniqueID, propertySet);
                switch (MailData.replyType)
                {
                    case ReplyType.Reply:
                        ResponseEmail = message.CreateReply(false);

                        Sendmessage = ResponseEmail.Save(new FolderId(WellKnownFolderName.Drafts, mb));
                        Sendmessage.Load();
                        break;
                    case ReplyType.ReplyAll:
                        ResponseEmail = message.CreateReply(true);
                        Sendmessage = ResponseEmail.Save(new FolderId(WellKnownFolderName.Drafts, mb));
                        Sendmessage.Load();
                        break;
                    case ReplyType.Forward:
                        ResponseEmail = message.CreateForward();
                        Sendmessage = ResponseEmail.Save(new FolderId(WellKnownFolderName.Drafts, mb));
                        Sendmessage.Load();
                        break;
                    case ReplyType.New:
                        Sendmessage = message;
                        break;
                    default:
                        Sendmessage = message;
                        break;
                }
                mb = null;
                //}
                Sendmessage.Subject = MailData.MsgSubject;
                if (MailData.bOutLook)
                {
                    Sendmessage.Body = MailData.DraftMsgBody;
                }
                else
                {
                    Sendmessage.Body = GetmailBody(MailData.DraftMsgBody, MailData.MsgBody);
                }
                Sendmessage.From = MailData.FromAddress.Trim();

                Sendmessage.ToRecipients.Clear();
                Sendmessage.CcRecipients.Clear();
                Sendmessage.BccRecipients.Clear();
                Sendmessage.Sensitivity = Sensitivity.Normal;
                Sendmessage.Importance = Importance.Normal;

                if (MailData.MailCCRecipients != null)
                {
                    foreach (var strc in MailData.MailCCRecipients)
                        Sendmessage.CcRecipients.Add(strc.Trim());
                }
                if (MailData.MailBCCRecipients != null)
                {
                    foreach (var strc in MailData.MailBCCRecipients)
                        Sendmessage.BccRecipients.Add(strc.Trim());
                }

                if (MailData.MailToRecipients != null)
                {
                    foreach (var str in MailData.MailToRecipients)
                        Sendmessage.ToRecipients.Add(str.Trim());
                }

                if (MailData.sImportant != null)
                {
                    if (MailData.sImportant.ToUpper() == "HIGH")
                        Sendmessage.Importance = Importance.High;
                    else if (MailData.sImportant.ToUpper() == "LOW")
                        Sendmessage.Importance = Importance.Low;
                    else
                        Sendmessage.Importance = Importance.Normal;
                }

                if (!string.IsNullOrEmpty(MailData.sSensitivity))
                {
                    if (MailData.sSensitivity.ToUpper() == "CONFIDENTIAL")
                        Sendmessage.Sensitivity = Sensitivity.Confidential;
                    else if (MailData.sSensitivity.ToUpper() == "PERSONAL")
                        Sendmessage.Sensitivity = Sensitivity.Personal;
                    else if (MailData.sSensitivity.ToUpper() == "PRIVATE")
                        Sendmessage.Sensitivity = Sensitivity.Private;
                    else
                        Sendmessage.Sensitivity = Sensitivity.Normal;
                }

                if (oMailConfig.iMailServerTypeID != EmailServerType.Exchange2010SP2)
                {
                    Sendmessage.Attachments.Clear();
                }

                //Sendmessage.Update(ConflictResolutionMode.AlwaysOverwrite);
                if (MailData.OMailAttachment != null)
                {
                    for (int i = 0; i < MailData.OMailAttachment.Count; i++)
                    {
                        if (MailData.OMailAttachment[i].isDeleted) continue;
                        string filename = "";
                        if (MailData.OMailAttachment[i].AttachmentType == AttachementType.ItemAttachment)
                        {
                            filename = MailData.OMailAttachment[i].AttachmentsName + ".eml";
                        }
                        else
                        {
                            filename = MailData.OMailAttachment[i].AttachmentsName;
                        }

                        if (MailData.OMailAttachment[i].AttachmentType == AttachementType.ItemAttachment)
                        {
                            ItemAttachment<EmailMessage> itemAttachment = Sendmessage.Attachments.AddItemAttachment<EmailMessage>();
                            itemAttachment.Item.MimeContent = new MimeContent("UTF-8", MailData.OMailAttachment[i].AttachmentData);
                            itemAttachment.Name = MailData.OMailAttachment[i].AttachmentsName;

                            if (oMailConfig.iMailServerTypeID != EmailServerType.Exchange2007SP1)
                            {
                                itemAttachment.IsInline = MailData.OMailAttachment[i].IsLineAttachment;
                            }
                            if (MailData.OMailAttachment[i].IsExistingAttachment)
                            {
                                itemAttachment.ContentId = MailData.OMailAttachment[i].ContentId;
                                itemAttachment.ContentType = MailData.OMailAttachment[i].ContentType;
                            }
                            else
                            {
                                itemAttachment.ContentId = MailData.OMailAttachment[i].ContentId;
                                itemAttachment.ContentType = MailData.OMailAttachment[i].ContentType;
                            }

                            // Sendmessage.Attachments.AddFileAttachment(filename, MailData.OMailAttachment[i].AttachmentData);

                        }
                        else
                        {
                            if (MailData.OMailAttachment[i].IsExistingAttachment)
                            {
                                Sendmessage.Attachments.AddFileAttachment(filename, MailData.OMailAttachment[i].AttachmentData);
                                if (MailData.OMailAttachment[i].ContentId != null || MailData.OMailAttachment[i].ContentId != "")
                                {
                                    if (Sendmessage.Attachments.Count > 0)
                                    {
                                        Sendmessage.Attachments[Sendmessage.Attachments.Count - 1].ContentId = MailData.OMailAttachment[i].ContentId;
                                        if (MailData.OMailAttachment[i].IsLineAttachment)
                                        {
                                            Sendmessage.Attachments[Sendmessage.Attachments.Count - 1].IsInline = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Sendmessage.Attachments.AddFileAttachment(filename, MailData.OMailAttachment[i].AttachmentData);
                                if (MailData.OMailAttachment[i].IsLineAttachment)
                                {
                                    if (Sendmessage.Attachments.Count > 0)
                                    {
                                        Sendmessage.Attachments[Sendmessage.Attachments.Count - 1].ContentId = MailData.OMailAttachment[i].ContentId;
                                        if (oMailConfig.iMailServerTypeID != EmailServerType.Exchange2007SP1)
                                            Sendmessage.Attachments[Sendmessage.Attachments.Count - 1].IsInline = true;
                                    }
                                }
                            }
                        }
                    }
                }


                if (MailData.replyType != ReplyType.NoAction)
                {
                    //if (oMailConfig.sEmailID.Trim() == MailData.FromAddress.Trim())
                    //{
                    if (MailData.replyType != ReplyType.New)
                        Sendmessage.Update(ConflictResolutionMode.AutoResolve);
                    if (MailData.replyType != ReplyType.New)
                        FindRecentlySent(service, Sendmessage);

                    //}
                    mailId = SendAndSaveCopy(Sendmessage, service, oMailConfig, MailData, propertySet);
                    if (MailData.replyType != ReplyType.New)
                    {
                        mailId = MailData.sMailUniqueID;
                    }
                }

                if (MailData.movefolderName != "" && MailData.movefolderName != null)
                {
                   // EHLogger.WriteLog("Mail Move Start" + oMailConfig.iCampaignID.ToString() + @"\n MailConfig: " + oMailConfig.iMailConfigID + " MailBoxName: " + oMailConfig.sMailBoxName, EHLogger.ApplicationLog.EMSServiceLog);
                    mailId = saveInMailFolder(service, MailData, mailId, propertySet, oMailConfig);
                   // EHLogger.WriteLog("Mail Move End" + oMailConfig.iCampaignID.ToString() + @"\n MailConfig: " + oMailConfig.iMailConfigID + " MailBoxName: " + oMailConfig.sMailBoxName, EHLogger.ApplicationLog.EMSServiceLog);
                }
                propertySet = null;

            }
            catch (Microsoft.Exchange.WebServices.Data.ServiceResponseException ex)
            {
                if (ex.Message.Contains("The remote server returned an error: (401) Unauthorized."))
                {
                    ExceptionValidation = 1;
                }
                else if (ex.Message.Contains("The specified object was not found in the store."))
                {
                    ExceptionValidation = 2;
                }
                else if (ex.Message.Contains("SMTP address has no mailbox associated with it"))
                {
                    ExceptionValidation = 3;
                }
                else if (ex.Message.Contains("Mailbox has exceeded maximum mailbox size"))
                {
                    ExceptionValidation = 4;
                }
                else if (ex.Message.Contains("The message exceeds the maximum supported size"))
                {
                    ExceptionValidation = 5;
                }
                else if (ex.Message.Contains("One or more recipients are invalid"))
                {
                    ExceptionValidation = 6;
                }
                else
                {
                    //PolicyBasedExceptionHandler.HandleException(PolicyBasedExceptionHandler.PolicyName.BusinessLogicExceptionPolicy, ex, "");//issue
                }
                using (IWorkUploadService oSendErrorMail = new BLWorkUpload())
                {
                    oSendErrorMail.SendErrorMail("", "Error occurred during MailSend99999" + ex.Message.ToString(), oMailConfig.iCampaignID, DateTime.Now.ToString(), "", oTenant);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The remote server returned an error: (401) Unauthorized."))
                {
                    ExceptionValidation = 1;
                }
                else if (ex.Message.Contains("The specified object was not found in the store."))
                {
                    ExceptionValidation = 2;
                }
                else if (ex.Message.Contains("SMTP address has no mailbox associated with it"))
                {
                    ExceptionValidation = 3;
                }
                else if (ex.Message.Contains("Mailbox has exceeded maximum mailbox size"))
                {
                    ExceptionValidation = 4;
                }
                else if (ex.Message.Contains("The message exceeds the maximum supported size"))
                {
                    ExceptionValidation = 5;
                }
                else if (ex.Message.Contains("One or more recipients are invalid"))
                {
                    ExceptionValidation = 6;
                }
                else
                {
                   // PolicyBasedExceptionHandler.HandleException(PolicyBasedExceptionHandler.PolicyName.BusinessLogicExceptionPolicy, ex, "");//issue
                }
                using (IWorkUploadService oSendErrorMail = new BLWorkUpload())
                {
                    oSendErrorMail.SendErrorMail("", "Error occurred during MailSend99999" + ex.Message.ToString(), oMailConfig.iCampaignID, DateTime.Now.ToString(), "", oTenant);
                }
            }
            finally
            {

                message = null;
                Sendmessage = null;
                ResponseEmail = null;

                service = null;
            }
            string logstr133 = "Campaign : " + oMailConfig.iCampaignID + " : oMailConfig.sEmailID :  " + oMailConfig.sEmailID +
                              " Service Mail Send End Time: " + DateTime.Now.ToLongDateString() + " Mail Subject : " + MailData.MsgSubject;

            //EHLogger.WriteLog(logstr133, EHLogger.ApplicationLog.EMSServiceLog);
            return mailId;
        }

        private string saveInMailFolder(ExchangeService service, BEMailHelper MailData, string mailID, PropertySet propertySet, BEMailConfiguration oMailConfig)
        {
            string mailId = "";
            if (MailData.movefolderName != "" && MailData.movefolderName != null)
            {
                EmailMessage message = MailExchangeAdapter.GetEmailMessage(service, string.IsNullOrEmpty(MailData.sMailUniqueID) ? mailID : MailData.sMailUniqueID, propertySet);
                if (oMailConfig.iMailServerTypeID != EmailServerType.Exchange2010SP2)
                {

                    if (message != null && message.ParentFolderId.ToString() != MailData.movefolderID)
                    {
                        Item item = message.Move(MailData.movefolderID);
                        mailId = item.Id.ToString();
                    }
                }
                else
                {
                    if (message != null && message.ParentFolderId.ToString() != MailData.movefolderID)
                    {
                        Item item = Item.Bind(service, string.IsNullOrEmpty(MailData.sMailUniqueID) ? mailID : MailData.sMailUniqueID);
                        if (item != null)
                        {
                            item.Load();
                            item = message.Move(MailData.movefolderID);
                            if (item != null)
                            {
                                if (item.Id != null)
                                    mailId = item.Id.ToString();
                                else
                                {
                                    string logstr5 = "  item.Id is null : " + " MailId: " + mailID + "sMailUniqueID" + MailData.sMailUniqueID +
                                                         "  Campaign : " + oMailConfig.iCampaignID;
                                    //EHLogger.WriteLog(logstr5, EHLogger.ApplicationLog.EMSServiceLog);
                                }
                            }
                            else
                            {
                                string logstr5 = "  item is null : " + " MailId: " + mailID + "sMailUniqueID" + MailData.sMailUniqueID +
                                                     "  Campaign : " + oMailConfig.iCampaignID;
                                //EHLogger.WriteLog(logstr5, EHLogger.ApplicationLog.EMSServiceLog);
                            }
                        }
                    }
                    else
                    {
                        string logstr5 = "  MailId : " + mailID + "sMailUniqueID" + MailData.sMailUniqueID +
                      "  Campaign : " + oMailConfig.iCampaignID;

                        //EHLogger.WriteLog(logstr5, EHLogger.ApplicationLog.EMSServiceLog);
                    }
                }
            }
            return mailId;
        }

        private string SendAndSaveCopy(EmailMessage message, ExchangeService service, BEMailConfiguration oMailConfig, BusinessEntity.BEMailHelper MailData, PropertySet propertySet)
        {
            string strmailID = MailData.sMailUniqueID;
            try
            {

                ExtendedPropertyDefinition myExtendedPropertyDefinition = null;

                Mailbox mb = new Mailbox(MailData.FromAddress.Trim());
                switch (oMailConfig.MailBoxType)
                {
                    case emailType.Primary:
                        if (MailData.replyType == ReplyType.New)
                        {
                            message.Save();
                            message.Load();
                            myExtendedPropertyDefinition = new ExtendedPropertyDefinition(_myPropertySetId, _extendedPropertyName, MapiPropertyType.String);
                            message.SetExtendedProperty(myExtendedPropertyDefinition, MailData.iWorkID.ToString());
                        }
                        message.SendAndSaveCopy(WellKnownFolderName.SentItems);
                        break;
                    case emailType.SharedMailbox:
                        if (MailData.replyType == ReplyType.New)
                        {
                            message.Save(new FolderId(WellKnownFolderName.SentItems, mb));
                            message.Load();
                            myExtendedPropertyDefinition = new ExtendedPropertyDefinition(_myPropertySetId, _extendedPropertyName, MapiPropertyType.String);
                            message.SetExtendedProperty(myExtendedPropertyDefinition, MailData.iWorkID.ToString());
                        }
                        message.SendAndSaveCopy(new FolderId(WellKnownFolderName.SentItems, mb));
                        break;
                    case emailType.PublicFolder:
                        if (MailData.replyType == ReplyType.New)
                        {
                            message.Save(new FolderId(WellKnownFolderName.SentItems, mb));
                            message.Load();
                            myExtendedPropertyDefinition = new ExtendedPropertyDefinition(_myPropertySetId, _extendedPropertyName, MapiPropertyType.String);
                            message.SetExtendedProperty(myExtendedPropertyDefinition, MailData.iWorkID.ToString());
                        }
                        message.SendAndSaveCopy(new FolderId(WellKnownFolderName.SentItems, mb));
                        break;
                    default:
                        break;
                }

                #region Search the send mail and get message ID
                //get the send mail messageID if a new mail is send. for other cases orignal messageid will be send.
                if (MailData.replyType == ReplyType.New)
                {
                    int count = 30;
                    while (count > 0)
                    {
                        count -= 1;
                        // Wait one second (while EWS sends and saves the message). 
                        System.Threading.Thread.Sleep(1000);
                        // Now, find the saved copy of the message by using the custom extended property. 
                        ItemView view = new ItemView(5);
                        SearchFilter searchFilter = new SearchFilter.IsEqualTo(myExtendedPropertyDefinition, MailData.iWorkID.ToString());
                        view.PropertySet = new PropertySet(BasePropertySet.IdOnly, ItemSchema.Subject, myExtendedPropertyDefinition);
                        FindItemsResults<Item> findResults = null;
                        switch (oMailConfig.MailBoxType)
                        {
                            case emailType.Primary:
                                findResults = service.FindItems(WellKnownFolderName.SentItems, searchFilter, view);
                                break;
                            case emailType.SharedMailbox:
                                findResults = service.FindItems(new FolderId(WellKnownFolderName.SentItems, mb), searchFilter, view);
                                break;
                            case emailType.PublicFolder:
                                findResults = service.FindItems(new FolderId(WellKnownFolderName.SentItems, mb), searchFilter, view);
                                break;
                            default:
                                break;
                        }

                        if (findResults.Items.Count > 0)
                        {
                            if (findResults.Items[0] is EmailMessage)
                            {
                                EmailMessage em = (EmailMessage)findResults.Items[0];
                                strmailID = em.Id.UniqueId;
                                em = null;
                                break;
                            }
                        }
                    }
                }
                #endregion
                mb = null;

            }
            catch (Exception ex)
            {
                string logstr5 = "  Exception : " + ex.Message +
                       "  Campaign : " + oMailConfig.iCampaignID;

                //EHLogger.WriteLog(logstr5, EHLogger.ApplicationLog.EMSServiceLog);
                throw ex;
            }
            return strmailID;
        }

        public static void FindRecentlySent(ExchangeService service, EmailMessage messageToCheck)
        {
            // Create extended property definitions for PidTagLastVerbExecuted and PidTagLastVerbExecutionTime.
            ExtendedPropertyDefinition PidTagLastVerbExecuted = new ExtendedPropertyDefinition(0x1081, MapiPropertyType.Integer);
            ExtendedPropertyDefinition PidTagLastVerbExecutionTime = new ExtendedPropertyDefinition(0x1082, MapiPropertyType.SystemTime);

            PropertySet propSet = new PropertySet(BasePropertySet.IdOnly, EmailMessageSchema.Subject, PidTagLastVerbExecutionTime, PidTagLastVerbExecuted);
            messageToCheck = EmailMessage.Bind(service, messageToCheck.Id, propSet);
            string str = "";

            // Determine the last verb executed on the message and display output.
            object responseType;
            if (messageToCheck.TryGetProperty(PidTagLastVerbExecuted, out responseType))
            {

                object ReplyTime = null;
                switch (((Int32)responseType))
                {
                    case 102:
                        str += "A reply was sent to the '" + messageToCheck.Subject.ToString() + "' email message at";
                        break;
                    case 103:
                        str += "A reply all was sent to the '" + messageToCheck.Subject.ToString() + "' email message at";
                        break;
                    case 104:
                        str += "The '" + messageToCheck.Subject.ToString() + "' email message was forwarded at";
                        break;
                }
                if (messageToCheck.TryGetProperty(PidTagLastVerbExecutionTime, out ReplyTime))
                {
                    str += ((DateTime)ReplyTime).ToString() + ".";
                }
            }
            else
            {
                str += "No changes were made to  '" + messageToCheck.Subject.ToString() + "'.";
            }
        }

        private string GetmailBody(string DraftMsg, string MsgBody)
        {
            //if(AutoMail)
            //{
            //    string returnvalue = DraftMsg + MsgBody;
            //    return returnvalue;
            //}

            //check if the header is there in the draft msg
            string strdrafthead = checkTag(DraftMsg, true);
            string strMsghead = checkTag(MsgBody, true);
            string Finalheader = "";
            string FinalBody = "";
            string FinalMail = "";

            Finalheader += "<HEAD>";
            Finalheader += strdrafthead.Replace("<head>", "").Replace("<HEAD>", "");
            Finalheader += strMsghead.Replace("<head>", "").Replace("<HEAD>", "");
            Finalheader += "</HEAD>";

            string strdraftbody = checkTag(DraftMsg, false);
            string strMsgbody = checkTag(MsgBody, false);
            string bodytag = checkBodyTag(strMsgbody);
            FinalBody += string.IsNullOrEmpty(bodytag) ? "<BODY>" : bodytag;
            FinalBody += strdraftbody.Replace("<body>", "").Replace("<BODY>", "");

            if (!string.IsNullOrEmpty(strMsgbody))
            {
                FinalBody += @"<HR> <DIV style = ""BORDER-TOP: #b5c4df 1pt solid; BORDER-RIGHT: medium none; BORDER-BOTTOM: medium none; PADDING-BOTTOM: 0in; PADDING-TOP: 3pt; PADDING-LEFT: 0in; BORDER-LEFT: medium none; PADDING-RIGHT: 0in"" ></DIV>";

            }

            FinalBody += strMsgbody.Replace((string.IsNullOrEmpty(bodytag) ? "<BODY>" : bodytag), "");
            FinalBody += "</BODY>";

            FinalMail += @"<html >";
            FinalMail += Finalheader;
            FinalMail += FinalBody;
            FinalMail += @"</html>";

            FinalMail = FinalMail.Replace("<Div/>", "").Replace("<DIV/>", "").Replace("<div />", "").Replace("<DIV />", "");


            return FinalMail;
        }

        private string checkBodyTag(string strMsg)
        {
            string Startvalues = "<body";
            string Endvalues = ">";
            string strActualImgTag = "";
            int start = 0;
            start = strMsg.IndexOf(Startvalues, start);
            if (start == -1) start = strMsg.IndexOf(Startvalues.ToUpper(), 0);
            while (start >= 0)
            {
                string str = strMsg.Substring(start);
                int countds = str.IndexOf(Endvalues, 0);
                if (countds == -1) countds = str.IndexOf(Endvalues.ToUpper(), 0);
                if (countds >= 0)
                {
                    strActualImgTag = str.Substring(0, countds + 1);
                }
                start = -1;
            }

            return (string.IsNullOrEmpty(strActualImgTag)) ? strMsg : strActualImgTag;
        }
        private string checkTag(string strMsg, bool isHeader)
        {
            string Startvalues = "<head>";
            string Endvalues = "</head>";
            string strActualImgTag = "";
            if (!isHeader)
            {
                Startvalues = "<body";
                Endvalues = "</body>";
            }

            int start = 0;
            start = strMsg.IndexOf(Startvalues, start);
            if (start == -1) start = strMsg.IndexOf(Startvalues.ToUpper(), 0);
            while (start >= 0)
            {
                string str = strMsg.Substring(start);
                int countds = str.IndexOf(Endvalues, 0);
                if (countds == -1) countds = str.IndexOf(Endvalues.ToUpper(), 0);
                if (countds >= 0)
                {
                    strActualImgTag = str.Substring(0, countds);
                }
                start = -1;
            }
            if (isHeader)
            {
                return strActualImgTag;
            }
            return (string.IsNullOrEmpty(strActualImgTag)) ? strMsg : strActualImgTag;
        }

    }

    public class VendorDetail
    {
        public string CaseNumber { get; set; }
        public string ClaimantName { get; set; }
        public string VendorName { get; set; }
        public string MDName { get; set; }
        public string MDPhoneNumber { get; set; }
        public string Diagnosis { get; set; }
        public string IsWorking { get; set; }
        public string NextMDAppointmentDate { get; set; }
        public string TaskRequired1 { get; set; }
        public string TaskDetails1 { get; set; }

        public string TaskRequired2 { get; set; }
        public string TaskDetails2 { get; set; }
        public string TaskRequired3 { get; set; }
        public string TaskDetails3 { get; set; }
        public string TaskRequired4 { get; set; }
        public string TaskDetails4 { get; set; }

        public string DateofService1 { get; set; }
        public string DateofService2 { get; set; }
        public string DateofService3 { get; set; }
        public string DateofService4 { get; set; }

        public string AdditionalComments1 { get; set; }
        public string AdditionalComments2 { get; set; }
        public string AdditionalComments3 { get; set; }
        public string AdditionalComments4 { get; set; }

        //NCM2


        public string INJUREDWORKER1 { get; set; }

        public string ATTORNEY1 { get; set; }
        public string EMPLOYER1 { get; set; }

        public string MEDICALPROVIDER1 { get; set; }
        public string PROVIDERNAME1 { get; set; }


        public string INJUREDWORKER2 { get; set; }
        public string ATTORNEY2 { get; set; }
        public string EMPLOYER2 { get; set; }

        public string MEDICALPROVIDER2 { get; set; }
        public string PROVIDERNAME2 { get; set; }


        public string INJUREDWORKER3 { get; set; }
        public string ATTORNEY3 { get; set; }
        public string EMPLOYER3 { get; set; }

        public string MEDICALPROVIDER3 { get; set; }
        public string PROVIDERNAME3 { get; set; }


        public string INJUREDWORKER4 { get; set; }
        public string ATTORNEY4 { get; set; }
        public string EMPLOYER4 { get; set; }

        public string MEDICALPROVIDER4 { get; set; }
        public string PROVIDERNAME4 { get; set; }

        public string MMA1 { get; set; }
        public string NCM1 { get; set; }
        public string MMA2 { get; set; }
        public string NCM2 { get; set; }
        public string MMA3 { get; set; }
        public string NCM3 { get; set; }

        public string MMA4 { get; set; }
        public string NCM4 { get; set; }


        public string RecurrentTask1 { get; set; }
        public string FollowupParameter1 { get; set; }
        public string RecurrentTask2 { get; set; }
        public string FollowupParameter2 { get; set; }

        public string RecurrentTask3 { get; set; }
        public string FollowupParameter3 { get; set; }
        public string RecurrentTask4 { get; set; }
        public string FollowupParameter4 { get; set; }
        public string IndicateAllRecipients1 { get; set; }
        public string IndicateAllRecipients2 { get; set; }
        public string IndicateAllRecipients3 { get; set; }
        public string IndicateAllRecipients4 { get; set; }




        //VOneRecurrentTaskOne
        //VOneFollowupParameterOne
        //VOneMMAOne
        //VOneNCMOne
        //VOneIndicateAllRecipientsOne
        //VOneInjuredWorkerOne
        //VOneAttorneyOne
        //VOneEmployerOne
        //VOneMedicalProviderOne
        //VOneProviderNameOne



        //        Recurrent Task Y/N:
        //Follow-up Parameter (# of business days):
        //Date of Service:
        //Additional Comments:
        //MMA:
        //NCM:
        //INJURED WORKER (Y/N):
        //ATTORNEY (Y/N):
        //EMPLOYER (Y/N):
        //MEDICAL PROVIDER (Y/N):
        //Additional Comments: 
        //Task Required: 
        //VENDOR NAME:
        //PROVIDER NAME:
        ////////




    }
}
