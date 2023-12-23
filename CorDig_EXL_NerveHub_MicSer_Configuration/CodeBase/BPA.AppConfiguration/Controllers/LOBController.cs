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
    public class LOBController : BaseController<LOBController>
    {
        private readonly ILOBService _repositoryLOB;

        public LOBController(ILogger<LOBController> logger, IWebHostEnvironment env, IConfiguration configuration,
            ILOBService repositoryLOB) : base(logger, env, configuration)
        {
            _repositoryLOB = repositoryLOB;
        }

        [ProducesResponseType(typeof(List<LOBModel>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetLOBList")]
        public async Task<MessageResponse<List<BELOBInfo>>> GetLOBList([FromQuery] string? LOBName, [FromQuery] bool activeLOB)
        {
            var result = new MessageResponse<List<BELOBInfo>>();
            List<LOBModel> oLOBModel = new List<LOBModel>();

            try
            {
                var LOBList = await Task.Run(() =>
                _repositoryLOB.GetLOBList(Encoder.HtmlEncode(LOBName), activeLOB, base.oTenant));
                result.Data = LOBList;
                result.TotalCount = oLOBModel.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(LOBModel), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetLOBById")]
        public async Task<MessageResponse<LOBModel>> GetLOBById([FromQuery] int LOBID)
        {
            var result = new MessageResponse<LOBModel>();
            try
            {
                var LOB = await Task.Run(() => GetModel(_repositoryLOB.GetLOBList(LOBID, oTenant).FirstOrDefault()));
                result.Data = LOB;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("AddLOB")]
        public async Task<MessageResponse<string>> AddLOB([FromBody] LOBModel LOBModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => this._repositoryLOB.InsertData(BindModel(LOBModel), 118, base.oTenant));

                result.Data = "display_Save";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateLOB")]
        public async Task<MessageResponse<string>> UpdateLOB([FromBody] LOBModel LOBModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => this._repositoryLOB.UpdateData(BindModel(LOBModel), 118, base.oTenant));

                result.Data = "display_Update";

            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [NonAction]
        private LOBModel GetModel(BELOBInfo info)
        {
            LOBModel ObjLOB = new LOBModel();
            ObjLOB.LOBID = info.iLOBID;
            ObjLOB.ERPID = info.iERPID;
            ObjLOB.LOBName = info.sLOBName;
            ObjLOB.Disabled = info.bDisabled;
            ObjLOB.Description = info.sDescription;
            ObjLOB.UserId = info.iCreatedBy;
            return ObjLOB;
        }

        [NonAction]
        private BELOBInfo BindModel(LOBModel LOBModel)
        {
            BELOBInfo objLOB = new BELOBInfo();
            objLOB.iERPID = LOBModel.ERPID;
            objLOB.iLOBID = LOBModel.LOBID;
            objLOB.sDescription = Encoder.HtmlEncode(LOBModel.Description, false);
            objLOB.sLOBName = Encoder.HtmlEncode(LOBModel.LOBName, false);
            objLOB.iCreatedBy = LOBModel.UserId;
            objLOB.bDisabled = LOBModel.Disabled;
            objLOB.iModifiedBy = LOBModel.UserId;
            return objLOB;
        }
    }
}
