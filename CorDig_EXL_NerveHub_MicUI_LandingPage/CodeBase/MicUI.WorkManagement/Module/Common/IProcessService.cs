using Microsoft.AspNetCore.Mvc;
using MicUI.WorkManagement.Services.Configuration;
using MicUI.WorkManagement.Services.ServiceModel;

namespace MicUI.WorkManagement.Module.Common
{
    public interface IProcessService
    {
        List<BEProcessInfo> GetProcessList(int clientID, string processName, bool activeProcess);
    }
    public class ProcessService : IProcessService
    {
        private readonly IConfigApiService _configService;

        public ProcessService(IConfigApiService configService)
        {
            _configService = configService;
        }

        public List<BEProcessInfo> GetProcessList(int clientID, string processName, bool activeProcess)
        {
            var result = _configService.GetProcessListByClientIdAsync(clientID, processName, activeProcess).GetAwaiter().GetResult();
            return result.data ?? new List<BEProcessInfo>();
        }
    }
}
