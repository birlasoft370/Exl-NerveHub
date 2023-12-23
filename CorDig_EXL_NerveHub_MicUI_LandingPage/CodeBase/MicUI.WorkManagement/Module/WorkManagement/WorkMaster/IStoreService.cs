using MicUI.WorkManagement.Helper;
using MicUI.WorkManagement.Services.ServiceModel;
using MicUI.WorkManagement.Services.WorkManagement;
using Newtonsoft.Json;
using System.Data;

namespace MicUI.WorkManagement.Module.WorkManagement.WorkMaster
{
    public interface IStoreService
    {
        List<BEStoreInfo> GetStoreList(int iCampaignId, string sStoreName, bool bActiveStore);
        List<BEStoreInfo> GetStoreListByStoreId(int iStoreID);
        void InsertDataXML(WorkDefinitionModel oStore, int iFormID, bool isNewStore);
        List<GetGridObject_TempModel> GetGridObject_Temp(string ProcessID, string CamID, string StoreID);
        List<BECampaignClientProcess> GetClientProcessList(int iCampID);
        List<BEWorkObjectTAB> GetTabMasterList(int iStoreID);
        List<GetGridObjectNameModel> GetGridObjectName(int StoreID);
        List<BEWorkObjectApprover> GetObjectApprovalList(int StoreID);
        void ApprovalAction(int approvalId, string action, int formId);
        void InsertGridConfiguration_Temp(GridConfigurationModel gridConfiguration, int iStoreID);
        DataSet GetGridObjectcONTROL_Temp(string ProcessID, string CamID, string GridObjectName, string iStoreID);
        bool UpdateGridTableMain(string StoreId, bool Disabled, GridConfigurationModel gridConfiguration);
        List<BEStoreInfo> GetStoreObjectList(int iCampaignId, string sStoreName, bool bActiveStore, int UserId);
    }

    public class StoreService : IStoreService
    {
        private readonly IWorkManagementApiService _workManagementApiService;

        public StoreService(IWorkManagementApiService workManagementApiService)
        {
            _workManagementApiService = workManagementApiService;
        }

        public List<BEStoreInfo> GetStoreList(int iCampaignId, string sStoreName, bool bActiveStore)
        {
            var result = _workManagementApiService.GetStoreListByCampIdAsync(iCampaignId, sStoreName, bActiveStore).GetAwaiter().GetResult();
            return result.data ?? new List<BEStoreInfo>();
        }
        public List<BEStoreInfo> GetStoreListByStoreId(int iStoreID)
        {
            var result = _workManagementApiService.GetStoreListByStoreIDAsync(iStoreID).GetAwaiter().GetResult();
            return result.data ?? new List<BEStoreInfo>();
        }
        public void InsertDataXML(WorkDefinitionModel oStore, int iFormID, bool isNewStore)
        {
            var result = _workManagementApiService.SaveWorkDataAsync(oStore, iFormID, isNewStore).GetAwaiter().GetResult();

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
                else if (result.message.ToLower().Contains(BPA.GlobalResources.DataLayer.Resources.msg_ObjectNameAlready.ToLower()))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_ObjectNameAlready);
                }
                else if (result.message.ToLower().Contains(BPA.GlobalResources.DataLayer.Resources.msg_WorkDefinitionAlready.ToLower()))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_WorkDefinitionAlready);
                }
                else if (!result.data)
                {
                    throw new ApplicationException("Store Definition for this Campaign already exist!");
                }
                else
                {
                    throw new Exception(result.message);
                }
            }
        }
        public List<GetGridObject_TempModel> GetGridObject_Temp(string ProcessID, string CamID, string StoreID)
        {
            var result = _workManagementApiService.GetGridObject_TempAsync(ProcessID, CamID, StoreID).GetAwaiter().GetResult();
            return result.data ?? new List<GetGridObject_TempModel>();
        }
        public List<BECampaignClientProcess> GetClientProcessList(int iCampID)
        {
            var result = _workManagementApiService.GetClientProcessListAsync(iCampID).GetAwaiter().GetResult();
            return result.data ?? new List<BECampaignClientProcess>();
        }
        public List<BEWorkObjectTAB> GetTabMasterList(int iStoreID)
        {
            var result = _workManagementApiService.GetTabMasterListAsync(iStoreID).GetAwaiter().GetResult();
            return result.data ?? new List<BEWorkObjectTAB>();
        }
        public List<GetGridObjectNameModel> GetGridObjectName(int StoreID)
        {
            var result = _workManagementApiService.GetGridObjectNameAsync(StoreID).GetAwaiter().GetResult();
            return result.data ?? new List<GetGridObjectNameModel>();
        }
        public List<BEWorkObjectApprover> GetObjectApprovalList(int StoreID)
        {
            var result = _workManagementApiService.GetObjectApprovalListAsync(StoreID).GetAwaiter().GetResult();
            return result.data ?? new List<BEWorkObjectApprover>();
        }
        public void ApprovalAction(int approvalId, string action, int formId)
        {
            _workManagementApiService.ApprovalActionAsync(approvalId, action, formId).GetAwaiter().GetResult();
        }
        public void InsertGridConfiguration_Temp(GridConfigurationModel gridConfiguration, int iStoreID)
        {
            var result = _workManagementApiService.InsertGridConfiguration_TempAsync(gridConfiguration, iStoreID).GetAwaiter().GetResult();

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
                else if (result.message.ToLower().Contains($"Store Definition for this Grid Object Name " + gridConfiguration.GridObject.GridObjectName + "  already exist!".ToLower()))
                {
                    throw new Exception($"Store Definition for this Grid Object Name " + gridConfiguration.GridObject.GridObjectName + "  already exist!");
                }
                else
                {
                    throw new Exception(result.message);
                }
            }
        }
        public DataSet GetGridObjectcONTROL_Temp(string ProcessID, string CamID, string GridObjectName, string iStoreID)
        {
            var result = _workManagementApiService.GetGridObjectcONTROL_TempAsync(ProcessID, CamID, GridObjectName, iStoreID).GetAwaiter().GetResult();
            if (result != null && result.data != null)
            {
                return JsonConvert.DeserializeObject<DataSet>(result.data) ?? new DataSet();
            }
            return new DataSet();
        }
        public bool UpdateGridTableMain(string StoreId, bool Disabled, GridConfigurationModel gridConfiguration)
        {
            var result = _workManagementApiService.UpdateGridTableMainAsync(StoreId, Disabled, gridConfiguration).GetAwaiter().GetResult();
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
            return true;
        }

        public List<BEStoreInfo> GetStoreObjectList(int iCampaignId, string sStoreName, bool bActiveStore, int UserId)
        {
            var result = _workManagementApiService.GetStoreObjectListAsync(iCampaignId, sStoreName, bActiveStore, UserId).GetAwaiter().GetResult();
            return result.data ?? new List<BEStoreInfo>();
        }
    }
}
