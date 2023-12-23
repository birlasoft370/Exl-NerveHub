using System.Text.Json.Serialization;

namespace BPA.EmailConfiguration
{
    public class EmailAdvancedConfigurationModel
    {
        //public int KEY { get; set; } = 2;
        public int CampaignID { get; set; }
        [JsonPropertyName("SendMailUniqueTrackIdIdentified")]
        public bool Sendmailuniqueidentified { get; set; } = false;
        public bool Scheduletosameuser { get; set; } = false;
        public bool Disabled { get; set; } = false;
        public int BatchFrequency { get; set; } = 0;
        public int TimeZoneID { get; set; } = 0;
        public bool InlineEditing { get; set; } = false;
        public bool NeedeFile { get; set; } = false;
        public bool NeedPrint { get; set; } = false;
        public bool ReadMailBody { get; set; } = false;
        public bool CFX { get; set; } = false;
        public bool DuringUpload { get; set; } = false;
        public int NeedTicketLength { get; set; } = 0;
        public bool IsFreshRequired { get; set; } = false;
        public string? CEXlauncherPath { get; set; } = "";
        public bool IsSensitivity { get; set; } = false;
        public string? TicketName { get; set; } = "";
        public bool NeedTicket { get; set; } = false;
        public int UploadBy { get; set; } = 0;
        public string? LastAssignType { get; set; } = "";
        public bool IsAssignLast { get; set; } = false;
        public string EFilePath { get; set; } = "";
        public bool IsSubmitDisplay { get; set; } = false;
        public string? SubmitDisplay { get; set; } = "";
        public bool OutLookMailEnabled { get; set; } = false;
    }
}
