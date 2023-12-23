using Microsoft.AspNetCore.Mvc.Rendering;
using MicUI.Configuration.Models;
using MicUI.Configuration.Models.Response;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Services.ServiceModel;
using System.Collections;

namespace MicUI.Configuration.Services.Configuration
{
    public class ConfigApiService : BaseApiService, IConfigApiService
    {
        public ConfigApiService(HttpClient client, IHttpContextAccessor httpContextAccessor) : base(client, httpContextAccessor)
        {

        }

        public async Task<MessageResponse<IEnumerable<BEVerticalInfo>>> GetVerticalListAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<IEnumerable<BEVerticalInfo>>>("Client/GetVerticalList");
            return content;
        }
        public async Task<MessageResponse<BEUserPreference>> GetUserPreferenceAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<BEUserPreference>>("UserPreference/GetUserPreference");
            return content;
        }

        public async Task<MessageResponse<List<BETimeZoneInfo>>> GetTimeZoneListAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BETimeZoneInfo>>>("TimeZone/GetTimeZoneList");
            return content;
        }

        public async Task<MessageResponse<IList<BELanguages>>> GetLanguageListAsync(bool IsActiveLanguages)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<IList<BELanguages>>>($"LanguageConfig/GetLanguages?IsActiveLanguages={IsActiveLanguages}");
            return content;
        }

        public async Task<MessageResponse<int>> SaveUpdateUserPreferenceAsync(UserPreferenceViewModel request)
        {
            var response = await _client.PutAsJsonAsync($"UserPreference/SaveUpdateUserPreference", request);
            return await response.Content.ReadFromJsonAsync<MessageResponse<int>>();
        }

        public async Task<MessageResponse<IEnumerable<BESBUInfo>>> GetSBUListbasedONClientAsync(int clientID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<IEnumerable<BESBUInfo>>>($"Client/GetSBUListbasedONClient?clientID={clientID}");
            return content;
        }
        public async Task<MessageResponse<IEnumerable<BESBUInfo>>> GetSBUListAsync(string? sbuName, bool isActive)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<IEnumerable<BESBUInfo>>>($"SBU/GetSBUList?sbuName={sbuName}&isActive={isActive}");
            return content;
        }

        public async Task<MessageResponse<List<BEClientInfo>>> GetClientAsync(string? searchText, bool isActive)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEClientInfo>>>($"Client/GetClient?searchText={searchText}&isActive={isActive}");
            return content ?? new MessageResponse<List<BEClientInfo>>();
        }
        public async Task<MessageResponse<BEClientInfo>> GetClientByIdAsync(int clientID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<BEClientInfo>>($"Client/GetClientById?clientID={clientID}");
            return content;
        }

        public async Task<MessageResponse<string>> AddClientAsync(ClientModel request)
        {
            var response = await _client.PostAsJsonAsync("Client/AddClient", request);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }
        public async Task<MessageResponse<string>> UpdateClientAsync(ClientModel request)
        {
            var response = await _client.PutAsJsonAsync("Client/UpdateClient", request);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }

        public async Task<MessageResponse<IList<BECalendarInfo>>> GetCalendarListAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<IList<BECalendarInfo>>>("Process/GetCalendarList");
            return content;
        }
        public async Task<MessageResponse<List<BEMasterTable>>> GetProcessWorkTypeListAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEMasterTable>>>("Process/GetProcessWorkTypeList");
            return content;
        }
        public async Task<MessageResponse<List<BEMasterTable>>> GetProcessComplexityListAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEMasterTable>>>("Process/GetProcessComplexityList");
            return content;
        }

        public async Task<MessageResponse<List<BELocation>>> GetLocationListAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BELocation>>>("Process/GetLocationList");
            return content;
        }
        public async Task<MessageResponse<List<BEERPProcess>>> GetERPGridWithSearchListAsync(string? erpProcessName, int locationId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEERPProcess>>>($"Process/GetERPGridWithSearchList?erpProcessName={erpProcessName}&locationId={locationId}");
            return content;
        }

        public async Task<MessageResponse<List<BELOBInfo>>> GetLOBListAsync(string lobName, bool activeLOB)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BELOBInfo>>>($"LOB/GetLOBList?LOBName={lobName}&activeLOB={activeLOB}");
            return content ?? new MessageResponse<List<BELOBInfo>>();
        }
        public async Task<MessageResponse<string>> AddLOBAsync(LOBModel lobModelModel)
        {
            var content = await _client.PostAsJsonAsync($"LOB/AddLOB", lobModelModel);

            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }
        public async Task<MessageResponse<string>> UpdateLOBAsync(LOBModel lobModelModel)
        {
            var content = await _client.PutAsJsonAsync($"LOB/UpdateLOB", lobModelModel);

            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }
        public async Task<MessageResponse<LOBModel>> GetLOBByIdAsync(int lobID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<LOBModel>>($"LOB/GetLOBById?LOBID={lobID}");
            return content;

        }
        public async Task<MessageResponse<List<BESBUInfo>>> GetSBUListAsync(string sbuName)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BESBUInfo>>>($"SBU/GetSBUList?sbuName={sbuName}");
            return content;
        }
        public async Task<MessageResponse<string>> AddSBUAsync(SBUModel sBUModelModel)
        {

            var content = await _client.PostAsJsonAsync($"SBU/AddSBU", sBUModelModel);

            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }
        public async Task<MessageResponse<string>> UpdateSBUAsync(SBUModel sBUModelModel)
        {
            var content = await _client.PutAsJsonAsync($"SBU/UpdateSBU", sBUModelModel);

            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }
        public async Task<MessageResponse<SBUModel>> GetSBUByIdAsync(int sbuID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<SBUModel>>($"SBU/GetSBUById?sbuID={sbuID}");
            return content;
        }
        public async Task<MessageResponse<List<VerticalMasterModel>>> GetVerticalMasterListAsync(string vName)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<VerticalMasterModel>>>($"VerticalMaster/GetVerticalMasterList?vName={vName}");
            return content;
        }
        public async Task<MessageResponse<string>> AddVerticalMasterAsync(VerticalMasterModel verticalMasterModel)
        {
            var content = await _client.PostAsJsonAsync($"VerticalMaster/AddVerticalMaster", verticalMasterModel);

            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }
        public async Task<MessageResponse<string>> UpdateVerticalMasterAsync(VerticalMasterModel verticalMasterModel)
        {
            var content = await _client.PutAsJsonAsync($"VerticalMaster/UpdateVerticalMaster", verticalMasterModel);

            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }
        public async Task<MessageResponse<VerticalMasterModel>> GetVerticalMasterByIdAsync(int verticalMasterID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<VerticalMasterModel>>($"VerticalMaster/GetVerticalMasterById?verticalMasterID={verticalMasterID}");
            return content;

        }
        public async Task<MessageResponse<string>> InsertDataAsync(SkillMasterModel SkillModel)
        {
            //string data= 
            var content = await _client.PostAsJsonAsync($"SkillMaster/AddSkill", SkillModel);

            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }
        public async Task<MessageResponse<string>> UpdateDataAsync(SkillMasterModel SkillModel)
        {
            //string data= 
            var content = await _client.PutAsJsonAsync("SkillMaster/UpdateSkill", SkillModel);

            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }
        public async Task<MessageResponse<IList<SkillMasterModel>>> GetSkillListAsync(string sSkillName)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<IList<SkillMasterModel>>>($"SkillMaster/GetSkillList?skillName={sSkillName}");
            return content;
        }
        public async Task<MessageResponse<SkillMasterModel>> GetSkillByIdAsync(int sSkillId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<SkillMasterModel>>($"SkillMaster/GetSkillById?skillID={sSkillId}");
            return content;
        }
        public async Task<MessageResponse<TimeZoneModel>> GetTimeZoneByIdAsync(int timeZoneId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<TimeZoneModel>>($"TimeZone/GetTimeZoneById?timeZoneID={timeZoneId}");
            return content;
        }
        public async Task<MessageResponse<List<BETimeZoneInfo>>> GetTimeZoneListAsync(string searchTimeZone)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BETimeZoneInfo>>>($"TimeZone/GetTimeZoneList?searchTimeZone={searchTimeZone}");
            return content;
        }
        public async Task<MessageResponse<SelectList>> GetLocationsAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<SelectList>>($"Facility/GetLocations");
            return content;
        }
        public async Task<MessageResponse<string>> AddLocationAsync(LocationModel LocationModel)
        {
            var content = await _client.PostAsJsonAsync($"Location/AddLocation", LocationModel);
            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }
        public async Task<MessageResponse<string>> UpdateLocationAsync(LocationModel LocationModel)
        {
            var content = await _client.PutAsJsonAsync($"Location/UpdateLocation", LocationModel);
            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }
        public async Task<MessageResponse<List<LocationModel>>> GetLocationListAsync(string searchLocation)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<LocationModel>>>($"Location/GetLocationList?searchLocation={searchLocation}");
            return content;
        }
        public async Task<MessageResponse<string>> AddFacilityAsync(FacilityModel facilityModel)
        {
            //string data= 
            var content = await _client.PostAsJsonAsync($"Facility/AddFacility", facilityModel);

            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }
        public async Task<MessageResponse<string>> UpdateFacilityAsync(FacilityModel facilityModel)
        {
            //string data= 
            var content = await _client.PutAsJsonAsync($"Facility/UpdateFacility", facilityModel);

            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }
        public async Task<MessageResponse<List<BEFacility>>> GetFacilityListAsync(string searchFacility, bool activeFacility)
        {

            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEFacility>>>($"Facility/GetFacilityList?searchFacility={searchFacility}&activeFacility={activeFacility}");
            return content ?? new MessageResponse<List<BEFacility>>();
        }
        public async Task<MessageResponse<FacilityModel>> GetFacilityByIdAsync(int searchFacility)
        {

            var content = await _client.GetFromJsonAsync<MessageResponse<FacilityModel>>($"Facility/GetFacilityById?facilityID={searchFacility}");
            return content;
        }

        public async Task<MessageResponse<List<BEERPProcess>>> GetERPProcessListByERPProcessIdsAsync(ArrayList aryDistinctERPProcessId)
        {
            var response = await _client.PostAsJsonAsync("Process/GetERPProcessListByERPProcessIds", aryDistinctERPProcessId);
            return await response.Content.ReadFromJsonAsync<MessageResponse<List<BEERPProcess>>>();
        }
        public async Task<MessageResponse<int>> CheckRoleForOrgProcessAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<int>>("Process/GetCheckRoleForOrgProcess");
            return content;
        }
        public async Task<MessageResponse<bool>> GetCheckProcessByClientForUniquenessAsync(string processName, int clientId, int processId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<bool>>($"Process/GetCheckProcessByClientForUniqueness?processName={processName}&clientId={clientId}&processId={processId}");
            return content;
        }
        public async Task<MessageResponse<string>> AddProcessAsync(ProcessModel processModel)
        {
            var response = await _client.PostAsJsonAsync("Process/AddProcess", processModel);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }
        public async Task<MessageResponse<string>> UpdateProcessAsync(ProcessModel processModel)
        {
            var response = await _client.PutAsJsonAsync("Process/UpdateProcess", processModel);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }
        public async Task<MessageResponse<List<BEProcessInfo>>> GetProcessListReportsAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEProcessInfo>>>("Process/GetProcessListReports");
            return content ?? new MessageResponse<List<BEProcessInfo>>();
        }
        public async Task<MessageResponse<List<BEProcessInfo>>> GetProcessListSearchAsync(int clientId, string? processName)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEProcessInfo>>>($"Process/GetProcessListSearch?clientId={clientId}&processName={processName}");
            return content;
        }
        public async Task<MessageResponse<BEProcessInfo>> GetProcessDetailsByIdAsync(int iProcessId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<BEProcessInfo>>($"Process/GetProcessDetailsById?iProcessId={iProcessId}");
            return content;
        }

        public async Task<MessageResponse<bool>> GetCheckPermission()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<bool>>($"Campaign/GetCheckPermission");
            return content;
        }
        public async Task<MessageResponse<List<BECampaignInfo>>> GetProcessWiseCampaignListAsync(int processId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BECampaignInfo>>>($"Campaign/GetProcessWiseCampaignList?processId={processId}");
            return content;
        }
        public async Task<MessageResponse<List<BEStoreInfo>>> GetStoreListWithCampIdAsync(int campaignId, string? storeName)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEStoreInfo>>>($"WorkDefinition/GetStoreListWithCampId?campaignId={campaignId}&storeName={storeName}");
            return content;
        }
        public async Task<MessageResponse<List<BEUserInfo>>> GetBusinessApproverListAsync(int processID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEUserInfo>>>($"Campaign/GetBusinessApproverList?processId={processID}");
            return content;
        }
        public async Task<MessageResponse<string>> AddCampaignAsync(CampaignModel campaignModel)
        {
            var response = await _client.PostAsJsonAsync("Campaign/AddCampaign", campaignModel);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }

        public async Task<MessageResponse<string>> UpdateCampaignAsync(CampaignModel campaignModel)
        {
            var response = await _client.PutAsJsonAsync("Campaign/UpdateCampaign", campaignModel);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }
        public async Task<MessageResponse<int>> GetCheckUserIsSuperOrFunctionalAdminAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<int>>("Campaign/GetCheckUserIsSuperOrFunctionalAdmin");
            return content;
        }
        public async Task<MessageResponse<List<Services.ServiceModel.CampaignApproval>>> GetPendingCampaignApprovalListAsync(DateTime dFrom, DateTime dTo)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<Services.ServiceModel.CampaignApproval>>>($"Campaign/GetPendingCampaignApprovalList?dFrom={dFrom}&dTo={dTo}");
            return content;
        }

        public async Task<MessageResponse<string>> ApprovalActionAsync(int approvalId, string action)
        {
            var response = await _client.PutAsync($"Campaign/ApprovalAction?approvalId={approvalId}&action={action}", null);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }
        public async Task<MessageResponse<CampaignRequestDetails>> FetchCampaignRequestDetailsAsync(int level, int approvalId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<CampaignRequestDetails>>($"Campaign/GetPendingCampaignApprovalById?level={level}&approvalId={approvalId}");
            return content;
        }
        public async Task<MessageResponse<string>> AddCampaignRequestChangeAsync(int level, int approvalId, string changeReq)
        {
            var response = await _client.PostAsync($"Campaign/AddCampaignRequestChange?level={level}&approvalId={approvalId}&changeReq={changeReq}", null);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }
        public async Task<MessageResponse<string>> UpdateCampaignRequestChangeAsync(CampaignRequestChange objCampaignInfo)
        {
            var response = await _client.PutAsJsonAsync("Campaign/UpdateCampaignRequestChange", objCampaignInfo);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }
        public async Task<MessageResponse<List<BECampaignInfo>>> GetCampaignListAsync(int processID, string? campSearchName, bool activeCampaign)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BECampaignInfo>>>($"Campaign/GetCampaignList?processID={processID}&campSearchName={campSearchName}&activeCampaign={activeCampaign}");
            return content;
        }
        public async Task<MessageResponse<BECampaignInfo>> GetCampaignByIdAsync(int campaignID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<BECampaignInfo>>($"Campaign/GetCampaignById?campaignID={campaignID}");
            return content;
        }
        public async Task<MessageResponse<BESubProcess>> GetSubProcessByIdAsync(int subProcessID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<BESubProcess>>($"SubProcessMaster/GetSubProcessById?subProcessID={subProcessID}");
            return content ?? new MessageResponse<BESubProcess>();
        }
        public async Task<MessageResponse<string>> AddSubProcessAsync(SubProcessMasterModel oSubProcessModel)
        {
            var response = await _client.PostAsJsonAsync("SubProcessMaster/AddSubProcess", oSubProcessModel);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<string>> UpdateSubProcessAsync(SubProcessMasterModel oSubProcessModel)
        {
            var response = await _client.PutAsJsonAsync("SubProcessMaster/UpdateSubProcess", oSubProcessModel);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<List<BESubProcess>>> GetSubProcessListAsync(int processId, string? subProcessName, bool activeSubProcess)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BESubProcess>>>($"SubProcessMaster/GetSubProcessList?processId={processId}&subProcessName={subProcessName}&activeSubProcess={activeSubProcess}");
            return content ?? new MessageResponse<List<BESubProcess>>();
        }
        public async Task<MessageResponse<MasterValueModel>> GetMasterDetailAsync(int iFieldID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<MasterValueModel>>($"MasterValue/GetMasterDetails?masterValueID={iFieldID}");
            return content;
        }

        public async Task<MessageResponse<string>> AddMasterTypeValueAsync(MasterValueModel model)
        {
            var content = await _client.PostAsJsonAsync($"MasterValue/AddMasterTypeValue", model);
            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>()?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<string>> UpdateMasterTypeValueAsync(MasterValueModel model)
        {
            var content = await _client.PutAsJsonAsync($"MasterValue/UpdateMasterTypeValue", model);
            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }

        public async Task<MessageResponse<List<MasterValueModel>>> GetMasterListAsync(string masterValueSearchName)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<MasterValueModel>>>($"MasterValue/GetMasterList?masterValueSearchName={masterValueSearchName}");
            return content;
        }

    }
}

