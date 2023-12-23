using BPA.GlobalResources;
using BPA.GlobalResources.UI.AppConfiguration;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicUI.Configuration.Helper;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Module.Configuration.CampaignInfoSetup;
using MicUI.Configuration.Module.Configuration.TimeZone;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Controllers
{
    public class CampaignController : BaseController
    {
        private readonly ICampaignService iCampaignService;
        private readonly ITimeZoneService _repositoryTimeZone;
        public CampaignController(ICampaignService iCampaignService, ITimeZoneService repositoryTimeZone)
        {
            this.iCampaignService = iCampaignService;
            _repositoryTimeZone = repositoryTimeZone;
        }

        private int iFormID
        {
            get
            {
                return int.Parse(Resources_FormID.Campaign);

            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!base.CheckViewPermission(iFormID))
            {
                return RedirectToActionPermanent("AccessDenied", "Error");
            }
            CampaignViewModel oCampaignViewModel = new CampaignViewModel();
            if (TempData["sCampaignID"] != null)
            {
                // Get the campaign list base on campaign id
                oCampaignViewModel = DisplayRecord(iCampaignService.GetCampaignByCampaignId(int.Parse(TempData["sCampaignID"].ToString())));
            }
            else
            {

                oCampaignViewModel.EndDate1 = DateTimeTimeZoneConversion.GetCurrentDateTime(false, base.oUser.sUserTimeZone, base.oUser.sServerTimeZone);
            }
            oCampaignViewModel.oTenant = base.oTenant;
            return View(oCampaignViewModel);
        }

        public JsonResult GetCascadeMode()
        {
            return Json(new List<SelectListItem>() {new SelectListItem{Text =@BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign.display_BackOffice,Value="199"} ,
                       new SelectListItem{Text= @BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign.display_InBound,Value="200"},
                       new SelectListItem{Text= @BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign.display_OutBound,Value="201"}
             });
        }

        public JsonResult GetCascadePurposeofcreationofWork()
        {
            return Json(new List<SelectListItem>() {new SelectListItem{Text =@BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign.display_WorkManagement,Value=@BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign.display_WorkManagement} ,
                       new SelectListItem{Text= @BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign.display_TimeTracking,Value=@BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign.display_TimeTracking},
                       new SelectListItem{Text= @BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign.display_TransactionsMonitoring,Value=@BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign.display_TransactionsMonitoring}});
        }
        public JsonResult GetTimeZone()
        {
            return Json(_repositoryTimeZone.GetTimeZoneList(""));
        }
        public JsonResult GetBusinessApprover(int iProcessID)
        {
            return Json(iCampaignService.GetUserApproverListByProcess(iProcessID));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CampaignViewModel oCampaignViewModel)
        {
            try
            {
                if (oCampaignViewModel.Q1 < 0 || oCampaignViewModel.Q2 < 0 || oCampaignViewModel.Q3 < 0 || oCampaignViewModel.Y1 < 0 || oCampaignViewModel.Y2 < 0 || oCampaignViewModel.Y3 < 0)
                {
                    ViewData["Message"] = "Please enter valid interger value";
                    oCampaignViewModel.oTenant = base.oTenant;
                    return View(oCampaignViewModel);
                }
                if (oCampaignViewModel.CampaignID != 0)
                {
                    ModelState.Remove("BusinessApprover");

                }
                ModelState.Remove("StartDate");
                ModelState.Remove("SearchName");
                oCampaignViewModel.oTenant = base.oTenant;

                if (oCampaignViewModel.CampaignID == 0)
                {
                    iCampaignService.InsertData(CatchRecord(oCampaignViewModel));
                    ViewData["Message"] = Resources_Campaign.display_Save;
                }
                else
                {
                    iCampaignService.UpdateData(CatchRecord(oCampaignViewModel));
                    ViewData["Message"] = Resources_Campaign.display_Update;
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message.ToString();
            }
            return View(new CampaignViewModel() { oTenant = base.oTenant });
        }


        [NonAction]
        private CampaignModel CatchRecord(CampaignViewModel oCampaignViewModel)
        {

            CampaignModel oBECampInfo = new CampaignModel();
            IList<BESkillInfo> lSkillInfo = new List<BESkillInfo>();

            oBECampInfo.CampaignID = oCampaignViewModel.CampaignID;
            oBECampInfo.CampaignName = oCampaignViewModel.CampaignName;
            oBECampInfo.Description = oCampaignViewModel.Description;
            oBECampInfo.Mode = oCampaignViewModel.Mode;
            if (oCampaignViewModel.EndDate1.ToString() != "")
                oBECampInfo.EndDate = oCampaignViewModel.EndDate1.ToString() == null ?
                    DateTimeTimeZoneConversion.GetCurrentDateTime(false, base.oUser.sUserTimeZone, base.oUser.sServerTimeZone) :
                    Convert.ToDateTime(oCampaignViewModel.EndDate1);
            oBECampInfo.ProcessID = int.Parse(oCampaignViewModel.ProcessName);
            oBECampInfo.TimeZone = int.Parse(oCampaignViewModel.TimeZone);
            oBECampInfo.ClientID = int.Parse(oCampaignViewModel.ClientName);
            oBECampInfo.Disabled = Convert.ToBoolean(oCampaignViewModel.Disable);
            oBECampInfo.NoFieldDataEntry = Convert.ToBoolean(oCampaignViewModel.NoFieldDataEntry);
            //oBECampInfo.SkillID = lSkillInfo;
            oBECampInfo.Location = oCampaignViewModel.Location;

            oBECampInfo.ShiftWindow = oCampaignViewModel.ShiftWindow;
            oBECampInfo.Purposeofcreationofcampaign = oCampaignViewModel.PurposeofcreationofWork;
            if (oCampaignViewModel.PurposeofcreationofWork.Any(x => x.Equals(Resources_Campaign.display_WorkManagement)))
            {
                oBECampInfo.PurposesWM = true;
            }
            if (oCampaignViewModel.PurposeofcreationofWork.Any(x => x.Equals(Resources_Campaign.display_TimeTracking)))
            {
                oBECampInfo.PurposeTime = true;
            }
            if (oCampaignViewModel.PurposeofcreationofWork.Any(x => x.Equals(Resources_Campaign.display_TransactionsMonitoring)))
            {
                oBECampInfo.PurposeTrans = true;
            }

            oBECampInfo.BusinessJustifications = oCampaignViewModel.BusinessJustifications;
            oBECampInfo.Q1 = oCampaignViewModel.Q1;
            oBECampInfo.Q2 = oCampaignViewModel.Q2;
            oBECampInfo.Q3 = oCampaignViewModel.Q3;
            oBECampInfo.Y1 = oCampaignViewModel.Y1;
            oBECampInfo.Y2 = oCampaignViewModel.Y2;
            oBECampInfo.Y3 = oCampaignViewModel.Y3;
            oBECampInfo.KeyBenefits = oCampaignViewModel.KeyBenefits;

            oBECampInfo.BillingSystem = Convert.ToBoolean(oCampaignViewModel.bBillingSystem.ToString());
            oBECampInfo.Email = oCampaignViewModel.sEmail;
            oBECampInfo.ThresholdForCompletion = int.Parse(oCampaignViewModel.iThresholdForCompletion.ToString());
            oBECampInfo.ThresholdForToOpen = int.Parse(oCampaignViewModel.iThresholdForToOpen.ToString());
            oBECampInfo.TargetEfficiency = double.Parse(oCampaignViewModel.dTargetEfficiency.ToString());
            if (oCampaignViewModel.BusinessApprover != null)
            {
                oBECampInfo.ApproverId = int.Parse(oCampaignViewModel.BusinessApprover);
            }
            return oBECampInfo;
        }

        public ActionResult ApprovalView()
        {
            return View(new Models.ViewModels.CampaignApproval { oTenant = base.oTenant });
        }

        public ActionResult WorkApproval_ReadP([DataSourceRequest] DataSourceRequest request, string dFrom, string dTo)
        {
            List<Models.ViewModels.CampaignApproval> lstCampaignApp = null;
            lstCampaignApp = this.iCampaignService.GetPandingCampaignApproval(Convert.ToDateTime(dFrom), Convert.ToDateTime(dTo)).Select(x => new Models.ViewModels.CampaignApproval()
            {
                ApprovalId = x.ApprovalId,
                BusinessApprover = x.BusinessApprover,
                BusinessApproverId = x.BusinessApproverId,
                CampaignName = x.CampaignName,
                ChangeRequest = x.ChangeRequest,
                ClientName = x.ClientName,
                CreatedBy = x.CreatedBy,
                FromDate = x.FromDate,
                IsChecked = x.IsChecked,
                KeyBenfits = x.KeyBenfits,
                ProcessName = x.ProcessName,
                RequestCreater = x.RequestCreater,
                RequestedOn = x.RequestedOn,
                Status = x.Status,
                StatusToShowHideButtons = x.StatusToShowHideButtons,
                StoreName = x.StoreName,
                TechApprover = x.TechApprover,
                TechnologyApproverId = x.TechnologyApproverId,
                ToDate = x.ToDate
            }).ToList();
            return Json(lstCampaignApp.ToDataSourceResult(request));
        }

        public JsonResult Approval(int iApprovalId, string action)
        {
            return Json(ApprovalAction(iApprovalId, action));
        }

        [NonAction]
        private string ApprovalAction(int iApprovalId, string action)
        {
            string sStatus = string.Empty;
            int ApprovalId = iApprovalId;

            if (action == @BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign.display_Approve)
            {
                sStatus = "Approved";
            }
            else if (action == @BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign.display_Reject)
            {
                sStatus = "Rejected";
            }
            else if (action == @BPA.GlobalResources.UI.AppConfiguration.Resources_Campaign.display_Cancel)
            {
                sStatus = "Cancelled";
            }
            iCampaignService.ApprovalAction(ApprovalId, sStatus);

            return action;
        }

        public ActionResult ApprovalAction(
         [Bind(Prefix = "updated")] List<Models.ViewModels.CampaignApproval> updatedWorkApproval,
         [Bind(Prefix = "modelData")] string modelData)
        {
            if (updatedWorkApproval != null && updatedWorkApproval.Count > 0)/*To perform operation after updating any existing record in grid*/
            {
                for (int i = 0; i < updatedWorkApproval.Count; i++)
                {
                    if (Convert.ToBoolean(updatedWorkApproval[i].IsChecked))
                    {
                        ApprovalAction(updatedWorkApproval[i].ApprovalId, modelData);
                    }
                }
            }
            else
            {
                modelData = string.Empty;
            }
            return Json(modelData);
        }

        [HttpPost]
        public ActionResult Details(int level, int iApprovalId)
        {
            CampaignViewModel Viewmodel = new CampaignViewModel();
            Viewmodel.oTenant = base.oTenant;
            var dtWorkObjReqDetails = iCampaignService.FetchCampaignRequestDetails(level, iApprovalId);
            if (dtWorkObjReqDetails != null)
            {
                Viewmodel.IsLevel = level;
                Viewmodel.iApproverId = iApprovalId;
                Viewmodel.RequestCreator = dtWorkObjReqDetails.Requestor.ToString();
                Viewmodel.ClientName = dtWorkObjReqDetails.ClientName.ToString();
                Viewmodel.ProcessName = dtWorkObjReqDetails.ProcessName.ToString();
                Viewmodel.CampaignName = dtWorkObjReqDetails.CampaignName.ToString();
                Viewmodel.Location = dtWorkObjReqDetails.Location.ToString();
                Viewmodel.ShiftWindow = dtWorkObjReqDetails.ShiftWindow.ToString();
                string[] arrPurpose = dtWorkObjReqDetails.Purpose.ToString().Split(',');
                for (int i = 0; i < arrPurpose.Count(); i++)
                {
                    if (arrPurpose[i].ToString().Trim() == "Work Management")
                    {
                        Viewmodel.PurposeofcreationofWork.AddRange(new List<string> { Resources_Campaign.display_WorkManagement });
                    }
                    else if (arrPurpose[i].ToString().Trim() == "Time Tracking")
                    {
                        Viewmodel.PurposeofcreationofWork.AddRange(new List<string> { Resources_Campaign.display_TimeTracking });
                    }
                    else if (arrPurpose[i].ToString().Trim() == "Transaction Monitoring")
                    {
                        Viewmodel.PurposeofcreationofWork.AddRange(new List<string> { Resources_Campaign.display_TransactionsMonitoring });
                    }
                }
                Viewmodel.Q1 = int.Parse(dtWorkObjReqDetails.Q1.ToString());
                Viewmodel.Q2 = int.Parse(dtWorkObjReqDetails.Q2.ToString());
                Viewmodel.Q3 = int.Parse(dtWorkObjReqDetails.Q3.ToString());
                Viewmodel.Y1 = int.Parse(dtWorkObjReqDetails.Y1.ToString());
                Viewmodel.Y2 = int.Parse(dtWorkObjReqDetails.Y2.ToString());
                Viewmodel.Y3 = int.Parse(dtWorkObjReqDetails.Y3.ToString());
                Viewmodel.BusinessJustifications = dtWorkObjReqDetails.BusinessJustifications.ToString();
                Viewmodel.KeyBenefits = dtWorkObjReqDetails.KeyBenefits.ToString();
                Viewmodel.ChangeRequest = dtWorkObjReqDetails.ChangeRequest.ToString();
                Viewmodel.ChangeRequestStatus = dtWorkObjReqDetails.ChangeRequestStatus.ToString();
            }
            return PartialView("RequestView", Viewmodel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult SaveRequestDetail(CampaignRequestChange objCampaignInfo, int? UserLevel, string ChangeReq)
        {
            if (UserLevel == 1)
            {
                iCampaignService.InsertCampaignRequestChange(objCampaignInfo.ApprovalId, UserLevel.Value, ChangeReq);
            }
            else if (UserLevel == 0)
            {
                if (objCampaignInfo.Purposeofcreationofcampaign.Any(x => x.Equals(Resources_Campaign.display_WorkManagement)))
                {
                    objCampaignInfo.PurposesWM = true;
                }
                if (objCampaignInfo.Purposeofcreationofcampaign.Any(x => x.Equals(Resources_Campaign.display_TimeTracking)))
                {
                    objCampaignInfo.PurposeTime = true;
                }
                if (objCampaignInfo.Purposeofcreationofcampaign.Any(x => x.Equals(Resources_Campaign.display_TransactionsMonitoring)))
                {
                    objCampaignInfo.PurposeTrans = true;
                }

                iCampaignService.UpdateCampaignRequestChange(objCampaignInfo);
            }
            return Json(new { strMessage = "Save" });
        }

        [HttpGet]
        public ActionResult CampaignSearchView()
        {
            CampaignViewModel oCamp = new CampaignViewModel();
            return View(oCamp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CampaignSearchView(CampaignViewModel oCamp)
        {
            ModelState.Remove("BusinessApprover");
            ModelState.Remove("StartDate");
            ModelState.Remove("SearchName");

            ModelState.Remove("TimeZone");
            ModelState.Remove("Location");
            ModelState.Remove("ShiftWindow");

            ModelState.Remove("Purposeofcreationofcampaign");
            ModelState.Remove("BusinessJustifications");
            ModelState.Remove("Q2");

            ModelState.Remove("Q1");
            ModelState.Remove("Q3");
            ModelState.Remove("Y1");

            ModelState.Remove("Y2");
            ModelState.Remove("Y3");
            ModelState.Remove("KeyBenefits");

            ModelState.Remove("Q1");
            ModelState.Remove("Q3");
            ModelState.Remove("Y1");

            ModelState.Remove("Mode");
            ModelState.Remove("CampaignName");
            ModelState.Remove("EndDate");
            if (oCamp == null) oCamp = new CampaignViewModel();
            if (string.IsNullOrEmpty(oCamp.ProcessName)) return View(oCamp);
            oCamp.SearchViewList = new List<CampaignViewModel>();
            oCamp.SearchViewList.AddRange(iCampaignService.GetCampaignList(int.Parse(oCamp.ProcessName), oCamp.CampSearchName, false).Select(x => new CampaignViewModel
            {
                CampaignID = x.iCampaignID,
                CampaignName = x.sCampaignName
            }));
            return View(oCamp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Cross-Site Request Forgery fixed
        public JsonResult EditingCustom_Edit(string sCampaignID)
        {
            TempData["sCampaignID"] = sCampaignID;
            TempData.Keep("sCampaignID");
            return Json("Ok");
        }
        [NonAction]
        private CampaignViewModel DisplayRecord(BECampaignInfo oCamp)
        {
            CampaignViewModel oCampaignViewModel = new CampaignViewModel();

            oCampaignViewModel.CampaignID = oCamp.iCampaignID;
            oCampaignViewModel.CampaignName = oCamp.sCampaignName;
            oCampaignViewModel.Description = oCamp.sCampaignDescription;
            oCampaignViewModel.ProcessName = oCamp.iProcessID.ToString();
            oCampaignViewModel.BusinessApprover = oCamp.iBuisnessID.ToString();
            oCampaignViewModel.BusinessJustifications = oCamp.sBusinessJustifications;
            oCampaignViewModel.ClientName = oCamp.iClientID.ToString();
            oCampaignViewModel.Disable = oCamp.bDisabled;
            oCampaignViewModel.TimeZone = oCamp.iTimeZoneID.ToString();
            oCampaignViewModel.ShiftWindow = oCamp.sShiftwindows;
            oCampaignViewModel.EndDate1 = oCamp.dtEndDate;
            oCampaignViewModel.NoFieldDataEntry = oCamp.bFreeField;
            oCampaignViewModel.Location = oCamp.sLocations;
            oCampaignViewModel.Mode = new List<string>();
            oCampaignViewModel.Mode.AddRange(oCamp.sModeIds.TrimEnd(',').Split(','));
            oCampaignViewModel.Q1 = int.Parse(oCamp.sTargetq1);
            oCampaignViewModel.Q2 = int.Parse(oCamp.sTargetq2);
            oCampaignViewModel.Q3 = int.Parse(oCamp.sTargetq3);
            oCampaignViewModel.Y1 = int.Parse(oCamp.sTargety1);
            oCampaignViewModel.Y2 = int.Parse(oCamp.sTargety2);
            oCampaignViewModel.Y3 = int.Parse(oCamp.sTargety3);
            oCampaignViewModel.bBillingSystem = oCamp.bBillingSystem;
            oCampaignViewModel.sEmail = oCamp.sEmail;
            oCampaignViewModel.iThresholdForCompletion = oCamp.iThresholdForCompletion;
            oCampaignViewModel.iThresholdForToOpen = oCamp.iThresholdForToOpen;
            oCampaignViewModel.dTargetEfficiency = oCamp.dTargetEfficiency;

            if (oCamp.bPurposesWM)
                oCampaignViewModel.PurposeofcreationofWork.AddRange(new List<string> { Resources_Campaign.display_WorkManagement });
            if (oCamp.bPurposeTime)
                oCampaignViewModel.PurposeofcreationofWork.AddRange(new List<string> { Resources_Campaign.display_TimeTracking });
            if (oCamp.bPurposeTrans)
                oCampaignViewModel.PurposeofcreationofWork.AddRange(new List<string> { Resources_Campaign.display_TransactionsMonitoring });
            oCampaignViewModel.KeyBenefits = oCamp.sKeyBenefits;
            return oCampaignViewModel;
        }
    }
}
