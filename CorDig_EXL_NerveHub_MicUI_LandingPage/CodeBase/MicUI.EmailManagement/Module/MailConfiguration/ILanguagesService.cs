using MicUI.EmailManagement.Helper;
using MicUI.EmailManagement.Models.Request;
using MicUI.EmailManagement.Services.MailConfiguration;
using MicUI.EmailManagement.Services.ServiceModel;

namespace MicUI.EmailManagement.Module.MailConfiguration
{
    public interface ILanguagesService
    {
        IList<BEMailTranslatorConfiguration> GetCampaignWiseLangList(int iStoreID);
        BEMailTranslatorConfiguration GetEMSLanguageConfigAllData(int langConfigID);
        IList<BEMailTranslatorConfiguration> GetLanguageProfile(int iCampaignID);
        void InsertUpdateData(EMSLanguageConfigModel oBEMailTranslatorConfiguration);
        List<LanguageApprovalRequestModel> GetIncomingTranslationData(int iCampaignID, int LanguageConfigID);
        void InsertIncomingTranslationData(LanguageApprovalRequestModel oBEMailTranslatorConfiguration);
    }
    public class LanguagesService : ILanguagesService
    {
        private readonly IMailConfigurationApiService _mailConfigurationService;

        public LanguagesService(IMailConfigurationApiService mailConfigurationService)
        {
            _mailConfigurationService = mailConfigurationService;
        }

        public IList<BEMailTranslatorConfiguration> GetCampaignWiseLangList(int iStoreID)
        {
            var result = _mailConfigurationService.GetCampaignWiseLangListAsync(iStoreID).GetAwaiter().GetResult();
            return result.data != null ? result.data.ToList() : new List<BEMailTranslatorConfiguration>();
        }
        public BEMailTranslatorConfiguration GetEMSLanguageConfigAllData(int langConfigID)
        {
            var result = _mailConfigurationService.GetEMSLanguageConfigAllDataAsync(langConfigID).GetAwaiter().GetResult();
            return result.data != null ? result.data : new();
        }
        public IList<BEMailTranslatorConfiguration> GetLanguageProfile(int iCampaignID)
        {
            var result = _mailConfigurationService.GetLanguageProfileAsync(iCampaignID).GetAwaiter().GetResult();
            return result.data != null ? result.data.ToList() : new List<BEMailTranslatorConfiguration>();
        }
        public void InsertUpdateData(EMSLanguageConfigModel oBEMailTranslatorConfiguration)
        {
            var result = _mailConfigurationService.InsertUpdateLanguageConfigurationAsync(oBEMailTranslatorConfiguration).GetAwaiter().GetResult();

            if (result != null && result.message != null && result.status != true)
            {
                if (result.message.ToLower().Contains(GlobalConstant.ExNumber547NullInsert) || result.message.ToLower().Contains(GlobalConstant.ExNumber547ReferenceKeyconstraint))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else if (result.message.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                else if (result.message.ToLower().Contains(BPA.GlobalResources.DataLayer.Resources.msg_MailConfigAlreadyExist.ToLower()))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_MailConfigAlreadyExist);
                }
                throw new Exception(result.message);
            }
        }
        public List<LanguageApprovalRequestModel> GetIncomingTranslationData(int iCampaignID, int LanguageConfigID)
        {
            var result = _mailConfigurationService.GetIncomingTranslationDataAsync(iCampaignID, LanguageConfigID).GetAwaiter().GetResult();

            if (result != null && result.message != null && result.status != true)
            {
                if (result.message.ToLower().Contains(GlobalConstant.ExNumber547NullInsert) || result.message.ToLower().Contains(GlobalConstant.ExNumber547ReferenceKeyconstraint))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else if (result.message.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                else if (result.message.ToLower().Contains(BPA.GlobalResources.DataLayer.Resources.msg_MailConfigAlreadyExist.ToLower()))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_MailConfigAlreadyExist);
                }
                throw new Exception(result.message);
            }
            return result.data ?? new List<LanguageApprovalRequestModel>();
        }
        public void InsertIncomingTranslationData(LanguageApprovalRequestModel oBEMailTranslatorConfiguration)
        {
            var result = _mailConfigurationService.InsertIncomingTranslationDataAsync(oBEMailTranslatorConfiguration).GetAwaiter().GetResult();
           
            if (result != null && result.message != null && result.status != true)
            {
                if (result.message.ToLower().Contains(GlobalConstant.ExNumber547NullInsert) || result.message.ToLower().Contains(GlobalConstant.ExNumber547ReferenceKeyconstraint))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else if (result.message.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
            }
        }
    }
}
