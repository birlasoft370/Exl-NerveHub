using MicUI.EmailManagement.Helper;
using MicUI.EmailManagement.Models.Request;
using MicUI.EmailManagement.Services.MailConfiguration;
using MicUI.EmailManagement.Services.ServiceModel;

namespace MicUI.EmailManagement.Module.MailConfiguration
{
    public interface IMailTemplateService
    {
        IList<BEMailTemplate> GetMailTemplateAll(bool IsActive, bool isAutoReply);
        IList<BEMailTemplate> GetMailTemplateList(int campaignId);
       BEMailTemplate GetMailTemplateDataList(int iEmailTemplateID);
        void InsertData(MailTemplateModel oEmailManagement, int iFormID);
        void UpdateData(MailTemplateModel oEmailManagement, int iFormID);
    }

    public class MailTemplateService : IMailTemplateService
    {
        private readonly IMailConfigurationApiService _mailConfigurationService;

        public MailTemplateService(IMailConfigurationApiService mailConfigurationService)
        {
            _mailConfigurationService = mailConfigurationService;
        }

        public IList<BEMailTemplate> GetMailTemplateAll(bool IsActive, bool isAutoReply)
        {
            var result = _mailConfigurationService.GetMailTemplateListAsync(IsActive, isAutoReply).GetAwaiter().GetResult();
            return result.data != null ? result.data.ToList() : new List<BEMailTemplate>();
        }
        public IList<BEMailTemplate> GetMailTemplateList(int campaignId)
        {
            var result = _mailConfigurationService.GetMailTemplateListAsync(campaignId).GetAwaiter().GetResult();
            return result.data != null ? result.data.ToList() : new List<BEMailTemplate>();
        }
        public BEMailTemplate GetMailTemplateDataList(int iEmailTemplateID)
        {
            var result = _mailConfigurationService.GetMailTemplateByIdAsync(iEmailTemplateID).GetAwaiter().GetResult();
            return result.data ?? new BEMailTemplate();
        }
        public void InsertData(MailTemplateModel oEmailManagement, int iFormID)
        {
            var result = _mailConfigurationService.AddMailTemplateAsync(oEmailManagement, iFormID).GetAwaiter().GetResult();

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
                else if (result.message.ToLower().Contains(BPA.GlobalResources.DataLayer.Resources.msg_AssessmentAlready.ToLower()))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_MailConfigAlreadyExist);
                }
                throw new Exception(result.message);
            }
        }
        public void UpdateData(MailTemplateModel oEmailManagement, int iFormID)
        {
            var result = _mailConfigurationService.UpdateMailTemplateAsync(oEmailManagement, iFormID).GetAwaiter().GetResult();
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
                else if (result.message.ToLower().Contains(BPA.GlobalResources.DataLayer.Resources.msg_AssessmentAlready.ToLower()))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_MailConfigAlreadyExist);
                }
                throw new Exception(result.message);
            }
        }
    }
}
