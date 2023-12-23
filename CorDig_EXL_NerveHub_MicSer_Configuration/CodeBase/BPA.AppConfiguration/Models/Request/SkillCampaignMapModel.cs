namespace BPA.AppConfiguration.Models.Request
{
    public class SkillCampaignMapModel
    {
        public int CampaignId { get; set; }
        public List<int> Skills { get; set; }
        public int UserId { get; set; }
    }
}
