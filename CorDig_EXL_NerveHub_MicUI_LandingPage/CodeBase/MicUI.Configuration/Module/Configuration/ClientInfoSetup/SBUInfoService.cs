using MicUI.Configuration.Services.Configuration;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Configuration.ClientInfoSetup
{
    public class SBUInfoService : ISBUInfoService
    {
        private readonly IConfigApiService _configService;

        public SBUInfoService(IConfigApiService configService)
        {
            _configService = configService;
        }
        public List<BESBUInfo> GetSBUListbasedONClient(int iclientId)
        {
            var result = _configService.GetSBUListbasedONClientAsync(iclientId).GetAwaiter().GetResult();
            return result.data.ToList();
        }
        public List<BESBUInfo> GetSBUList(bool isActive, string sbuName = null)
        {
            var result = _configService.GetSBUListAsync(sbuName, isActive).GetAwaiter().GetResult();
            return result.data.ToList();
        }
        public List<BESBUInfo> GetSBUList(string sbuName)
        {
            var result = _configService.GetSBUListAsync(sbuName).GetAwaiter().GetResult();
            return result.data;
        }
        public string UpdateData(SBUModel sBUModelModel)
        {
            var result = _configService.UpdateSBUAsync(sBUModelModel).GetAwaiter().GetResult();
            return result.data;
        }

        public string InsertData(SBUModel sBUModelModel)
        {
            var result = _configService.AddSBUAsync(sBUModelModel).GetAwaiter().GetResult();
            return result.data;
        }
        public SBUModel GetSBUById(int sbuId)
        {
            var result = _configService.GetSBUByIdAsync(sbuId).GetAwaiter().GetResult();
            return result.data;

        }
    }

}

