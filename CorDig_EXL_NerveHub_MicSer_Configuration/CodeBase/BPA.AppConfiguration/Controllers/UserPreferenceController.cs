using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;
using BPA.AppConfiguration.BaseController;
using BPA.AppConfiguration.Helper;
using BPA.AppConfiguration.Helper.Filter;
using BPA.AppConfiguration.Models;
using BPA.AppConfiguration.Models.Response;
using Microsoft.AspNetCore.Mvc;
using MicSer.Configuration.Models.Request;
using System.Reflection;

namespace MicSer.Configuration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthentication]
    public class UserPreferenceController : BaseController<UserPreferenceController>
    {
        private readonly IUserPreferenceService _repositoryUserPreference;
        private readonly ILanguagesService _repositoryLanguage;
        public UserPreferenceController(ILogger<UserPreferenceController> logger, IWebHostEnvironment env, IConfiguration configuration,
           IUserPreferenceService repositoryUserPreference, ILanguagesService repositoryLanguage) : base(logger, env, configuration)
        {
            _repositoryUserPreference = repositoryUserPreference;
            _repositoryLanguage = repositoryLanguage;
        }

        [ProducesResponseType(typeof(BEUserPreference), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetUserPreference")]
        public async Task<MessageResponse<BEUserPreference>> GetUserPreference()
        {
            var result = new MessageResponse<BEUserPreference>();

            try
            {
                var dbresult = await Task.Run(() => _repositoryUserPreference.GetUserPerefernceDetail(base.oUser.iUserID, base.oTenant));

                result.Data = dbresult;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("SaveUpdateUserPreference")]
        public async Task<MessageResponse<int>> SaveUpdateUserPreference([FromBody] UserPreferenceModel request)
        {
            var result = new MessageResponse<int>();
            BEUserPreference objBEUserPreference = new();
            BETimeZoneInfo objBETimeZoneInfo = new();

            try
            {
                objBEUserPreference.iUserID = base.oUser.iUserID;
                objBEUserPreference.sLanguage = request.Language == null ? "en-US" : request.Language;
                objBETimeZoneInfo.iTimeZoneID = request.TimeZoneID;
                objBEUserPreference.oTimezone = objBETimeZoneInfo;
                await Task.Run(() => _repositoryUserPreference.SaveUpdateUserPreference(objBEUserPreference, base.oTenant));
                result.Data = 1;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
    }
}
