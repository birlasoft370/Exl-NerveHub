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
    public class TimeZoneController : BaseController<TimeZoneController>
    {
        private readonly ITimeZoneService _repositoryTimeZone;

        public TimeZoneController(ILogger<TimeZoneController> logger, IWebHostEnvironment env, IConfiguration configuration,
            ITimeZoneService repositoryTimeZone) : base(logger, env, configuration)
        {
            _repositoryTimeZone = repositoryTimeZone;
        }


        [ProducesResponseType(typeof(List<BETimeZoneInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetTimeZoneList")]
        public async Task<MessageResponse<List<BETimeZoneInfo>>> GetTimeZoneList([FromQuery] string? searchTimeZone)
        {
            var result = new MessageResponse<List<BETimeZoneInfo>>();
            List<BETimeZoneInfo> lstTimeZoneview = new List<BETimeZoneInfo>();

            try
            {
                var dbresult = await Task.Run(() => searchTimeZone == null ? _repositoryTimeZone.GetTimeZoneList(true, base.oTenant) :
                                                       _repositoryTimeZone.GetTimeZoneList(searchTimeZone.Trim(), true, base.oTenant));
                /*
                lstTimeZoneview = dbresult.Select(x => new TimeZoneModel
                {
                    TimeZoneID = x.iTimeZoneID,
                    TimeZoneName = x.sTimeZoneName,
                    Description = x.sTimeZoneDescription,
                    Disabled = x.bDisabled,
                    OffSetGMT = x.sOffsetGMT
                }).ToList();*/
                result.Data = dbresult;
                result.TotalCount = dbresult.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(TimeZoneModel), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetTimeZoneById")]
        public async Task<MessageResponse<TimeZoneModel>> GetTimeZoneById([FromQuery] int timeZoneID)
        {
            var result = new MessageResponse<TimeZoneModel>();
            try
            {
                var timeZone = await Task.Run(() => DisplayRecord(_repositoryTimeZone.GetTimeZoneList(timeZoneID, oTenant).FirstOrDefault()));
                result.Data = timeZone;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("AddTimeZone")]
        public async Task<MessageResponse<string>> AddTimeZone([FromBody] TimeZoneModel timeZoneModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryTimeZone.InsertData(CatchRecord(timeZoneModel), 17, base.oTenant));
                result.Data = "dispTimeZoneSavedSuccessfully";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateTimeZone")]
        public async Task<MessageResponse<string>> UpdateTimeZone([FromBody] TimeZoneModel TimeZoneModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryTimeZone.UpdateData(CatchRecord(TimeZoneModel), 17, base.oTenant));
                result.Data = "dispTimeZoneUpdatedSuccessfully";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [NonAction]
        private TimeZoneModel DisplayRecord(BETimeZoneInfo oTimeZone)
        {
            TimeZoneModel objTimeZoneModel = new TimeZoneModel();
            objTimeZoneModel.TimeZoneID = int.Parse(oTimeZone.iTimeZoneID.ToString());
            objTimeZoneModel.TimeZoneName = Encoder.HtmlEncode(oTimeZone.sTimeZoneName, false);
            objTimeZoneModel.Description = oTimeZone.sTimeZoneDescription;
            objTimeZoneModel.OffSetGMT = oTimeZone.sOffsetGMT;
            objTimeZoneModel.Disabled = oTimeZone.bDisabled;

            return objTimeZoneModel;
        }

        [NonAction]
        private BETimeZoneInfo CatchRecord(TimeZoneModel oModel)
        {
            BETimeZoneInfo oTimeZone = new();
            oTimeZone.iTimeZoneID = oModel.TimeZoneID;
            oTimeZone.sTimeZoneName = Encoder.HtmlEncode(oModel.TimeZoneName, false);
            oTimeZone.sTimeZoneDescription = Encoder.HtmlEncode(oModel.Description, false);
            oTimeZone.sOffsetGMT = Encoder.HtmlEncode(oModel.OffSetGMT.Split('/')[1], false);
            oTimeZone.bDisabled = oModel.Disabled;
            if (oModel.TimeZoneID == 0)
            {
                oTimeZone.iCreatedBy = base.oUser.iUserID;
            }
            else
            {
                oTimeZone.iModifiedBy = base.oUser.iUserID;
            }
            return oTimeZone;
        }

    }
}
