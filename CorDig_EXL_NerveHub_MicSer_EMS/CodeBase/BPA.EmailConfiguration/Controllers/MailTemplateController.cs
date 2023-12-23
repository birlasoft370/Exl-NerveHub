using BPA.EmailConfiguration.BaseController;
using BPA.EmailConfiguration.Helper;
using BPA.EmailConfiguration.Models;
using BPA.EmailConfiguration.Models.Request;
using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.ServiceContract.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using MicSer.EmailConfiguration.Helper.Filter;

namespace BPA.EmailConfiguration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthentication]
    public class MailTemplateController : BaseController<MailTemplateController>
    {
        private readonly IMailTemplateService iMailTemplateService;
        public MailTemplateController(ILogger<MailTemplateController> logger,
            IWebHostEnvironment env,
            IConfiguration configuration, IMailTemplateService iMailTemplate) : base(logger, env, configuration)
        {
            this.iMailTemplateService = iMailTemplate;
        }

        [ProducesResponseType(typeof(IList<BEMailTemplate>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetMailTemplateList")]
        public async Task<MessageResponse<IList<BEMailTemplate>>> GetMailTemplateList([FromQuery] int campaignId)
        {
            var result = new MessageResponse<IList<BEMailTemplate>>();
            try
            {
                var mailTemplates = new List<MailTemplateModel>();
                var listMailTemplate = await Task.Run(() => iMailTemplateService.GetMailTemplateList(base.oUser.iUserID, campaignId, base.oTenant));
                result.Data = listMailTemplate;
                result.TotalCount = listMailTemplate.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = "GetMailTemplateList", message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(BEMailTemplate), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetMailTemplateById")]
        public async Task<MessageResponse<BEMailTemplate>> GetMailTemplateById([FromQuery] int iMailTemplateId)
        {
            var result = new MessageResponse<BEMailTemplate>();
            try
            {
                var mailTemplate = await Task.Run(() => iMailTemplateService.GetMailTemplateDataList(iMailTemplateId, base.oUser.iUserID, base.oTenant)[0]);
                result.Data = mailTemplate;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = "GetMailTemplateById", message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("AddMailTemplate")]
        public async Task<MessageResponse<string>> AddMailTemplate([FromBody] MailTemplateModel model, [FromQuery] int formId)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => iMailTemplateService.InsertData(CatchRecord(model), formId, base.oTenant));

                result.Data = "Mail Template data saved successfully !.";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = "AddMailTemplate", message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }


        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("UpdateMailTemplate")]
        public async Task<MessageResponse<string>> UpdateMailTemplate([FromBody] MailTemplateModel model, [FromQuery] int formId)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => iMailTemplateService.UpdateData(CatchRecord(model), formId, base.oTenant));

                result.Data = "Mail Template data updated successfully !.";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = "UpdateMailTemplate", message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }        
        private BEMailTemplate CatchRecord(MailTemplateModel oMailTemplateModel)
        {
            return new BEMailTemplate()
            {
                iMailTemplateId = oMailTemplateModel.MailTemplateId,
                sMailTemplateName = oMailTemplateModel.MailTemplateName,
                sMailTemplate = oMailTemplateModel.MailTemplate,
                //sMailTemplateName = AntiXssEncoder.HtmlEncode(Regex.Replace(oMailTemplateViewModel.MailTemplateName, @"\s+", " ").Trim(),false),
                //sMailTemplate = AntiXssEncoder.HtmlEncode(oMailTemplateViewModel.MailTemplate,false),
                bDisabled = Convert.ToBoolean(oMailTemplateModel.Disabled),
                bIsAutoReplay = Convert.ToBoolean(oMailTemplateModel.IsAutoReply),
                iCampaignID = Convert.ToInt32(oMailTemplateModel.CampaignId),
                iCreatedBy = base.oUser.iUserID
            };
        }

    }
}
