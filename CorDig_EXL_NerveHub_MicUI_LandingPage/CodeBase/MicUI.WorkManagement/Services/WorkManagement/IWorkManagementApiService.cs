using MicUI.WorkManagement.Models.Response;
using MicUI.WorkManagement.Services.ServiceModel;

namespace MicUI.WorkManagement.Services.WorkManagement
{
    public interface IWorkManagementApiService
    {
        Task<MessageResponse<List<BEWorkObject>>> GetObjectListAsync(int iStoreId, bool iGetAll);
        Task<MessageResponse<List<BEWorkObjectChoice>>> GetObjectChoiceListAsync(int iObjectId);
        Task<MessageResponse<List<BEValidations>>> GetValidationsAsync();
        Task<MessageResponse<List<BEControlTypeInfo>>> GetControlTypeListAsync(string sControlName, bool bGetAll);
        Task<MessageResponse<List<BEStoreInfo>>> GetStoreListByCampIdAsync(int campaignId, string storeName, bool activeStore);
        Task<MessageResponse<List<BEStoreInfo>>> GetStoreListByStoreIDAsync(int StoreID);
        Task<MessageResponse<List<BETechBuisness>>> GetBusinessApproverListAsync(int iProcessId);
        Task<MessageResponse<bool>> SaveWorkDataAsync(WorkDefinitionModel model, int iFormId, bool isNewStore);
        Task<MessageResponse<List<BEDocDetail>>> GetTemplateListAsync(bool isActive);
        Task<MessageResponse<List<GetGridObject_TempModel>>> GetGridObject_TempAsync(string ProcessID, string CamID, string StoreID);
        Task<MessageResponse<List<BECampaignClientProcess>>> GetClientProcessListAsync(int iCampId);
        Task<MessageResponse<List<BEWorkObjectTAB>>> GetTabMasterListAsync(int iStoreId);
        Task<MessageResponse<List<GetGridObjectNameModel>>> GetGridObjectNameAsync(int iStoreId);
        Task<MessageResponse<List<BEWorkObjectApprover>>> GetObjectApprovalListAsync(int iStoreId);
        Task<MessageResponse<List<GetPendingWorkObjectApprovalModel>>> GetPendingWorkObjectApprovalAsync(DateTime startDate, DateTime endDate);
        Task<MessageResponse<GetWorkObjRequestDetails>> FetchWorkObjRequestDetailsAsync(int iApprovalId);
        Task<MessageResponse<string>> InsertObjectRequestChangeAsync(int iApprovalId, int iUserLevel, string sChangeRequest);
        Task<MessageResponse<string>> UpdateObjectRequestChangeAsync(BEWorkObjectApprover oWorkObject);
        Task<MessageResponse<string>> ApprovalActionAsync(int approvalId, string action, int iFormId);
        Task<MessageResponse<List<BETerminationCodeInfo>>> GetTerminationCodeDirectAsync(int CampaignId);
        Task<MessageResponse<IList<BEObjectformula>>> GetWorkObjectFormulaSearchAsync(int iObjectId);
        Task<MessageResponse<List<GetWorkObjectForControlNameModel>>> GetWorkObjectForControlNameAsync(int iCampaignId);
        Task<MessageResponse<List<BEFormulaData>>> GetFilterList_FormulaAsync(int iCampaignId, string sFilterName, bool bActiveFilter);
        Task<MessageResponse<List<GetWorkObjectForFormulaModel>>> GetWorkObjectForFormulaAsync(int iCampaignID);
        Task<MessageResponse<bool>> GetExistObjFormulaAsync(int campaignID, int objID, string objEvent);
        Task<MessageResponse<string>> InsertObjectformulaAsync(ObjectformulaModel oObject);
        Task<MessageResponse<int>> DeleteObjectFormulaAsync(int iObjectId);
        Task<MessageResponse<string>> InsertGridConfiguration_TempAsync(GridConfigurationModel model, int storeId);
        Task<MessageResponse<string>> GetGridObjectcONTROL_TempAsync(string ProcessID, string CamID, string GridObjectName, string iStoreID);
        Task<MessageResponse<string>> GetGridConfigurationDataAsync(int iStoreID);
        Task<MessageResponse<string>> UpdateGridTableMainAsync(string StoreId, bool Disabled, GridConfigurationModel model);
        Task<MessageResponse<string>> UpdateDataAsync(List<UpdatePreViewDataModel> objLayout, int iFormID);
        Task<MessageResponse<List<BEStoreInfo>>> GetStoreObjectListAsync(int iCampaignId, string sStoreName, bool bActiveStore, int UserId);
        Task<MessageResponse<string>> GetAllLinkCampaignDataAsync(int iCampaignId);
        Task<MessageResponse<int>> GridInserUpdateDataAsync(LinkCampaignGridConfigurationModel objGridConfiguration, int userid, string Action);
    }

