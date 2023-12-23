using BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.WorkAllocation;
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
    public class BreakMasterController : BaseController<BreakMasterController>
    {
        private readonly IBreakService _repositoryBreak;

        public BreakMasterController(ILogger<BreakMasterController> logger, IWebHostEnvironment env, IConfiguration configuration, 
            IBreakService repositoryBreak) : base(logger, env, configuration)
        {
            _repositoryBreak = repositoryBreak;
        }


        [ProducesResponseType(typeof(List<BreakMasterModel>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetBreakInfoList")]
        public async Task<MessageResponse<List<BreakMasterModel>>> GetBreakInfoList([FromQuery] string? breakName)
        {
            var result = new MessageResponse<List<BreakMasterModel>>();
            List<BreakMasterModel> lstBreakMasterview = new();

            try
            {
                var dbresult = await Task.Run(() => breakName == null ? _repositoryBreak.GetBreakList(false, base.oTenant) :
                                                       _repositoryBreak.GetBreakList(Encoder.HtmlEncode(breakName, false), true, base.oTenant));

                lstBreakMasterview = dbresult.Select(x => new BreakMasterModel
                {
                    BreakID = x.iBreakID,
                    BreakName = x.sBreakName,
                    Description = x.sBreakDescription,
                    Disabled = x.bDisabled,
                    UserId = x.iCreatedBy
                }).ToList();
                result.Data = lstBreakMasterview;
                result.TotalCount = lstBreakMasterview.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(BreakMasterModel), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetBreakInfoById")]
        public async Task<MessageResponse<BreakMasterModel>> GetBreakMasterById([FromQuery] int breakID)
        {
            var result = new MessageResponse<BreakMasterModel>();
            BreakMasterModel objBreakMaster = new BreakMasterModel();
            try
            {
                BEBreakInfo BEBreakInfoList = await Task.Run(() => _repositoryBreak.GetBreakList(breakID, base.oTenant)[0]);
                objBreakMaster.BreakID = BEBreakInfoList.iBreakID;
                objBreakMaster.BreakName = BEBreakInfoList.sBreakName;
                objBreakMaster.Description = BEBreakInfoList.sBreakDescription;
                objBreakMaster.Disabled = BEBreakInfoList.bDisabled;
                result.Data = objBreakMaster;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("AddBreakInfo")]
        public async Task<MessageResponse<string>> AddBreakMaster([FromBody] BreakMasterModel BreakMasterModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryBreak.InsertData(CatchRecord(BreakMasterModel), 3, base.oTenant));
                result.Data = "display_Save_Message";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateBreakInfo")]
        public async Task<MessageResponse<string>> UpdateBreakMaster([FromBody] BreakMasterModel BreakMasterModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryBreak.UpdateData(CatchRecord(BreakMasterModel), 3, base.oTenant));
                result.Data = "display_Update_Message";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [NonAction]
        private BEBreakInfo CatchRecord(BreakMasterModel objBreakMaster)
        {

            using (BEBreakInfo objBEBreakInfo = new BEBreakInfo())
            {
                objBEBreakInfo.iBreakID = objBreakMaster.BreakID;
                objBEBreakInfo.sBreakName = Encoder.HtmlEncode(objBreakMaster.BreakName.ToString(), false);
                objBEBreakInfo.sBreakDescription = Encoder.HtmlEncode(objBreakMaster.Description, false);
                objBEBreakInfo.bDisabled = objBreakMaster.Disabled;
                objBEBreakInfo.iCreatedBy = objBreakMaster.UserId;
                objBEBreakInfo.iModifiedBy = objBreakMaster.UserId;
                return objBEBreakInfo;
            }
        }
    }
}
