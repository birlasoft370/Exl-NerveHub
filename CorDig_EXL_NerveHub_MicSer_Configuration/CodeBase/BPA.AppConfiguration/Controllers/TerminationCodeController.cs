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
    public class TerminationCodeController : BaseController<TerminationCodeController>
    {
        private readonly ITerminationCodeService _repositoryTerminationCode;
        public TerminationCodeController(ILogger<TerminationCodeController> logger, IWebHostEnvironment env, IConfiguration configuration, 
            ITerminationCodeService repositoryTerminationCode) : base(logger, env, configuration)
        {
            _repositoryTerminationCode = repositoryTerminationCode;
        }


        [ProducesResponseType(typeof(List<TerminationCodeModel>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetTerminationCodeList")]
        public async Task<MessageResponse<List<TerminationCodeModel>>> GetTerminationCodeList([FromQuery] string? searchTerminationCode)
        {
            var result = new MessageResponse<List<TerminationCodeModel>>();
            List<TerminationCodeModel> lstTerminationCodeview = new List<TerminationCodeModel>();

            try
            {
                searchTerminationCode = string.IsNullOrEmpty(searchTerminationCode) ? "" : searchTerminationCode;
                var dbresult = await Task.Run(() => _repositoryTerminationCode.GetTermCodeList(Encoder.HtmlEncode(searchTerminationCode, false), false, base.oTenant));

                lstTerminationCodeview = dbresult.Select(x => new TerminationCodeModel
                {
                    TerminationCodeID = x.iTerminationCodeID,
                    TerminationCodeName = x.sTermCodeName,
                    Description = x.sTermCodeDesc,
                    Disabled = x.bDisabled,
                    UserId = x.iCreatedBy
                }).ToList();
                result.Data = lstTerminationCodeview;
                result.TotalCount = lstTerminationCodeview.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(TerminationCodeModel), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetTerminationCodeById")]
        public async Task<MessageResponse<TerminationCodeModel>> GetTerminationCodeById([FromQuery] int terminationCodeID)
        {
            var result = new MessageResponse<TerminationCodeModel>();
            TerminationCodeModel objTerminationCode = new();
            try
            {
                BETerminationCodeInfo BETermCodeInfo = await Task.Run(() => _repositoryTerminationCode.GetTermCodeList(terminationCodeID, oTenant)[0]);

                objTerminationCode.TerminationCodeID = BETermCodeInfo.iTerminationCodeID;
                objTerminationCode.TerminationCodeName = Encoder.HtmlEncode(BETermCodeInfo.sTermCodeName.ToString(), false);
                objTerminationCode.Description = Encoder.HtmlEncode(BETermCodeInfo.sTermCodeDesc.ToString(), false);
                objTerminationCode.Disabled = BETermCodeInfo.bDisabled;
                objTerminationCode.UserId = BETermCodeInfo.iCreatedBy;

                result.Data = objTerminationCode;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("AddTerminationCode")]
        public async Task<MessageResponse<string>> AddTerminationCode([FromBody] TerminationCodeModel TerminationCodeModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryTerminationCode.InsertData(CatchRecord(TerminationCodeModel), 16, base.oTenant));
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
        [Route("UpdateTerminationCode")]
        public async Task<MessageResponse<string>> UpdateTerminationCode([FromBody] TerminationCodeModel TerminationCodeModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositoryTerminationCode.UpdateData(CatchRecord(TerminationCodeModel), 16, base.oTenant));
                result.Data = string.Empty;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }



        [NonAction]
        private BETerminationCodeInfo CatchRecord(TerminationCodeModel objTerminationCode)
        {
            using (BETerminationCodeInfo objBETerminationCodeInfo = new BETerminationCodeInfo())
            {

                objBETerminationCodeInfo.iTerminationCodeID = objTerminationCode.TerminationCodeID;
                objBETerminationCodeInfo.sTermCodeName = Encoder.HtmlEncode(objTerminationCode.TerminationCodeName.ToString().TrimStart().TrimEnd().Trim(), false);
                objBETerminationCodeInfo.sTermCodeDesc = Encoder.HtmlEncode(objTerminationCode.Description, false);
                objBETerminationCodeInfo.bDisabled = objTerminationCode.Disabled;
                objBETerminationCodeInfo.iCreatedBy = objTerminationCode.UserId;
                objBETerminationCodeInfo.iModifiedBy = objTerminationCode.UserId;
                return objBETerminationCodeInfo;
            }
        }
    }
}
