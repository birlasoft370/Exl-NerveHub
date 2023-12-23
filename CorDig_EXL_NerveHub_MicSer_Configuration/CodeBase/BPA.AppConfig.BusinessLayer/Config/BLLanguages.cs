using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.BusinessEntity.ExternalRef;
using BPA.AppConfig.Datalayer.Config;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessLayer.Config
{
    public class BLLanguages : ILanguagesService, IDisposable
    {
        public void Dispose()
        { }

        public BLLanguages()
        { }

        public IList<BELanguages> GetLanguageList(bool IsActiveLanguage, BETenant oTenant)
        {
            return GetLanguageList("", IsActiveLanguage, oTenant);
        }
        public IList<BELanguages> GetLanguageList(string LanguageName, bool IsActiveLanguage, BETenant oTenant)
        {
            using (DLLanguages objLanguages = new DLLanguages(oTenant))
            {
                return objLanguages.GetLanguageList(LanguageName, IsActiveLanguage);
            }
        }
        public void InsertUpdateData(BEMailTranslatorConfiguration oBEMailTranslatorConfiguration, int iFormID, BETenant oTenant)
        {
            using (DLLanguages objLanguages = new DLLanguages(oTenant))
            {
                objLanguages.InsertUpdateData(oBEMailTranslatorConfiguration, iFormID, oTenant);
            }
        }
        public IList<BEMailTranslatorConfiguration> GetCampaignWiseLangList(int iStoreID, BETenant oTenant, int iFormId = 1)
        {
            using (DLLanguages objLanguages = new DLLanguages(oTenant))
            {
                return objLanguages.GetCampaignWiseLangList(iStoreID, false, iFormId);
            }
        }

        public IList<BEMailTranslatorConfiguration> GetLanguageConfigAllData(int langConfigID, BETenant oTenant)
        {
            using (DLLanguages objLanguages = new DLLanguages(oTenant))
            {
                return objLanguages.GetLanguageConfigAllData(langConfigID, oTenant);
            }
        }

        public IList<BEMailTranslatorConfiguration> GetEMSLanguageConfigAllData(int langConfigID, BETenant oTenant)
        {
            using (DLLanguages objLanguages = new DLLanguages(oTenant))
            {
                return objLanguages.GetEMSLanguageConfigAllData(langConfigID, oTenant);
            }
        }

        public void InsertIncomingTranslationData(BEMailTranslatorConfiguration oBEMailTranslatorConfiguration, BETenant oTenant)
        {
            using (DLLanguages objLanguages = new DLLanguages(oTenant))
            {
                objLanguages.InsertIncomingTranslationData(oBEMailTranslatorConfiguration);
            }
        }

        public DataTable GetIncomingTranslationData(int CampaignID, int LanguageConfigID, BETenant oTenant)
        {
            using (DLLanguages objLanguages = new DLLanguages(oTenant))
            {
                return objLanguages.GetIncomingTranslationData(CampaignID, LanguageConfigID);
            }
        }

        public IList<BEMailTranslatorConfiguration> GetLanguageProfile(int iCampaignID, BETenant oTenant)
        {
            using (DLLanguages objLanguages = new DLLanguages(oTenant))
            {
                return objLanguages.GetLanguageProfile(iCampaignID);
            }
        }
    }
}
