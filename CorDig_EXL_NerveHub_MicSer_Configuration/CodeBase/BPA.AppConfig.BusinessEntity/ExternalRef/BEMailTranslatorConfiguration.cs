using BPA.AppConfig.BusinessEntity.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.ExternalRef
{
    [Serializable]
    [DataContract]
    public class BEMailTranslatorConfiguration
    {
        [DataMember]
        public int iUserID { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public string Translateddocumentfolder { get; set; }

        [DataMember]
        public string Completedfolder { get; set; }
        [DataMember]
        public string Intakefolder { get; set; }
        [DataMember]
        public string Exceptionfolder { get; set; }
        [DataMember]
        public BatchFrequencyType LangBatchFrequency { get; set; }
        [DataMember]
        public string BatchName { get; set; }

        [DataMember]
        public string LanguageConfigName { get; set; }

        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public int ProcessId { get; set; }
        [DataMember]
        public int CampaignId { get; set; }

        [DataMember]
        public int LanguageConfigID { get; set; }

        [DataMember]
        public bool bSWMWorkUpload { get; set; }

        [DataMember]
        public bool IncomingMail { get; set; }

        [DataMember]
        public bool IsApproved { get; set; }

        [DataMember]
        public int LngID { get; set; }

        [DataMember]
        public int ApprovedBy { get; set; }

        [DataMember]
        public bool IsRejectedTech { get; set; }

        [DataMember]
        public string SMEChangesText { get; set; }

        [DataMember]
        public string SystranText { get; set; }

        [DataMember]
        public string SpanishText { get; set; }

        [DataMember]
        public string AzureKey { get; set; }

        [DataMember]
        public string CategoryID { get; set; }

        [DataMember]
        public int ProviderID { get; set; }

        [DataMember]
        public string AppId { get; set; }

        [DataMember]
        public string EndPointAddress { get; set; }

        [DataMember]
        public string UseAdvancedSettings { get; set; }

        [DataMember]
        public string Adv_CategoryId { get; set; }

        [DataMember]
        public string UseAzureGovernment { get; set; }

        [DataMember]
        public string Language { get; set; }

        [DataMember]
        public string SystranKey { get; set; }

        [DataMember]
        public int DStoreID { get; set; }

        [DataMember]
        public bool Disabled { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public string Target { get; set; }

        [DataMember]
        public string Source { get; set; }

        [DataMember]
        public bool IgnoreHidden { get; set; }

        [DataMember]
        public LanguageProvider iLanguageProviderTypeID { get; set; }

        [DataMember]
        public string ApiKey { get; set; }
        [DataMember]
        public string ApiUrl { get; set; }

        [DataMember]
        public string BatchId { get; set; }

        [DataMember]
        public string Callback { get; set; }

        [DataMember]
        public string RequestId { get; set; }

        [DataMember]
        public List<int?> Id { get; set; }

        [DataMember]
        public List<string> SourceLanguage { get; set; }

        [DataMember]
        public List<string> TargetLanguage { get; set; }
        [DataMember]
        public string Format { get; set; }
        //[DataMember]
        //public int? Profile { get; set; }


        [DataMember]
        public string ProfileID { get; set; }


        [DataMember]
        public bool? WithSource { get; set; }

        [DataMember]
        public bool? WithAnnotations { get; set; }
        [DataMember]
        public string WithDictionary { get; set; }
        [DataMember]
        public string WithCorpus { get; set; }

        [DataMember]
        public List<string> Options { get; set; }

        [DataMember]
        public string Encoding { get; set; }
        [DataMember]
        public bool? Async { get; set; }
        [DataMember]
        public bool? BackTranslation { get; set; }
        [DataMember]
        public List<string> Input { get; set; }

        [DataMember]
        public string InputData { get; set; }
        [DataMember]
        public int Retrycount { get; set; }

        [DataMember]
        public string ContentType { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public bool BoolUseAzureGovernment { get; set; }

        [DataMember]
        public int icurrentTimer { get; set; }

        [DataMember]
        public int ScheduleInterval { get; set; }
        [DataMember]
        public BETenant oTenant { get; set; }
        [DataMember]
        public bool isRunning { get; set; }

        [DataMember]
        public int ExceptionCount { get; set; }

        [DataMember]
        public bool CreateYearFolder { get; set; }
    }
}
