using BPA.GlobalResources;
using Microsoft.AspNetCore.Mvc;
using MicUI.EmailManagement.Models.Request;
using MicUI.EmailManagement.Models.ViewModels;
using MicUI.EmailManagement.Module.MailConfiguration;
using MicUI.EmailManagement.Services.ServiceModel;
using System.Net;

namespace MicUI.EmailManagement.Controllers
{
    public class MailTemplateController : BaseController
    {
        private readonly IMailTemplateService iMailTemplateService;

        public MailTemplateController(IMailTemplateService iMailTemplateService)
        {
            this.iMailTemplateService = iMailTemplateService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            MailTemplateViewModel oMailTemplateViewModel = new MailTemplateViewModel();
            if (TempData["iMailTemplateId"] != null)
            {
                // Get the Email Template list base on Mail Template ID
                oMailTemplateViewModel = DisplayRecord(iMailTemplateService.GetMailTemplateDataList(Convert.ToInt32(TempData["iMailTemplateId"].ToString())));
            }
            return View(oMailTemplateViewModel);
        }
        [NonAction]
        private MailTemplateViewModel DisplayRecord(BEMailTemplate oBEMailTemplate)
        {
            MailTemplateViewModel oMailTemplateViewModel = new MailTemplateViewModel();
            oMailTemplateViewModel.MailTemplateId = oBEMailTemplate.iMailTemplateId;
            oMailTemplateViewModel.MailTemplateName = oBEMailTemplate.sMailTemplateName;
            string strTemplate = WebUtility.HtmlDecode(oBEMailTemplate.sMailTemplate);
            //HttpUtility.HtmlDecode(Regex.Replace(Server.HtmlDecode(oBEMailTemplate.sMailTemplate), "<(.|\n)*?>", ""));
            oMailTemplateViewModel.MailTemplate = strTemplate;
            oMailTemplateViewModel.IsAutoReplay = oBEMailTemplate.bIsAutoReplay;
            oMailTemplateViewModel.Disable = oBEMailTemplate.bDisabled;
            oMailTemplateViewModel.ClientName = oBEMailTemplate.iClientID.ToString();
            oMailTemplateViewModel.ProcessName = oBEMailTemplate.iProcessID.ToString();
            oMailTemplateViewModel.CampaignName = oBEMailTemplate.iCampaignID.ToString();
            return oMailTemplateViewModel;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(MailTemplateViewModel oMailTemplateViewModel)
        {
            try
            {
                ModelState.Remove("StartDate");
                ModelState.Remove("SearchName");
                ModelState.Remove("EndDate");
                if (oMailTemplateViewModel.MailTemplate != null)
                {
                    string validate = oMailTemplateViewModel.MailTemplate.Trim().Replace("&amp;", "").Replace("&lt;p&gt;", "").Replace("&lt;/p&gt;", "").Replace("&nbsp;", "").Replace("&amp;", "").Replace("amp;", "").Replace("nbsp;", "");
                    if (string.IsNullOrEmpty(validate.Trim()))
                    {
                        oMailTemplateViewModel.MailTemplate = null;
                        ViewData["Message"] = "Email template can't be blank.";
                        return View(oMailTemplateViewModel);
                    }
                }
                else
                {
                    ViewData["Message"] = "Email template can't be blank.";
                    return View(oMailTemplateViewModel);
                }
                if (oMailTemplateViewModel.MailTemplateId == 0)
                {
                    iMailTemplateService.InsertData(CatchRecord(oMailTemplateViewModel), int.Parse(Resources_FormID.AssessmentType));
                    ViewData["Message"] = BPA.GlobalResources.UI.EmailManagement.MailTemplate_Resources.display_saveMessage;
                }
                else
                {
                    iMailTemplateService.UpdateData(CatchRecord(oMailTemplateViewModel), int.Parse(Resources_FormID.AssessmentType));
                    ViewData["Message"] = BPA.GlobalResources.UI.EmailManagement.MailTemplate_Resources.display_UpdateMessage;
                }

                ModelState.Clear();
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message.ToString();
            }
            return View(new MailTemplateViewModel());
        }
        [NonAction]
        private MailTemplateModel CatchRecord(MailTemplateViewModel oMailTemplateViewModel)
        {
            return new MailTemplateModel()
            {
                MailTemplateId = oMailTemplateViewModel.MailTemplateId,
                MailTemplateName = oMailTemplateViewModel.MailTemplateName,
                MailTemplate = oMailTemplateViewModel.MailTemplate,
                //sMailTemplateName = AntiXssEncoder.HtmlEncode(Regex.Replace(oMailTemplateViewModel.MailTemplateName, @"\s+", " ").Trim(),false),
                //sMailTemplate = AntiXssEncoder.HtmlEncode(oMailTemplateViewModel.MailTemplate,false),
                Disabled = Convert.ToBoolean(oMailTemplateViewModel.Disable),
                IsAutoReply = Convert.ToBoolean(oMailTemplateViewModel.IsAutoReplay),
                CampaignId = Convert.ToInt32(oMailTemplateViewModel.CampaignName),
            };
        }

        [HttpGet]
        public IActionResult SearchView()
        {
            MailTemplateViewModel oMailTemplateViewModel = new MailTemplateViewModel();
            oMailTemplateViewModel.SearchViewList = new List<MailTemplateViewModel>();
            return View(oMailTemplateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchView(MailTemplateViewModel oMailTemplateViewModel)
        {
            ModelState.Remove("StartDate");
            ModelState.Remove("SearchName");
            ModelState.Remove("EndDate");
            ModelState.Remove("MailTemplateName");

            if (oMailTemplateViewModel == null) oMailTemplateViewModel = new MailTemplateViewModel();
            oMailTemplateViewModel.SearchViewList = new List<MailTemplateViewModel>();
            oMailTemplateViewModel.SearchViewList.AddRange(iMailTemplateService.GetMailTemplateList(Convert.ToInt32(oMailTemplateViewModel.CampaignName)).Select(x =>
            new MailTemplateViewModel
            {
                MailTemplateId = x.iMailTemplateId,
                MailTemplateName = x.sMailTemplateName
            }));
            return View(oMailTemplateViewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken] // Cross-Site Request Forgery fixed
        public JsonResult EditingCustom_Edit(int iMailTemplateId)
        {
            TempData["iMailTemplateId"] = iMailTemplateId;
            TempData.Keep("iMailTemplateId"); // Set the Assessment Type Id in TempData 
            return Json("OK");
        }
    }
}
