using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.BusinessEntity.ExternalRef.Configuration;
using BPA.AppConfig.BusinessEntity.ExternalRef.Security;
using BPA.AppConfig.BusinessEntity.Security;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.Configuration;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.Security;
using BPA.AppConfiguration.BaseController;
using BPA.AppConfiguration.Helper;
using BPA.AppConfiguration.Helper.Filter;
using BPA.AppConfiguration.Models;
using BPA.AppConfiguration.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Security.Application;
using System.Reflection;

namespace BPA.AppConfiguration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthentication]
    public class SkillUserMapController : BaseController<SkillUserMapController>
    {
        private readonly ITeamService _repositoryTeam;
        private readonly ISkillService _repositorySkill;

        public SkillUserMapController(ILogger<SkillUserMapController> logger, IWebHostEnvironment env, IConfiguration configuration, 
            ITeamService repositoryTeam, ISkillService repositorySkill) : base(logger, env, configuration)
        {
            _repositoryTeam = repositoryTeam;
            _repositorySkill = repositorySkill;
        }

        [ProducesResponseType(typeof(List<BETeamInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetProcessWiseTeamList")]
        public async Task<MessageResponse<List<BETeamInfo>>> GetProcessWiseTeamList([FromQuery] int processID)
        {
            var result = new MessageResponse<List<BETeamInfo>>();
            List<BETeamInfo> lstBETeamInfo = new();

            try
            {
                lstBETeamInfo = await Task.Run(() => _repositoryTeam.GetProcessWiseTeamList(processID, oTenant));
                result.Data = lstBETeamInfo;
                result.TotalCount = lstBETeamInfo.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<BESkillInfo>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetCampaignSkillList")]
        public async Task<MessageResponse<List<BESkillInfo>>> GetCampaignSkillList([FromQuery] int campaignId)
        {
            List<BESkillInfo> lstBESkillInfo = new();
            var result = new MessageResponse<List<BESkillInfo>>();

            try
            {
                lstBESkillInfo = await Task.Run(() => _repositorySkill.GetCampaignSkillList(campaignId, oTenant));
                result.Data = lstBESkillInfo;
                result.TotalCount = lstBESkillInfo.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(UserSkillMapModel), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetUserSkillMapList")]
        public async Task<MessageResponse<UserSkillMapModel>> GetUserSkillMapList([FromQuery] int teamID, [FromQuery] int campaignId)
        {
            UserSkillMapModel userSkillMapObj = new();
            var lstSkillMap = new List<BEUserSkillMap>();
            IList<BEUserInfo> lUserInfo = new List<BEUserInfo>();
            var result = new MessageResponse<UserSkillMapModel>();

            try
            {

                lstSkillMap = await Task.Run(() => _repositorySkill.GetMapSkill(Convert.ToInt32(campaignId), base.oTenant));
                IList<BETeamInfo> lTeam = await Task.Run(() => _repositoryTeam.GetTeamList(Convert.ToInt32(teamID), base.oTenant));

                lUserInfo = lTeam[0].iUserID;

                foreach (var user in lUserInfo)
                {
                    UserSkill objUserskill = new();
                    objUserskill.UserName = user.Name;
                    objUserskill.UserId = user.iUserID;

                    for (int j = 0; j < lstSkillMap.Count; j++)
                    {
                        if (user.iUserID == lstSkillMap[j].iUserID)
                        {
                            objUserskill.SkillInfoList.Add(new UserSkillInfo { SkillID = lstSkillMap[j].iSkillID, SkillName = lstSkillMap[j].sSkillName });
                        }
                    }

                    userSkillMapObj.UserSkillList.Add(objUserskill);
                }
                userSkillMapObj.CampaignId = campaignId;
                result.Data = userSkillMapObj;
                result.TotalCount = userSkillMapObj.UserSkillList.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return await Task.FromResult(result);
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("AddSkillS")]
        public async Task<MessageResponse<string>> AddSkillS([FromBody] UserSkillMapModel UserSkillMapModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _repositorySkill.InsertUserSkillMap(CatchRecord(UserSkillMapModel.UserSkillList, UserSkillMapModel.CampaignId.ToString()), base.oTenant));
                result.Data = "Success";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
                result.Data = "Error";
            }
            return result;
        }

        [NonAction]
        private string CatchRecord(List<UserSkill> updatedSkills, string iCampaignId)
        {
            string strXml = "", sKillID = "";
            strXml = "<SkillConditioMap>";
            for (int i = 0; i < updatedSkills.Count; i++)
            {
                if (updatedSkills[i].SkillInfoList != null && updatedSkills[i].SkillInfoList != null)
                {
                    if (updatedSkills[i].SkillInfoList[0] != null)
                    {
                        strXml += "<MapData>";
                        strXml += "<UserId>" + updatedSkills[i].UserId.ToString() + "</UserId>";
                        strXml += "<UserName>" + Encoder.HtmlEncode(updatedSkills[i].UserName, false) + "</UserName>";
                        for (int j = 0; j < updatedSkills[i].SkillInfoList.Count; j++)
                        {
                            if (updatedSkills[i].SkillInfoList[j] != null)
                            {
                                sKillID = sKillID + updatedSkills[i].SkillInfoList[j].SkillID.ToString() + ",";
                            }
                        }
                        strXml += "<SkillID>" + Encoder.HtmlEncode(sKillID.TrimEnd(','), false) + "</SkillID>";
                        strXml += "<CreatedBy>" + base.oUser.iUserID + "</CreatedBy>";
                        strXml += "<CampaignID>" + iCampaignId + "</CampaignID>";
                        strXml += "</MapData>";
                        sKillID = "";
                    }
                }
                else
                {
                    strXml += "<MapData>";
                    strXml += "<UserId>" + updatedSkills[i].UserId.ToString() + "</UserId>";
                    strXml += "<UserName>" + Encoder.HtmlEncode(updatedSkills[i].UserName, false) + "</UserName>";
                    strXml += "<SkillID></SkillID>";
                    strXml += "<CreatedBy>" + base.oUser.iUserID + "</CreatedBy>";
                    strXml += "<CampaignID>" + iCampaignId + "</CampaignID>";
                    strXml += "</MapData>";
                }
            }
            strXml += "</SkillConditioMap>";
            return strXml;
        }


    }
}
