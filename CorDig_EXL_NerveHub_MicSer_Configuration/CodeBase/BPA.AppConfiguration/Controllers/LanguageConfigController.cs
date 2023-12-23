using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.BusinessEntity.ExternalRef;
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
    public class LanguageConfigController : BaseController<LanguageConfigController>
    {
        private readonly ILanguagesService _languagesService;
        public LanguageConfigController(ILogger<LanguageConfigController> logger, IWebHostEnvironment env, IConfiguration configuration,
            ILanguagesService languagesService) : base(logger, env, configuration)
        {
            _languagesService = languagesService;
        }

        [ProducesResponseType(typeof(IList<BELanguages>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetLanguages")]
        public async Task<MessageResponse<IList<BELanguages>>> GetLanguages([FromQuery] bool IsActiveLanguages)
        {
            var result = new MessageResponse<IList<BELanguages>>();

            try
            {
                var dbresult = await Task.Run(() => _languagesService.GetLanguageList(IsActiveLanguages, base.oTenant));

                result.Data = dbresult;
                result.TotalCount = dbresult.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [HttpPost]
        [Route("InsertUpdateData")]
        public ActionResult<MessageResponse<string>> InsertUpdateData([FromBody] BEMailTranslatorConfiguration oBEMailTranslatorConfiguration, int FormID)
        {
            var result = new MessageResponse<string>();
            try
            {
                _languagesService.InsertUpdateData(oBEMailTranslatorConfiguration, FormID, oTenant);
                result.Data = "Ok";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
        [HttpGet]
        [Route("GetLanguageConfigAllData")]
        public ActionResult<MessageResponse<IList<BEMailTranslatorConfiguration>>> GetLanguageConfigAllData(int langConfigID)
        {
            var result = new MessageResponse<IList<BEMailTranslatorConfiguration>>();
            try
            {
                var value = _languagesService.GetLanguageConfigAllData(langConfigID, oTenant);
                result.Data = value;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(IList<BEMailTranslatorConfiguration>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetCampaignWiseLangList")]
        public async Task<MessageResponse<IList<BEMailTranslatorConfiguration>>> GetCampaignWiseLangList([FromQuery] int storeId, [FromQuery] int formId = 1)
        {
            var result = new MessageResponse<IList<BEMailTranslatorConfiguration>>();
            try
            {
                var value = await Task.Run(() => _languagesService.GetCampaignWiseLangList(storeId, oTenant, formId));
                result.TotalCount = value.Count;
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
