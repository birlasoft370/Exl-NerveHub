using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;
using BPA.AppConfiguration.BaseController;
using BPA.AppConfiguration.Helper;
using BPA.AppConfiguration.Helper.Filter;
using BPA.AppConfiguration.Models;
using BPA.AppConfiguration.Models.Request;
using BPA.AppConfiguration.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Security.Application;
using System.Reflection;

namespace BPA.AppConfiguration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthentication]
    public class CalendarMasterController : BaseController<CalendarMasterController>
    {
        private readonly ICalenderService _repositoryCalender;

        public CalendarMasterController(ILogger<CalendarMasterController> logger, IWebHostEnvironment env, IConfiguration configuration,
            ICalenderService repositoryCalender) : base(logger, env, configuration)
        {
            _repositoryCalender = repositoryCalender;
        }

        [ProducesResponseType(typeof(List<CalendarMasterModel>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetCalendarList")]
        public async Task<MessageResponse<List<CalendarMasterModel>>> GetCalendarList([FromQuery] string? calendarSearchName, [FromQuery] bool IsActive)
        {
            var result = new MessageResponse<List<CalendarMasterModel>>();
            List<CalendarMasterModel> objCalendarList = new List<CalendarMasterModel>();
            IList<BECalendarInfo> CalendarList = new List<BECalendarInfo>();
            try
            {
                await Task.Run(() =>
                {
                    if (string.IsNullOrEmpty(calendarSearchName))
                    {
                        CalendarList = _repositoryCalender.GetCalendarList(IsActive, base.oTenant);
                    }
                    else
                    {
                        CalendarList = _repositoryCalender.GetCalendarList(calendarSearchName.ToString().Trim(), IsActive, base.oTenant);
                    }
                });
                foreach (var item in CalendarList)
                {
                    objCalendarList.Add(new CalendarMasterModel
                    {
                        CalenderID = item.iCalendarID,
                        CalenderName = item.sCalendarName,
                        Description = item.sDescription,
                        Disabled = item.bDisabled,
                        UserId = item.iCreatedBy
                    });
                }
                result.Data = objCalendarList;
                result.TotalCount = objCalendarList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(CalendarMasterModel), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetCalendarById")]
        public async Task<MessageResponse<CalendarMasterModel>> GetCalendarById([FromQuery] int calendarID)
        {
            var result = new MessageResponse<CalendarMasterModel>();
            CalendarMasterModel ObjCalendar = new CalendarMasterModel();
            try
            {
                var ObjListBE = await Task.Run(() => GetModel(_repositoryCalender.GetCalendarList(calendarID, base.oTenant)[0]));

                result.Data = ObjListBE;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("AddCalendar")]
        public async Task<MessageResponse<string>> AddCalendar([FromBody] CalendarMasterModel CalendarMasterModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => this._repositoryCalender.InsertData(CatchRecord(CalendarMasterModel), 116, base.oTenant));

                result.Data = string.Empty;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateCalendar")]
        public async Task<MessageResponse<string>> UpdateCalendar([FromBody] CalendarMasterModel CalendarMasterModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => this._repositoryCalender.UpdateData(CatchRecord(CalendarMasterModel), 116, base.oTenant));

                result.Data = string.Empty;

            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [NonAction]
        public CalendarMasterModel GetModel(BECalendarInfo info)
        {
            CalendarMasterModel CALObj = new CalendarMasterModel();
            CALObj.CalenderID = info.iCalendarID;
            CALObj.CalenderName = info.sCalendarName.Trim();
            CALObj.Description = info.sDescription.Trim();
            CALObj.Disabled = info.bDisabled;
            return CALObj;
        }

        [NonAction]
        private BECalendarInfo CatchRecord(CalendarMasterModel objCalendar)
        {
            BECalendarInfo objCalendarInfo = new BECalendarInfo();
            objCalendarInfo.iCalendarID = Convert.ToInt32(objCalendar.CalenderID);
            objCalendarInfo.sCalendarName = Encoder.HtmlEncode(objCalendar.CalenderName.Trim().ToString(), false);
            objCalendarInfo.sDescription = Encoder.HtmlEncode(objCalendar.Description, false);
            objCalendarInfo.bDisabled = objCalendar.Disabled;
            objCalendarInfo.iCreatedBy = objCalendar.UserId;
            objCalendarInfo.iModifiedBy = objCalendar.UserId;
            return objCalendarInfo;

        }
    }
}
