using Microsoft.AspNetCore.Mvc;
using MicUI.EmailManagement.Models.Request;
using MicUI.EmailManagement.Models.Response;
using MicUI.EmailManagement.Models.ViewModels;
using MicUI.EmailManagement.Services.ServiceModel;

namespace MicUI.EmailManagement.Services.MailConfiguration
{
    public interface IMailConfigurationApiService
    {
        Task<MessageResponse<IEnumerable<BEMailConfiguration>>> GetCampaignWiseListAsync(int StoreID);
        Task<MessageResponse<IList<BEMailTemplate>>> GetMailTemplateListAsync(bool isDisabled, bool isAutoReply);
        Task<MessageResponse<IList<BEMailConfiguration>>> GetMailConfigAllDataAsync(int mailConfigId);
        Task<MessageResponse<IEnumerable<TreeStructure>>> TestConnectionAsync(TestConnectionModel model);
        Task<MessageResponse<IEnumerable<TreeStructure>>> GetNodeValueAsync(TestConnectionModel model);
        Task<MessageResponse<string>> InsertMailConfigurationAsync(EmailConfigurationModel oMailConfiguration, int formID);
        Task<MessageResponse<string>> UpdateMailConfigurationAsync(EmailConfigurationModel oMailConfiguration, int formID);
        Task<MessageResponse<string>> InsertUpdateAdvancedConfigurationAsync(EmailAdvancedConfigurationModel oMailAdvConfiguration);
        Task<MessageResponse<IEnumerable<BEMailConfiguration>>> GetAdvancedConfigurationAsync(int iCampaignID);
        Task<MessageResponse<IList<BEMailTemplate>>> GetMailTemplateListAsync(int campaignId);
        Task<MessageResponse<BEMailTemplate>> GetMailTemplateByIdAsync(int iMailTemplateId);
        Task<MessageResponse<string>> AddMailTemplateAsync(MailTemplateModel model, int formId);
        Task<MessageResponse<string>> UpdateMailTemplateAsync(MailTemplateModel model, int formId);
        Task<MessageResponse<IEnumerable<BEMailTranslatorConfiguration>>> GetCampaignWiseLangListAsync(int workObjectId);
        Task<MessageResponse<BEMailTranslatorConfiguration>> GetEMSLanguageConfigAllDataAsync(int languageConfigID);
        Task<MessageResponse<string>> InsertUpdateLanguageConfigurationAsync(EMSLanguageConfigModel objEMSLanguageConfigModel);
        Task<MessageResponse<string>> InsertIncomingTranslationDataAsync(LanguageApprovalRequestModel updatedWorkApproval);
        Task<MessageResponse<IEnumerable<BEMailTranslatorConfiguration>>> GetLanguageProfileAsync(int campaignID);
        Task<MessageResponse<List<LanguageApprovalRequestModel>>> GetIncomingTranslationDataAsync(int campaignId, int languageConfigID);
    }
    public class MailConfigurationApiService : BaseApiService, IMailConfigurationApiService
    {
        public MailConfigurationApiService(HttpClient client, IHttpContextAccessor httpContextAccessor) : base(client, httpContextAccessor)
        {

        }

