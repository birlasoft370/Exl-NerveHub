using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using MicUI.EmailManagement.Models.Request;
using MicUI.EmailManagement.Models.ViewModels;
using MicUI.EmailManagement.Module.MailConfiguration;
using MicUI.EmailManagement.Services.ServiceModel;
using System.Globalization;

namespace MicUI.EmailManagement.Controllers
{
    public class LanguageConfigController : BaseController
    {
        private readonly ILanguagesService _languagesService;

        public LanguageConfigController(ILanguagesService languagesService)
        {
            _languagesService = languagesService;
        }

        public IActionResult Index()
        {
            return View(new EMSLanguageConfigViewModel());
        }

        public ActionResult EditingPopup_Read([DataSourceRequest] DataSourceRequest request, string StoreID, [Bind(Prefix = "models")] IEnumerable<GridItems> GridItemsData)
        {
            IList<BEMailTranslatorConfiguration> listLangConfiguration = new List<BEMailTranslatorConfiguration>();
            listLangConfiguration = _languagesService.GetCampaignWiseLangList(int.Parse(string.IsNullOrWhiteSpace(StoreID) ? "0" : StoreID));

            if (listLangConfiguration.Count > 0)
            {
                var jsonResult = Json(listLangConfiguration.ToDataSourceResult(request));
                return jsonResult;
            }
            else
            {
                return Json("");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Cross-Site Request Forgery fixed
        public int SetConfigurationID(string objLangID)
        {
            int returnValue = 0;
            if (!string.IsNullOrEmpty(objLangID))
            {
                TempData["SetLanguageID"] = objLangID;
                returnValue = 1;
            }
            return returnValue;
        }

        public IActionResult SearchViewPopUp()
        {
            EMSLanguageConfigViewModel objEMSLanguageConfigViewModel = new EMSLanguageConfigViewModel();
            IList<BEMailTranslatorConfiguration> lLangConfiguration = new List<BEMailTranslatorConfiguration>();
            try
            {
                if (TempData["SetLanguageID"] != null)
                {
                    //if (((EMSLanguageConfigViewModel)(TempData["SetLanguageID"])).LanguageConfigID != 0)
                    //{
                    var langConfigurationObj = _languagesService.GetEMSLanguageConfigAllData(Convert.ToInt32(TempData["SetLanguageID"].ToString()));

                    if (langConfigurationObj != null && langConfigurationObj.CampaignId > 0)
                    {
                        objEMSLanguageConfigViewModel.CampaignName = langConfigurationObj.CampaignId.ToString();
                        objEMSLanguageConfigViewModel.LanguageConfigName = langConfigurationObj.LanguageConfigName;
                        objEMSLanguageConfigViewModel.LanguageConfigID = langConfigurationObj.LanguageConfigID;
                        objEMSLanguageConfigViewModel.ApiKey = langConfigurationObj.ApiKey;
                        objEMSLanguageConfigViewModel.ApiUrl = langConfigurationObj.ApiUrl;
                        objEMSLanguageConfigViewModel.BatchId = langConfigurationObj.BatchId;
                        objEMSLanguageConfigViewModel.Source = langConfigurationObj.Source;
                        objEMSLanguageConfigViewModel.Target = langConfigurationObj.Target;
                        objEMSLanguageConfigViewModel.ProfileID = langConfigurationObj.ProfileID;
                        objEMSLanguageConfigViewModel.WithSource = langConfigurationObj.WithSource;
                        objEMSLanguageConfigViewModel.WithDictionary = langConfigurationObj.WithDictionary;
                        objEMSLanguageConfigViewModel.WithCorpus = langConfigurationObj.WithCorpus;
                        objEMSLanguageConfigViewModel.WithAnnotations = langConfigurationObj.WithAnnotations;
                        objEMSLanguageConfigViewModel.BackTranslation = langConfigurationObj.BackTranslation;
                        objEMSLanguageConfigViewModel.Async = langConfigurationObj.Async;
                        objEMSLanguageConfigViewModel.Callback = langConfigurationObj.Callback;
                        objEMSLanguageConfigViewModel.Encoding = langConfigurationObj.Encoding;
                        objEMSLanguageConfigViewModel.Options = langConfigurationObj.Options == null ? new List<string>() { "" } : new List<string>() { langConfigurationObj.Options[0] };
                        objEMSLanguageConfigViewModel.Disabled = langConfigurationObj.Disabled;
                        objEMSLanguageConfigViewModel.IncomingMail = langConfigurationObj.IncomingMail;
                    }
                    //}
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return PartialView("_LanguageConfig", objEMSLanguageConfigViewModel);
        }

        public JsonResult GetSourceLanguage()
        {
            CultureInfo[] cinfo = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
            EMSLanguageConfigViewModel objModel = new EMSLanguageConfigViewModel();

            foreach (var item in cinfo)
            {
                if (!string.IsNullOrEmpty(item.Name))
                    objModel.lstLanguageBind.Add(new LanguageBind { Name = item.Name, DisplayName = item.DisplayName });
            }
            return Json(objModel.lstLanguageBind);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult InsertUpdateLanguageConfiguration(EMSLanguageConfigViewModel objEMSLanguageConfigViewModel)
        {
            string ReturnVal = "";
            try
            {
                BEMailTranslatorConfiguration objBEMailTranslatorConfiguration = new BEMailTranslatorConfiguration();
                _languagesService.InsertUpdateData(CatchRecord(objEMSLanguageConfigViewModel));
                if (objEMSLanguageConfigViewModel.LanguageConfigID == 0)
                {
                    ReturnVal = "Language Configuration Save Successfully" + ",OK"; //_languagesService.InsertUpdateData(CatchRecord(objEMSLanguageConfigViewModel), 1, base.oTenant);
                }
                else
                {
                    ReturnVal = "Language Configuration Update Successfully" + ",OK";//_languagesService.InsertUpdateData(CatchRecord(objEMSLanguageConfigViewModel), 1, base.oTenant);
                }
            }
            catch (Exception ex)
            {
                ReturnVal = ex.Message.ToString();
            }
            return Json(ReturnVal);
        }

        private EMSLanguageConfigModel CatchRecord(EMSLanguageConfigViewModel objEMSLanguageConfigViewModel)
        {
            EMSLanguageConfigModel obj = new EMSLanguageConfigModel();
            obj.CampaignId = Convert.ToInt32(string.IsNullOrEmpty(objEMSLanguageConfigViewModel.CampaignName) ? "0" : objEMSLanguageConfigViewModel.CampaignName);
            obj.LanguageConfigID = objEMSLanguageConfigViewModel.LanguageConfigID;
            obj.LanguageConfigName = objEMSLanguageConfigViewModel.LanguageConfigName;
            obj.ApiKey = objEMSLanguageConfigViewModel.ApiKey;
            obj.ApiUrl = objEMSLanguageConfigViewModel.ApiUrl;
            obj.BatchId = objEMSLanguageConfigViewModel.BatchId;
            obj.Source = objEMSLanguageConfigViewModel.Source;
            obj.Target = objEMSLanguageConfigViewModel.Target;
            obj.ProfileID = objEMSLanguageConfigViewModel.ProfileID;
            obj.WithSource = objEMSLanguageConfigViewModel.WithSource;
            obj.WithDictionary = objEMSLanguageConfigViewModel.WithDictionary;
            obj.WithCorpus = objEMSLanguageConfigViewModel.WithCorpus;
            obj.WithAnnotations = objEMSLanguageConfigViewModel.WithAnnotations;
            obj.BackTranslation = objEMSLanguageConfigViewModel.BackTranslation;
            obj.Async = objEMSLanguageConfigViewModel.Async;
            obj.Callback = objEMSLanguageConfigViewModel.Callback;
            obj.Encoding = objEMSLanguageConfigViewModel.Encoding;
            obj.Options = objEMSLanguageConfigViewModel.Options;
            obj.Disabled = objEMSLanguageConfigViewModel.Disabled;
            obj.IncomingMail = objEMSLanguageConfigViewModel.IncomingMail;
            return obj;
        }

        public ActionResult SearchView()
        {

            Models.ViewModels.LanguageApproval oLanguage = new();
            oLanguage.oTenant = base.oTenant;
            return View(oLanguage);
        }

        [HttpGet]
        public JsonResult GetLanguageProfile(string CampaignID)
        {
            IList<BEMailTranslatorConfiguration> lLanguageProfileInfo;
            lLanguageProfileInfo = _languagesService.GetLanguageProfile(string.IsNullOrEmpty(CampaignID) ? 0 : Convert.ToInt32(CampaignID));
            return Json(lLanguageProfileInfo);
        }

        public IActionResult LanguageApproval_ReadP([DataSourceRequest] DataSourceRequest request, string CampaignName, string LanguageConfigID)
        {
            List<LanguageApproval> lstObj = new List<LanguageApproval>();
            if (!string.IsNullOrEmpty(CampaignName))
            {
                var lstObjData = _languagesService.GetIncomingTranslationData(Convert.ToInt32(CampaignName), string.IsNullOrEmpty(LanguageConfigID) ? 0 : Convert.ToInt32(LanguageConfigID));
                lstObj = lstObjData.Select(a => new LanguageApproval()
                {
                    CampaignID = a.CampaignID,
                    LngID = a.LngID,
                    //ProfileID=a.ProfileID,
                    TranslatedText = a.TranslatedText,
                    OriginalText = a.OriginalText,
                    SMEChangesText = a.SMEChangesText,
                    CreatedBy = a.CreatedBy,
                    // Disable= a.Disabled,
                    StatusToShowHideButtons = a.StatusToShowHideButtons ?? ""
                }).ToList();
            }

            if (lstObj.Count > 0)
            {
                var jsonResult = Json(lstObj.ToDataSourceResult(request));
                return jsonResult;
            }
            else
            {
                return Json(lstObj.ToDataSourceResult(request));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Approval(List<LanguageApproval> updatedWorkApproval, string action)
        {
            return Json(ApprovalAction(updatedWorkApproval[0], action));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        private JsonResult ApprovalAction(LanguageApproval updatedWorkApproval, string action)
        {
            try
            {

                LanguageApprovalRequestModel oBEMailTranslatorConfiguration = new LanguageApprovalRequestModel();
                oBEMailTranslatorConfiguration.LngID = updatedWorkApproval.LngID;
                oBEMailTranslatorConfiguration.CampaignID = updatedWorkApproval.CampaignID;
                oBEMailTranslatorConfiguration.SMEChangesText = updatedWorkApproval.SMEChangesText;

                if (action == BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition.display_Approve)
                {
                    oBEMailTranslatorConfiguration.IsApproved = true;
                }
                else if (action == BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition.display_Reject)
                {
                    oBEMailTranslatorConfiguration.IsRejectedTech = true;
                }
                else if (action == BPA.GlobalResources.UI.WorkManagement.Resource_WorkDefinition.display_Cancel)
                {
                    //objdata.sStatus = "Cancelled";
                }

                 _languagesService.InsertIncomingTranslationData(oBEMailTranslatorConfiguration);
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    strAction = action,
                    Successed = true,
                    msg = ex.Message.ToString()
                });

            }

            return Json(new { strAction = action, Successed = false, msg = "" });
            //  return Json("1", JsonRequestBehavior.AllowGet);          
            //return Json(action, JsonRequestBehavior.AllowGet);
        }

        public IActionResult ApprovalAction(
         [Bind(Prefix = "updated")] List<LanguageApproval> updatedWorkApproval,
         [Bind(Prefix = "modelData")] string modelData)
        {
            if (updatedWorkApproval != null && updatedWorkApproval.Count > 0)/*To perform operation after updating any existing record in grid*/
            {
                for (int i = 0; i < updatedWorkApproval.Count; i++)
                {
                    if (Convert.ToBoolean(updatedWorkApproval[i].IsChecked))
                    {
                        ApprovalAction(updatedWorkApproval[i], modelData);
                    }
                }
            }
            else
            {
                modelData = "Please select at least one checkbox Option !";
            }
            return Json(modelData);
        }
    }
}
