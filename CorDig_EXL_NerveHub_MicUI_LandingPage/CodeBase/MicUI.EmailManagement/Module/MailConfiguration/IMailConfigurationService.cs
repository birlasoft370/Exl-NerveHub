using MicUI.EmailManagement.Helper;
using MicUI.EmailManagement.Models.Request;
using MicUI.EmailManagement.Models.ViewModels;
using MicUI.EmailManagement.Services.MailConfiguration;
using MicUI.EmailManagement.Services.ServiceModel;

namespace MicUI.EmailManagement.Module.MailConfiguration
{
    public interface IMailConfigurationService
    {
        IList<BEMailConfiguration> GetCampaignWiseList(int iStoreID);
        IList<BEMailConfiguration> GetMailConfigAllData(int MailConfigID);
        IEnumerable<TreeStructure> GetMailFolderList(TestConnectionModel objMailConfiguration);
        IEnumerable<TreeStructure> GetMailSubFolderList(TestConnectionModel objMailConfiguration);
        void InsertData(EmailConfigurationModel oBEMailConfiguration, int iFormID);
        void UpdateData(EmailConfigurationModel oBEMailConfiguration, int iFormID);
        void InsertUpdateAdvancedConfiguration(EmailAdvancedConfigurationModel MailConfiguration);
        IList<BEMailConfiguration> GetAdvancedConfiguration(int iCampaignID);
    }

    public class MailConfigurationService : IMailConfigurationService
    {
        private readonly IMailConfigurationApiService _mailConfigurationService;

        public MailConfigurationService(IMailConfigurationApiService mailConfigurationService)
        {
            _mailConfigurationService = mailConfigurationService;
        }

        public IList<BEMailConfiguration> GetCampaignWiseList(int iStoreID)
        {
            var result = _mailConfigurationService.GetCampaignWiseListAsync(iStoreID).GetAwaiter().GetResult();
            if (result != null && result.data != null)
            {
                return result.data.ToList();
            }
            return new List<BEMailConfiguration>();
        }
        public IList<BEMailConfiguration> GetMailConfigAllData(int MailConfigID)
        {
            var result = _mailConfigurationService.GetMailConfigAllDataAsync(MailConfigID).GetAwaiter().GetResult();
            if (result != null && result.data != null)
            {
                return result.data.ToList();
            }
            return new List<BEMailConfiguration>();
        }
        public IEnumerable<TreeStructure> GetMailFolderList(TestConnectionModel objMailConfiguration)
        {
            var result = _mailConfigurationService.TestConnectionAsync(objMailConfiguration).GetAwaiter().GetResult();
            if (result != null && result.data != null)
            {
                return result.data.ToList();
            }
            return Enumerable.Empty<TreeStructure>();
        }
        public IEnumerable<TreeStructure> GetMailSubFolderList(TestConnectionModel objMailConfiguration)
        {
            var result = _mailConfigurationService.GetNodeValueAsync(objMailConfiguration).GetAwaiter().GetResult();
            if (result != null && result.data != null)
            {
                return result.data.ToList();
            }
            return Enumerable.Empty<TreeStructure>();
        }
        public void InsertData(EmailConfigurationModel oBEMailConfiguration, int iFormID)
        {
            var result = _mailConfigurationService.InsertMailConfigurationAsync(oBEMailConfiguration, iFormID).GetAwaiter().GetResult();

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
        public void UpdateData(EmailConfigurationModel oBEMailConfiguration, int iFormID)
        {
            var result = _mailConfigurationService.UpdateMailConfigurationAsync(oBEMailConfiguration, iFormID).GetAwaiter().GetResult();

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
        public void InsertUpdateAdvancedConfiguration(EmailAdvancedConfigurationModel MailConfiguration)
        {
            //var jsonstr = JsonConvert.SerializeObject(MailConfiguration);
            var result = _mailConfigurationService.InsertUpdateAdvancedConfigurationAsync(MailConfiguration).GetAwaiter().GetResult();

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
                throw new Exception(result.message);
            }
        }
        public IList<BEMailConfiguration> GetAdvancedConfiguration(int iCampaignID)
        {
            var result = _mailConfigurationService.GetAdvancedConfigurationAsync(iCampaignID).GetAwaiter().GetResult();
            if (result != null && result.data != null)
            {
                return result.data.ToList();
            }
            return new List<BEMailConfiguration>();
        }
    }
}
