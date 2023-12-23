using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.BusinessEntity.ExternalRef.BIReports;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;
using BPA.AppConfiguration.BaseController;
using BPA.AppConfiguration.Helper;
using BPA.AppConfiguration.Helper.Filter;
using BPA.AppConfiguration.Models;
using BPA.AppConfiguration.Models.Request;
using BPA.AppConfiguration.Models.Response;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using System.Reflection;

namespace BPA.AppConfiguration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthentication]
    public class CalendarDataController : BaseController<CalendarDataController>
    {
        private readonly ICalenderService _repositoryCalender;
        public CalendarDataController(ILogger<CalendarDataController> logger, IWebHostEnvironment env, IConfiguration configuration,
            ICalenderService repositoryCalender) : base(logger, env, configuration)
        {
            this._repositoryCalender = repositoryCalender;
        }

        [ProducesResponseType(typeof(List<BEProcessOff>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetCalendarDataList")]
        public async Task<MessageResponse<List<CalendarDataModel>>> GetCalendarDataList([FromQuery] int CalendarID, [FromQuery] int Month, [FromQuery] int Year)
        {
            var result = new MessageResponse<List<CalendarDataModel>>();
            IList<BECalendarInfo> mCalendarList = new List<BECalendarInfo>();
            try
            {
                if (CalendarID > 0)
                {
                    mCalendarList = await Task.Run(() => _repositoryCalender.GetCalendarDataList(CalendarID, Year, Month, true, base.oTenant));
                }
                else
                {
                    mCalendarList = await Task.Run(() => _repositoryCalender.GetCalendarDataList("", false, base.oTenant));
                }
                result.Data = mCalendarList.Select(x => new CalendarDataModel()
                {
                    CalendarID = x.iCalendarID,
                    CalID = x.iCalendarDataId,
                    Disabled = x.bDisabled,
                    Month = x.iMonth,
                    Year = x.iYear,
                    CalendarName = x.sCalendarName,
                    Description=x.sDescription
                }).ToList();
                result.TotalCount = result.Data.Count;
            }
            catch (Exception ex)
            {

                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(CalendarDataModel), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetCalendarDataById")]
        public async Task<MessageResponse<CalendarDataModel>> GetCalendarDataById([FromQuery] int CalendarID, [FromQuery] int Month, [FromQuery] int Year)
        {
            var result = new MessageResponse<CalendarDataModel>();
            CalendarDataModel objCalendarData = new();
            try
            {
                using (BECalendarInfo oCalendar = new())
                {
                    oCalendar.iCalendarID = CalendarID;
                    oCalendar.iYear = Year;
                    oCalendar.iMonth = Month;
                    objCalendarData = await Task.Run(() => DisplayRecord(_repositoryCalender.GetCalendarDataList(oCalendar, base.oTenant)));
                }
                result.Data = objCalendarData;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("AddCalenderData")]
        public async Task<MessageResponse<string>> AddCalenderData([FromBody] CalendarDataDetails strCalendarDataModel)
        {
            var response = new MessageResponse<string>();
            string result = "0";
            try
            {
                CalendarDataDetails objCalendarDataModel = strCalendarDataModel;// JsonConvert.DeserializeObject<CalendarDataModel>(jsonCalendarGridData);
                List<Calendarweek> objCalendarweek = strCalendarDataModel.WeekList;
                string flag = await Task.Run(() => _repositoryCalender.ManageCalendarData(CatchRecord(strCalendarDataModel, objCalendarweek), 117, base.oTenant));

                //string[] aryDataFlag = flag.Split('|');
                //if (aryDataFlag[0].ToString() == "1")
                //{
                //    result = "1";
                //}
                //else if (aryDataFlag[0].ToString() == "2")
                //{
                //    result = "2";// + objCalendarDataModel.CalendarDate + ", Year " + objCalendarDataModel.CalendarDate + " and Week " + objCalendarDataModel.CalendarDate;
                //}
                //else if (aryDataFlag[0].ToString() == "3")
                //{
                //    result = aryDataFlag[1].ToString();
                //}
                response.Data = flag;
            }
            catch (Exception ex)
            {
                response.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return response;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("UpdateCalenderData")]
        public async Task<MessageResponse<string>> UpdateCalenderData([FromBody] CalendarDataModel strCalendarDataModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                CalendarDataModel objCalendarDataModel = strCalendarDataModel;
                List<Calendarweek> objCalendarweek = strCalendarDataModel.WeekList;
                string sReturnVal = await Task.Run(() => _repositoryCalender.ManageCalendarData(CatchRecordUpdate(objCalendarDataModel, objCalendarweek), 117, base.oTenant));

                string[] aryReturnVal = sReturnVal.Split('|');
                if (aryReturnVal[0].ToString() == "1")
                {
                    result.Data = "msgCalendarDataInformation";
                }
                else if (aryReturnVal[0].ToString() == "3")
                {
                    result.Data = "msgDateRangeOverLap" + aryReturnVal[1].ToString();
                }
                else if (aryReturnVal[0].ToString() == "2")
                {
                    result.Data = "msgCalendarAlreadyDefined" + aryReturnVal[1].ToString();
                }
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpDelete]
        [Route("DeleteCalenderData")]
        public async Task<MessageResponse<string>> DeleteCalenderData([FromQuery] int calenderId)
        {
            var result = new MessageResponse<string>();
            try
            {
                BECalendarInfo oBECalInfo = new();
                oBECalInfo.iCalendarID = Convert.ToInt16(calenderId);
                oBECalInfo.iModifiedBy = base.oUser.iUserID;
                await Task.Run(() => _repositoryCalender.DeleteData(oBECalInfo, 117, base.oTenant));
                result.Data = "OK";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetMaxWeek")]
        public async Task<MessageResponse<int>> GetMaxWeek([FromQuery]int calanderId, [FromQuery] int Year)
        {
            var result = new MessageResponse<int>();
            try
            {
                BECalendarInfo oCalendar = new();
                oCalendar.iCalendarID = calanderId;
                oCalendar.iYear=Year;
                result.Data = await Task.Run(() => _repositoryCalender.GetMaxWeek(oCalendar, base.oTenant));
               
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [NonAction]
        private CalendarDataModel DisplayRecord(IList<BECalendarInfo> lCalendar)
        {
            CalendarDataModel objCalendarDataModel = new();
            List<Calendarweek> WeekList = new();

            objCalendarDataModel.CalID = lCalendar[0].iCalendarDataId;
            objCalendarDataModel.CalendarID = lCalendar[0].iCalendarID;
            objCalendarDataModel.StartDateofMonth = lCalendar[0].dtStartDate.ToString();

            DateTime date = (lCalendar[0].dtStartDate);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var WeekstartDate = new DateTime(date.Year, date.Month, 1);

            objCalendarDataModel.StartDateofMonth = firstDayOfMonth.ToString();
            objCalendarDataModel.EndDateofMonth = lastDayOfMonth.ToString();
            objCalendarDataModel.Year = lCalendar[0].iYear;
            objCalendarDataModel.Month = lCalendar[0].iMonth;
            objCalendarDataModel.Disabled = lCalendar[0].bDisabled;
            objCalendarDataModel.WeekStartDay = lCalendar[0].iWeek.ToString();


            for (int count = 0; count < lCalendar.Count; count++)
            {
                Calendarweek onkCalendarweek = new();
                onkCalendarweek.miWeek = lCalendar[count].iWeek; ;
                //.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                onkCalendarweek.DisplayStartDate = DateTime.Parse(lCalendar[count].dtStartDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern));
                onkCalendarweek.DisplayEndDate = DateTime.Parse(lCalendar[count].dtEndDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern));
                onkCalendarweek.msRowState = "update";
                onkCalendarweek.miCalendarId = lCalendar[count].iCalendarDataId; ;
                WeekList.Add(onkCalendarweek);
                onkCalendarweek = null;
            }
            objCalendarDataModel.WeekList = WeekList;
            return objCalendarDataModel;
        }

        [NonAction]
        private BECalendarInfo CatchRecord(CalendarDataDetails objCalendarDataModel, List<Calendarweek> objCalendarweek)
        {

            DateTime dt = Convert.ToDateTime("1" + objCalendarDataModel.MonthYear);
            BECalendarInfo objCalendarInfo = new BECalendarInfo();
            objCalendarInfo.iCalendarID = Convert.ToInt32(objCalendarDataModel.CalendarID);
            objCalendarInfo.iMonth = dt.Month;
            objCalendarInfo.iYear = dt.Year;

            DateTimeFormatInfo DateInfo = CultureInfo.CurrentCulture.DateTimeFormat;
            DateTime DT = Convert.ToDateTime(String.Format("{0:" + DateInfo.ShortDatePattern + "}", objCalendarDataModel.StartDateofMonth.Trim()), CultureInfo.CurrentCulture);

            objCalendarInfo.dtStartDate = Convert.ToDateTime(DT.ToShortDateString());
            objCalendarInfo.bDisabled = Convert.ToBoolean(objCalendarDataModel.Disabled);

            objCalendarInfo.iCreatedBy = base.oUser.iUserID;
            objCalendarInfo.iModifiedBy = base.oUser.iUserID;
            DataTable dtCalendar = CreateTable();
            dtCalendar.Rows.Clear();

            for (int i = 0; i < objCalendarweek.Count; i++)
            {
                string txtWeek = objCalendarweek[i].miWeek.ToString();
                string txtStartDate = objCalendarweek[i].StrDisplayStartDate.ToString();
                string txtEndDate = objCalendarweek[i].StrDisplayEndDate.ToString();
                DateTime dtStartDate = Convert.ToDateTime(String.Format("{0:" + DateInfo.ShortDatePattern + "}", txtStartDate.ToString().Trim()), CultureInfo.CurrentCulture);
                DateTime dtEndDate = Convert.ToDateTime(String.Format("{0:" + DateInfo.ShortDatePattern + "}", txtEndDate.ToString().Trim()), CultureInfo.CurrentCulture);
                DataRow dr = dtCalendar.NewRow();
                if (objCalendarDataModel.CalID == 0)
                {
                    dr["iWeek"] = int.Parse(txtWeek);
                    dr["sRowState"] = "insert";
                    dr["iCalendarId"] = 0;
                    dr["dtStartDate"] = dtStartDate;
                    dr["dtEndDate"] = dtEndDate;
                }
                dtCalendar.Rows.Add(dr);
            }
            objCalendarInfo.dtCalanderData = dtCalendar;
            return objCalendarInfo;
        }

        [NonAction]
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

        [NonAction]
        private BECalendarInfo CatchRecordUpdate(CalendarDataModel objCalendarDataModel, List<Calendarweek> objCalendarweek)
        {

            DateTime dt = Convert.ToDateTime("1" + objCalendarDataModel.MonthYear);
            BECalendarInfo objCalendarInfo = new BECalendarInfo();
            objCalendarInfo.iCalendarID = Convert.ToInt32(objCalendarDataModel.CalendarID); // Calendar ID 1
            objCalendarInfo.iMonth = dt.Month; // Month 2
            objCalendarInfo.iYear = dt.Year; // Year 3
            DateTimeFormatInfo DateInfo = CultureInfo.CurrentCulture.DateTimeFormat;
            DateTime DT = Convert.ToDateTime(String.Format("{0:" + DateInfo.ShortDatePattern + "}", objCalendarDataModel.StartDateofMonth.Trim()), CultureInfo.CurrentCulture);
            objCalendarInfo.dtStartDate = Convert.ToDateTime(DT.ToShortDateString()); // Start Date 5
            objCalendarInfo.bDisabled = Convert.ToBoolean(objCalendarDataModel.Disabled); // bDisabled 7
            objCalendarInfo.iCreatedBy = base.oUser.iUserID;
            objCalendarInfo.iModifiedBy = base.oUser.iUserID;
            DataTable dtCalendar = CreateTable();
            dtCalendar.Rows.Clear();
            for (int i = 0; i < objCalendarweek.Count; i++)
            {
                string txtWeek = objCalendarweek[i].miWeek.ToString();
                string txtStartDate = objCalendarweek[i].DisplayStartDate.ToString();
                string txtEndDate = objCalendarweek[i].DisplayEndDate.ToString();

                DateTime dtStartDate = Convert.ToDateTime(String.Format("{0:" + DateInfo.ShortDatePattern + "}", txtStartDate.ToString().Trim()), CultureInfo.CurrentCulture);
                DateTime dtEndDate = Convert.ToDateTime(String.Format("{0:" + DateInfo.ShortDatePattern + "}", txtEndDate.ToString().Trim()), CultureInfo.CurrentCulture);
                DataRow dr = dtCalendar.NewRow();
                if (objCalendarDataModel.CalID == 0)
                {
                    dr["iWeek"] = int.Parse(txtWeek);
                    dr["sRowState"] = "insert";
                    dr["iCalendarId"] = 0;
                    dr["dtStartDate"] = dtStartDate;
                    dr["dtEndDate"] = dtEndDate;
                }
                dtCalendar.Rows.Add(dr);

            }

            objCalendarInfo.dtCalanderData = dtCalendar;
            return objCalendarInfo;
        }
    }
}
