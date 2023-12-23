using BPA.GlobalResources.UI.EmailManagement;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Security.Application;
using MicUI.EmailManagement.Models.Request;
using MicUI.EmailManagement.Models.ViewModels;
using MicUI.EmailManagement.Module.MailConfiguration;
using MicUI.EmailManagement.Services.ServiceModel;
using Newtonsoft.Json;
using System.Data;

namespace MicUI.EmailManagement.Controllers
{
    public class MailConfigurationController : BaseController
    {
        private readonly IMailConfigurationService iMailConfigurationService;
        private readonly IMailTemplateService _repositoryMailTemplate;
        private readonly ITimeZoneService _repositoryTimeZone;

        public MailConfigurationController(IMailConfigurationService iMailConfigurationService, IMailTemplateService repositoryMailTemplate, ITimeZoneService repositoryTimeZone)
        {
            this.iMailConfigurationService = iMailConfigurationService;
            _repositoryMailTemplate = repositoryMailTemplate;
            _repositoryTimeZone = repositoryTimeZone;
        }

        public IActionResult Index()
        {
            return View(new MailConfigurationViewModel());
        }

        public IActionResult EditingPopup_Read([DataSourceRequest] DataSourceRequest request, string StoreID, [Bind(Prefix = "models")] IEnumerable<GridItem> GridItemData)
        {
            IList<BEMailConfiguration> listMailConfiguration = new List<BEMailConfiguration>();
            //StoreID = "-1"; //for testing.
            listMailConfiguration = iMailConfigurationService.GetCampaignWiseList(int.Parse(string.IsNullOrEmpty(StoreID) ? "0" : StoreID));

            if (listMailConfiguration.Count > 0)
            {
                var jsonResult = Json(listMailConfiguration.ToDataSourceResult(request));
                return jsonResult;
            }
            else
            {
                return Json("");
            }
        }

