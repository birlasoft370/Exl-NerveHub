using BPA.GlobalResources;
using Microsoft.AspNetCore.Mvc;
using MicUI.WorkManagement.Models.ViewModels;
using MicUI.WorkManagement.Module.Administration;
using MicUI.WorkManagement.Services.ServiceModel;

namespace MicUI.WorkManagement.Controllers
{
    public class ShiftMasterController : BaseController
    {
        private readonly IShiftService iShiftService;
        public ShiftMasterController(IShiftService iShiftService)
        {
            this.iShiftService = iShiftService;
        }
        public IActionResult Index()
        {
            if (!base.CheckViewPermission(int.Parse(Resources_FormID.ShiftMaster)))
            {
                return RedirectToActionPermanent("AccessDenied", "Error");
            }
           
            ShiftMasterViewModel oShiftMasterViewModel = null;
            if (TempData["sShiftID"] == null)
            {
                oShiftMasterViewModel = new ShiftMasterViewModel();
                oShiftMasterViewModel.ShiftStartTime = "00:00";
                oShiftMasterViewModel.ShiftEndTime = "00:00";
            }
            else
            {
                oShiftMasterViewModel = new ShiftMasterViewModel();
               oShiftMasterViewModel = this.DisplayRecord(iShiftService.GetShiftById(int.Parse(TempData.Peek("sShiftID").ToString())));
            }
            return View(oShiftMasterViewModel);
        }
        private ShiftMasterViewModel DisplayRecord(ShiftMasterModel oShift)
        {
            ShiftMasterViewModel oShiftViewModel = new ShiftMasterViewModel();
            oShiftViewModel.ShiftID = oShift.ShiftID;
            oShiftViewModel.ShiftName = oShift.ShiftName;
            oShiftViewModel.Description = oShift.Description;
            oShiftViewModel.Disable = oShift.Disabled;
            // string[] ArrayStartTime = oShift.sStartTime.Split(':');
            //oShiftViewModel.ShiftStartHrTime = ArrayStartTime[0].ToString();
            //oShiftViewModel.ShiftStartMinTime = ArrayStartTime[1].ToString();
            //string[] ArrayEndTime = oShift.sEndTime.Split(':');
            oShiftViewModel.ShiftStartTime = oShift.ShiftStartTime;
            oShiftViewModel.ShiftEndTime = oShift.ShiftEndTime;
            return oShiftViewModel;
        }
        [HttpGet]
        public ActionResult ShiftSearchView()
        {
            return View(new ShiftMasterViewModel());
        }
        [HttpPost]
     
        public ActionResult ShiftSearchView(ShiftMasterViewModel oShiftViewModel)
        {
            try
            {
                oShiftViewModel.ValidationFilter(ModelState, "btnShiftMasterSearch");
                if (oShiftViewModel == null) oShiftViewModel = new ShiftMasterViewModel();
                oShiftViewModel.ShiftList = new List<ShiftMasterViewModel>();
                // add the data into Shift list to display into the grid
                if (oShiftViewModel.SearchShiftName != null)
                {
                    oShiftViewModel.ShiftList.AddRange(iShiftService.GetShiftList(oShiftViewModel.SearchShiftName).Select(item => new ShiftMasterViewModel
                    {
                        ShiftID = item.ShiftID,
                        SearchShiftName = item.ShiftName


                    }));
                }
                else
                {
                    oShiftViewModel.ShiftList.AddRange(iShiftService.GetShiftList(oShiftViewModel.ShiftName).Select(item => new ShiftMasterViewModel
                    {
                        ShiftID = item.ShiftID,
                        SearchShiftName = item.ShiftName

                    }));
                }

                if (oShiftViewModel.ShiftList.Count <= 0)
                {
                    ViewData["Message"] = @BPA.GlobalResources.UI.Resources_common.dispNoRecordFound;
                }
            }
            catch (Exception er)
            {

            }
            return View(oShiftViewModel);
        }

        private ShiftMasterModel CatchRecord(ShiftMasterViewModel oShiftViewModel)
        {
            ShiftMasterModel objShiftInfo = new();
            
                objShiftInfo.ShiftID = oShiftViewModel.ShiftID;
                objShiftInfo.ShiftName = oShiftViewModel.ShiftName.ToString();
                objShiftInfo.Description = oShiftViewModel.Description.ToString();
                objShiftInfo.Disabled = oShiftViewModel.Disable;
                objShiftInfo.UserId = base.oUser.iUserID;
                objShiftInfo.ShiftStartTime = oShiftViewModel.ShiftStartTime.ToString();
                objShiftInfo.ShiftEndTime = oShiftViewModel.ShiftEndTime.ToString();
                return objShiftInfo;
            
        }
        //
        // POST: /WorkMangement/ShiftMaster/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oShiftViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ShiftMasterViewModel oShiftViewModel)
        {
            string Flag = String.Empty;
            if (oShiftViewModel.ShiftID == 0)
            {

                Flag = iShiftService.AddShift(CatchRecord(oShiftViewModel));
                if (Flag != "")
                {
                    ViewData["Message"] = Flag;
                }
                else
                {
                    ViewData["Message"] = @BPA.GlobalResources.UI.WorkManagement.Resources_Shift.display_Save_Msg;
                    ModelState.Clear();
                    oShiftViewModel = new ShiftMasterViewModel();
                }

            }
            else
            {
                iShiftService.UpdateShift(CatchRecord(oShiftViewModel));
                ViewData["Message"] = @BPA.GlobalResources.UI.WorkManagement.Resources_Shift.display_ShiftUpdated;
                ModelState.Clear();
                oShiftViewModel = new ShiftMasterViewModel();
            }
            return View(oShiftViewModel);
        }
        [HttpPost]
        public JsonResult EditingCustom_Edit(string sShiftID)
        {
            TempData["sShiftID"] = sShiftID; // Stroe shift id into Temp variable
            TempData.Keep("sShiftID");
            return Json("1");
        }
    }
}
