using BPA.GlobalResources;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using MicUI.WorkManagement.Models.ViewModels;
using MicUI.WorkManagement.Module.Administration;
using MicUI.WorkManagement.Services.ServiceModel;
using System.Data;

namespace MicUI.WorkManagement.Controllers
{
    public class ProcessOffsController : BaseController
    {
        private readonly IHolidayCalenderMaintenanceService _holidayCalenderMaintenance;
       public ProcessOffsController(IHolidayCalenderMaintenanceService holidayCalenderMaintenance)
        {
            _holidayCalenderMaintenance = holidayCalenderMaintenance;
        }

        public IActionResult Index()
        {
            if (!base.CheckViewPermission(int.Parse(Resources_FormID.ProcessOffsFormID)))
            {
                return RedirectToActionPermanent("AccessDenied", "Error");
            }
          
            ProcessOffsModel objProcessOffsModel = null;

            if (string.IsNullOrEmpty((string)TempData["ProcessOffsID"]))
            {
                objProcessOffsModel = new ProcessOffsModel();
                objProcessOffsModel.IsEditMode = false;
            }
            else
            {
                string SearchID = ((string)TempData["ProcessOffsID"]).Split('|')[0].ToString();
                string flagMonth = ((string)TempData["ProcessOffsID"]).Split('(')[1].ToString().Trim();
                string Month = flagMonth.Split('-')[0].ToString().Trim();
                string flagYear = flagMonth.Split('-')[1].ToString().Trim();
                string Year = flagYear.Split(')')[0];
                string monthofYear = GetMonthNumber(Month);
                int GD = GetDaysInMonth(Convert.ToInt16(monthofYear), Convert.ToInt16(Year));
                objProcessOffsModel = new ProcessOffsModel();
                objProcessOffsModel = DisplayRecord(_holidayCalenderMaintenance.GetProcessOffsById(int.Parse(SearchID), int.Parse(monthofYear), int.Parse(Year)), TempData["ProcessOffsID"].ToString());
                objProcessOffsModel.IsEditMode = true;
            }
            return View(objProcessOffsModel);
        }
        [HttpPost]
      
        public ActionResult Index(ProcessOffsModel objProcessOff, IFormCollection frm, string button)
        {
            objProcessOff.ValidationFilter(ModelState, "btnGenerate");

            if (objProcessOff.SubmitMode == "Generate")
            {

                DataTable GridData = new DataTable();
                GridData = GenerateGrid(objProcessOff);
                objProcessOff.DaysList = GridData;
                for (int rowcout = 0; rowcout < GridData.Rows.Count; rowcout++)
               {
                    GridList obj = new GridList();
                   obj.IsSelected = 0;
                   obj.DateName = (GridData.Rows[rowcout]["DateName"].ToString());
                  obj.dayName = (GridData.Rows[rowcout]["dayName"].ToString());
                    objProcessOff.GridListView.Add(obj);
                    obj = null;
               }

            }
           
            else
                {

                    if (objProcessOff.iprocessOffId != 0)
                    {
                        if (frm["chkRow"].ToString() != null)
                        {
                            Update(objProcessOff, frm["chkRow"].ToString().Split(','));
                            objProcessOff = new ProcessOffsModel();
                            ModelState.Clear();
                        }

                    }
                    else if (objProcessOff.iprocessOffId == 0)
                    {
                        if (!string.IsNullOrEmpty(frm["chkRow"]))
                        {
                            Add(objProcessOff, frm["chkRow"].ToString().Split(','));
                            objProcessOff = new ProcessOffsModel();
                            ModelState.Clear();
                        }
                        else
                        {
                            DataTable GridData = new DataTable();
                            GridData = GenerateGrid(objProcessOff);
                            objProcessOff.DaysList = GridData;
                            for (int rowcout = 0; rowcout < GridData.Rows.Count; rowcout++)
                            {
                                GridList obj = new GridList();
                                obj.IsSelected = 0;
                                obj.DateName = (GridData.Rows[rowcout]["DateName"].ToString());
                                obj.dayName = (GridData.Rows[rowcout]["dayName"].ToString());
                                objProcessOff.GridListView.Add(obj);
                                obj = null;
                            }
                            ViewData["Message"] = @BPA.GlobalResources.UI.WorkManagement.Resources_ProcessOff.dispalySelectDay;
                            return View("Index", objProcessOff);
                        }
                    }
                }
                return View("Index", objProcessOff);
        }
        public void Add(ProcessOffsModel objProcessOff, string[] chk)
        {
            ProcessOffModel oProcess = CatchRecord(objProcessOff, chk);
            if (oProcess.DaysNameList.Length < 1)
            {
                ViewData["Message"] = BPA.GlobalResources.UI.WorkManagement.Resources_ProcessOff.dispalySelectDay;
                return;
            }
            else
            {
                _holidayCalenderMaintenance.AddProcessOffs(oProcess);
                ViewData["Message"] = BPA.GlobalResources.UI.WorkManagement.Resources_ProcessOff.displaySaved;
            }

        }
        public void Update(ProcessOffsModel ObjProcessOff, string[] chk)
        {
            _holidayCalenderMaintenance.UpdateProcessOffs(CatchRecord(ObjProcessOff, chk));
            ViewData["Message"] = BPA.GlobalResources.UI.WorkManagement.Resources_ProcessOff.displayUpdated;
        }
        private ProcessOffModel CatchRecord(ProcessOffsModel objProcessOffsModel, string[] WeekDays)
        {
            ProcessOffModel objProcessOff = new ProcessOffModel();
            objProcessOff.ProcessID = Convert.ToInt32(objProcessOffsModel.mProcessID);
            objProcessOff.Description = objProcessOffsModel.mDescription;
            objProcessOff.DaysNameList = WeekDays;
            return objProcessOff;
        }
       
