using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.Configuration;
using BPA.AppConfiguration.BaseController;
using BPA.AppConfiguration.Helper;
using BPA.AppConfiguration.Helper.Filter;
using BPA.AppConfiguration.Models;
using BPA.AppConfiguration.Models.Response;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace BPA.AppConfiguration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthentication]
    public class ProcessShiftController : BaseController<ProcessShiftController>
    {
        private readonly IProcessShiftService _repositoryProcessShift;
        private readonly IProcessService _repositoryProcess;
        private readonly IShiftService _repositoryShift;
        public ProcessShiftController(ILogger<ProcessShiftController> logger, IWebHostEnvironment env, IConfiguration configuration,
            IProcessShiftService repositoryProcessShift, IProcessService repositoryProcess, IShiftService repositoryShift) : base(logger, env, configuration)
        {
            _repositoryProcessShift = repositoryProcessShift;
            _repositoryProcess = repositoryProcess;
            _repositoryShift = repositoryShift;
        }

        [HttpGet]
        [Route("GetProcessShift")]
        public ActionResult<MessageResponse<IList<BEProcessShiftInfo>>> GetProcessShift(int iProcessId)
        {
            var result = new MessageResponse<IList<BEProcessShiftInfo>>();
            try
            {
                var value = _repositoryProcessShift.GetProcessShift(iProcessId, oTenant);
                result.Data = value;
            }
            catch (Exception ex)
            {
                result.Message =  $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}";
            }
            return result;
        }
        [HttpGet]
        [Route("GetProcessList")]
        public ActionResult<MessageResponse<IList<BEProcessInfo>>> GetProcessList(int iProcessId)
        {
            var result = new MessageResponse<IList<BEProcessInfo>>();
            try
            {
                var value = _repositoryProcess.GetProcessList(iProcessId, oTenant);
                result.Data = value;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpPut]

        [Route("UpdateData")]
        public ActionResult<MessageResponse<string>> UpdateData([FromBody] BEProcessShiftInfo oProcess, int FormID)
        {
            var result = new MessageResponse<string>();
            try
            {

                _repositoryProcessShift.UpdateData(oProcess, FormID, oTenant);
                result.Data = "Ok";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
       
        [HttpPost]
        [Route("InsertData")]
        public ActionResult<MessageResponse<string>> InsertData([FromBody] BEProcessShiftInfo oShift, int FormID)
        {
            var result = new MessageResponse<string>();
            try
            {
                _repositoryProcessShift.InsertData(oShift, FormID, oTenant);
                result.Data = "Ok";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpGet]
        [Route("GetShiftList")]
        public ActionResult<MessageResponse<IList<BEShiftInfo>>> GetShiftList(bool bGetAll)
        {
            var result = new MessageResponse<IList<BEShiftInfo>>();
            try
            {
                var value = _repositoryShift.GetShiftList(bGetAll, oTenant);
                result.Data = value;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
    }
}
