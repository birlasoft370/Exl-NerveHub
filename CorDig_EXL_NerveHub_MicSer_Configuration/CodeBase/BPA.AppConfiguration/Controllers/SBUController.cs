using BPA.AppConfig.BusinessEntity.ExternalRef.Configuration;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.Configuration;
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
    public class SBUController : BaseController<SBUController>
    {
        private readonly ISBUInfoService _repositorySBU;

        public SBUController(ILogger<SBUController> logger, IWebHostEnvironment env, IConfiguration configuration,
            ISBUInfoService repositorySBU) : base(logger, env, configuration)
        {
            _repositorySBU = repositorySBU;
        }

        [ProducesResponseType(typeof(IEnumerable<BESBUInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetSBUList")]
        public async Task<MessageResponse<IEnumerable<BESBUInfo>>> GetSBUList([FromQuery] string? sbuName, [FromQuery] bool isActive)
        {
            var result = new MessageResponse<IEnumerable<BESBUInfo>>();

            try
            {
                sbuName = sbuName ?? "";
                var sbuList = await Task.Run(() => _repositorySBU.GetSBUList(Encoder.HtmlEncode(sbuName), isActive, base.oTenant));

                result.Data = sbuList;
                result.TotalCount = sbuList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(SBUModel), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetSBUById")]
        public async Task<MessageResponse<SBUModel>> GetSBUById([FromQuery] int sbuID)
        {
            var result = new MessageResponse<SBUModel>();
            try
            {
                var sbu = await Task.Run(() => GetModel(_repositorySBU.GetSBUList(sbuID, oTenant).FirstOrDefault()));
                result.Data = sbu;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("AddSBU")]
        public async Task<MessageResponse<string>> AddSBU([FromBody] SBUModel sbuModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                string Output = await Task.Run(() => this._repositorySBU.InsertData(BindModel(sbuModel), 118, base.oTenant));

                if (Output == "" || Output == String.Empty)
                {
                    result.Data = "msgSave";
                }
                else
                {
                    result.Data = Output;
                }
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateSBU")]
        public async Task<MessageResponse<string>> UpdateSBU([FromBody] SBUModel sbuModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => this._repositorySBU.UpdateData(BindModel(sbuModel), 118, base.oTenant));

                result.Data = "msgUpdated";

            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [NonAction]
        private SBUModel GetModel(BESBUInfo info)
        {
            SBUModel SBUObj = new SBUModel();
            SBUObj.SBUID = info.iSBUID;
            SBUObj.ERPID = info.iERPID;
            SBUObj.SBUName = info.sName;
            SBUObj.IsClientSBU = info.bIsClientSBU;
            SBUObj.Disabled = info.bDisabled;
            SBUObj.Description = info.sDescription;
            return SBUObj;
        }

        [NonAction]
        private BESBUInfo BindModel(SBUModel sbuModel)
        {
            BESBUInfo objSBU = new BESBUInfo();
            objSBU.iERPID = sbuModel.ERPID;
            objSBU.iSBUID = sbuModel.SBUID;
            objSBU.sDescription = Encoder.HtmlEncode(sbuModel.Description, false);
            objSBU.sName = Encoder.HtmlEncode(sbuModel.SBUName, false);
            objSBU.iCreatedBy = sbuModel.UserId;
            objSBU.bIsClientSBU = sbuModel.IsClientSBU;
            objSBU.bDisabled = sbuModel.Disabled;
            objSBU.iModifiedBy = sbuModel.UserId;
            return objSBU;
        }
    }
}
