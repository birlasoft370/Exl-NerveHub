using BPA.GlobalResources;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using MicUI.WorkManagement.Models.ViewModels;
using MicUI.WorkManagement.Module.Administration;
using MicUI.WorkManagement.Services.ServiceModel;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace MicUI.WorkManagement.Controllers
{
    public class CalendarDataController : BaseController
    {
        private readonly IHolidayCalenderMaintenanceService _holidayCalenderMaintenance;
        private readonly ICalenderService _repositoryCalender;
        public CalendarDataController(IHolidayCalenderMaintenanceService holidayCalenderMaintenance, ICalenderService repositoryCalender)
        {
            _holidayCalenderMaintenance = holidayCalenderMaintenance;
            _repositoryCalender = repositoryCalender;
        }
        public IActionResult Index()
        {
            if (!base.CheckViewPermission(int.Parse(Resources_FormID.CalendarDataFormID)))
            {
                return RedirectToActionPermanent("AccessDenied", "Error");
            }
            CalendarDataModel objCalendarData = new CalendarDataModel();
            string[] arySearchID = null;
            if (TempData["CalendarID"] != null)
            {
                using (BECalendarInfo oCalendar = new BECalendarInfo())
                {
                    arySearchID = TempData["CalendarID"].ToString().Split('|');

                    if (arySearchID.Length > 2)
                    {
                        oCalendar.iCalendarID = int.Parse(arySearchID[0]);
                        oCalendar.iYear = int.Parse(arySearchID[1]);
                        oCalendar.iMonth = int.Parse(arySearchID[2]);

                        objCalendarData = DisplayRecord(_repositoryCalender.GetCalendarDataById(oCalendar.iCalendarID, oCalendar.iMonth,oCalendar.iYear));
                    }
                }
            }
            else
            {
                objCalendarData = new CalendarDataModel();
            }
            return View("Index", objCalendarData);
        }
        public JsonResult ReBindeWeek(string Parameters)
        {
            CalendarDataModel objCalendarData = new CalendarDataModel();

            if (Parameters != null)
            {
                using (BECalendarInfo oCalendar = new BECalendarInfo())
                {

                    if (Parameters.Split('|')[0] != null && Parameters.Split('|')[2] != null && Parameters.Split('|')[1] != null)
                    {
                        oCalendar.iCalendarID = int.Parse(Parameters.Split('|')[0]);
                        oCalendar.iYear = int.Parse(Parameters.Split('|')[2]);
                        oCalendar.iMonth = int.Parse(Parameters.Split('|')[1]);
                        objCalendarData = DisplayRecord(_repositoryCalender.GetCalendarDataById(oCalendar.iCalendarID,oCalendar.iMonth,oCalendar.iYear));
                    }
                }
            }
            else
            {
                objCalendarData = new CalendarDataModel();
            }

            return Json(objCalendarData.WeekList);
        }
        private CalendarDataModel DisplayRecord(CalendarDataDetails lCalendar)
        {
            CalendarDataModel objCalendarDataModel = new CalendarDataModel();
            objCalendarDataModel.iCalID = lCalendar.CalID;
            objCalendarDataModel.mCalendarDate = lCalendar.CalendarID.ToString();
            objCalendarDataModel.mStartDateofMonth = lCalendar.StartDateofMonth.ToString();

            DateTime date = Convert.ToDateTime(lCalendar.StartDateofMonth);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var WeekstartDate = new DateTime(date.Year, date.Month, 1);

            objCalendarDataModel.mStartDateofMonth = firstDayOfMonth.ToString();
            objCalendarDataModel.mEndDateofMonth = lastDayOfMonth.ToString();
            objCalendarDataModel.mYear = lCalendar.Year.ToString();
            objCalendarDataModel.mMonth = lCalendar.Month.ToString();
            objCalendarDataModel.Disable = lCalendar.Disabled;
            objCalendarDataModel.mWeekStartDay = lCalendar.WeekStartDay.ToString();
            //WeekstartDate.DayOfWeek.ToString("d");


            DataTable dtCalander = new DataTable();
            dtCalander.Columns.Add("iWeek", System.Type.GetType("System.Int32"));
            dtCalander.Columns.Add("dtStartDate", System.Type.GetType("System.String"));
            dtCalander.Columns.Add("dtEndDate", System.Type.GetType("System.String"));
            dtCalander.Columns.Add("sRowState", System.Type.GetType("System.String"));
            dtCalander.Columns.Add("iCalendarId", System.Type.GetType("System.Int32"));
            DataRow dr = null;
            List<Calendarweek> WeekList = new List<Calendarweek>();
            for (int count = 0; count < lCalendar.WeekList.Count; count++)
            {

                dr = dtCalander.NewRow();
                dr["iWeek"] = lCalendar.WeekList[count].miWeek;
                dr["sRowState"] = "update";
                dr["iCalendarId"] = lCalendar.WeekList[count].miCalendarId;
                dr["dtStartDate"] = Convert.ToDateTime(lCalendar.WeekList[count].StrDisplayStartDate).ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                dr["dtEndDate"] = Convert.ToDateTime(lCalendar.WeekList[count].StrDisplayEndDate).ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);

                dtCalander.Rows.Add(dr);
                Calendarweek onkCalendarweek = new Calendarweek();
                onkCalendarweek.miWeek = lCalendar.WeekList[count].miWeek;
                //.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                onkCalendarweek.DisplayStartDate = DateTime.Parse(Convert.ToDateTime(lCalendar.WeekList[count].DisplayStartDate).ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern));
                onkCalendarweek.DisplayEndDate = DateTime.Parse(Convert.ToDateTime(lCalendar.WeekList[count].DisplayEndDate).ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern));
                onkCalendarweek.msRowState = "update";
                onkCalendarweek.miCalendarId = lCalendar.WeekList[count].miCalendarId;
                WeekList.Add(onkCalendarweek);
                onkCalendarweek = null;

            }

            objCalendarDataModel.DTWeek = dtCalander;
            objCalendarDataModel.WeekList = WeekList;
            //TempData["DTWeek"] = dtCalander;
            //TempData.Keep("DTWeek");


            //TempData["CalendarDT"] = objCalendarDataModel;
            return objCalendarDataModel;
        }
        [HttpPost]
        public ActionResult Index(CalendarDataModel objCalendarData)
        {
            objCalendarData.ValidationFilter(ModelState, "btnDefineWeek,btnSave_CalendarData");
            return View(objCalendarData);
        }
        [HttpPost]
      //  [ValidateAntiForgeryToken]
        public ActionResult InsertUpdateCalenderData(CalendarDataModel strCalendarDataModel, List<Calendarweek> jsonCalendarGridData)
        {

            CalendarDataModel objCalendarDataModel = strCalendarDataModel;// JsonConvert.DeserializeObject<CalendarDataModel>(jsonCalendarGridData);
            List<Calendarweek> objCalendarweek = jsonCalendarGridData;// JsonConvert.DeserializeObject<List<Calendarweek>>(jsonCalendarGridData);

            string result = "0";
            try
            {
                if (objCalendarDataModel.iCalID == 0)
                {
                    string flag = _repositoryCalender.AddCalenderData(CatchRecord(objCalendarDataModel, objCalendarweek));

                    string[] aryDataFlag = flag.Split('|');
                    if (aryDataFlag[0].ToString() == "1")
                    {
                        result = "1";
                    }
                    else if (aryDataFlag[0].ToString() == "2")
                    {
                        result = @BPA.GlobalResources.UI.AppConfiguration.Resources_Calender.dispCalendarDataExist + objCalendarDataModel.mCalendarDate + ", Year " + objCalendarDataModel.mCalendarDate + " and Week " + objCalendarDataModel.mCalendarDate;
                    }
                    else if (aryDataFlag[0].ToString() == "3")
                    {
                        result = @BPA.GlobalResources.UI.AppConfiguration.Resources_Calender.dispDateRangeOverlaps + aryDataFlag[1].ToString();
                    }
                }
                else
                {
                    Update(objCalendarDataModel, objCalendarweek);
                    result = "5";
                }
            }

            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }
            return Json(result);
        }
        private void Update(CalendarDataModel objCalendarDataModel, List<Calendarweek> objCalendarweek)
        {

            string sReturnVal = _repositoryCalender.UpdateCalenderData(CatchRecord(objCalendarDataModel, objCalendarweek));

            string[] aryReturnVal = sReturnVal.Split('|');
            if (aryReturnVal[0].ToString() == "1")
            {
                ViewData["Message"] = BPA.GlobalResources.UI.AppConfiguration.Resources_Calender.msgCalendarDataInformation;
            }
            else if (aryReturnVal[0].ToString() == "3")
            {
                ViewData["Message"] = BPA.GlobalResources.UI.AppConfiguration.Resources_Calender.msgDateRangeOverLap + aryReturnVal[1].ToString();
            }
            else if (aryReturnVal[0].ToString() == "2")
            {
                ViewData["Message"] = BPA.GlobalResources.UI.AppConfiguration.Resources_Calender.msgCalendarAlreadyDefined + aryReturnVal[1].ToString();
            }
        }
        public JsonResult GetCalendar()
        {
            var list = _holidayCalenderMaintenance.GetCalendarList("", true);
            return Json(list);
       
        }
        public JsonResult Products_Read_Grd(string Parameters)
        {
            return Json(GetList(Parameters).WeekList);
        }
        [HttpPost]
        public ActionResult Products_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<Calendarweek> Calendarweek, FormCollection frm)
        {
            return Json(Calendarweek.ToDataSourceResult(request, ModelState));
        }
        private CalendarDataModel GetList(string Parameter)
        {
            DateTimeFormatInfo DateInfo = CultureInfo.CurrentCulture.DateTimeFormat;
            CalendarDataModel objCalendarData = new CalendarDataModel();
            // Have to do further 
            if (Parameter != null)
            {
                string[] Pram = Parameter.Split('|');
                objCalendarData.mCalendarDate = Pram[0];
                objCalendarData.mMonth = Pram[1];
                objCalendarData.mYear = Pram[2];
                objCalendarData.mStartDateofMonth = Pram[3];
                objCalendarData.mEndDateofMonth = Pram[4];
                objCalendarData.mWeekStartDay = Pram[5];
                objCalendarData.iCalID = Convert.ToInt16(Pram[6]);
                objCalendarData.mMonthYear = Pram[7];

            }

            DataTable CalendarDT = new DataTable();
            if (objCalendarData.iCalID != 0)
            {
                //Start Date
                CalendarDT = objCalendarData.DTWeek;
                using (BECalendarInfo oCalendar = new BECalendarInfo())
                {

                    oCalendar.iCalendarID = int.Parse(objCalendarData.mCalendarDate);
                    oCalendar.iYear = int.Parse(objCalendarData.mYear);
                    oCalendar.iMonth = int.Parse(objCalendarData.mMonth);

                    IList<CalendarDataModel> CalendarDataList = _repositoryCalender.GetCalendarDataList(oCalendar.iCalendarID,oCalendar.iMonth,oCalendar.iYear);

                    for (int count = 0; count < CalendarDataList.Count; count++)
                    {

                        var obj = new Calendarweek();
                        obj.miWeek = Convert.ToInt32(CalendarDataList[count].miWeek.ToString());
                        obj.DisplayStartDate = DateTime.Parse(CalendarDataList[count].DisplayStartDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern));
                        obj.DisplayEndDate = DateTime.Parse(CalendarDataList[count].DisplayEndDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern));
                        obj.msRowState = "update";
                        obj.miCalendarId = Convert.ToInt32(CalendarDataList[count].iCalendarID);
                        objCalendarData.WeekList.Add(obj);
                    }
                }
            }
            else
            {
                if (objCalendarData.mCalendarDate != "")
                {
                    CalendarDT = DefineWeeks(objCalendarData);
                    for (int count = 0; count < CalendarDT.Rows.Count; count++)
                    {
                        var obj = new Calendarweek();
                        obj.miWeek = Convert.ToInt32(CalendarDT.Rows[count]["iWeek"].ToString());
                        DateTime DT = Convert.ToDateTime(String.Format("{0:" + DateInfo.ShortDatePattern + "}", CalendarDT.Rows[count]["dtStartDate"].ToString()), CultureInfo.CurrentCulture);
                        obj.DisplayStartDate = DateTime.Parse(CalendarDT.Rows[count]["dtStartDate"].ToString()); // Convert.ToDateTime(DT.ToShortDateString()).Date; 
                        obj.DisplayEndDate = DateTime.Parse(CalendarDT.Rows[count]["dtEndDate"].ToString());
                        obj.msRowState = (CalendarDT.Rows[count]["sRowState"].ToString());
                        obj.miCalendarId = Convert.ToInt32(CalendarDT.Rows[count]["iCalendarId"].ToString());
                        objCalendarData.WeekList.Add(obj);

                    }
                }

            }
            return objCalendarData;
        }
        private static DataTable CreateTable()
        {
            DataTable dtCalander = new DataTable();

            dtCalander.Columns.Add("iWeek", System.Type.GetType("System.Int32"));
            dtCalander.Columns.Add("dtStartDate", typeof(DateTime));
            dtCalander.Columns.Add("dtEndDate", typeof(DateTime));
            dtCalander.Columns.Add("sRowState", System.Type.GetType("System.String"));
            dtCalander.Columns.Add("iCalendarId", System.Type.GetType("System.Int32"));
            return dtCalander;
        }
        private DataTable DefineWeeks(CalendarDataModel objCalendar)
        {
            DataTable dtCalander = CreateTable();
            string sStartDate = objCalendar.mStartDateofMonth;
            string sEndDate = objCalendar.mEndDateofMonth;
            int iDayOfWeek = Convert.ToInt32(objCalendar.mWeekStartDay);
            DateTime dtWeekEndDate = Convert.ToDateTime(sEndDate);
            DateTime dtWeekStartDate = Convert.ToDateTime(sStartDate);
            DayOfWeek dwDate;

            int iRowCounter = 0;
            int iDateDiff = 0;
            int iDays = 0;
            TimeSpan ts = Convert.ToDateTime(sEndDate) - Convert.ToDateTime(sStartDate);
            iDateDiff = ts.Days;
            BECalendarInfo oCalInfo = new BECalendarInfo();
            oCalInfo.iCalendarID = Convert.ToInt32(objCalendar.mCalendarDate);
            DateTime dt = Convert.ToDateTime("1" + objCalendar.mMonthYear);
            oCalInfo.iYear = Convert.ToInt32(dt.Year);
            objCalendar.MaxWeek = _repositoryCalender.GetMaxWeek(oCalInfo.iCalendarID,oCalInfo.iYear );
            oCalInfo = null;
            int days = (dtWeekEndDate - dtWeekStartDate).Days + 1;
            int NumOfWeeks = 0;
            if ((days % 7) == 0)
            {
                NumOfWeeks = (days / 7);
            }
            else
            {
                NumOfWeeks = (days / 7) + 1;
            }
            if (objCalendar.MaxWeek == 0)
            {
                for (int i = 1; i <= NumOfWeeks; i++)
                {
                    DataRow dr = dtCalander.NewRow();
                    dr["iWeek"] = i;
                    dr["sRowState"] = "insert";
                    dr["iCalendarId"] = 0;
                    if (i == 1)
                    {
                        dr["dtStartDate"] = sStartDate;
                        dtWeekStartDate = Convert.ToDateTime(sStartDate);
                        dwDate = dtWeekStartDate.DayOfWeek;
                        iDays = dwDate - (DayOfWeek)iDayOfWeek;
                        dtWeekEndDate = iDays < 0 ? dtWeekStartDate.AddDays((-iDays) - 1) : dtWeekStartDate.AddDays(6 - iDays);
                        dr["dtEndDate"] = dtWeekEndDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                        dtWeekStartDate = dtWeekEndDate.AddDays(1);
                        iDateDiff = iDays < 0 ? (((iDateDiff + iDays)) < 0 ? 0 : (iDateDiff + iDays)) : ((iDateDiff - (6 - iDays)) < 0 ? 0 : iDateDiff - (6 - iDays));
                    }
                    else
                    {
                        dr["dtStartDate"] = dtWeekStartDate >= Convert.ToDateTime(sEndDate) ? sEndDate : dtWeekStartDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                        if (iDateDiff != 0)
                        {
                            if ((iDateDiff / 7) != 0)
                            {
                                dtWeekEndDate = dtWeekStartDate.AddDays(6);
                                iDateDiff = (iDateDiff - 6) < 0 ? 0 : iDateDiff - 6;
                            }
                            else
                            {
                                dtWeekEndDate = dtWeekStartDate.AddDays(iDateDiff % 7);
                                iDateDiff = (iDateDiff - iDateDiff % 7 - 1) < 0 ? 0 : iDateDiff - iDateDiff % 7 - 1;
                            }
                            dr["dtEndDate"] = dtWeekEndDate >= Convert.ToDateTime(sEndDate) ? sEndDate : dtWeekEndDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                            dtWeekStartDate = dtWeekEndDate.AddDays(1);
                        }
                        else
                        {
                            dr["dtEndDate"] = dtWeekStartDate >= Convert.ToDateTime(sEndDate) ? sEndDate : dtWeekStartDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                        }

                    }
                    dtCalander.Rows.Add(dr);
                }
            }
            else
            {

                iRowCounter = iRowCounter + 4;
                for (int i = objCalendar.MaxWeek + 1; i <= Convert.ToInt16(objCalendar.MaxWeek) + NumOfWeeks; i++)
                {
                    //if (i <= 52)
                    //{
                    DataRow dr = dtCalander.NewRow();
                    dr["iWeek"] = i;
                    dr["sRowState"] = "insert";
                    dr["iCalendarId"] = 0;
                    if (i == Convert.ToInt16(objCalendar.MaxWeek) + 1)
                    {
                        dr["dtStartDate"] = sStartDate;
                        dtWeekStartDate = Convert.ToDateTime(sStartDate);
                        dwDate = dtWeekStartDate.DayOfWeek;
                        iDays = dwDate - (DayOfWeek)iDayOfWeek;
                        dtWeekEndDate = iDays < 0 ? dtWeekStartDate.AddDays((-iDays) - 1) : dtWeekStartDate.AddDays(6 - iDays);
                        dr["dtEndDate"] = dtWeekEndDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                        dtWeekStartDate = dtWeekEndDate.AddDays(1);
                        iDateDiff = iDays < 0 ? (((iDateDiff + iDays)) < 0 ? 0 : (iDateDiff + iDays)) : ((iDateDiff - (6 - iDays)) < 0 ? 0 : iDateDiff - (6 - iDays));
                    }
                    else
                    {
                        dr["dtStartDate"] = dtWeekStartDate >= Convert.ToDateTime(sEndDate) ? sEndDate : dtWeekStartDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                        if (iDateDiff != 0)
                        {
                            if ((iDateDiff / 7) != 0)
                            {
                                dtWeekEndDate = dtWeekStartDate.AddDays(6);
                                iDateDiff = (iDateDiff - 6) < 0 ? 0 : iDateDiff - 6;
                            }
                            else
                            {
                                dtWeekEndDate = dtWeekStartDate.AddDays(iDateDiff % 7);
                                iDateDiff = (iDateDiff - iDateDiff % 7 - 1) < 0 ? 0 : iDateDiff - iDateDiff % 7 - 1;
                            }
                            dr["dtEndDate"] = dtWeekEndDate >= Convert.ToDateTime(sEndDate) ? sEndDate : dtWeekEndDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                            dtWeekStartDate = dtWeekEndDate.AddDays(1);
                        }
                        else
                        {
                            dr["dtEndDate"] = dtWeekStartDate >= Convert.ToDateTime(sEndDate) ? sEndDate : dtWeekStartDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                        }

                    }

                    dtCalander.Rows.Add(dr);

                }
            }
            if (dtWeekStartDate <= Convert.ToDateTime(sEndDate))
            {
                DataRow dr = dtCalander.NewRow();
                dr["iWeek"] = int.Parse(dtCalander.Rows[dtCalander.Rows.Count - 1][0].ToString()) + 1;
                dr["sRowState"] = "insert";
                dr["iCalendarId"] = 0;
                dr["dtStartDate"] = dtWeekStartDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                dr["dtEndDate"] = sEndDate;
                dtCalander.Rows.Add(dr);
            }
            return dtCalander;
        }
        #region SetCalendarID
        [HttpPost]
        public JsonResult SetCalendarID(string CalendarID)
        {
           
            int returnValue = 0;
            if (CalendarID != null)
            {
                TempData["CalendarID"] = CalendarID;
                TempData.Keep("CalendarID");
                returnValue = 1;
            }
            return Json(returnValue);
        }
        #endregion
        public ActionResult ViewCalendarData()
        {
            return View("ViewCalendarData", new CalendarDataModel());
        }
        [HttpPost]
       
        public ActionResult ViewCalendarData(CalendarDataModel objCalendarDataModel)
        {
      
            objCalendarDataModel.ValidationFilter(ModelState, "btnSearchCalendarData");
            if (objCalendarDataModel.mCalendarDateSearch != null && objCalendarDataModel.mYearSearch != null && objCalendarDataModel.mMonthSearch != null)
            {
                objCalendarDataModel.mCalendarList = GetDetails(_repositoryCalender.GetCalendarDataList(Convert.ToInt16(objCalendarDataModel.mCalendarDateSearch), Convert.ToInt16(objCalendarDataModel.mMonthSearch), Convert.ToInt16(objCalendarDataModel.mYearSearch)));
            }
            else if (objCalendarDataModel.mCalendarDateSearch != null)
            {
                objCalendarDataModel.mCalendarList = GetDetails(_repositoryCalender.GetCalendarDataList(Convert.ToInt16(objCalendarDataModel.mCalendarDateSearch),Convert.ToInt16(objCalendarDataModel.mMonthSearch), Convert.ToInt16(objCalendarDataModel.mYearSearch)));
            }
            else
            {
                objCalendarDataModel.mCalendarList = GetDetails(_repositoryCalender.GetCalendarDataList(0, Convert.ToInt16(objCalendarDataModel.mMonthSearch), Convert.ToInt16(objCalendarDataModel.mYearSearch)));
            }
            if (objCalendarDataModel.mCalendarList.Count <= 0)
            {
                ViewData["Message"] = @BPA.GlobalResources.UI.Resources_common.display_msgNotFound;
            }
            return View("ViewCalendarData", objCalendarDataModel);
        }
        private List<BECalendarInfo> GetDetails(List<CalendarDataModel> CalendarDataList)
        {
            List<BECalendarInfo> list = new();
            foreach (var item in CalendarDataList)
            {
                BECalendarInfo calanderDet = new();
                calanderDet.sCalendarName = item.CalendarName;
                calanderDet.sDescription=item.Description;
               

                list.Add(calanderDet);
            }
            return list;


        }
        private BECalendarInfo GetCalanderData(CalendarDataModel CalendarDataList)
        {
                BECalendarInfo calanderDet = new();
                calanderDet.sCalendarName = CalendarDataList.CalendarName;
                calanderDet.sDescription = CalendarDataList.Description;
                return calanderDet;


        }


        private CalendarDataDetails CatchRecord(CalendarDataModel objCalendarDataModel, List<Calendarweek> objCalendarweek)
        {

            DateTime dt = Convert.ToDateTime("1" + objCalendarDataModel.mMonthYear);
            CalendarDataDetails objCalendarInfo = new CalendarDataDetails();
            objCalendarInfo.CalendarID = Convert.ToInt32(objCalendarDataModel.mCalendarDate);
            objCalendarInfo.Month = dt.Month;
            objCalendarInfo.Year = dt.Year;

            DateTimeFormatInfo DateInfo = CultureInfo.CurrentCulture.DateTimeFormat;
            DateTime DT = Convert.ToDateTime(String.Format("{0:" + DateInfo.ShortDatePattern + "}", objCalendarDataModel.mStartDateofMonth.Trim()), CultureInfo.CurrentCulture);

            objCalendarInfo.StartDateofMonth = DT.ToShortDateString();
            objCalendarInfo.Disabled = Convert.ToBoolean(objCalendarDataModel.Disable);
            objCalendarInfo.WeekList = objCalendarweek;
            objCalendarInfo.MonthYear = objCalendarDataModel.mMonthYear;
            return objCalendarInfo;
        }
        [HttpPost]
        public ActionResult Products_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<Calendarweek> Calendarweek, FormCollection frm)
        {
            IList<Calendarweek> results = new List<Calendarweek>();
            if (Calendarweek != null && ModelState.IsValid)
            {
                foreach (var week in Calendarweek)
                {
                    results.Add(week);
                }
            }
            return Json(results.ToDataSourceResult(request, ModelState, p => new Calendarweek { miWeek = results[0].miWeek, DisplayStartDate = results[0].DisplayStartDate, DisplayEndDate = results[0].DisplayEndDate }));
        }
        public ActionResult Products_Read([DataSourceRequest] DataSourceRequest request, string Parameters)
        {
            return Json(GetList(Parameters).WeekList.ToDataSourceResult(request));
        }
    }
}
