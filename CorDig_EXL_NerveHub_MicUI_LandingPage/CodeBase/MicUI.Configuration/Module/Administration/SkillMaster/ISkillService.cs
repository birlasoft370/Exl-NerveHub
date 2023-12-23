using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Administration.SkillMaster
{
    public interface ISkillService
    {
        string InsertData(SkillMasterModel SkillModel);
        string UpdateData(SkillMasterModel SkillModel);
        IList<SkillMasterModel> GetSkillListByName(string sSkillName);
        SkillMasterModel GetSkillById(int sSkillId);
    }
}
