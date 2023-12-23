using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicUI.Configuration.Models;
using MicUI.Configuration.Models.Response;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Services.ServiceModel;
using System.Collections;

namespace MicUI.Configuration.Services.Configuration
{
    public interface IConfigApiService
    {
        Task<MessageResponse<IEnumerable<BEVerticalInfo>>> GetVerticalListAsync();
        Task<MessageResponse<BEUserPreference>> GetUserPreferenceAsync();
        Task<MessageResponse<List<BETimeZoneInfo>>> GetTimeZoneListAsync();
        Task<MessageResponse<IList<BELanguages>>> GetLanguageListAsync(bool IsActiveLanguages);
        Task<MessageResponse<int>> SaveUpdateUserPreferenceAsync(UserPreferenceViewModel request);
        Task<MessageResponse<IEnumerable<BESBUInfo>>> GetSBUListbasedONClientAsync(int clientID);
        Task<MessageResponse<IEnumerable<BESBUInfo>>> GetSBUListAsync(string? sbuName, bool isActive);
        Task<MessageResponse<List<BEClientInfo>>> GetClientAsync(string? searchText, bool isActive);
        Task<MessageResponse<BEClientInfo>> GetClientByIdAsync(int clientID);
        Task<MessageResponse<string>> AddClientAsync(ClientModel request);
        Task<MessageResponse<string>> UpdateClientAsync(ClientModel request);
        Task<MessageResponse<IList<BECalendarInfo>>> GetCalendarListAsync();
        Task<MessageResponse<List<BEMasterTable>>> GetProcessWorkTypeListAsync();
        Task<MessageResponse<List<BEMasterTable>>> GetProcessComplexityListAsync();
        Task<MessageResponse<List<BELocation>>> GetLocationListAsync();
        Task<MessageResponse<List<BEERPProcess>>> GetERPGridWithSearchListAsync(string? erpProcessName, int locationId);
        Task<MessageResponse<List<BELOBInfo>>> GetLOBListAsync(string lobName, bool activeLOB);
        Task<MessageResponse<string>> AddLOBAsync(LOBModel lobModelModel);
        Task<MessageResponse<string>> UpdateLOBAsync(LOBModel lobModelModel);
        Task<MessageResponse<LOBModel>> GetLOBByIdAsync(int lobID);
        Task<MessageResponse<List<BESBUInfo>>> GetSBUListAsync(string sbuName);
        Task<MessageResponse<string>> AddSBUAsync(SBUModel sBUModelModel);
        Task<MessageResponse<string>> UpdateSBUAsync(SBUModel sBUModelModel);
        Task<MessageResponse<SBUModel>> GetSBUByIdAsync(int sbuID);
        Task<MessageResponse<VerticalMasterModel>> GetVerticalMasterByIdAsync(int verticalMasterID);
        Task<MessageResponse<string>> UpdateVerticalMasterAsync(VerticalMasterModel verticalMasterModel);
        Task<MessageResponse<string>> AddVerticalMasterAsync(VerticalMasterModel verticalMasterModel);
        Task<MessageResponse<List<VerticalMasterModel>>> GetVerticalMasterListAsync(string vName);
        Task<MessageResponse<string>> InsertDataAsync(SkillMasterModel SkillModel);
        Task<MessageResponse<string>> UpdateDataAsync(SkillMasterModel SkillModel);
        Task<MessageResponse<IList<SkillMasterModel>>> GetSkillListAsync(string sSkillName);
        Task<MessageResponse<SkillMasterModel>> GetSkillByIdAsync(int skillId);
        Task<MessageResponse<List<BETimeZoneInfo>>> GetTimeZoneListAsync(string searchTimeZone);
        Task<MessageResponse<TimeZoneModel>> GetTimeZoneByIdAsync(int timeZoneId);
        Task<MessageResponse<SelectList>> GetLocationsAsync();
        Task<MessageResponse<List<LocationModel>>> GetLocationListAsync(string searchLocation);
        Task<MessageResponse<string>> AddLocationAsync(LocationModel LocationModel);
        Task<MessageResponse<string>> UpdateLocationAsync(LocationModel LocationModel);
        Task<MessageResponse<string>> AddFacilityAsync(FacilityModel facilityModel);
        Task<MessageResponse<string>> UpdateFacilityAsync(FacilityModel facilityModel);
        Task<MessageResponse<List<BEFacility>>> GetFacilityListAsync(string searchFacility, bool activeFacility);
        Task<MessageResponse<FacilityModel>> GetFacilityByIdAsync(int searchFacility);
        Task<MessageResponse<List<BEERPProcess>>> GetERPProcessListByERPProcessIdsAsync(ArrayList aryDistinctERPProcessId);
        Task<MessageResponse<int>> CheckRoleForOrgProcessAsync();
        Task<MessageResponse<bool>> GetCheckProcessByClientForUniquenessAsync(string processName, int clientId, int processId);
        Task<MessageResponse<string>> AddProcessAsync(ProcessModel processModel);
        Task<MessageResponse<string>> UpdateProcessAsync(ProcessModel processModel);
        Task<MessageResponse<List<BEProcessInfo>>> GetProcessListSearchAsync(int clientId, string? processName);
        Task<MessageResponse<BEProcessInfo>> GetProcessDetailsByIdAsync(int iProcessId);
        Task<MessageResponse<bool>> GetCheckPermission();
        Task<MessageResponse<List<BECampaignInfo>>> GetProcessWiseCampaignListAsync(int processId);
        Task<MessageResponse<List<BEStoreInfo>>> GetStoreListWithCampIdAsync(int campaignId, string? storeName);
        Task<MessageResponse<List<BEUserInfo>>> GetBusinessApproverListAsync(int processID);
        Task<MessageResponse<string>> AddCampaignAsync([FromBody] CampaignModel campaignModel);
        Task<MessageResponse<string>> UpdateCampaignAsync([FromBody] CampaignModel campaignModel);
        Task<MessageResponse<int>> GetCheckUserIsSuperOrFunctionalAdminAsync();
        Task<MessageResponse<List<Services.ServiceModel.CampaignApproval>>> GetPendingCampaignApprovalListAsync(DateTime dFrom, DateTime dTo);
        Task<MessageResponse<string>> ApprovalActionAsync(int approvalId, string action);
        Task<MessageResponse<CampaignRequestDetails>> FetchCampaignRequestDetailsAsync(int level, int approvalId);
        Task<MessageResponse<string>> AddCampaignRequestChangeAsync(int level, int approvalId, string changeReq);
        Task<MessageResponse<string>> UpdateCampaignRequestChangeAsync(CampaignRequestChange objCampaignInfo);
        Task<MessageResponse<List<BECampaignInfo>>> GetCampaignListAsync(int processID, string? campSearchName, bool activeCampaign);
        Task<MessageResponse<BECampaignInfo>> GetCampaignByIdAsync(int campaignID);
        Task<MessageResponse<BESubProcess>> GetSubProcessByIdAsync(int subProcessID);
        Task<MessageResponse<string>> AddSubProcessAsync(SubProcessMasterModel oSubProcessModel);
        Task<MessageResponse<string>> UpdateSubProcessAsync(SubProcessMasterModel oSubProcessModel);
        Task<MessageResponse<List<BESubProcess>>> GetSubProcessListAsync(int processId, string? subProcessName, bool activeSubProcess);
        Task<MessageResponse<List<BEProcessInfo>>> GetProcessListReportsAsync();

        Task<MessageResponse<List<MasterValueModel>>> GetMasterListAsync(string masterValueSearchName);
        Task<MessageResponse<string>> AddMasterTypeValueAsync(MasterValueModel model);
        Task<MessageResponse<string>> UpdateMasterTypeValueAsync(MasterValueModel model);
        Task<MessageResponse<MasterValueModel>> GetMasterDetailAsync(int iFieldID);

    }
}

