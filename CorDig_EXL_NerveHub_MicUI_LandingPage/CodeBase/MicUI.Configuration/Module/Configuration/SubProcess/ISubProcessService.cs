using MicUI.Configuration.Helper;
using MicUI.Configuration.Services.Configuration;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Configuration.SubProcess
{
    public interface ISubProcessService
    {
        BESubProcess GetSubProcessById(int iSubProcess);
        string InsertData(SubProcessMasterModel oSubprocess);
        string UpdateData(SubProcessMasterModel oSubprocess);
        List<BESubProcess> GetSubProcessList(int iProcessID, string sSubProcessName, bool bActiveSubProcess);
    }
    public class SubProcessService : ISubProcessService
    {
        private readonly IConfigApiService _configService;

        public SubProcessService(IConfigApiService configService)
        {
            _configService = configService;
        }
        public BESubProcess GetSubProcessById(int iSubProcess)
        {
            var result = _configService.GetSubProcessByIdAsync(iSubProcess).GetAwaiter().GetResult();
            return result.data ?? new BESubProcess();
        }

        public List<BESubProcess> GetSubProcessList(int iProcessID, string sSubProcessName, bool bActiveSubProcess)
        {
            var result = _configService.GetSubProcessListAsync(iProcessID, sSubProcessName, bActiveSubProcess).GetAwaiter().GetResult();
            return result.data ?? new List<BESubProcess>();
        }

        public string InsertData(SubProcessMasterModel oSubprocess)
        {
            var result = _configService.AddSubProcessAsync(oSubprocess).GetAwaiter().GetResult();
            if (!result.status)
            {
                if (result.message != null && result.message.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                else if (result.message != null && result.message.ToLower().Contains(GlobalConstant.ExNumber547NullInsert) || result.message.ToLower().Contains(GlobalConstant.ExNumber547ReferenceKeyconstraint))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else
                {
                    throw new Exception(result.message);
                }
            }
            return result.data ?? "";
        }

        public string UpdateData(SubProcessMasterModel oSubprocess)
        {
            var result = _configService.UpdateSubProcessAsync(oSubprocess).GetAwaiter().GetResult();
            if (!result.status)
            {
                if (result.message != null && result.message.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                else if (result.message != null && result.message.ToLower().Contains(GlobalConstant.ExNumber547NullInsert) || result.message.ToLower().Contains(GlobalConstant.ExNumber547ReferenceKeyconstraint))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else
                {
                    throw new Exception(result.message);
                }
            }
            return result.data ?? "";
        }
    }
}
