using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.Configuration;
using BPA.AppConfiguration.BaseController;
using BPA.AppConfiguration.Helper;
using BPA.AppConfiguration.Helper.Filter;
using BPA.AppConfiguration.Models;
using BPA.AppConfiguration.Models.Request;
using BPA.AppConfiguration.Models.Response;
using BPA.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Security.Application;
using System.Reflection;

namespace BPA.AppConfiguration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthentication]
    public class SkillMasterController : BaseController<SkillMasterController>
    {
        private readonly ISkillService _repositorySkill;

        public SkillMasterController(ILogger<SkillMasterController> logger, IWebHostEnvironment env, IConfiguration configuration,
            ISkillService repositorySkill) : base(logger, env, configuration)
        {
            _repositorySkill = repositorySkill;
        }


        [ProducesResponseType(typeof(List<SkillMasterModel>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetSkillList")]
        public async Task<MessageResponse<List<SkillMasterModel>>> GetSkillList([FromQuery] string? skillName)
        {
            var result = new MessageResponse<List<SkillMasterModel>>();
            List<SkillMasterModel> oSkillMasterModel = new List<SkillMasterModel>();

            try
            {
                var skillList = await Task.Run(() =>
                _repositorySkill.GetSkillListByName(Encoder.HtmlEncode(skillName), true, base.oTenant));
                oSkillMasterModel = skillList.Select(x => new SkillMasterModel
                {
                    SkillID = x.iSkillID,
                    SkillDescription = x.sSkillDescription,
                    SkillName = x.sSkillName,
                    Disabled = x.bDisabled
                }).ToList();
                result.Data = oSkillMasterModel;
                result.TotalCount = oSkillMasterModel.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(SkillMasterModel), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetSkillById")]
        public async Task<MessageResponse<SkillMasterModel>> GetSkillById([FromQuery] int skillID)
        {
            var result = new MessageResponse<SkillMasterModel>();
            try
            {
                var skill = await Task.Run(() => GetModel(_repositorySkill.GetSkillList(skillID, oTenant).FirstOrDefault()));
                result.Data = skill;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("AddSkill")]
        public async Task<MessageResponse<string>> AddSkill([FromBody] SkillMasterModel skillModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                string Output = await Task.Run(() => this._repositorySkill.InsertData(GetSkillInfo(skillModel), 14, base.oTenant));

                if (Output == "" || Output == String.Empty)
                {
                   // result.Data = "display_SaveMsg";
                }
                else
                {
                    result.Data = Output;
                }
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateSkill")]
        public async Task<MessageResponse<string>> UpdateSkill([FromBody] SkillMasterModel skillModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                string Output = await Task.Run(() => this._repositorySkill.UpdateData(GetSkillInfo(skillModel), 14, base.oTenant));

                if (Output == "" || Output == String.Empty)
                {
                   // result.Data = "display_UpdateMsg";
                }
                else
                {
                    result.Data = Output;
                }
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = MethodBase.GetCurrentMethod().Name, message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }


        [NonAction]
        private SkillMasterModel GetModel(BESkillInfo info)
        {
            SkillMasterModel SBUObj = new SkillMasterModel();
            SBUObj.SkillID = info.iSkillID;
            SBUObj.SkillName = Encoder.HtmlEncode(info.sSkillName, false);
            SBUObj.SkillDescription = Encoder.HtmlEncode(info.sSkillDescription, false);
            SBUObj.Disabled = info.bDisabled;

            return SBUObj;
        }

        [NonAction]
        private BESkillInfo GetSkillInfo(SkillMasterModel SkillMaster)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.ConvertToList<BESkillInfo>();
            BESkillInfo skillInfo = new BESkillInfo();
            skillInfo.sSkillName = Encoder.HtmlEncode(SkillMaster.SkillName, false);
            skillInfo.sSkillDescription = Encoder.HtmlEncode(SkillMaster.SkillDescription, false);
            skillInfo.iSkillID = SkillMaster.SkillID;
            skillInfo.bDisabled = SkillMaster.Disabled;
            skillInfo.iCreatedBy = base.oUser.iUserID;
            skillInfo.iModifiedBy = base.oUser.iUserID;
            return skillInfo;
        }
    }
}
