using BPA.AppConfig.BusinessEntity.ExternalRef.SharePointConfiguration;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.SharePointConfiguration;
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
    public class SharePointConfigurationController : BaseController<SharePointConfigurationController>
    {
        private readonly ISharePointConfiguration _iSharePointConfiguration;
        public SharePointConfigurationController(ILogger<SharePointConfigurationController> logger, IWebHostEnvironment env, IConfiguration configuration,
            ISharePointConfiguration SharePointConfiguration) : base(logger, env, configuration)
        {
            _iSharePointConfiguration = SharePointConfiguration;
        }
        [HttpGet]
        [Route("CheckIfCampaignMappingExist")]
        public ActionResult<MessageResponse<bool>> CheckIfCampaignMappingExist(string campaignId)
        {
            var result = new MessageResponse<bool>();
            try
            {
                var value = _iSharePointConfiguration.CheckIfCampaignMappingExist(campaignId, oTenant);
                result.Data = value;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpGet]
        [Route("GetSharepointSchedulerList")]
        public ActionResult<MessageResponse<IList<BESharePointConfiguration>>> GetSharepointSchedulerList(int ShedulerID)
        {
            var result = new MessageResponse<IList<BESharePointConfiguration>>();
            try
            {
                var value = _iSharePointConfiguration.GetSharepointSchedulerList(ShedulerID, oTenant);
                result.Data = value;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpGet]
        [Route("GetSearchSharepointList")]
        public ActionResult<MessageResponse<IList<BESharePointConfiguration>>> GetSearchSharepointList(int campaignId)
        {
            var result = new MessageResponse<IList<BESharePointConfiguration>>();
            try
            {
                var value = _iSharePointConfiguration.GetSharepointSchedulerList(campaignId, oTenant);
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
        public ActionResult<MessageResponse<string>> InsertData([FromBody] BESharePointConfiguration oSharePointConfiguration, int iFormID)
        {
            var result = new MessageResponse<string>();
            try
            {
                _iSharePointConfiguration.InsertData(oSharePointConfiguration, iFormID, oTenant);
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
        public ActionResult<MessageResponse<string>> UpdateData([FromBody] BESharePointConfiguration oBESharePointConfiguration, int iFormID)
        {
            var result = new MessageResponse<string>();
            try
            {
                _iSharePointConfiguration.UpdateData(oBESharePointConfiguration, iFormID, oTenant);
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
