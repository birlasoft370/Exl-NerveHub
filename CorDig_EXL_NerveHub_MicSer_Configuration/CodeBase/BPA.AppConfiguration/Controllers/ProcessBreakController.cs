using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.WorkAllocation;
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
    public class ProcessBreakController : BaseController<ProcessBreakController>
    {
        private readonly IProcessBreakMappingService _repositoryProcessBreak;
        private readonly IBreakService _repositoryBreak;

        public ProcessBreakController(ILogger<ProcessBreakController> logger, IWebHostEnvironment env, IConfiguration configuration,
            IProcessBreakMappingService repositoryProcessBreak, IBreakService repositoryBreak) : base(logger, env, configuration)
        {
            _repositoryProcessBreak = repositoryProcessBreak;
            _repositoryBreak = repositoryBreak;
        }

        [HttpGet]
        [Route("GetProcessBreakMappedDetails")]
        public ActionResult<MessageResponse<BEProcessBreakMapping>> GetProcessBreakMappedDetails(int iProcessId)
        {
            var result = new MessageResponse<BEProcessBreakMapping>();
            try
            {
                var value = _repositoryProcessBreak.GetProcessBreakMappedDetails(iProcessId, oTenant);
                result.Data = value;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpGet]

        [Route("GetBreakList")]
        public ActionResult<MessageResponse<IList<BEBreakInfo>>> GetBreakList(bool status)
        {
            var result = new MessageResponse<IList<BEBreakInfo>>();
            try
            {
                var value = _repositoryBreak.GetBreakList(status, oTenant);
                result.Data = value;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpPost]

        [Route("InsertData")]
        public ActionResult<MessageResponse<string>> InsertData([FromBody] BEProcessBreakMapping oProcess, int FormID)
        {
            var result = new MessageResponse<string>();
            try
            {
                _repositoryProcessBreak.InsertData(oProcess, FormID, oTenant);
                result.Data = "Ok";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpPut]

        [Route("UpdateData")]
        public ActionResult<MessageResponse<string>> UpdateData([FromBody] BEProcessBreakMapping oProcess, int FormID)
        {
            var result = new MessageResponse<string>();
            try
            {
                _repositoryProcessBreak.UpdateData(oProcess, FormID, oTenant);
                result.Data = "Ok";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [HttpGet]
        [Route("GetProcessBreakMappedList")]
        public ActionResult<MessageResponse<List<BEProcessInfo>>> GetProcessBreakMappedList(int ClientID, string ProcessName, int UserId)
        {
            var result = new MessageResponse<List<BEProcessInfo>>();
            try
            {

                var value = _repositoryProcessBreak.GetProcessBreakMappedList(ClientID, ProcessName, UserId, oTenant);
                result.Data = value;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;


        }
        [HttpGet]

        [Route("GetBreakListByProcessID")]
        public ActionResult<MessageResponse<List<BEBreakInfo>>> GetBreakListByProcessID(int Id)
        {
            var result = new MessageResponse<List<BEBreakInfo>>();
            try
            {

                var value = _repositoryBreak.GetBreakListByProcessID(Id, oTenant);
                result.Data = value;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;


        }
        [HttpGet]

        [Route("GetBreakListBySearch")]
        public ActionResult<MessageResponse<List<BEBreakInfo>>> GetBreakListBySearch(string sBreakName, int iProcessId)
        {
            var result = new MessageResponse<List<BEBreakInfo>>();
            try
            {

                var value = _repositoryBreak.GetBreakListBySearch(sBreakName, iProcessId, oTenant);
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
