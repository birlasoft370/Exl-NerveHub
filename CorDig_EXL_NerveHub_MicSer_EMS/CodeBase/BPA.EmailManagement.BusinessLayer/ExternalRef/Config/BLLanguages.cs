using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.DataLayer.ExternalRef.Config;
using BPA.EmailManagement.ServiceContract.ExternalRef.Config;
using BPA.Translator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.EmailManagement.BusinessLayer.ExternalRef.Config
{
    public class BLLanguages : ILanguagesService, IDisposable
    {
        public void Dispose()
        { }

        public BLLanguages()
        { }

        public IList<BEMailTranslatorConfiguration> GetLanguageProfile(int iCampaignID, BETenant oTenant)
        {
            using (DLLanguages objLanguages = new DLLanguages(oTenant))
            {
                return objLanguages.GetLanguageProfile(iCampaignID);
            }
        }

        public IList<BEMailTranslatorConfiguration> GetCampaignWiseLangList(int iStoreID, BETenant oTenant, int iFormId = 1)
        {
            using (DLLanguages objLanguages = new DLLanguages(oTenant))
            {
                return objLanguages.GetCampaignWiseLangList(iStoreID, false, iFormId);
            }
        }

        public IList<BEMailTranslatorConfiguration> GetEMSLanguageConfigAllData(int langConfigID, BETenant oTenant)
        {
            using (DLLanguages objLanguages = new DLLanguages(oTenant))
            {
                return objLanguages.GetEMSLanguageConfigAllData(langConfigID, oTenant);
            }
        }
        public void InsertUpdateData(BEMailTranslatorConfiguration oBEMailTranslatorConfiguration, int iFormID, BETenant oTenant)
        {
            using (DLLanguages objLanguages = new DLLanguages(oTenant))
            {
                objLanguages.InsertUpdateData(oBEMailTranslatorConfiguration, iFormID, oTenant);
            }
        }

        public DataTable GetIncomingTranslationData(int CampaignID, int LanguageConfigID, BETenant oTenant)
        {
            using (DLLanguages objLanguages = new DLLanguages(oTenant))
            {
                return objLanguages.GetIncomingTranslationData(CampaignID, LanguageConfigID);
            }
        }

        public void InsertIncomingTranslationData(BEMailTranslatorConfiguration oBEMailTranslatorConfiguration, BETenant oTenant)
        {
            using (DLLanguages objLanguages = new DLLanguages(oTenant))
            {
                objLanguages.InsertIncomingTranslationData(oBEMailTranslatorConfiguration);
            }
        }
    }
}
