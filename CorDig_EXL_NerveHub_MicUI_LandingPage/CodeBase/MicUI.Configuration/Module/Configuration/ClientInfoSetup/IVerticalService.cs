using MicUI.Configuration.Services.Configuration;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Configuration.ClientInfoSetup
{
    public interface IVerticalService
    {
        List<BEVerticalInfo> GetVerticalList();
        List<VerticalMasterModel> GetVerticalMasterList(string verticalMasterName);
        string UpdateData(VerticalMasterModel verticalMasterModel);
        string InsertData(VerticalMasterModel verticalMasterModel);
        VerticalMasterModel GetVerticalMasterById(int verticalId);
    }
    public class VerticalService : IVerticalService
    {
        private readonly IConfigApiService _configService;
        public VerticalService(IConfigApiService configService)
        {
            _configService = configService;
        }
        public List<BEVerticalInfo> GetVerticalList()
        {
            var result = _configService.GetVerticalListAsync().GetAwaiter().GetResult();
            return result.data.ToList();
        }
        public List<VerticalMasterModel> GetVerticalMasterList(string verticalMasterName)
        {
            var result = _configService.GetVerticalMasterListAsync(verticalMasterName).GetAwaiter().GetResult();
            return result.data;
        }
        public string UpdateData(VerticalMasterModel verticalMasterModel)
        {
            var result = _configService.UpdateVerticalMasterAsync(verticalMasterModel).GetAwaiter().GetResult();
            return result.data;
        }

        public string InsertData(VerticalMasterModel verticalMasterModel)
        {
            var result = _configService.AddVerticalMasterAsync(verticalMasterModel).GetAwaiter().GetResult();
            return result.data;
        }
        public VerticalMasterModel GetVerticalMasterById(int verticalId)
        {
            var result = _configService.GetVerticalMasterByIdAsync(verticalId).GetAwaiter().GetResult();
            return result.data;

        }
    }
}