        public IActionResult SearchViewPopUp()
        {
            MailConfigurationViewModel objModel = new MailConfigurationViewModel();
            IList<BEMailConfiguration> lMailConfiguration = new List<BEMailConfiguration>();
            try
            {
                if (TempData["SetMailConfigurationModel"] != null)
                {
                    var deserializeData = JsonConvert.DeserializeObject<MailConfigurationViewModel>(TempData["SetMailConfigurationModel"].ToString());
                    if (deserializeData != null && deserializeData.MailConfigID != 0)
                    {
                        lMailConfiguration = iMailConfigurationService.GetMailConfigAllData(deserializeData.MailConfigID);

                        if (lMailConfiguration.Count > 0)
                        {
                            objModel.MailConfigID = lMailConfiguration[0].iMailConfigID;
                            objModel.CampaignName = lMailConfiguration[0].iCampaignID.ToString();
                            objModel.MailBoxName = lMailConfiguration[0].sMailBoxName.ToString();
                            objModel.EmailID = lMailConfiguration[0].sEmailID.ToString();
                            objModel.Password = ""; //lMailConfiguration[0].sPassword.ToString() != "" ? BPA.Utility.EncryptDecrypt.Decrypt(lMailConfiguration[0].sPassword.ToString()) : "";
                            objModel.UseServiceCredentialToPull = bool.Parse(lMailConfiguration[0].bUseServiceCredentialToPull.ToString());
                            objModel.UseUserCredentialToSend = bool.Parse(lMailConfiguration[0].bUseUserCredentialToSend.ToString());
                            objModel.UserID = lMailConfiguration[0].sUserID.ToString();
                            objModel.MailServerTypeID = ((int)((EmailServerType)Enum.Parse(typeof(EmailServerType), lMailConfiguration[0].iMailServerTypeID.ToString()))).ToString();
                            objModel.ServiceType = lMailConfiguration[0].bEMSWebServerHosting;
                            objModel.ScheduleInterval = lMailConfiguration[0].iScheduleInterval.ToString();
                            objModel.AutoReplyTemplate = int.Parse(lMailConfiguration[0].MailTemplateID.ToString());
                            objModel.AutoReplyEnableDisable = bool.Parse(lMailConfiguration[0].AutoReply.ToString());
                            objModel.AutoDiscoveryPath = lMailConfiguration[0].sAutoDiscoveryPath.ToString();
                            objModel.LotusServerPath = lMailConfiguration[0].sLotusServerPath.ToString();
                            objModel.NFSFilePath = lMailConfiguration[0].sNFSFilePath.ToString();
                            objModel.WebEnabled = bool.Parse(lMailConfiguration[0].bWebEnabled.ToString());
                            objModel.WebServerURL = lMailConfiguration[0].EMSWebServerURL.ToString();
                            objModel.IsReadMail = bool.Parse(lMailConfiguration[0].IsReadMail.ToString());
                            objModel.FolderType = int.Parse(lMailConfiguration[0].iFolderType.ToString());
                            objModel.PoolingValue = bool.Parse(lMailConfiguration[0].PoolingValue.ToString());
                            objModel.bInlineEditing = string.IsNullOrEmpty(lMailConfiguration[0].bInlineEditing.ToString()) ? false : bool.Parse(lMailConfiguration[0].bInlineEditing.ToString());
                            objModel.bOutLookMailEnabled = string.IsNullOrEmpty(lMailConfiguration[0].bOutLookMailEnabled.ToString()) ? false : bool.Parse(lMailConfiguration[0].bOutLookMailEnabled.ToString());
                            objModel.bNeedPrint = string.IsNullOrEmpty(lMailConfiguration[0].bNeedPrintEnabled.ToString()) ? false : bool.Parse(lMailConfiguration[0].bNeedPrintEnabled.ToString());
                            objModel.bNeedeFile = string.IsNullOrEmpty(lMailConfiguration[0].beFileEnabled.ToString()) ? false : bool.Parse(lMailConfiguration[0].beFileEnabled.ToString());
                            objModel.bReadMailBody = string.IsNullOrEmpty(lMailConfiguration[0].bReadMailBodyEnabled.ToString()) ? false : bool.Parse(lMailConfiguration[0].bReadMailBodyEnabled.ToString());
                            objModel.bCFX = string.IsNullOrEmpty(lMailConfiguration[0].bCFXEnabled.ToString()) ? false : bool.Parse(lMailConfiguration[0].bCFXEnabled.ToString());
                            objModel.bDuringUpload = string.IsNullOrEmpty(lMailConfiguration[0].bDuringUploadEnabled.ToString()) ? false : bool.Parse(lMailConfiguration[0].bDuringUploadEnabled.ToString());


                            objModel.Disable = lMailConfiguration[0].bDisabled;
                            objModel.LotusDomainName = lMailConfiguration[0].sLotusDomainName;
                            objModel.LotusDomainPrefix = lMailConfiguration[0].sLotusDomainPrefix;
                            objModel.bOutofOfficeEnabled = string.IsNullOrEmpty(lMailConfiguration[0].bOutofOfficeEnabled.ToString()) ? false : bool.Parse(lMailConfiguration[0].bOutofOfficeEnabled.ToString());
                            objModel.sOutofOffice = lMailConfiguration[0].sOutofOffice.ToString();

                            objModel.bImpersonation = string.IsNullOrEmpty(lMailConfiguration[0].bImpersonation.ToString()) ? false : bool.Parse(lMailConfiguration[0].bImpersonation.ToString());
                            objModel.sImpersonationIDType = ((int)((ImpersonationIDType)Enum.Parse(typeof(ImpersonationIDType), lMailConfiguration[0].sImpersonationIDType.ToString()))).ToString();
                            objModel.sImpersonationID = lMailConfiguration[0].sImpersonationID.ToString();
                            objModel.bOutLookEnabled = string.IsNullOrEmpty(lMailConfiguration[0].bOutLookEnabled.ToString()) ? false : bool.Parse(lMailConfiguration[0].bOutLookEnabled.ToString());
                            objModel.bTranslationEnabled = string.IsNullOrEmpty(lMailConfiguration[0].bTranslationEnabled.ToString()) ? false : bool.Parse(lMailConfiguration[0].bTranslationEnabled.ToString());

                            objModel.GClinetID = string.IsNullOrEmpty(lMailConfiguration[0].ClinetID.ToString()) ? "" : lMailConfiguration[0].ClinetID.ToString();
                            objModel.TenentID = string.IsNullOrEmpty(lMailConfiguration[0].TenentID.ToString()) ? "" : lMailConfiguration[0].TenentID.ToString();
                            objModel.Scope = string.IsNullOrEmpty(lMailConfiguration[0].Scope.ToString()) ? "" : lMailConfiguration[0].Scope.ToString();
                            objModel.RedirectUrl = string.IsNullOrEmpty(lMailConfiguration[0].RedirectUrl.ToString()) ? "" : lMailConfiguration[0].RedirectUrl.ToString();
                            objModel.Instance = string.IsNullOrEmpty(lMailConfiguration[0].Instance.ToString()) ? "" : lMailConfiguration[0].Instance.ToString();
                            objModel.IsForSWMIntegration = string.IsNullOrEmpty(lMailConfiguration[0].IsForSWMIntegration.ToString()) ? false : bool.Parse(lMailConfiguration[0].IsForSWMIntegration.ToString());
                            objModel.IsSWMEMSIntegration = string.IsNullOrEmpty(lMailConfiguration[0].bSWMEMSIntegration.ToString()) ? false : bool.Parse(lMailConfiguration[0].bSWMEMSIntegration.ToString());
                            for (int i = 0; i < lMailConfiguration[0].oMailfolderdetails.Count; i++)
                            {
                                MicUI.EmailManagement.Models.ViewModels.GridList objGridList = new MicUI.EmailManagement.Models.ViewModels.GridList();
                                objGridList.MailFolderDetailID = lMailConfiguration[0].oMailfolderdetails[i].MailFolderDetailID;
                                objGridList.Id = lMailConfiguration[0].oMailfolderdetails[i].sMailFolderID;
                                objGridList.Name = lMailConfiguration[0].oMailfolderdetails[i].sMailFolderName;
                                objGridList.Search = (lMailConfiguration[0].oMailfolderdetails[i].bSearchFolder);
                                objGridList.MailFolderPath = lMailConfiguration[0].oMailfolderdetails[i].sMailFolderPath;
                                objGridList.MoveFolder = lMailConfiguration[0].oMailfolderdetails[i].bMoveFolder;
                                objGridList.Ingestion = lMailConfiguration[0].oMailfolderdetails[i].bIngestion;
                                objGridList.Disable = lMailConfiguration[0].oMailfolderdetails[i].bDisabled;

                                objModel.GridFolderList.Add(objGridList);
                                objGridList = null;

                            }
                        }
                        TempData["FolderList"] = JsonConvert.SerializeObject(objModel.GridFolderList);
                        TempData.Keep("FolderList");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["result"] = ex.Message;
            }
            return PartialView("_MailConfigurationView", objModel);
        }

        public JsonResult GetMailTemplateList(bool isDisabled, bool isAutoReply)
        {
            return Json(_repositoryMailTemplate.GetMailTemplateAll(isDisabled, isAutoReply));
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Cross-Site Request Forgery fixed
        public int SetConfigurationID(MailConfigurationViewModel objMailConfigurationViewModel)
        {
            int returnValue = 0;
            if (objMailConfigurationViewModel.MailConfigID != 0)
            {
                TempData["SetMailConfigurationModel"] = JsonConvert.SerializeObject(objMailConfigurationViewModel);
                returnValue = 1;
            }
            return returnValue;
        }

        #region TestConnection
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult TestConnection(MailConfigurationViewModel objMailConfigurationViewModel)
        {
            List<TreeStructure> lst = new List<TreeStructure>();

            try
            {
                if (!string.IsNullOrWhiteSpace(objMailConfigurationViewModel.Password))
                {
                    lst = iMailConfigurationService.GetMailFolderList(CatchRecordCheckMailServer(objMailConfigurationViewModel)).ToList();
                }
                if (TempData.Peek("FolderList") != null)
                {
                    return Json(new { lst = lst, Data = JsonConvert.DeserializeObject<IList<MicUI.EmailManagement.Models.ViewModels.GridList>>(TempData.Peek("FolderList").ToString()) });
                }
                else
                {
                    return Json(new { lst = lst, Data = "NODATA" });
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The request failed. The remote server returned an error: (401) Unauthorized."))
                {
                    return Json(new { ErrorMessage = ex.Message, erorVal = "ERROR", Data = TempData.Peek("FolderList") });

                }
                else
                {
                    return Json(new { ErrorMessage = ex.Message, erorVal = "ERROR" });
                }
            }
        }

        public TestConnectionModel CatchRecordCheckMailServer(MailConfigurationViewModel objMailConfigurationModel)
        {
            TestConnectionModel connectionModel = new TestConnectionModel();
            connectionModel.MailServerTypeID = int.Parse(objMailConfigurationModel.MailServerTypeID.ToString());
            connectionModel.UserID = Encoder.HtmlEncode(objMailConfigurationModel.UserID, false);
            connectionModel.AutoDiscoveryPath = Encoder.HtmlEncode(objMailConfigurationModel.AutoDiscoveryPath, false);
            connectionModel.EMSWebServerHostingAtClient = objMailConfigurationModel.ServiceType;
            connectionModel.ClientID = objMailConfigurationModel.GClinetID ?? "";
            connectionModel.TenentID = objMailConfigurationModel.TenentID ?? "";
            connectionModel.Scope = objMailConfigurationModel.Scope ?? "";
            connectionModel.RedirectUrl = objMailConfigurationModel.RedirectUrl ?? "";
            connectionModel.Instance = objMailConfigurationModel.Instance ?? "";
            connectionModel.ParentFolderId = objMailConfigurationModel.FolderID;
            connectionModel.ParentFolderName = objMailConfigurationModel.ParentFolderName;
            return connectionModel;
        }

        private EmailConfigurationModel CatChRecord(MailConfigurationViewModel objMailConfigurationModel)
        {
            //TimeZone localZone = TimeZone.CurrentTimeZone;
            //BETimeZoneInfo oBETimeZoneInfo = new BETimeZoneInfo();
            ////  CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            //oBETimeZoneInfo.sTimeZoneID = (localZone.StandardName == "Coordinated Universal Time") ? "UTC" : localZone.StandardName;

            EmailConfigurationModel objBEMailConfiguration = new EmailConfigurationModel();
            objBEMailConfiguration.StoreID = objMailConfigurationModel.StoreID;
            objBEMailConfiguration.MailConfigID = objMailConfigurationModel.MailConfigID;
            objBEMailConfiguration.CampaignID = objMailConfigurationModel.CampaignID;
            objBEMailConfiguration.MailBoxName = Encoder.HtmlEncode(objMailConfigurationModel.MailBoxName, false).Trim();
            objBEMailConfiguration.EmailID = Encoder.HtmlEncode(objMailConfigurationModel.EmailID, false);
            objBEMailConfiguration.UseServiceCredentialToPull = objMailConfigurationModel.UseServiceCredentialToPull;
            objBEMailConfiguration.UseUserCredentialToSend = objMailConfigurationModel.UseUserCredentialToSend;
            objBEMailConfiguration.UserID = Encoder.HtmlEncode(objMailConfigurationModel.UserID, false);
            objBEMailConfiguration.MailServerTypeID = int.Parse(objMailConfigurationModel.MailServerTypeID.ToString());
            objBEMailConfiguration.ScheduleInterval = int.Parse(objMailConfigurationModel.ScheduleInterval);
            objBEMailConfiguration.FolderType = objMailConfigurationModel.FolderType;
            //objBEMailConfiguration.sPassword = AntiXss.HtmlEncode(BPA.Utility.EncryptDecrypt.Encrypt(objMailConfigurationModel.Password).ToString());
            objBEMailConfiguration.AutoDiscoveryPath = Encoder.HtmlEncode(objMailConfigurationModel.AutoDiscoveryPath, false);
            objBEMailConfiguration.LotusServerPath = Encoder.HtmlEncode(objMailConfigurationModel.LotusServerPath, false);
            objBEMailConfiguration.NFSFilePath = Encoder.HtmlEncode(objMailConfigurationModel.NFSFilePath, false);
            objBEMailConfiguration.WebEnabled = objMailConfigurationModel.WebEnabled;
            objBEMailConfiguration.EMSWebServerHostingAtClient = objMailConfigurationModel.ServiceType;
            objBEMailConfiguration.EMSWebServerURL = objMailConfigurationModel.WebServerURL;
            //objBEMailConfiguration.isRunning = false;
            objBEMailConfiguration.isPasswordExpire = false;
            objBEMailConfiguration.PoolingValue = false;
            objBEMailConfiguration.AutoReply = objMailConfigurationModel.AutoReplyEnableDisable;
            objBEMailConfiguration.AutoReplyTemplate = objMailConfigurationModel.AutoReplyTemplate;
            objBEMailConfiguration.IsReadMail = objMailConfigurationModel.IsReadMail;
            objBEMailConfiguration.Disabled = Convert.ToBoolean(objMailConfigurationModel.Disable);
            objBEMailConfiguration.LotusDomainName = Encoder.HtmlEncode(objMailConfigurationModel.LotusDomainName, false);
            objBEMailConfiguration.LotusDomainPrefix = Encoder.HtmlEncode(objMailConfigurationModel.LotusDomainPrefix, false);
            objBEMailConfiguration.OutofOffice = objMailConfigurationModel.bOutofOfficeEnabled;
            //objBEMailConfiguration.BCCEnabled = objMailConfigurationModel.bBCCEnabled;
            objBEMailConfiguration.OutofOfficeText = Encoder.HtmlEncode(objMailConfigurationModel.sOutofOffice, false);
            objBEMailConfiguration.Impersonation = objMailConfigurationModel.bImpersonation;
            objBEMailConfiguration.ImpersonationIDType = string.IsNullOrEmpty(objMailConfigurationModel.sImpersonationIDType) ? "0" : Convert.ToString((ImpersonationIDType)int.Parse(objMailConfigurationModel.sImpersonationIDType.ToString()));
            objBEMailConfiguration.ImpersonationID = Encoder.HtmlEncode(objMailConfigurationModel.sImpersonationID);
            objBEMailConfiguration.OutLook = objMailConfigurationModel.bOutLookEnabled;
            objBEMailConfiguration.TranslationEnabled = objMailConfigurationModel.bTranslationEnabled;
            // added by ManishDwivedi
            objBEMailConfiguration.ClientID = objMailConfigurationModel.GClinetID;
            objBEMailConfiguration.TenentID = objMailConfigurationModel.TenentID;
            objBEMailConfiguration.Scope = objMailConfigurationModel.Scope;
            objBEMailConfiguration.RedirectUrl = objMailConfigurationModel.RedirectUrl;
            objBEMailConfiguration.Instance = objMailConfigurationModel.Instance;
            objBEMailConfiguration.IsForSWMIntegration = objMailConfigurationModel.IsForSWMIntegration;
            objBEMailConfiguration.IsSWMEMSIntegration = objMailConfigurationModel.IsSWMEMSIntegration;

            //objBEMailConfiguration.oTimeZone = oBETimeZoneInfo;
            // objBEMailConfiguration.Authority = string.Format(CultureInfo.InvariantCulture, objMailConfigurationModel.Instance == null ? "" : objMailConfigurationModel.Instance, objMailConfigurationModel.TenentID == null ? "" : objMailConfigurationModel.TenentID);

            if (objMailConfigurationModel.Password != null)
            {
                objBEMailConfiguration.Password = Encoder.HtmlEncode(objMailConfigurationModel.Password.ToString(), false);
            }
            objBEMailConfiguration.ParentFolderId = objMailConfigurationModel.FolderID;
            objBEMailConfiguration.ParentFolderName = objMailConfigurationModel.ParentFolderName;

            objBEMailConfiguration.GridList = objMailConfigurationModel.GridFolderList.Select(x => new Models.Request.GridList()
            {
                MailFolderDetailID = x.MailFolderDetailID,
                Disable = x.Disable,
                Id = x.Id,
                Ingestion = x.Ingestion,
                MailFolderPath = x.MailFolderPath,
                MoveFolder = x.MoveFolder,
                Name = x.Name,
                Search = x.Search
            }).ToList();

            return objBEMailConfiguration;
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Cross-Site Request Forgery fixed
        public JsonResult GetNodeValue(MailConfigurationViewModel objMailConfigurationViewModel)
        {

            List<TreeStructure> lst = new List<TreeStructure>();

            try
            {
                if (objMailConfigurationViewModel.UserID != null)
                {
                    lst = iMailConfigurationService.GetMailSubFolderList(CatchRecordCheckMailServer(objMailConfigurationViewModel)).ToList();
                }
                if (TempData["FolderList"] != null)
                {
                    return Json(new { lst = lst, Data = JsonConvert.DeserializeObject<IList<Models.ViewModels.GridList>>(TempData["FolderList"].ToString()) });
                }
                else
                {
                    return Json(new { lst = lst, Data = "NODATA" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { lst = "ERROR", Data = ex.Message });
            }
        }

        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult InsertUpdateMailConfiguration(MailConfigurationViewModel objMailConfigurationViewModel, IList<MicUI.EmailManagement.Models.ViewModels.GridList> JData)
        {

            string ReturnVal = "";
            DataSet ds = new DataSet();
            if (JData == null) return Json(ReturnVal);
            objMailConfigurationViewModel.GridFolderList = JData.ToList();
            try
            {
                string result = string.Empty;
                BEMailConfiguration objBEMailConfiguration = new BEMailConfiguration();
                BEMailfolderdetails objBEMailfolderdetails = new BEMailfolderdetails();

                if (objMailConfigurationViewModel.MailConfigID == 0)
                {
                    iMailConfigurationService.InsertData(CatChRecord(objMailConfigurationViewModel), Int16.Parse(@BPA.GlobalResources.Resources_FormID.Client));
                    ReturnVal = MailConfiguration_Resources.display_SaveMailConfig + ",OK";
                }
                else
                {
                    iMailConfigurationService.UpdateData(CatChRecord(objMailConfigurationViewModel), Int16.Parse(@BPA.GlobalResources.Resources_FormID.Client));
                    ReturnVal = MailConfiguration_Resources.display_UpdateMialConfig + ",OK";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = ex.Message.ToString();
            }
            return Json(ReturnVal);
        }

        public PartialViewResult AdvanceConfigurationPopUp()
        {
            MailConfigurationViewModel objModel = new MailConfigurationViewModel();
            IList<BEMailConfiguration> lMailConfiguration = new List<BEMailConfiguration>();
            try
            {
                if (TempData["SetMailConfigurationModel"] != null)
                {
                    var deserializeData = JsonConvert.DeserializeObject<MailConfigurationViewModel>(TempData["SetMailConfigurationModel"].ToString());
                    if (deserializeData != null && deserializeData.MailConfigID != 0)
                    {
                        lMailConfiguration = iMailConfigurationService.GetAdvancedConfiguration(deserializeData.CampaignID);
                        if (lMailConfiguration.Count > 0)
                        {
                            objModel.bSendmailquiqueidentified = bool.Parse(lMailConfiguration[0].bSendmailQuiqueIdentified.ToString());
                            objModel.bScheduletosameuser = bool.Parse(lMailConfiguration[0].bScheduletoSameUser.ToString());
                            objModel.BatchFrequencyType = lMailConfiguration[0].BatchFrequency.ToString();
                            objModel.bInlineEditing = bool.Parse(lMailConfiguration[0].bInlineEditing.ToString());
                            objModel.bNeedeFile = bool.Parse(lMailConfiguration[0].beFileEnabled.ToString());
                            objModel.bOutLookMailEnabled = bool.Parse(lMailConfiguration[0].bOutLookMailEnabled.ToString());
                            objModel.bNeedPrint = bool.Parse(lMailConfiguration[0].bNeedPrintEnabled.ToString());
                            objModel.bReadMailBody = bool.Parse(lMailConfiguration[0].bReadMailBodyEnabled.ToString());
                            objModel.bCFX = bool.Parse(lMailConfiguration[0].bCFXEnabled.ToString());
                            objModel.bDuringUpload = bool.Parse(lMailConfiguration[0].bDuringUploadEnabled.ToString());
                            objModel.AssignType = lMailConfiguration[0].sAssignType.ToString();
                            objModel.iUploadBy = Convert.ToInt32(lMailConfiguration[0].iUploadBy.ToString());
                            objModel.bSensitivity = bool.Parse(lMailConfiguration[0].bSensitivityEnabled.ToString());
                            objModel.sCEXlauncherPath = lMailConfiguration[0].strCEXlauncherPath.ToString();

                            objModel.bSubmitDisplay = bool.Parse(lMailConfiguration[0].bSubmitDisplayEnabled.ToString()); ;
                            objModel.sEfilePath = lMailConfiguration[0].strEFilePath.ToString();
                            objModel.sSubmitDisplayMsg = lMailConfiguration[0].strSubmitDisplay.ToString();
                            objModel.iNeedTicketLenth = Convert.ToInt32(lMailConfiguration[0].iNeedTicketLenth.ToString());
                            objModel.bFreshRequired = bool.Parse(lMailConfiguration[0].bFreshRequiredEnabled.ToString());
                            objModel.sTicketName = lMailConfiguration[0].strTicketName.ToString();
                            objModel.bNeedTicket = bool.Parse(lMailConfiguration[0].bNeedTicketEnabled.ToString());
                            objModel.bAssignLast = bool.Parse(lMailConfiguration[0].IsAssignLast.ToString());
                            objModel.iTimeZoneID = lMailConfiguration[0].oTimeZone.iTimeZoneID;
                        }

                    }
                }

            }

            catch (Exception ex)
            {
                ViewData["result"] = ex.Message;
            }

            return PartialView("_AdvanceConfigurationView", objModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Cross-Site Request Forgery fixed
        public JsonResult InsertUpdateAdvancedConfiguration(MailConfigurationViewModel objMailConfigurationViewModel)
        {
            string ReturnVal = "";
            try
            {
                string result = string.Empty;
                if (objMailConfigurationViewModel.MailConfigID == 0)
                {
                    iMailConfigurationService.InsertUpdateAdvancedConfiguration(CatchRecord2(objMailConfigurationViewModel));
                    ReturnVal = MailConfiguration_Resources.display_SaveAdvancedConfiguration;
                }

            }

            catch (Exception ex)
            {
                ReturnVal = ex.Message; ;
            }

            return Json(ReturnVal);
        }

        protected EmailAdvancedConfigurationModel CatchRecord2(MailConfigurationViewModel objMailConfigurationViewModel)
        {
            EmailAdvancedConfigurationModel obj = new EmailAdvancedConfigurationModel();
            obj.BatchFrequency = (int)((BatchFrequencyType)Enum.Parse(typeof(BatchFrequencyType), objMailConfigurationViewModel.BatchFrequencyType.ToString())); //(BatchFrequencyType)Enum.Parse(typeof(BatchFrequencyType), objMailConfigurationViewModel.BatchFrequencyType.ToString());
            obj.Scheduletosameuser = objMailConfigurationViewModel.bScheduletosameuser;
            obj.Sendmailuniqueidentified = objMailConfigurationViewModel.bSendmailquiqueidentified;
            obj.TimeZoneID = objMailConfigurationViewModel.iTimeZoneID;
            obj.CampaignID = objMailConfigurationViewModel.CampaignID;
            obj.InlineEditing = objMailConfigurationViewModel.bInlineEditing;
            obj.NeedPrint = objMailConfigurationViewModel.bNeedPrint;
            obj.OutLookMailEnabled = objMailConfigurationViewModel.bOutLookMailEnabled;
            obj.NeedeFile = objMailConfigurationViewModel.bNeedeFile;
            obj.ReadMailBody = objMailConfigurationViewModel.bReadMailBody;
            obj.CFX = objMailConfigurationViewModel.bCFX;
            obj.NeedTicket = objMailConfigurationViewModel.bNeedTicket;

            obj.UploadBy = objMailConfigurationViewModel.iUploadBy;
            obj.IsSensitivity = objMailConfigurationViewModel.bSensitivity;
            obj.CEXlauncherPath = objMailConfigurationViewModel.sCEXlauncherPath;

            obj.IsSubmitDisplay = objMailConfigurationViewModel.bSubmitDisplay;
            obj.EFilePath = objMailConfigurationViewModel.sEfilePath;
            obj.SubmitDisplay = objMailConfigurationViewModel.sSubmitDisplayMsg;

            obj.NeedTicketLength = objMailConfigurationViewModel.iNeedTicketLenth;
            obj.IsFreshRequired = objMailConfigurationViewModel.bFreshRequired;
            obj.TicketName = objMailConfigurationViewModel.sTicketName;
            obj.DuringUpload = objMailConfigurationViewModel.bDuringUpload;
            obj.LastAssignType = objMailConfigurationViewModel.AssignType;
            obj.IsAssignLast = objMailConfigurationViewModel.bAssignLast;
            return obj;
        }

        public JsonResult GetTimeZone()
        {
            return Json(_repositoryTimeZone.GetTimeZoneList());
        }

    }
}
