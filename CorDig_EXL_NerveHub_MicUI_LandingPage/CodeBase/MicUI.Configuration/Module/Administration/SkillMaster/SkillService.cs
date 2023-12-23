using MicUI.Configuration.Helper;
using MicUI.Configuration.Helper.Sessions;
using MicUI.Configuration.Services.Configuration;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Administration.SkillMaster
{
    public class SkillService : ISkillService
    {
        private readonly IConfigApiService _configService;
    
        public SkillService(IConfigApiService configService)
        {
            _configService = configService;
        }
        public string InsertData(SkillMasterModel SkillModel)
        {
            var result = _configService.InsertDataAsync(SkillModel).GetAwaiter().GetResult();
            if (result.message == null)
            {
                return result.data;
            }
            else
            {

                return result.data;
            }
        }
        public string UpdateData(SkillMasterModel SkillModel)
        {
            var result = _configService.UpdateDataAsync(SkillModel).GetAwaiter().GetResult();
            if (result.message == null)
            {
                return result.data;
            }
            else
            {

                return result.data;
            }
        }
        public IList<SkillMasterModel> GetSkillListByName(string sSkillName)
        {
            var result = _configService.GetSkillListAsync(sSkillName).GetAwaiter().GetResult();
            return result.data;
        }
        public SkillMasterModel GetSkillById(int sSkillId)
        {
            var result = _configService.GetSkillByIdAsync(sSkillId).GetAwaiter().GetResult();
            return result.data;
        }
    }
}