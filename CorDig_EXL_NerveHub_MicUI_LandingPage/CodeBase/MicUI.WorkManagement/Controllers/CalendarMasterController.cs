using BPA.GlobalResources;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using MicUI.WorkManagement.Models.ViewModels;
using MicUI.WorkManagement.Module.Administration;
using MicUI.WorkManagement.Services.ServiceModel;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MicUI.WorkManagement.Controllers
{
    public class CalendarMasterController : BaseController
    {
        private readonly IHolidayCalenderMaintenanceService _holidayCalenderMaintenance;
        public CalendarMasterController(IHolidayCalenderMaintenanceService holidayCalenderMaintenance)
        {
            _holidayCalenderMaintenance = holidayCalenderMaintenance;
        }
        public IActionResult Index()
        {
            if (!base.CheckViewPermission(int.Parse(Resources_FormID.CalendarMasterFormID)))
            {
                return RedirectToActionPermanent("AccessDenied", "Error");
            }
          
            CalendarMasterModel objCalendar=new();
           

            if (string.IsNullOrEmpty((string)TempData["CalendarID"]))
            {
                objCalendar = new CalendarMasterModel();
            }
            else
            {
                objCalendar = GetModel(_holidayCalenderMaintenance.GetCalendarListID(int.Parse((string)TempData["CalendarID"])));
            }
            return View(objCalendar);
        }
        [HttpPost]
      //  [ValidateAntiForgeryToken]
        public ActionResult Index(CalendarMasterModel objCalendar)
        {

            if (objCalendar.mCalenderID == 0)
            {
                string f = _holidayCalenderMaintenance.AddCalendar(CatchRecord(objCalendar));
                if (f == string.Empty)
                    ViewData["Message"] = BPA.GlobalResources.UI.AppConfiguration.Resources_Calender.msgSave;
            }
            else
            {
                _holidayCalenderMaintenance.UpdateCalendar(CatchRecord(objCalendar));
                ViewData["Message"] = BPA.GlobalResources.UI.AppConfiguration.Resources_Calender.msgUpdate;
            }
            ModelState.Clear();
            objCalendar = new CalendarMasterModel();
            return View(objCalendar);
        }
        private CalendarInfoMasterModel CatchRecord(CalendarMasterModel objCalendar)
        {
            CalendarInfoMasterModel objCalendarInfo = new CalendarInfoMasterModel();
            objCalendarInfo.CalenderID = Convert.ToInt32(objCalendar.mCalenderID);
            objCalendarInfo.CalenderName = objCalendar.mCalenderName.Trim().ToString();
            objCalendarInfo.Description = objCalendar.mDescription;
            objCalendarInfo.Disabled = objCalendar.mIsdisable;
            objCalendarInfo.UserId = base.oUser.iUserID;
            return objCalendarInfo;

        }
        public ActionResult ShowCalendarMaster()
        {

            return View(new CalendarMasterModel());
        }

        public ActionResult CAL_Read([DataSourceRequest] DataSourceRequest request, string CalenderName)
        {
            var list = CatchCalanderRecords(_holidayCalenderMaintenance.GetCalendarList(CalenderName, false));
            
            return Json(list.ToDataSourceResult(request));
        }
        private List<BECalendarInfo> CatchCalanderRecords(List<CalendarInfoMasterModel> objCalendar)
        {
            List<BECalendarInfo> _list = new();
            foreach (var item in objCalendar)
            {
                BECalendarInfo objCalendarInfo = new BECalendarInfo();
                objCalendarInfo.iCalendarID = Convert.ToInt32(item.CalenderID);
                objCalendarInfo.sCalendarName = item.CalenderName.Trim().ToString();
                objCalendarInfo.sDescription = item.Description;
                objCalendarInfo.bDisabled = item.Disabled;
                objCalendarInfo.iCreatedBy = base.oUser.iUserID;
                objCalendarInfo.iModifiedBy = base.oUser.iUserID;
                _list.Add(objCalendarInfo);
            }
            return _list;

        }
        public CalendarMasterModel GetModel(CalendarInfoMasterModel info)
        {
            CalendarMasterModel CALObj = new CalendarMasterModel();
            CALObj.mCalenderID = info.CalenderID;
            CALObj.mCalenderName = info.CalenderName.Trim();
            CALObj.mDescription = info.Description.Trim();
            CALObj.mIsdisable = info.Disabled;
            return CALObj;
        }
        [HttpPost]
        public JsonResult SetCalendarID(string Calids)
        {
           
            int returnValue = 0;
            if (Calids != null)
            {
                TempData["CalendarID"] = Calids;
                TempData.Keep("CalendarID");
                returnValue = 1;
            }
            return Json(returnValue);
        }
    }
}
