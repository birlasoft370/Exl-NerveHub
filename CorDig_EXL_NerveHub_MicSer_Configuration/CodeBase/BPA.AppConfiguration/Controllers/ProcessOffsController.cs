using BPA.AppConfig.BusinessEntity.ExternalRef.BIReports;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.BIReports;
using BPA.AppConfiguration.BaseController;
using BPA.AppConfiguration.Helper;
using BPA.AppConfiguration.Helper.Filter;
using BPA.AppConfiguration.Models;
using BPA.AppConfiguration.Models.Request;
using BPA.AppConfiguration.Models.Response;
using BPA.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using System.Reflection;

namespace BPA.AppConfiguration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthentication]
    public class ProcessOffsController : BaseController<ProcessOffsController>
    {
        private readonly IProcessOffService _repositoryProcessOff;

        public ProcessOffsController(ILogger<ProcessOffsController> logger, IWebHostEnvironment env, IConfiguration configuration,
            IProcessOffService repositoryProcessOff) : base(logger, env, configuration)
        {
            _repositoryProcessOff = repositoryProcessOff;
        }


        [ProducesResponseType(typeof(List<BEProcessOff>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetProcessOffList")]
        public async Task<MessageResponse<IList<BEProcessOff>>> GetProcessOffList([FromQuery] int processId, [FromQuery] int month, [FromQuery] int year)
        {
            var result = new MessageResponse<IList<BEProcessOff>>();
            IList<BEProcessOff>? objProcessOffList = null;
            try
            {
                objProcessOffList = await Task.Run(() => _repositoryProcessOff.GetProcessOffList(processId, year, month, false, base.oTenant));
                result.Data = objProcessOffList;
                result.TotalCount = objProcessOffList.Count;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            //finally
            //{
            //    result = null;
            //}
            return result;
        }
        [ProducesResponseType(typeof(List<BEProcessOff>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetFirstLastDayOfCalender")]
        public async Task<MessageResponse<string>> GetFirstLastDayOfCalender([FromQuery] int processId, [FromQuery] int month, [FromQuery] int year)
        {
            var result = new MessageResponse<string>();
           
            try
            {
               string strFirstLastDay = await Task.Run(() => _repositoryProcessOff.GetFirstLastDayOfCalender(Convert.ToInt16(processId), Convert.ToInt16(month), Convert.ToInt16(year), base.oTenant));

                result.Data = strFirstLastDay;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            //finally
            //{
            //    result = null;
            //}
            return result;
        }


        [ProducesResponseType(typeof(ProcessOffsModel), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetProcessOffsById")]
        [Route("Generate")]
        public async Task<MessageResponse<ProcessOffsModel>> GetProcessOffsById([FromQuery] int processId, [FromQuery] int month, [FromQuery] int year)
        {
            var result = new MessageResponse<ProcessOffsModel>();
            ProcessOffsModel ObjProcessOff = new();
            try
            {
                ObjProcessOff = await Task.Run(() => DisplayRecord(_repositoryProcessOff.GetProcessOffListProcessDayWise(processId, month, year, base.oTenant)[0], processId.ToString(), month, year));
                result.Data = ObjProcessOff;
            }
            catch (Exception ex)
            {
                //  result.Message = new LoggerInfo() { actionName = "ActionName", message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = ""correlationId };
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("AddProcessOffs")]
        public async Task<MessageResponse<string>> AddProcessOffs([FromBody] ProcessOffModel objProcessOff)
        {
            var result = new MessageResponse<string>();
            try
            {
                BEProcessOff oProcess = CatchRecord(objProcessOff);
            await Task.Run(() => _repositoryProcessOff.InsertData(oProcess, 124, base.oTenant));
                result.Data = "displaySaved";
            }
            catch (Exception ex)
            {
                var CorrId = CorrelationId;
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateProcessOffs")]
        public async Task<MessageResponse<string>> UpdateProcessOffs([FromBody] ProcessOffModel objProcessOff)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryProcessOff.UpdateData(CatchRecordForUpdate(objProcessOff), 124, base.oTenant));
                result.Data = "displaySaved";// "Process Off data updated successfully !.";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [NonAction]
        private ProcessOffsModel DisplayRecord(BEProcessOff oProcessOff, string ProcessId, int monthofYear, int Year)
        {

            ProcessOffsModel objProcessOffModel = new();

            objProcessOffModel.BEProcessOffList = _repositoryProcessOff.GetProcessOffListProcessDayWise(Convert.ToInt32(ProcessId), Convert.ToInt16(monthofYear), Convert.ToInt16(Year), base.oTenant);

            objProcessOffModel.ProcessOffId = int.Parse(ProcessId);
            objProcessOffModel.ClientID = oProcessOff.iClientId;
            objProcessOffModel.ProcessID = oProcessOff.iProcessId;

            objProcessOffModel.Description = oProcessOff.sDescription;

            objProcessOffModel.Year = oProcessOff.iYear;
            objProcessOffModel.Month = oProcessOff.iMonth;
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(oProcessOff.iMonth);
            objProcessOffModel.MonthYear = monthName.Substring(0, 3) + " " + oProcessOff.iYear;// Mar 2022
            objProcessOffModel.DaysList = GenerateGrid(objProcessOffModel);

            return objProcessOffModel;

        }


        [NonAction]
        private List<GridList> GenerateGrid(ProcessOffsModel objProcessOffsModel)
        {
            string strFirstLastDay = "1/1/1900|1/1/1900";
            DateTime dtFirstDay;
            DateTime dtLastDay;
            string[] arrMonthYear = objProcessOffsModel.MonthYear.Split(' ');
            DateTime dt = Convert.ToDateTime("1 " + objProcessOffsModel.MonthYear);
            strFirstLastDay = _repositoryProcessOff.GetFirstLastDayOfCalender(Convert.ToInt16(objProcessOffsModel.ProcessID), Convert.ToInt16(dt.Month), Convert.ToInt16(dt.Year), base.oTenant);
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
                return new List<GridList>();

            }
            return Dt.ConvertToList<GridList>();

        }

        [NonAction]
        private BEProcessOff CatchRecord(ProcessOffModel objProcessOffsModel)
        {

            DataTable dtProcessoff = new DataTable();
            dtProcessoff.Columns.Add("iWeek", System.Type.GetType("System.Int32"));
            dtProcessoff.Columns.Add("WeekDays", System.Type.GetType("System.String"));
            dtProcessoff.Columns.Add("DISABLED", System.Type.GetType("System.Boolean"));
            BEProcessOff objProcessOff = new BEProcessOff();
            objProcessOff.iProcessId = Convert.ToInt32(objProcessOffsModel.ProcessID);
            objProcessOff.sDescription = objProcessOffsModel.Description;
            objProcessOff.iCreatedBy = base.oUser.iUserID;
            objProcessOff.iModifiedBy = base.oUser.iUserID;

            for (int WeekDaysCount = 0; WeekDaysCount < objProcessOffsModel.DaysNameList.Length; WeekDaysCount++)
            {
                DataRow dr = dtProcessoff.NewRow();
                dr["WeekDays"] = objProcessOffsModel.DaysNameList[WeekDaysCount].ToString().Split(';')[0]; ;
                dr["DISABLED"] = 0;
                dtProcessoff.Rows.Add(dr);
            }
            objProcessOff.dtWeekInfo = dtProcessoff;
            return objProcessOff;
        }

        [NonAction]
        private BEProcessOff CatchRecordForUpdate(ProcessOffModel ObjProcessOff)
        {
            DataTable dtProcessoff = CreateTable();
            BEProcessOff objBEProcessOff = new BEProcessOff();
            objBEProcessOff.iProcessId = ObjProcessOff.ProcessOffId;
            objBEProcessOff.sDescription = ObjProcessOff.Description;
            objBEProcessOff.iCreatedBy = oUser.iUserID;
            objBEProcessOff.iModifiedBy = oUser.iUserID;

            for (int count = 0; count < ObjProcessOff.DaysNameList.Length; count++)
            {

                DataRow dr = dtProcessoff.NewRow();
                dr["WeekDays"] = ObjProcessOff.DaysNameList[count].Split(';')[0].Trim();
                string[] sProcessoff = ObjProcessOff.DaysNameList[count].Split(';')[0].Split('/');
                dr["DISABLED"] = 0;
                dr["Days"] = sProcessoff[1].ToString();
                dr["Months"] = sProcessoff[0].ToString();
                dr["Years"] = sProcessoff[2].ToString();
                dtProcessoff.Rows.Add(dr);
            }
            objBEProcessOff.dtWeekInfo = dtProcessoff;
            return objBEProcessOff;

        }

        [NonAction]
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
    }

    [Route("api/[controller]")]
    [ApiController]
    public class CircuitBreakerTest : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CircuitBreakerTest(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var client = _httpClientFactory.CreateClient("loggerApiClient");
            return await client.GetStringAsync("WeatherForecast");
        }
    }
}
