using BPA.GlobalResources;
using Microsoft.AspNetCore.Mvc;
using MicUI.Configuration.Helper;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Module.Configuration.ProcessInfoSetup;
using MicUI.Configuration.Module.Configuration.SubProcess;
using MicUI.Configuration.Services.ServiceModel;
using System.Globalization;

namespace MicUI.Configuration.Controllers
{
    public class SubProcessMasterController : BaseController
    {
        private readonly ISubProcessService iSubProcessService;
        private readonly IProcessService iProcessService;

        public SubProcessMasterController(ISubProcessService iSubProcessService, IProcessService iProcessService)
        {
            this.iSubProcessService = iSubProcessService;
            this.iProcessService = iProcessService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            //it's Check the User Permission
            if (!base.CheckViewPermission(int.Parse(Resources_FormID.SubProcessMaster)))
            {
                return RedirectToActionPermanent("AccessDenied", "Error");
            }
            SubProcessMasterViewModel oSubProcessViewModel = new SubProcessMasterViewModel();
            if (TempData["sSubProcessID"] != null)
            {
                oSubProcessViewModel = DisplayRecord(iSubProcessService.GetSubProcessById(int.Parse(TempData.Peek("sSubProcessID").ToString())));
            }
            else
            {
                oSubProcessViewModel.ProductionStartDate = DateTimeTimeZoneConversion.GetCurrentDateTime(false, base.oUser.sUserTimeZone, base.oUser.sServerTimeZone).ToShortDateString();
                oSubProcessViewModel.SubProcessEndDate = DateTimeTimeZoneConversion.GetCurrentDateTime(false, base.oUser.sUserTimeZone, base.oUser.sServerTimeZone).ToShortDateString();
                oSubProcessViewModel.StabilizationStartDate = DateTimeTimeZoneConversion.GetCurrentDateTime(false, base.oUser.sUserTimeZone, base.oUser.sServerTimeZone).ToShortDateString();
                oSubProcessViewModel.StabilizationEndDate = DateTimeTimeZoneConversion.GetCurrentDateTime(false, base.oUser.sUserTimeZone, base.oUser.sServerTimeZone).ToShortDateString();
                oSubProcessViewModel.GoLiveDate = DateTimeTimeZoneConversion.GetCurrentDateTime(false, base.oUser.sUserTimeZone, base.oUser.sServerTimeZone).ToShortDateString();
                oSubProcessViewModel.ProductionEndDate = DateTimeTimeZoneConversion.GetCurrentDateTime(false, base.oUser.sUserTimeZone, base.oUser.sServerTimeZone).ToShortDateString();
                oSubProcessViewModel.SubProcessStartDate = DateTimeTimeZoneConversion.GetCurrentDateTime(false, base.oUser.sUserTimeZone, base.oUser.sServerTimeZone).ToShortDateString();
            }
            return View(oSubProcessViewModel);
        }