    public class WorkManagementApiService : BaseApiService, IWorkManagementApiService
    {
        public WorkManagementApiService(HttpClient client, IHttpContextAccessor httpContextAccessor) : base(client, httpContextAccessor)
        {

        }

        public async Task<MessageResponse<List<BEWorkObject>>> GetObjectListAsync(int iStoreId, bool iGetAll)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEWorkObject>>>($"WorkDefinition/GetObjectList?iStoreId={iStoreId}&iGetAll={iGetAll}");
            return content ?? new MessageResponse<List<BEWorkObject>>();
        }
        public async Task<MessageResponse<List<BEWorkObjectChoice>>> GetObjectChoiceListAsync(int iObjectId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEWorkObjectChoice>>>($"WorkDefinition/GetObjectChoiceList?iObjectId={iObjectId}");
            return content ?? new MessageResponse<List<BEWorkObjectChoice>>();
        }
        public async Task<MessageResponse<List<BEValidations>>> GetValidationsAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEValidations>>>("WorkDefinition/GetValidations");
            return content ?? new MessageResponse<List<BEValidations>>();
        }
        public async Task<MessageResponse<List<BEControlTypeInfo>>> GetControlTypeListAsync(string sControlName, bool bGetAll)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEControlTypeInfo>>>($"WorkDefinition/GetControlTypeList?sControlName={sControlName}&bGetAll={bGetAll}");
            return content ?? new MessageResponse<List<BEControlTypeInfo>>();
        }
        public async Task<MessageResponse<List<BEStoreInfo>>> GetStoreListByCampIdAsync(int campaignId, string storeName, bool activeStore)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEStoreInfo>>>($"WorkDefinition/GetStoreListByCampId?campaignId={campaignId}&storeName={storeName}&activeStore={activeStore}");
            return content ?? new MessageResponse<List<BEStoreInfo>>();
        }
        public async Task<MessageResponse<List<BEStoreInfo>>> GetStoreListByStoreIDAsync(int StoreID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEStoreInfo>>>($"WorkDefinition/GetStoreListByStoreID?StoreID={StoreID}");
            return content ?? new MessageResponse<List<BEStoreInfo>>();
        }
        public async Task<MessageResponse<List<BETechBuisness>>> GetBusinessApproverListAsync(int iProcessId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BETechBuisness>>>($"WorkDefinition/GetBusinessApproverList?iProcessId={iProcessId}");
            return content ?? new MessageResponse<List<BETechBuisness>>();
        }
        public async Task<MessageResponse<bool>> SaveWorkDataAsync(WorkDefinitionModel model, int iFormId, bool isNewStore)
        {
            var response = await _client.PostAsJsonAsync($"WorkDefinition/SaveWorkData?iFormId={iFormId}&isNewStore={isNewStore}", model);
            return await response.Content.ReadFromJsonAsync<MessageResponse<bool>>() ?? new MessageResponse<bool>();
        }
        public async Task<MessageResponse<List<BEDocDetail>>> GetTemplateListAsync(bool isActive)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEDocDetail>>>($"WorkDefinition/GetTemplateList?isActive={isActive}");
            return content ?? new MessageResponse<List<BEDocDetail>>();
        }
        public async Task<MessageResponse<List<GetGridObject_TempModel>>> GetGridObject_TempAsync(string ProcessID, string CamID, string StoreID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<GetGridObject_TempModel>>>($"WorkDefinition/GetGridObject_Temp?ProcessID={ProcessID}&CamID={CamID}&StoreID={StoreID}");
            return content ?? new MessageResponse<List<GetGridObject_TempModel>>();
        }
        public async Task<MessageResponse<List<BECampaignClientProcess>>> GetClientProcessListAsync(int iCampId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BECampaignClientProcess>>>($"WorkDefinition/GetClientProcessList?iCampId={iCampId}");
            return content ?? new MessageResponse<List<BECampaignClientProcess>>();
        }
        public async Task<MessageResponse<List<BEWorkObjectTAB>>> GetTabMasterListAsync(int iStoreId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEWorkObjectTAB>>>($"WorkDefinition/GetTabMasterList?iStoreId={iStoreId}");
            return content ?? new MessageResponse<List<BEWorkObjectTAB>>();
        }
        public async Task<MessageResponse<List<GetGridObjectNameModel>>> GetGridObjectNameAsync(int iStoreId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<GetGridObjectNameModel>>>($"WorkDefinition/GetGridObjectName?iStoreId={iStoreId}");
            return content ?? new MessageResponse<List<GetGridObjectNameModel>>();
        }
        public async Task<MessageResponse<List<BEWorkObjectApprover>>> GetObjectApprovalListAsync(int iStoreId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEWorkObjectApprover>>>($"WorkDefinition/GetObjectApprovalList?iStoreId={iStoreId}");
            return content ?? new MessageResponse<List<BEWorkObjectApprover>>();
        }
        public async Task<MessageResponse<List<GetPendingWorkObjectApprovalModel>>> GetPendingWorkObjectApprovalAsync(DateTime startDate, DateTime endDate)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<GetPendingWorkObjectApprovalModel>>>($"WorkDefinition/GetPendingWorkObjectApproval?startDate={startDate}&endDate={endDate}");
            return content ?? new MessageResponse<List<GetPendingWorkObjectApprovalModel>>();
        }
        public async Task<MessageResponse<GetWorkObjRequestDetails>> FetchWorkObjRequestDetailsAsync(int iApprovalId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<GetWorkObjRequestDetails>>($"WorkDefinition/FetchWorkObjRequestDetails?iApprovalId={iApprovalId}");
            return content ?? new MessageResponse<GetWorkObjRequestDetails>();
        }
        public async Task<MessageResponse<string>> InsertObjectRequestChangeAsync(int iApprovalId, int iUserLevel, string sChangeRequest)
        {
            var response = await _client.PostAsync($"WorkDefinition/InsertObjectRequestChange?iApprovalId={iApprovalId}&iUserLevel={iUserLevel}&sChangeRequest={sChangeRequest}", null);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<string>> UpdateObjectRequestChangeAsync(BEWorkObjectApprover oWorkObject)
        {
            var response = await _client.PutAsJsonAsync($"WorkDefinition/UpdateObjectRequestChange", oWorkObject);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<string>> ApprovalActionAsync(int approvalId, string action, int iFormId)
        {
            var response = await _client.PutAsync($"WorkDefinition/ApprovalAction?approvalId={approvalId}&action={action}&iFormId={iFormId}", null);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<List<BETerminationCodeInfo>>> GetTerminationCodeDirectAsync(int CampaignId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BETerminationCodeInfo>>>($"WorkDefinition/GetTerminationCodeDirect?CampaignId={CampaignId}");
            return content ?? new MessageResponse<List<BETerminationCodeInfo>>();
        }
        public async Task<MessageResponse<IList<BEObjectformula>>> GetWorkObjectFormulaSearchAsync(int iObjectId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<IList<BEObjectformula>>>($"WorkDefinition/GetWorkObjectFormulaSearch?iObjectId={iObjectId}");
            return content ?? new MessageResponse<IList<BEObjectformula>>();
        }
        public async Task<MessageResponse<List<GetWorkObjectForControlNameModel>>> GetWorkObjectForControlNameAsync(int iCampaignId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<GetWorkObjectForControlNameModel>>>($"WorkDefinition/GetWorkObjectForControlName?iCampaignId={iCampaignId}");
            return content ?? new MessageResponse<List<GetWorkObjectForControlNameModel>>();
        }
        public async Task<MessageResponse<List<BEFormulaData>>> GetFilterList_FormulaAsync(int iCampaignId, string sFilterName, bool bActiveFilter)
        {
            string url = $"WorkDefinition/GetFilterList_Formula?iCampaignId={iCampaignId}&sFilterName={sFilterName}&bActiveFilter={bActiveFilter}";
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEFormulaData>>>($"WorkDefinition/GetFilterList_Formula?iCampaignId={iCampaignId}&sFilterName={sFilterName}&bActiveFilter={bActiveFilter}");
            return content ?? new MessageResponse<List<BEFormulaData>>();
        }
        public async Task<MessageResponse<List<GetWorkObjectForFormulaModel>>> GetWorkObjectForFormulaAsync(int iCampaignID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<GetWorkObjectForFormulaModel>>>($"WorkDefinition/GetWorkObjectForFormula?iCampaignID={iCampaignID}");
            return content ?? new MessageResponse<List<GetWorkObjectForFormulaModel>>();
        }
        public async Task<MessageResponse<bool>> GetExistObjFormulaAsync(int campaignID, int objID, string objEvent)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<bool>>($"WorkDefinition/GetExistObjFormula?campaignID={campaignID}&objID={objID}&objEvent={objEvent}");
            return content ?? new MessageResponse<bool>();
        }
        public async Task<MessageResponse<string>> InsertObjectformulaAsync(ObjectformulaModel oObject)
        {
            var response = await _client.PostAsJsonAsync($"WorkDefinition/InsertObjectformula", oObject);
            // response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<int>> DeleteObjectFormulaAsync(int iObjectId)
        {
            var response = await _client.DeleteAsync($"WorkDefinition/DeleteObjectFormula?iObjectId={iObjectId}");
            return await response.Content.ReadFromJsonAsync<MessageResponse<int>>() ?? new MessageResponse<int>();
        }
        public async Task<MessageResponse<string>> InsertGridConfiguration_TempAsync(GridConfigurationModel model, int storeId)
        {
            var response = await _client.PostAsJsonAsync($"WorkDefinition/InsertGridConfiguration_Temp?storeId={storeId}", model);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<string>> GetGridObjectcONTROL_TempAsync(string ProcessID, string CamID, string GridObjectName, string iStoreID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<string>>($"WorkDefinition/GetGridObjectcONTROL_Temp?ProcessID={ProcessID}&CamID={CamID}&GridObjectName={GridObjectName}&iStoreID={iStoreID}");
            return content ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<string>> GetGridConfigurationDataAsync(int iStoreID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<string>>($"WorkDefinition/GetGridConfigurationData?iStoreID={iStoreID}");
            return content ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<string>> UpdateGridTableMainAsync(string StoreId, bool Disabled, GridConfigurationModel model)
        {
            var response = await _client.PutAsJsonAsync($"WorkDefinition/UpdateGridTableMain?StoreId={StoreId}&Disabled={Disabled}", model);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<string>> UpdateDataAsync(List<UpdatePreViewDataModel> objLayout, int iFormID)
        {
            var response = await _client.PutAsJsonAsync($"WorkDefinition/UpdateData?iFormID={iFormID}", objLayout);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<List<BEStoreInfo>>> GetStoreObjectListAsync(int iCampaignId, string sStoreName, bool bActiveStore, int UserId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEStoreInfo>>>($"GridConfiguration/GetStoreObjectList?iCampaignId={iCampaignId}&sStoreName={sStoreName}&bActiveStore={bActiveStore}&UserId={UserId}");
            return content ?? new MessageResponse<List<BEStoreInfo>>();
        }
        public async Task<MessageResponse<string>> GetAllLinkCampaignDataAsync(int iCampaignId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<string>>($"GridConfiguration/GetAllLinkCampaignData?iCampaignId={iCampaignId}");
            return content ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<int>> GridInserUpdateDataAsync(LinkCampaignGridConfigurationModel objGridConfiguration, int userid, string Action)
        {
            var response = await _client.PutAsJsonAsync($"GridConfiguration/GridInserUpdateData?userid={userid}&Action={Action}", objGridConfiguration);
            return await response.Content.ReadFromJsonAsync<MessageResponse<int>>() ?? new MessageResponse<int>();
        }
    }
}
