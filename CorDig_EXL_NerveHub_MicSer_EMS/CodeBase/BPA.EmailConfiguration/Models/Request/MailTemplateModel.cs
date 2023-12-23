namespace BPA.EmailConfiguration.Models.Request
{
    public class MailTemplateModel
    {
        public int ClientID { get; set; }
        public int ProcessID { get; set; }
        public int CampaignId { get; set; }
        public int MailTemplateId { get; set; }
        public string? MailTemplate { get; set; }
        public bool? Disabled { get; set; }
        public bool? IsAutoReply { get; set; }
        public string? MailTemplateName { get; set; }
    }
}
