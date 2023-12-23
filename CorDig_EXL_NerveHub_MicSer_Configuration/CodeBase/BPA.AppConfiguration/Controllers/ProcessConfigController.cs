using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;
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
    public class ProcessConfigController : BaseController<ProcessConfigController>
    {
        private IProcessShiftWindowService _repositoryShiftWindow;
        public ProcessConfigController(ILogger<ProcessConfigController> logger, IWebHostEnvironment env, IConfiguration configuration,
            IProcessShiftWindowService processShiftWindowService) : base(logger, env, configuration)
        {
            _repositoryShiftWindow = processShiftWindowService;
        }
        [HttpPost]
        [Route("InsertProcessSiftConfig")]
        public ActionResult<MessageResponse<string>> InsertProcessSiftConfig(string strBreakXml)
        {
            var result = new MessageResponse<string>();
            try
            {
                _repositoryShiftWindow.InsertProcessSiftConfig(strBreakXml, oTenant);
                result.Data = "Ok";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpPut]
        [Route("UpdateProcessSiftConfig")]
        public ActionResult<MessageResponse<string>> UpdateProcessSiftConfig(string strBreakXml)
        {
            var result = new MessageResponse<string>();
            try
            {
                _repositoryShiftWindow.UpdateProcessSiftConfig(strBreakXml, oTenant);
                result.Data = "Ok";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpGet]
        [Route("GetProcessShiftConfig")]
        public ActionResult<MessageResponse<List<BEProcessConfigData>>> GetProcessShiftConfig(string searchText)
        {
            var result = new MessageResponse<List<BEProcessConfigData>>();
            try
            {
                var value = _repositoryShiftWindow.GetProcessShiftConfig(searchText, oTenant);
                result.Data = value;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpGet]
        [Route("GetProcessShiftConfigDetail")]
        public ActionResult<MessageResponse<BEProcessWindow>> GetProcessShiftConfigDetail(int iProcessshiftConfigId)
        {
            var result = new MessageResponse<BEProcessWindow>();
            try
            {
                var value = _repositoryShiftWindow.GetProcessShiftConfigDetail(iProcessshiftConfigId, oTenant);
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