        public async Task<MessageResponse<IEnumerable<BEMailConfiguration>>> GetCampaignWiseListAsync(int StoreID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<IEnumerable<BEMailConfiguration>>>($"MailConfiguration/GetCampaignWiseList?StoreID={StoreID}");
            return content ?? new MessageResponse<IEnumerable<BEMailConfiguration>>();
        }
        public async Task<MessageResponse<IList<BEMailTemplate>>> GetMailTemplateListAsync(bool isDisabled, bool isAutoReply)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<IList<BEMailTemplate>>>($"MailConfiguration/GetMailTemplateList?isDisabled={isDisabled}&isAutoReply={isAutoReply}");
            return content ?? new MessageResponse<IList<BEMailTemplate>>();
        }
        public async Task<MessageResponse<IList<BEMailConfiguration>>> GetMailConfigAllDataAsync(int mailConfigId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<IList<BEMailConfiguration>>>($"MailConfiguration/GetMailConfigAllData?mailConfigId={mailConfigId}");
            return content ?? new MessageResponse<IList<BEMailConfiguration>>();
        }
        public async Task<MessageResponse<IEnumerable<TreeStructure>>> TestConnectionAsync(TestConnectionModel model)
        {
            var response = await _client.PostAsJsonAsync("MailConfiguration/TestConnection", model);
            return await response.Content.ReadFromJsonAsync<MessageResponse<IEnumerable<TreeStructure>>>() ?? new MessageResponse<IEnumerable<TreeStructure>>(); ;
        }
        public async Task<MessageResponse<IEnumerable<TreeStructure>>> GetNodeValueAsync(TestConnectionModel model)
        {
            var response = await _client.PostAsJsonAsync("MailConfiguration/GetNodeValue", model);
            return await response.Content.ReadFromJsonAsync<MessageResponse<IEnumerable<TreeStructure>>>() ?? new MessageResponse<IEnumerable<TreeStructure>>(); ;
        }
        public async Task<MessageResponse<string>> InsertMailConfigurationAsync(EmailConfigurationModel oMailConfiguration, int formID)
        {
            var response = await _client.PostAsJsonAsync($"MailConfiguration/InsertMailConfiguration?formID={formID}", oMailConfiguration);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<string>> UpdateMailConfigurationAsync(EmailConfigurationModel oMailConfiguration, int formID)
        {
            var response = await _client.PutAsJsonAsync($"MailConfiguration/UpdateMailConfiguration?formID={formID}", oMailConfiguration);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<string>> InsertUpdateAdvancedConfigurationAsync(EmailAdvancedConfigurationModel oMailAdvConfiguration)
        {
            var response = await _client.PostAsJsonAsync($"MailConfiguration/InsertUpdateAdvancedConfiguration", oMailAdvConfiguration);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<IEnumerable<BEMailConfiguration>>> GetAdvancedConfigurationAsync(int iCampaignID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<IEnumerable<BEMailConfiguration>>>($"MailConfiguration/GetAdvancedConfiguration?iCampaignID={iCampaignID}");
            return content ?? new MessageResponse<IEnumerable<BEMailConfiguration>>();
        }
        public async Task<MessageResponse<IList<BEMailTemplate>>> GetMailTemplateListAsync(int campaignId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<IList<BEMailTemplate>>>($"MailTemplate/GetMailTemplateList?campaignId={campaignId}");
            return content ?? new MessageResponse<IList<BEMailTemplate>>();
        }
        public async Task<MessageResponse<BEMailTemplate>> GetMailTemplateByIdAsync(int iMailTemplateId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<BEMailTemplate>>($"MailTemplate/GetMailTemplateById?iMailTemplateId={iMailTemplateId}");
            return content ?? new MessageResponse<BEMailTemplate>();
        }
        public async Task<MessageResponse<string>> AddMailTemplateAsync(MailTemplateModel model, int formId)
        {
            var response = await _client.PostAsJsonAsync($"MailTemplate/AddMailTemplate?formId={formId}", model);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<string>> UpdateMailTemplateAsync(MailTemplateModel model, int formId)
        {
            var response = await _client.PutAsJsonAsync($"MailTemplate/UpdateMailTemplate?formId={formId}", model);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }

        public async Task<MessageResponse<IEnumerable<BEMailTranslatorConfiguration>>> GetCampaignWiseLangListAsync(int workObjectId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<IEnumerable<BEMailTranslatorConfiguration>>>($"LanguageConfig/GetCampaignWiseLangList?workObjectId={workObjectId}");
            return content ?? new MessageResponse<IEnumerable<BEMailTranslatorConfiguration>>();
        }
        public async Task<MessageResponse<BEMailTranslatorConfiguration>> GetEMSLanguageConfigAllDataAsync(int languageConfigID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<BEMailTranslatorConfiguration>>($"LanguageConfig/GetEMSLanguageConfigAllData?languageConfigID={languageConfigID}");
            return content ?? new MessageResponse<BEMailTranslatorConfiguration>();
        }
        public async Task<MessageResponse<string>> InsertUpdateLanguageConfigurationAsync(EMSLanguageConfigModel objEMSLanguageConfigModel)
        {
            var response = await _client.PutAsJsonAsync($"LanguageConfig/InsertUpdateLanguageConfiguration", objEMSLanguageConfigModel);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<IEnumerable<BEMailTranslatorConfiguration>>> GetLanguageProfileAsync(int campaignID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<IEnumerable<BEMailTranslatorConfiguration>>>($"LanguageConfig/GetLanguageProfile?campaignID={campaignID}");
            return content ?? new MessageResponse<IEnumerable<BEMailTranslatorConfiguration>>();
        }
        public async Task<MessageResponse<string>> InsertIncomingTranslationDataAsync(LanguageApprovalRequestModel updatedWorkApproval)
        {
            var response = await _client.PutAsJsonAsync($"LanguageConfig/InsertIncomingTranslationData", updatedWorkApproval);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<List<LanguageApprovalRequestModel>>> GetIncomingTranslationDataAsync(int campaignId, int languageConfigID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<LanguageApprovalRequestModel>>>($"LanguageConfig/GetIncomingTranslationData?campaignID={campaignId}&languageConfigID={languageConfigID}");
            return content ?? new MessageResponse<List<LanguageApprovalRequestModel>>();
        }

    }
}
