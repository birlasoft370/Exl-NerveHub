using BPA.AppConfig.BusinessEntity.Config;
using System.ComponentModel.DataAnnotations;

namespace BPA.AppConfiguration.Models.Response
{
    public class UserSkillMapModel
    {
        public UserSkillMapModel()
        {
            UserSkillList = new();
        }
        public int CampaignId { get; set; }
        public List<UserSkill> UserSkillList { get; set; }
    }

    public class UserSkill
    {
        public UserSkill()
        {
            SkillInfoList = new();
        }
        //[UIHint("Skills")]
        //public string Skills { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }

        public List<UserSkillInfo> SkillInfoList { get; set; }
        // public List<BESkillInfo> BESkillInfoList { get; set; }//not used due to sDescription field validation Issue
    }

    public class UserSkillInfo
    {
        public int SkillID { get; set; }
        public string SkillName { get; set; }
    }
}