        private static DataTable CreateTable()
        {
            DataTable dtProcessoff = new DataTable();
            dtProcessoff.Columns.Add("iWeek", System.Type.GetType("System.Int32"));
            dtProcessoff.Columns.Add("WeekDays", System.Type.GetType("System.String"));
            dtProcessoff.Columns.Add("DISABLED", System.Type.GetType("System.Boolean"));
            dtProcessoff.Columns.Add("Days", System.Type.GetType("System.String"));
            dtProcessoff.Columns.Add("Months", System.Type.GetType("System.String"));
            dtProcessoff.Columns.Add("Years", System.Type.GetType("System.String"));
            return dtProcessoff;
        }
        private DataTable GenerateGrid(ProcessOffsModel objProcessOffsModel)
        {
            string strFirstLastDay = "1/1/1900|1/1/1900";
            DateTime dtFirstDay;
            DateTime dtLastDay;
            string[] arrMonthYear = objProcessOffsModel.mMonthYear.Split(' ');
            DateTime dt = Convert.ToDateTime("1 " + objProcessOffsModel.mMonthYear);
            strFirstLastDay = _holidayCalenderMaintenance.GetFirstLastDayOfCalender(Convert.ToInt16(objProcessOffsModel.mProcessID), Convert.ToInt16(dt.Month), Convert.ToInt16(dt.Year));
            dtFirstDay = DateTime.Parse(strFirstLastDay.Split('|')[0].ToString());
            dtLastDay = DateTime.Parse(strFirstLastDay.Split('|')[1].ToString());
            int MaxDay = int.Parse(strFirstLastDay.Split('|')[2].ToString());
            DataTable Dt = new DataTable();
            Dt.Columns.Add("DateName", typeof(string));
            Dt.Columns.Add("dayName", typeof(string));
            DataRow dr;
            DateTime DateTimevalue;
            if (MaxDay > 0)
            {
                for (int counter = 0; counter <= MaxDay; counter++)
                {
                    string[] dayArray = new string[8];
                    dayArray[0] = "Sunday";
                    dayArray[1] = "Monday";
                    dayArray[2] = "Tuesday";
                    dayArray[3] = "Wednesday";
                    dayArray[4] = "Thrusday";
                    dayArray[5] = "Friday";
                    dayArray[6] = "Saturday";
                    dr = Dt.NewRow();
                    TimeSpan ts = new TimeSpan(counter, 0, 0, 0);
                    DateTimevalue = dtFirstDay.Add(ts);
                    dr["DateName"] = DateTimevalue.ToString("MM/dd/yyyy");
                    if (DateTimevalue >= dtFirstDay && DateTimevalue <= dtLastDay)
                    {
                        string dayofwe = dayArray[Convert.ToInt16(DateTimevalue.DayOfWeek)];
                        dr["dayName"] = dayofwe;
                        Dt.Rows.Add(dr);
                    }
                }

            }
            else
            {
                ViewData["Message"] = BPA.GlobalResources.UI.WorkManagement.Resources_ProcessOff.displayCanderNotSetUp;

            }
            return Dt;

        }
        public ActionResult ShowProcessOffs()
        {
            return View("ShowProcessOffs", new ProcessOffsModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowProcessOffs(ProcessOffsModel ObjProcessOffs)
        {
            ObjProcessOffs.ValidationFilter(ModelState, "ProcessOffsbtnSearch");
            if (ObjProcessOffs.ProcessIDSearch != null && ObjProcessOffs.YearSearch != null && ObjProcessOffs.MonthSearch != null)
            {
                ObjProcessOffs.BEProcessOffList = _holidayCalenderMaintenance.GetProcessOffList(int.Parse(ObjProcessOffs.ProcessIDSearch), int.Parse(ObjProcessOffs.MonthSearch), int.Parse(ObjProcessOffs.YearSearch));
            }
            if (ObjProcessOffs.BEProcessOffList.Count <= 0) { ViewData["Message"] = BPA.GlobalResources.UI.Resources_common.dispNoRecordFound; }
            return View("ShowProcessOffs", ObjProcessOffs);
        }
        public JsonResult GetCascadeClient()
        {
            return Json(_holidayCalenderMaintenance.GetClientList(true));
        }
        public ActionResult ProcessOffs_Read([DataSourceRequest] DataSourceRequest request, int? processid, int? year, int? month)
        {
            ProcessOffsModel ObjProcessOffs = new ProcessOffsModel();
            if (processid != null && year != 0 && month != 0)
            {
                ObjProcessOffs.BEProcessOffList = _holidayCalenderMaintenance.GetProcessOffList(processid.GetValueOrDefault(), year.GetValueOrDefault(), month.GetValueOrDefault());
            }
            return Json(ObjProcessOffs.BEProcessOffList.ToDataSourceResult(request));
        }
        public JsonResult GetCascadeProcess(int iClientID)
        {
            if (Request.GetTypedHeaders().Referer != null)
            {
                return Json(_holidayCalenderMaintenance.GetProcessListSearch(iClientID, ""));
            }
            else
            {
                return Json(new List<BEProcessInfo>());
            }
        }
        [HttpPost]
        public JsonResult SetProcessOffsID(string Proccessid)
        {
            int returnValue = 0;
            if (Proccessid != null)
            {
                TempData["ProcessOffsID"] = Proccessid;
                TempData.Keep("ProcessOffsID");
                returnValue = 1;

            }
            return Json(returnValue);
        }
        public static int GetDaysInMonth(int month, int year)
        {

            if (1 == month || 3 == month || 5 == month || 7 == month || 8 == month ||
            10 == month || 12 == month)
            {
                return 31;
            }
            else if (2 == month)
            {

                if (0 == (year % 4))
                {

                    if (0 == (year % 400))
                    {
                        return 29;
                    }
                    else if (0 == (year % 100))
                    {
                        return 28;
                    }


                    return 29;
                }

                return 28;
            }
            return 30;
        }
        private ProcessOffsModel DisplayRecord(ProcessOffDisplayDetail oProcessOff, string ProcessId)
        {
            string SearchID = "";
            ProcessOffsModel objProcessOffModel = new ProcessOffsModel();
            SearchID = ProcessId.Split('|')[0].ToString();
            string flagMonth = ProcessId.Split('(')[1].ToString().Trim();
            string Month = flagMonth.Split('-')[0].ToString().Trim();
            string flagYear = flagMonth.Split('-')[1].ToString().Trim();
            string Year = flagYear.Split(')')[0];
            string monthofYear = GetMonthNumber(Month);
            GetDaysInMonth(Convert.ToInt16(monthofYear), Convert.ToInt16(Year));

            objProcessOffModel.BEProcessOffList = oProcessOff.BEProcessOffList;

            objProcessOffModel.iprocessOffId = int.Parse(SearchID);
            objProcessOffModel.mClientID = oProcessOff.ClientID.ToString();
            objProcessOffModel.mProcessID = oProcessOff.ProcessID.ToString();

            objProcessOffModel.mDescription = oProcessOff.Description;

            objProcessOffModel.mYear = oProcessOff.Year.ToString();
            objProcessOffModel.mMonth = oProcessOff.Month.ToString();
            objProcessOffModel.mMonthYear = Month + " " + Year;
            objProcessOffModel.DaysList = GenerateGrid(objProcessOffModel);

            return objProcessOffModel;

        }
        private string GetMonthNumber(string Month)
        {
            string MonthName = "";
            if (Month.Trim() == "Jan")
                MonthName = "1";
            if (Month.Trim() == "Feb")
                MonthName = "2";
            if (Month.Trim() == "Mar")
                MonthName = "3";
            if (Month.Trim() == "Apr")
                MonthName = "4";
            if (Month.Trim() == "May")
                MonthName = "5";
            if (Month.Trim() == "Jun")
                MonthName = "6";
            if (Month.Trim() == "Jul")
                MonthName = "7";
            if (Month.Trim() == "Aug")
                MonthName = "8";
            if (Month.Trim() == "Sep")
                MonthName = "9";
            if (Month.Trim() == "Oct")
                MonthName = "10";
            if (Month.Trim() == "Nov")
                MonthName = "11";
            if (Month.Trim() == "Dec")
                MonthName = "12";

            return MonthName;

        }
    }
}
