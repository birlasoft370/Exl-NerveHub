using BPA.AppConfig.BusinessEntity.Config;
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
    public class SkillCampaignMapController : BaseController<SkillCampaignMapController>
    {
        private readonly ISkillService _repositorySkill;

        public SkillCampaignMapController(ILogger<SkillCampaignMapController> logger, IWebHostEnvironment env, IConfiguration configuration,
            ISkillService skillService) : base(logger, env, configuration)
        {
            this._repositorySkill = skillService;
        }

        [ProducesResponseType(typeof(List<BESkillInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetActiveSkillList")]
        public async Task<MessageResponse<List<BESkillInfo>>> GetActiveSkillList()
        {
            var result = new MessageResponse<List<BESkillInfo>>();
            try
            {
                var activeSkillList = await Task.Run(() => _repositorySkill.GetActiveSkillList(base.oTenant));
                result.Data = activeSkillList;
                result.TotalCount = activeSkillList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<BESkillInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetSkillMapByCampIdList")]
        public async Task<MessageResponse<List<BESkillInfo>>> GetSkillMapByCampIdList([FromQuery] int campaignId)
        {
            var result = new MessageResponse<List<BESkillInfo>>();
            try
            {
                var skillMapByCampIdList = await Task.Run(() => _repositorySkill.GetCampaignSkillList(campaignId, base.oTenant));
                result.Data = skillMapByCampIdList;
                result.TotalCount = skillMapByCampIdList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateCampSkillMap")]
        public async Task<MessageResponse<string>> UpdateCampSkillMap([FromBody] SkillCampaignMapModel skillCampaign)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositorySkill.UpdateCampSkillMap(skillCampaign.CampaignId, Encoder.HtmlEncode(string.Join(",", skillCampaign.Skills), false), skillCampaign.UserId, base.oTenant));
                result.Data = "ok";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }
    }
}
