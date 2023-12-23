using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.BusinessEntity.ExternalRef;
using BPA.AppConfig.ServiceContracts.ServiceContracts.FaultContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.Config
{
    [ServiceContract(Name = "LanguagesServiceContract")]
    public interface ILanguagesService : IDisposable
    {
        [OperationContract(Name = "InsertUpdateData")]
        [FaultContract(typeof(ServiceFault))]
        void InsertUpdateData(BEMailTranslatorConfiguration oBEMailTranslatorConfiguration, int iFormID, BETenant oTenant);
        
        [OperationContract(Name = "GetCampaignWiseLangList")]
        [FaultContract(typeof(ServiceFault))]
        IList<BEMailTranslatorConfiguration> GetCampaignWiseLangList(int iStoreID, BETenant oTenant, int iFormId = 1);
        
        [OperationContract(Name = "GetLanguageConfigAllData")]
        [FaultContract(typeof(ServiceFault))]
        IList<BEMailTranslatorConfiguration> GetLanguageConfigAllData(int langConfigID, BETenant oTenant);

        [OperationContract(Name = "GetEMSLanguageConfigAllData")]
        [FaultContract(typeof(ServiceFault))]
        IList<BEMailTranslatorConfiguration> GetEMSLanguageConfigAllData(int langConfigID, BETenant oTenant);
        
        [OperationContract(Name = "InsertIncomingTranslationData")]
        [FaultContract(typeof(ServiceFault))]
        void InsertIncomingTranslationData(BEMailTranslatorConfiguration oBEMailTranslatorConfiguration, BETenant oTenant);

        [OperationContract(Name = "GetIncomingTranslationData")]
        [FaultContract(typeof(ServiceFault))]
        DataTable GetIncomingTranslationData(int iCampaignID, int LanguageConfigID, BETenant oTenant);
        
        [OperationContract(Name = "GetLanguageProfile")]
        [FaultContract(typeof(ServiceFault))]
        IList<BEMailTranslatorConfiguration> GetLanguageProfile(int iCampaignID, BETenant oTenant);

        [OperationContract(Name = "GetLanguageList")]
        [FaultContract(typeof(ServiceFault))]
        IList<BELanguages> GetLanguageList(bool IsActiveLanguages, BETenant oTenant);

    }
}
