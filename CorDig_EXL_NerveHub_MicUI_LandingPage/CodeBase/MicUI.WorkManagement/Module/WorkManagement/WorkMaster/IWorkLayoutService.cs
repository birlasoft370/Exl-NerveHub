using MicUI.WorkManagement.Helper;
using MicUI.WorkManagement.Services.ServiceModel;
using MicUI.WorkManagement.Services.WorkManagement;

namespace MicUI.WorkManagement.Module.WorkManagement.WorkMaster
{
    public interface IWorkLayoutService
    {
        void UpdateData(List<UpdatePreViewDataModel> objLayout, int iFormID);
    }
    public class WorkLayoutService : IWorkLayoutService
    {
        private readonly IWorkManagementApiService _workManagementApiService;
        public WorkLayoutService(IWorkManagementApiService workManagementApiService)
        {
            _workManagementApiService = workManagementApiService;
        }

        public void UpdateData(List<UpdatePreViewDataModel> objLayout, int iFormID)
        {
            var result = _workManagementApiService.UpdateDataAsync(objLayout, iFormID).GetAwaiter().GetResult();

            if (result != null && result.message != null)
            {
                if (result.message.ToLower().Contains(GlobalConstant.ExNumber547NullInsert) || result.message.ToLower().Contains(GlobalConstant.ExNumber547ReferenceKeyconstraint))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else if (result.message.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                else
                {
                    throw new Exception(result.message);
                }
            }
        }
    }
}
