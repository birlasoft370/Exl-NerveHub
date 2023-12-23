using System.ComponentModel.DataAnnotations;

namespace MicUI.EmailManagement.Models.Request
{
    public class EMSLanguageConfigModel
    {
        public int CampaignId { get; set; }
        public int LanguageConfigID { get; set; }
        public string LanguageConfigName { get; set; }
        public string? ApiKey { get; set; }
        public string? ApiUrl { get; set; }
        public string? BatchId { get; set; }
        public string? Source { get; set; }
        public string? Target { get; set; }
        public string? ProfileID { get; set; }
        public bool? WithSource { get; set; }
        public string? WithDictionary { get; set; }
        public string? WithCorpus { get; set; }
        public bool? WithAnnotations { get; set; }
        public bool? BackTranslation { get; set; }
        public bool? Async { get; set; }
        public string? Callback { get; set; }
        public string? Encoding { get; set; }
        public List<string>? Options { get; set; } = null;
        public bool Disabled { get; set; }
        public bool IncomingMail { get; set; }
    }

    [Serializable]
    public class LanguageApprovalRequestModel
    {
        public int LngID { get; set; }
        public string? ProfileID { get; set; }
        public string? TranslatedText { get; set; }
        public string? OriginalText { get; set; }
        [DataType(DataType.MultilineText)]
        public string? SMEChangesText { get; set; }
        public int CreatedBy { get; set; }
        public int CampaignID { get; set; }
        public bool Disabled { get; set; }
        public string? StatusToShowHideButtons { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejectedTech { get; set; }
        
    }
}
