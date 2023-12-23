using MicUI.Configuration.Models.Response;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Services.Security
{
    public class SecurityApiService : BaseApiService, ISecurityApiService
    {
        public SecurityApiService(HttpClient client, IHttpContextAccessor httpContextAccessor) : base(client, httpContextAccessor)
        {

        }

        public async Task<MessageResponse<List<BEUserInfo>>> IsLADPUserAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEUserInfo>>>($"UserManagement/IsLADPUser?LoginName={SessionLoginName}");
            return content ?? new MessageResponse<List<BEUserInfo>>();
        }

        public async Task<MessageResponse<List<BEMenuItems>>> GetRoleWiseMenuAsync(int roleId = 1)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEMenuItems>>>($"Menus/GetRoleWiseMenu?RoleId={roleId}&isMossApplicationMenu=true");
            return content ?? new MessageResponse<List<BEMenuItems>>();
        }
        public async Task<MessageResponse<List<BEFormActionInfo>>> PopulateFormActionAsync(int roleId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEFormActionInfo>>>($"Roles/GetFormAction?roleId={roleId}");
            return content ?? new MessageResponse<List<BEFormActionInfo>>();
        }
        public async Task<MessageResponse<List<BEMasterTable>>> FillRoleLevelAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEMasterTable>>>($"Roles/FillRoleLevel");
            return content ?? new MessageResponse<List<BEMasterTable>>();

        }
        public async Task<MessageResponse<List<BEApproval>>> PopulateRoleApproverDropDownListAsync(int roleId)
        {

            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEApproval>>>($"ERPJobRoleMap/GetUserRoleApproverList?roleId={roleId}");
            return content ?? new MessageResponse<List<BEApproval>>();

        }
        //public async Task<MessageResponse<List<BERoleInfo>>> GetRoleListByNameAsync(string iRoleName)
        //{
        //    var content = await _client.GetFromJsonAsync<MessageResponse<List<BERoleInfo>>>($"Roles/GetRoleListByName?iRoleName={iRoleName}");
        //    return content ?? new MessageResponse<List<BERoleInfo>>();
        //}
        public async Task<MessageResponse<List<BERoleInfo>>> GetRoleListByNameAsync(string iRoleName, bool activeRoles)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BERoleInfo>>>($"Roles/GetRoleListByName?iRoleName={iRoleName}&activeRoles={activeRoles}");
            return content ?? new MessageResponse<List<BERoleInfo>>();
        }
        public async Task<MessageResponse<List<RoleApprovalModel>>> GetRoleApprovalListAsync(int iUserID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<RoleApprovalModel>>>($"Roles/GetRoleApprovalList?iUserID={iUserID}");
            return content ?? new MessageResponse<List<RoleApprovalModel>>();
        }
        public async Task<MessageResponse<List<RoleApprovalModel>>> GetRoleRequestStatusAsync(int iUser, DateTime dtFromDate, DateTime dtToDate)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<RoleApprovalModel>>>($"Roles/GetRoleRequestStatus?iUserID={iUser}&dtFromDate={dtFromDate}&dtToDate={dtToDate}");
            return content ?? new MessageResponse<List<RoleApprovalModel>>();
        }
        public async Task<MessageResponse<List<BEErpJobRoleMap>>> GetJobRoleMapAsync(int iFilterId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEErpJobRoleMap>>>($"ERPJobRoleMap/GetJobRoleMap?filterId={iFilterId}");
            return content ?? new MessageResponse<List<BEErpJobRoleMap>>();
        }
        public async Task<MessageResponse<List<BEJobCodeInfo>>> GetJobAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEJobCodeInfo>>>($"ERPJobRoleMap/GetJob");
            return content ?? new MessageResponse<List<BEJobCodeInfo>>();
        }
        public async Task<MessageResponse<List<BERoleInfo>>> GetRoleListAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BERoleInfo>>>($"ERPJobRoleMap/GetRoleList");
            return content ?? new MessageResponse<List<BERoleInfo>>();
        }
        public async Task<MessageResponse<List<BEErpJobRoleMap>>> GetJobRoleMapByNameAsync(string? iJobDesc)
        {

            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEErpJobRoleMap>>>($"ERPJobRoleMap/GetJobRoleMapByName?jobDesc={iJobDesc}");
            return content ?? new MessageResponse<List<BEErpJobRoleMap>>();
        }
        public async Task<MessageResponse<string>> AddERPJobDataAsync(ErpJobRoleMap oJobRole, int FormID)
        {
            var content = await _client.PostAsJsonAsync($"ERPJobRoleMap/InsertData?FormID={FormID}", oJobRole);
            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<List<BEProcessInfo>>> GetClientWiseProcessListAsync(int userId, int ClientId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEProcessInfo>>>($"ProcessOwner/GetClientWiseProcessList?userId={userId}&ClientId={ClientId}");
            return content ?? new MessageResponse<List<BEProcessInfo>>();
        }
        public async Task<MessageResponse<List<BEApproval>>> GetUserProcessOwnerAsync(int processId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEApproval>>>($"ProcessOwner/GetUserProcessOwner?processId={processId}");
            return content ?? new MessageResponse<List<BEApproval>>();
        }
        public async Task<MessageResponse<ProcessOwnerApprovalUser>> GetUserProcessOwnerUserListAsync(int processId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<ProcessOwnerApprovalUser>>($"ProcessOwner/GetUserProcessOwnerUserList?processId={processId}");
            return content ?? new MessageResponse<ProcessOwnerApprovalUser>();
        }
        public async Task<MessageResponse<string>> ExistingUserRequestAsync(int ProcessId, string ProcessOwner)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<string>>($"ProcessOwner/ExistingUserRequest?ProcessId={ProcessId}&ProcessOwner={ProcessOwner}");
            return content ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<int>> SendApproveProcessRequestAsync(ProcessOwnerViewModel oProcess, int FormID, int ProcRequest_Id, int Action)
        {
            var content = await _client.PostAsJsonAsync($"ProcessOwner/SendApproveProcessReqest?FormID={FormID}&ProcRequest_Id={ProcRequest_Id}&Action={Action}", oProcess);
            return await content.Content.ReadFromJsonAsync<MessageResponse<int>>() ?? new MessageResponse<int>();
        }
        public async Task<MessageResponse<string>> CheckProcessOwnerApproverLevelAsync(ProcessOwnerViewModel oProcess)
        {
            var content = await _client.PostAsJsonAsync($"ProcessOwner/CheckProcessOwnerApproverLevel", oProcess);
            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<List<ProcessApproval>>> GetPandingApprovalAsync(int iUserId, string sFromDate, string sToDate)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<ProcessApproval>>>($"ProcessOwner/GetPandingApproval?iUserId={iUserId}&sFromDate={sFromDate}&sToDate={sToDate}");
            return content ?? new MessageResponse<List<ProcessApproval>>();

        }
        public async Task<MessageResponse<string>> InsertDataRoleAsync(RoleInfoModel oRole, int iApproverId, int iFormID)
        {
            var content = await _client.PostAsJsonAsync($"Roles/InsertData?iApproverId={iApproverId}&iFormID={iFormID}", oRole);
            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }

        public async Task<MessageResponse<List<RoleApprovalRequestModel>>> GetRoleRequestStatusAsync(DateTime dtFromDate, DateTime dtToDate)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<RoleApprovalRequestModel>>>($"Roles/GetRoleRequestStatus?dtFromDate={dtFromDate}&dtToDate={dtToDate}");
            return content ?? new MessageResponse<List<RoleApprovalRequestModel>>();
        }
        public async Task<MessageResponse<string>> CancelRoleRequestAsync(int iRequestID, int iFormID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<string>>($"Roles/CancelRoleRequest?iRequestID={iRequestID}&iFormID={iFormID}");
            return content ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<string>> ApproveRoleRequestAsync(int iRequestID, int iFormID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<string>>($"Roles/ApproveRoleRequest?iRequestID={iRequestID}&iFormID={iFormID}");
            return content ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<string>> RejectRoleRequestAsync(int iRequestID, int iFormID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<string>>($"Roles/RejectRoleRequest?iRequestID={iRequestID}&iFormID={iFormID}");
            return content ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<List<RoleInfoModel>>> GetRoleListAsync(int iRoleID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<RoleInfoModel>>>($"Roles/GetRoleList?iRoleID={iRoleID}");
            return content ?? new MessageResponse<List<RoleInfoModel>>();
        }
        public async Task<MessageResponse<string>> UpdateDataRoleAsync(RoleInfoModel oRole, int iApproverId, int iFormID)
        {
            var content = await _client.PutAsJsonAsync($"Roles/UpdateData?iApproverId={iApproverId}&iFormID={iFormID}", oRole);
            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<List<BEUserInfo>>> GetUserJobCodeAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEUserInfo>>>($"UserManagement/GetUserJobCode");
            return content ?? new MessageResponse<List<BEUserInfo>>();
        }
        public async Task<MessageResponse<List<BEUserInfo>>> GetUserRoleListUserRoleAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEUserInfo>>>($"UserManagement/GetUserRoleListUserRole");
            return content ?? new MessageResponse<List<BEUserInfo>>();
        }
        public async Task<MessageResponse<List<BEUserInfo>>> GetUserListAsync(string sLoginName, bool bActiveUser, string searchCondition)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEUserInfo>>>($"UserManagement/GetUserList?sLoginName={sLoginName}&bActiveUser={bActiveUser}&searchCondition={searchCondition}");
            return content ?? new MessageResponse<List<BEUserInfo>>();
        }
        public async Task<MessageResponse<string>> InsertClientUserRecordAsync(ClientUserInfoModel oUser, int iFormID)
        {
            var content = await _client.PostAsJsonAsync($"UserManagement/InsertClientUserRecord?iFormID={iFormID}", oUser);
            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<string>> GetUserDetailsAsync(int iUserID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<string>>($"UserManagement/GetUserDetails?iUserID={iUserID}");
            return content ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<BEUserInfo>> GetClientUserDetailsAsync(int userId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<BEUserInfo>>($"UserManagement/GetClientUserDetails?userId={userId}");
            return content ?? new MessageResponse<BEUserInfo>();
        }
        public async Task<MessageResponse<string>> UpdateClientUserRecordAsync(ClientUserInfoModel oUser, int iFormID)
        {
            var content = await _client.PutAsJsonAsync($"UserManagement/UpdateClientUserRecord?iFormID={iFormID}", oUser);
            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<List<UserRequestStatus>>> GetUserRequestStatusAsync(DateTime dtFromDate, DateTime dtToDate)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<UserRequestStatus>>>($"UserManagement/GetUserRequestStatus?dtFromDate={dtFromDate}&dtToDate={dtToDate}");
            return content ?? new MessageResponse<List<UserRequestStatus>>();
        }
        public async Task<MessageResponse<List<BERoleInfo>>> GetClientRoleListAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BERoleInfo>>>("UserManagement/GetClientRoleList");
            return content ?? new MessageResponse<List<BERoleInfo>>();
        }
        public async Task<MessageResponse<List<BEUserSetting>>> GetUserSettingAsync(int iUserID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEUserSetting>>>($"UserManagement/GetUserSetting?iUserID={iUserID}");
            return content ?? new MessageResponse<List<BEUserSetting>>();
        }
        public async Task<MessageResponse<List<BEClientInfo>>> GetClientAccessListAsync(int iAgentID, bool bActiveClient)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEClientInfo>>>($"UserManagement/GetClientAccessList?iAgentID={iAgentID}&bActiveClient={bActiveClient}");
            return content ?? new MessageResponse<List<BEClientInfo>>();
        }
        public async Task<MessageResponse<List<BEProcessInfo>>> GetProcessAccessListAsync(int iAgentID, bool bActiveProcess)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEProcessInfo>>>($"UserManagement/GetProcessAccessList?iAgentID={iAgentID}&bActiveProcess={bActiveProcess}");
            return content ?? new MessageResponse<List<BEProcessInfo>>();
        }
        public async Task<MessageResponse<List<BECampaignInfo>>> GetCampaignAccessListAsync(int iAgentID, bool bActiveCampaign)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BECampaignInfo>>>($"UserManagement/GetCampaignAccessList?iAgentID={iAgentID}&bActiveClient={bActiveCampaign}");
            return content ?? new MessageResponse<List<BECampaignInfo>>();
        }
        public async Task<MessageResponse<string>> CancelRequestInBetweenAsync(int iRequestID, int iFormID)
        {
            var content = await _client.PutAsync($"UserManagement/CancelRequestInBetween?iRequestID={iRequestID}&iFormID={iFormID}", null);
            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<string>> CancelAccessRequestAsync(int iRequestID, int iFormID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<string>>($"UserManagement/CancelAccessRequest?iRequestID={iRequestID}&iFormID={iFormID}");
            return content ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<int>> InsertUserMappingForApprovalAsync(InsertUserMappingForApprovalModel oUserMapping, int iFormID)
        {
            var content = await _client.PutAsJsonAsync($"UserManagement/InsertUserMappingForApproval?iFormID={iFormID}", oUserMapping);
            return await content.Content.ReadFromJsonAsync<MessageResponse<int>>() ?? new MessageResponse<int>();
        }
        public async Task<MessageResponse<List<UserRequestType>>> GetUserRequestTypeAsync(int userId, int requestId, int mappedOn)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<UserRequestType>>>($"UserManagement/GetUserRequestType?userId={userId}&requestId={requestId}&mappedOn={mappedOn}");
            return content ?? new MessageResponse<List<UserRequestType>>();
        }
        public async Task<MessageResponse<List<UserApproverDetail>>> GetUserApproverListAsync(int userId, int clientId, int processId, int flag, int iFormID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<UserApproverDetail>>>($"UserManagement/GetUserApproverList?userId={userId}&clientId={clientId}&flag={flag}&iFormID={iFormID}");
            return content ?? new MessageResponse<List<UserApproverDetail>>();
        }
        public async Task<MessageResponse<string>> InsertUserMappingApproversAndSendMailAsync(UserMappingApproverModel oUser, int iFormID)
        {
            var content = await _client.PutAsJsonAsync($"UserManagement/InsertUserMappingApproversAndSendMail?iFormID={iFormID}", oUser);
            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<string>> ApproveAccessRequestAsync(int iRequestID, int iRequestTypeID, int iRequestType, int iApprovalLevel, int iFormID)
        {
            var content = await _client.PutAsync($"UserManagement/ApproveAccessRequest?iRequestID={iRequestID}&iRequestTypeID={iRequestTypeID}&iRequestType={iRequestType}&iApprovalLevel={iApprovalLevel}&iFormID={iFormID}", null);
            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();

        }
        public async Task<MessageResponse<string>> RejectAccessRequestAsync(int iRequestID, int iRequestTypeID, int iRequestType, int iApprovalLevel, int iFormID)
        {
            var content = await _client.PutAsync($"UserManagement/RejectAccessRequest?iRequestID={iRequestID}&iRequestTypeID={iRequestTypeID}&iRequestType={iRequestType}&iApprovalLevel={iApprovalLevel}&iFormID={iFormID}", null);
            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();
        }
        public async Task<MessageResponse<List<AccessRequestApprovalList>>> GetUserApprovalListAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<AccessRequestApprovalList>>>($"UserManagement/GetUserApprovalList");
            return content ?? new MessageResponse<List<AccessRequestApprovalList>>();
        }
        public async Task<MessageResponse<BEUserInfo>> GetUserDetailsWithRoleAsync(int iUserID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<BEUserInfo>>($"PowerUser/GetUserDetailsWithRole?iUserID={iUserID}");
            return content ?? new MessageResponse<BEUserInfo>();
        }
        public async Task<MessageResponse<List<RoleApproverUserList>>> GetUserRoleApproverListAsync(int iRoleId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<RoleApproverUserList>>>($"PowerUser/GetUserRoleApproverList?iRoleId={iRoleId}");
            return content ?? new MessageResponse<List<RoleApproverUserList>>();
        }
        public async Task<MessageResponse<string>> InsertUserRoleDataAsync(PowerUserInfo oUser, int iMode, int iFormID)
        {
            var content = await _client.PostAsJsonAsync($"PowerUser/InsertUserRoleData?iMode={iMode}&iFormID={iFormID}", oUser);
            return await content.Content.ReadFromJsonAsync<MessageResponse<string>>() ?? new MessageResponse<string>();

        }
    }
}
