using BPA.EmailConfiguration.BaseController;
using BPA.EmailConfiguration.Helper;
using BPA.EmailConfiguration.Models;
using BPA.EmailManagement.ServiceContract.ExternalRef.Config;
using BPA.Translator;
using Microsoft.AspNetCore.Mvc;
using MicSer.EmailConfiguration.Helper;
using MicSer.EmailConfiguration.Helper.Filter;

namespace BPA.EmailConfiguration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthentication]
    public class LanguageConfigController : BaseController<LanguageConfigController>
    {
        private readonly ILanguagesService _languagesService;
        public LanguageConfigController(ILogger<LanguageConfigController> logger,
            IWebHostEnvironment env,
            IConfiguration configuration, ILanguagesService languagesService) : base(logger, env, configuration)
        {
            this._languagesService = languagesService;
        }

        [ProducesResponseType(typeof(IEnumerable<BEMailTranslatorConfiguration>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetCampaignWiseLangList")]
        public async Task<MessageResponse<IEnumerable<BEMailTranslatorConfiguration>>> GetCampaignWiseLangList([FromQuery] int workObjectId)
        {
            var result = new MessageResponse<IEnumerable<BEMailTranslatorConfiguration>>();
            try
            {
                IList<BEMailTranslatorConfiguration> lLanguageProfileInfo;
                lLanguageProfileInfo = await Task.Run(() => _languagesService.GetCampaignWiseLangList(workObjectId, base.oTenant, 2));
                result.Data = lLanguageProfileInfo;
                result.TotalCount = lLanguageProfileInfo.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = "GetCampaignWiseLangList", message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(BEMailTranslatorConfiguration), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetEMSLanguageConfigAllData")]
        public async Task<MessageResponse<BEMailTranslatorConfiguration>> GetEMSLanguageConfigAllData([FromQuery] int languageConfigID)
        {
            var result = new MessageResponse<BEMailTranslatorConfiguration>();
            try
            {
                var lLangConfiguration = await Task.Run(() =>
                  _languagesService.GetEMSLanguageConfigAllData(languageConfigID, base.oTenant)
                 );
                result.Data = lLangConfiguration[0];
                result.TotalCount = lLangConfiguration.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = "GetEMSLanguageConfigAllData", message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("InsertUpdateLanguageConfiguration")]
        public async Task<MessageResponse<string>> InsertUpdateLanguageConfiguration([FromBody] EMSLanguageConfigModel objEMSLanguageConfigModel)
        {
            var result = new MessageResponse<string>();
            try
            {
                await Task.Run(() => _languagesService.InsertUpdateData(CatchRecord(objEMSLanguageConfigModel), 2, base.oTenant));
                result.Data = "Ok";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = "InsertUpdateLanguageConfiguration", message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(IEnumerable<BEMailTranslatorConfiguration>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetLanguageProfile")]
        public async Task<MessageResponse<IEnumerable<BEMailTranslatorConfiguration>>> GetLanguageProfile([FromQuery] int campaignID)
        {
            var result = new MessageResponse<IEnumerable<BEMailTranslatorConfiguration>>();
            try
            {
                IList<BEMailTranslatorConfiguration> lLanguageProfileInfo;
                lLanguageProfileInfo = await Task.Run(() => _languagesService.GetLanguageProfile(campaignID, base.oTenant));
                result.Data = lLanguageProfileInfo;
                result.TotalCount = lLanguageProfileInfo.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = "GetLanguageProfile", message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(List<LanguageApprovalRequestModel>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetIncomingTranslationData")]
        public async Task<MessageResponse<List<LanguageApprovalRequestModel>>> GetIncomingTranslationData([FromQuery] int campaignId, [FromQuery] int languageConfigID)
        {
            var result = new MessageResponse<List<LanguageApprovalRequestModel>>();
            try
            {
                List<LanguageApprovalRequestModel> lstObj = new List<LanguageApprovalRequestModel>();
                lstObj = await Task.Run(() => _languagesService.GetIncomingTranslationData(campaignId, languageConfigID, oTenant).ConvertToList<LanguageApprovalRequestModel>());

                /*
                lstObj.ForEach(x =>
                {
                    //if (x.StatusToShowHideButtons == "Rejected")
                    //{
                    //    x.IsApproved = false;
                    //    x.IsRejectedTech = true;
                    //}
                    //else if (x.StatusToShowHideButtons == "Approved")
                    //{
                    //    x.IsApproved = true;
                    //    x.IsRejectedTech = false;
                    //}
                    //else if (x.StatusToShowHideButtons == "Pending")
                    //{
                    x.IsApproved = false;
                    x.IsRejectedTech = false;
                    //}

                });*/

                result.Data = lstObj;
                result.TotalCount = lstObj.Count;
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = "GetIncomingTranslationData", message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPut]
        [Route("InsertIncomingTranslationData")]
        public async Task<MessageResponse<string>> InsertIncomingTranslationData([FromBody] LanguageApprovalRequestModel updatedWorkApproval)
        {
            var result = new MessageResponse<string>();
            try
            {
                BEMailTranslatorConfiguration oBEMailTranslatorConfiguration = new BEMailTranslatorConfiguration();
                oBEMailTranslatorConfiguration.LngID = updatedWorkApproval.LngID;
                oBEMailTranslatorConfiguration.CampaignId = updatedWorkApproval.CampaignID;
                oBEMailTranslatorConfiguration.SMEChangesText = updatedWorkApproval.SMEChangesText;
                oBEMailTranslatorConfiguration.ApprovedBy = base.oUser.iUserID;
                oBEMailTranslatorConfiguration.iUserID = base.oUser.iUserID;

                // oBEMailTranslatorConfiguration.SpanishText = updatedWorkApproval.OriginalText;
                // oBEMailTranslatorConfiguration.SystranText = updatedWorkApproval.TranslatedText;
                oBEMailTranslatorConfiguration.IsApproved = updatedWorkApproval.IsApproved;
                oBEMailTranslatorConfiguration.IsRejectedTech = updatedWorkApproval.IsRejectedTech;

                await Task.Run(() => _languagesService.InsertIncomingTranslationData(oBEMailTranslatorConfiguration, base.oTenant));
                result.Data = "Updated";
            }
            catch (Exception ex)
            {
                result.Message = _logger.Error(new LoggerInfo() { actionName = "InsertIncomingTranslationData", message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}", requestId = CorrelationId });
            }
            return result;
        }

        private BEMailTranslatorConfiguration CatchRecord(EMSLanguageConfigModel objEMSLanguageConfigModel)
        {
            BEMailTranslatorConfiguration obj = new BEMailTranslatorConfiguration();
            obj.CampaignId = objEMSLanguageConfigModel.CampaignId;
            obj.LanguageConfigID = objEMSLanguageConfigModel.LanguageConfigID;
            obj.LanguageConfigName = objEMSLanguageConfigModel.LanguageConfigName;
            obj.ApiKey = objEMSLanguageConfigModel.ApiKey;
            obj.ApiUrl = objEMSLanguageConfigModel.ApiUrl;
            obj.BatchId = objEMSLanguageConfigModel.BatchId;
            obj.Source = objEMSLanguageConfigModel.Source;
            obj.Target = objEMSLanguageConfigModel.Target;
            obj.ProfileID = objEMSLanguageConfigModel.ProfileID;
            obj.WithSource = objEMSLanguageConfigModel.WithSource;
            obj.WithDictionary = objEMSLanguageConfigModel.WithDictionary;
            obj.WithCorpus = objEMSLanguageConfigModel.WithCorpus;
            obj.WithAnnotations = objEMSLanguageConfigModel.WithAnnotations;
            obj.BackTranslation = objEMSLanguageConfigModel.BackTranslation;
            obj.Async = objEMSLanguageConfigModel.Async;
            obj.Callback = objEMSLanguageConfigModel.Callback;
            obj.Encoding = objEMSLanguageConfigModel.Encoding;
            obj.Options = objEMSLanguageConfigModel.Options;
            obj.Disabled = objEMSLanguageConfigModel.Disabled;
            obj.IncomingMail = objEMSLanguageConfigModel.IncomingMail;
            obj.CreatedBy = base.oUser.iUserID;
            obj.iUserID = base.oUser.iUserID;
            return obj;
        }
    }
}