        private SubProcessMasterViewModel DisplayRecord(BESubProcess oSubProcess)
        {
            SubProcessMasterViewModel oSubProcessViewModel = new SubProcessMasterViewModel();
            oSubProcessViewModel.ClientName = oSubProcess.iClientID.ToString() != "0" ? oSubProcess.iClientID.ToString() : "";
            oSubProcessViewModel.ProcessName = oSubProcess.iProcessID.ToString() != "0" ? oSubProcess.iProcessID.ToString() : "";
            oSubProcessViewModel.SubProcessID = oSubProcess.iSubProcessID;
            oSubProcessViewModel.SubProcessName = oSubProcess.sSubProcessName;
            oSubProcessViewModel.Description = oSubProcess.sDescription;
            oSubProcessViewModel.GoLiveDate = oSubProcess.dGoLiveDate == DateTime.MinValue ? "" : oSubProcess.dGoLiveDate.ToString("MM/dd/yyyy");
            oSubProcessViewModel.ProductionEndDate = oSubProcess.dProductionEndDate == DateTime.MinValue ? "" : oSubProcess.dProductionEndDate.ToString("MM/dd/yyyy");
            oSubProcessViewModel.ProductionStartDate = oSubProcess.dProductionStartDate == DateTime.MinValue ? "" : oSubProcess.dProductionStartDate.ToString("MM/dd/yyyy");
            oSubProcessViewModel.StabilizationEndDate = oSubProcess.dStabilizationEndDate == DateTime.MinValue ? "" : oSubProcess.dStabilizationEndDate.ToString("MM/dd/yyyy");
            oSubProcessViewModel.StabilizationStartDate = oSubProcess.dStabilizationStartDate == DateTime.MinValue ? "" : oSubProcess.dStabilizationStartDate.ToString("MM/dd/yyyy");
            oSubProcessViewModel.SubProcessEndDate = oSubProcess.dSubProcessEndDate == DateTime.MinValue ? "" : oSubProcess.dSubProcessEndDate.ToString("MM/dd/yyyy");
            oSubProcessViewModel.SubProcessStartDate = oSubProcess.dSubProcessStartDate == DateTime.MinValue ? "" : oSubProcess.dSubProcessStartDate.ToString("MM/dd/yyyy");
            oSubProcessViewModel.PDSubProcessName = oSubProcess.sPDSubProcessName;
            oSubProcessViewModel.HFPDSubProcessId = oSubProcess.iPDSubProcessID.ToString();
            oSubProcessViewModel.Disable = oSubProcess.bDisabled;
            return oSubProcessViewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SubProcessMasterViewModel oSubProcessViewModel)
        {
            try
            {
                ModelState.Remove("StartDate");
                ModelState.Remove("EndDate");
                ModelState.Remove("SearchName");
                ModelState.Remove("CampaignName");

                /*
                if (iProcessService.CheckCalenderExistance(int.Parse(oSubProcessViewModel.ProcessName), DateTimeTimeZoneConversion.GetCurrentDateTime(true, base.oUser.sUserTimeZone, base.oUser.sServerTimeZone), DateTimeTimeZoneConversion.GetCurrentDateTime(true, base.oUser.sUserTimeZone, base.oUser.sServerTimeZone), 0, base.oTenant) == 0)
                {
                    ViewData["Message"] = BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster.required_NoCalender;
                    return View(oSubProcessViewModel);
                }*/
                if (oSubProcessViewModel.SubProcessID == 0)
                {
                    string response = iSubProcessService.InsertData(CatchRecord(oSubProcessViewModel));
                    string dispalyMsg = response.ToLower().Equals(GlobalConstant.Required_NoCalender.ToLower()) ?
                                        BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster.required_NoCalender :
                                        BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster.display_SaveMessage;

                    ViewData["Message"] = dispalyMsg;
                    ModelState.Clear();
                    oSubProcessViewModel = new SubProcessMasterViewModel();
                }
                else
                {
                    string response = iSubProcessService.UpdateData(CatchRecord(oSubProcessViewModel));
                    string displayMsg = response.ToLower().Equals(GlobalConstant.Required_NoCalender) ?
                                       BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster.required_NoCalender :
                                       BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster.display_Update;

                    ViewData["Message"] = displayMsg;
                    ModelState.Clear();
                    oSubProcessViewModel = new SubProcessMasterViewModel();
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message.ToString();
            }
            return View(oSubProcessViewModel);
        }
        private SubProcessMasterModel CatchRecord(SubProcessMasterViewModel oSubProcessViewModel)
        {
            SubProcessMasterModel oBESubProcess = new SubProcessMasterModel();
            oBESubProcess.ProcessID = int.Parse(oSubProcessViewModel.ProcessName);
            oBESubProcess.SubProcessName = oSubProcessViewModel.SubProcessName;
            oBESubProcess.Description = oSubProcessViewModel.Description;
            oBESubProcess.SubProcessID = oSubProcessViewModel.SubProcessID;
            if (oSubProcessViewModel.GoLiveDate.ToString() != "" && oSubProcessViewModel.GoLiveDate != null) oBESubProcess.GoLiveDate = DateTime.Parse(oSubProcessViewModel.GoLiveDate, CultureInfo.InvariantCulture);
            if (oSubProcessViewModel.ProductionEndDate.ToString() != "") oBESubProcess.ProductionEndDate = DateTime.Parse(oSubProcessViewModel.ProductionEndDate, CultureInfo.InvariantCulture); // DateTime.ParseExact(oSubProcessViewModel.ProductionEndDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture); 
            if (oSubProcessViewModel.ProductionStartDate.ToString() != "") oBESubProcess.ProductionStartDate = DateTime.Parse(oSubProcessViewModel.ProductionStartDate, CultureInfo.InvariantCulture); // DateTime.ParseExact(oSubProcessViewModel.ProductionStartDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture); 
            if (oSubProcessViewModel.StabilizationEndDate.ToString() != "") oBESubProcess.StabilizationEndDate = DateTime.Parse(oSubProcessViewModel.StabilizationEndDate, CultureInfo.InvariantCulture); //DateTime.ParseExact(oSubProcessViewModel.StabilizationEndDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture); 
            if (oSubProcessViewModel.StabilizationStartDate.ToString() != "") oBESubProcess.StabilizationStartDate = DateTime.Parse(oSubProcessViewModel.StabilizationStartDate, CultureInfo.InvariantCulture); // DateTime.ParseExact(oSubProcessViewModel.StabilizationStartDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture); 
            if (oSubProcessViewModel.SubProcessEndDate.ToString() != "") oBESubProcess.SubProcessEndDate = DateTime.Parse(oSubProcessViewModel.SubProcessEndDate, CultureInfo.InvariantCulture); // DateTime.ParseExact(oSubProcessViewModel.SubProcessEndDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture); 
            if (oSubProcessViewModel.SubProcessStartDate.ToString() != "") oBESubProcess.SubProcessStartDate = DateTime.Parse(oSubProcessViewModel.SubProcessStartDate, CultureInfo.InvariantCulture); // DateTime.ParseExact(oSubProcessViewModel.SubProcessStartDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture); 
            oBESubProcess.Disabled = Convert.ToBoolean(oSubProcessViewModel.Disable);
            if (oBESubProcess.ProductionStartDate > oBESubProcess.ProductionEndDate)
                throw new ApplicationException(BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster.display_ProductionStartDateCannotGreater);
            if (oBESubProcess.StabilizationStartDate > oBESubProcess.StabilizationEndDate)
                throw new ApplicationException(BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster.display_StabilizationDatecannotgreater);
            if (oBESubProcess.SubProcessStartDate > oBESubProcess.SubProcessEndDate)
                throw new ApplicationException(BPA.GlobalResources.UI.AppConfiguration.Resources_SubProcessMaster.display_SubprocessStartDateGreater);
            // oBESubProcess.PDSubProcessID = Convert.ToInt32(oSubProcessViewModel.PDSubProcessName);
            return oBESubProcess;
        }

        [HttpGet]
        public ActionResult SubProcessSearchView()
        {
            SubProcessMasterViewModel oSubProcess = new SubProcessMasterViewModel();
            oSubProcess.SubProcessList = new List<SubProcessMasterViewModel>();
            return View(oSubProcess);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubProcessSearchView(SubProcessMasterViewModel oSubProcessViewModel)
        {
            ModelState.Remove("StartDate");
            ModelState.Remove("EndDate");
            ModelState.Remove("SearchName");
            ModelState.Remove("CampaignName");
            ModelState.Remove("SubProcessEndDate");
            ModelState.Remove("SubProcessStartDate");
            ModelState.Remove("SubProcessName");
            if (oSubProcessViewModel == null) oSubProcessViewModel = new SubProcessMasterViewModel();
            if (string.IsNullOrEmpty(oSubProcessViewModel.ProcessName)) return View(oSubProcessViewModel);
            oSubProcessViewModel.SubProcessList = new List<SubProcessMasterViewModel>();
            oSubProcessViewModel.SubProcessList.AddRange(iSubProcessService.GetSubProcessList(int.Parse(oSubProcessViewModel.ProcessName), oSubProcessViewModel.SubProcessSearchName, false).Where(c => c.sSubProcessName != "None")
           .Select(item => new SubProcessMasterViewModel
           {
               SubProcessID = item.iSubProcessID,
               SubProcessName = item.sSubProcessName + " " + (Convert.ToBoolean(item.bDisabled) ? "(Disabled)" : "")

           }));
            return View(oSubProcessViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditingCustom_Edit(string sSubProcessID)
        {
            TempData["sSubProcessID"] = sSubProcessID;
            TempData.Keep("sSubProcessID");
            return Json("Ok");
        }

    }
}
