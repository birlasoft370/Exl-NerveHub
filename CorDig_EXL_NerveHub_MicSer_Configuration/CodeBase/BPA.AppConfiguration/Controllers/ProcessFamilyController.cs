using BPA.AppConfig.BusinessEntity.ExternalRef;
using BPA.AppConfig.Datalayer.ExternalRef;
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
    public class ProcessFamilyController : BaseController<ProcessFamilyController>
    {
        private IWatReport _repositaryWATReport;
        public ProcessFamilyController(ILogger<ProcessFamilyController> logger, IWebHostEnvironment env, IConfiguration configuration,
            IWatReport repositaryWATReport) : base(logger, env, configuration)
        {
            _repositaryWATReport = repositaryWATReport;
        }
        [HttpPost]
        [Route("SaveProcessFamily")]
        public ActionResult<MessageResponse<string>> SaveProcessFamily([FromBody] List<BEProcessFamilyMap> lstProcessFamily)
        {
            var result = new MessageResponse<string>();
            try
            {
                _repositaryWATReport.SaveProcessFamily(lstProcessFamily, oTenant);
                result.Data = "Ok";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpPut]
        [Route("UpdateProcessFamily")]
        public ActionResult<MessageResponse<string>> UpdateProcessFamily([FromBody] List<BEProcessFamilyMap> lstProcessFamily, int iProcessFamilyID)
        {
            var result = new MessageResponse<string>();
            try
            {
                _repositaryWATReport.UpdateProcessFamily(lstProcessFamily, iProcessFamilyID, oTenant);
                result.Data = "Ok";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpGet]
        [Route("GetProcessFamilyList")]
        public ActionResult<MessageResponse<List<BEProcessFamilyMap>>> GetProcessFamilyList(string sProcessFamilyName)
        {
            var result = new MessageResponse<List<BEProcessFamilyMap>>();
            try
            {
                var value = _repositaryWATReport.GetProcessFamilyList(sProcessFamilyName, oTenant);
                result.Data = value;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpPut]
        [Route("DisableProcessFamily")]
        public ActionResult<MessageResponse<string>> DisableProcessFamily(int iProcessFamilyID, int iUserID)
        {
            var result = new MessageResponse<string>();
            try
            {
                _repositaryWATReport.DisableProcessFamily(iProcessFamilyID, iUserID, oTenant);
                result.Data = "Ok";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
    }
}
