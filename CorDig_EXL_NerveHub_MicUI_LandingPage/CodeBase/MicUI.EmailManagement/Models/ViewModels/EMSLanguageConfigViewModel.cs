using BPA.GlobalResources.UI;
using MicUI.EmailManagement.Services.ServiceModel;
using System.ComponentModel.DataAnnotations;

namespace MicUI.EmailManagement.Models.ViewModels
{
    [Serializable]
    public class EMSLanguageConfigViewModel : SearchViewModel
    {
        public EMSLanguageConfigViewModel()
            : base(new string[] { Resources_common.display_Client, Resources_common.display_Process, Resources_common.display_Campaign_Dropdown, Resources_common.display_WorkObjName })
        { }

        public IList<BEMailTranslatorConfiguration> GridItems = new List<BEMailTranslatorConfiguration>();

        public IList<LanguageBind> lstLanguageBind = new List<LanguageBind>();



        public int CampaignId { get; set; }
        public int ClientId { get; set; }

        public bool CreateYearFolder { get; set; }
        public int ProcessId { get; set; }
        public string LanguageConfigName { get; set; }

        public int ScheduleInterval { get; set; }
        public int LanguageConfigID { get; set; }
        public string Translateddocumentfolder { get; set; }
        public string Completedfolder { get; set; }
        public string Intakefolder { get; set; }

        public string Exceptionfolder { get; set; }
        public string BatchFrequencyType { get; set; }

        public string BatchName { get; set; }
        public bool bSWMWorkUpload { get; set; }

        public bool IncomingMail { get; set; }

        public string AzureKey { get; set; }
        public string CategoryID { get; set; }

        public int ProviderID { get; set; }

        public string AppId { get; set; }


        public string EndPointAddress { get; set; }


        public string UseAdvancedSettings { get; set; }

        public string Adv_CategoryId { get; set; }

        public string UseAzureGovernment { get; set; }


        public string Language { get; set; }


        public string SystranKey { get; set; }


        public int DStoreID { get; set; }

        public bool Disabled { get; set; }


        public string FilePath { get; set; }


        public string Target { get; set; }


        public string Source { get; set; }


        public bool IgnoreHidden { get; set; }

        public string ApiKey { get; set; }

        public string ApiUrl { get; set; }


        public string BatchId { get; set; }


        public string Callback { get; set; }


        public string RequestId { get; set; }


        public List<int?> Id { get; set; }

        public List<string> SourceLanguage { get; set; }


        public List<string> TargetLanguage { get; set; }

        public string Format { get; set; }

        public int? Profile { get; set; }



        public string ProfileID { get; set; }



        public bool? WithSource { get; set; }


        public bool? WithAnnotations { get; set; }

        public string WithDictionary { get; set; }

        public string WithCorpus { get; set; }


        public List<string> Options { get; set; }


        public string Encoding { get; set; }

        public bool? Async { get; set; }

        public bool? BackTranslation { get; set; }

        public List<string> Input { get; set; }

        public string InputData { get; set; }

        public int Retrycount { get; set; }


        public string ContentType { get; set; }

        public string Category { get; set; }

        public bool BoolUseAzureGovernment { get; set; }

    }
    [Serializable]
    public class GridItems
    {
        public GridItems()
        {

        }
        public int LanguageConfigID { get; set; }
        public string Name { get; set; }
        public int ScheduleInterval { get; set; }
        public bool? Disable { get; set; }
    }
    [Serializable]
    public class LanguageBind
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
    [Serializable]
    public class LanguageApproval
    {

        #region Fields
        public int LanguageConfigID { get; set; }
        public string TranslatedText { get; set; }
        public string OriginalText { get; set; }

        [DataType(DataType.MultilineText)]
        public string SMEChangesText { get; set; }
        public int CreatedBy { get; set; }

        public string ProfileName { get; set; }
        public int LngID { get; set; }

        public int CampaignID { get; set; }

        //public string Status { get; set; }
        public string StatusToShowHideButtons { get; set; }
        //public string ChangeRequest { get; set; }

        public bool? IsChecked { get; set; }

        public BETenantInfo oTenant { get; set; }

        public string ClientNameSearch { get; set; }
        public string ProcessNameSearch { get; set; }
        public string CampaignNameSearch { get; set; }
        #endregion
    }
}
