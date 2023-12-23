using MicUI.Configuration.Models.Response;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Services.Security
{
    public interface ISecurityApiService
    {
        Task<MessageResponse<List<BEUserInfo>>> IsLADPUserAsync();
        Task<MessageResponse<List<BEMenuItems>>> GetRoleWiseMenuAsync(int roleId);
        Task<MessageResponse<List<BEFormActionInfo>>> PopulateFormActionAsync(int roleId);
         Task<MessageResponse<List<BEMasterTable>>> FillRoleLevelAsync();
        Task<MessageResponse<List<BEApproval>>> PopulateRoleApproverDropDownListAsync(int roleId);
        // Task<MessageResponse<List<BERoleInfo>>> GetRoleListByNameAsync(string iRoleName);
        Task<MessageResponse<List<BERoleInfo>>> GetRoleListByNameAsync(string iRoleName, bool activeRoles);
        Task<MessageResponse<List<RoleApprovalModel>>> GetRoleApprovalListAsync(int iUserID);
        Task<MessageResponse<List<RoleApprovalModel>>> GetRoleRequestStatusAsync(int iUser, DateTime dtFromDate, DateTime dtToDate);
        Task<MessageResponse<List<BEErpJobRoleMap>>> GetJobRoleMapAsync(int iFilterId);
        Task<MessageResponse<List<BEJobCodeInfo>>> GetJobAsync();
        Task<MessageResponse<List<BERoleInfo>>> GetRoleListAsync();
        Task<MessageResponse<List<BEErpJobRoleMap>>> GetJobRoleMapByNameAsync(string JobDesc);
        Task<MessageResponse<string>> AddERPJobDataAsync(ErpJobRoleMap oJobRole, int FormID);
        Task<MessageResponse<List<BEProcessInfo>>> GetClientWiseProcessListAsync(int userId, int ClientId);
        Task<MessageResponse<List<BEApproval>>> GetUserProcessOwnerAsync(int processId);
        Task<MessageResponse<ProcessOwnerApprovalUser>> GetUserProcessOwnerUserListAsync(int processId);
        Task<MessageResponse<string>> ExistingUserRequestAsync(int ProcessId, string ProcessOwner);
        Task<MessageResponse<int>> SendApproveProcessRequestAsync(ProcessOwnerViewModel oProcess, int FormID, int ProcRequest_Id, int Action);
        Task<MessageResponse<string>> CheckProcessOwnerApproverLevelAsync(ProcessOwnerViewModel oProcess);
        Task<MessageResponse<List<ProcessApproval>>> GetPandingApprovalAsync(int iUserId, string sFromDate, string sToDate);
        Task<MessageResponse<string>> InsertDataRoleAsync(RoleInfoModel oRole, int iApproverId, int iFormID);
        Task<MessageResponse<List<RoleApprovalRequestModel>>> GetRoleRequestStatusAsync(DateTime dtFromDate, DateTime dtToDate);
        Task<MessageResponse<string>> CancelRoleRequestAsync(int iRequestID, int iFormID);
        Task<MessageResponse<string>> ApproveRoleRequestAsync(int iRequestID, int iFormID);
        Task<MessageResponse<string>> RejectRoleRequestAsync(int iRequestID, int iFormID);
        Task<MessageResponse<List<RoleInfoModel>>> GetRoleListAsync(int iRoleID);
        Task<MessageResponse<string>> UpdateDataRoleAsync(RoleInfoModel oRole, int iApproverId, int iFormID);
        Task<MessageResponse<List<BEUserInfo>>> GetUserJobCodeAsync();
        Task<MessageResponse<List<BEUserInfo>>> GetUserRoleListUserRoleAsync();
        Task<MessageResponse<List<BEUserInfo>>> GetUserListAsync(string sLoginName, bool bActiveUser, string searchCondition);
        Task<MessageResponse<string>> InsertClientUserRecordAsync(ClientUserInfoModel oUser, int iFormID);
        Task<MessageResponse<string>> GetUserDetailsAsync(int iUserID);
        Task<MessageResponse<BEUserInfo>> GetClientUserDetailsAsync(int userId);
        Task<MessageResponse<string>> UpdateClientUserRecordAsync(ClientUserInfoModel oUser, int iFormID);
        Task<MessageResponse<List<UserRequestStatus>>> GetUserRequestStatusAsync(DateTime dtFromDate, DateTime dtToDate);
        Task<MessageResponse<List<BERoleInfo>>> GetClientRoleListAsync();
        Task<MessageResponse<List<BEUserSetting>>> GetUserSettingAsync(int iUserID);
        Task<MessageResponse<List<BEClientInfo>>> GetClientAccessListAsync(int iAgentID, bool bActiveClient);
        Task<MessageResponse<List<BEProcessInfo>>> GetProcessAccessListAsync(int iAgentID, bool bActiveProcess);
        Task<MessageResponse<List<BECampaignInfo>>> GetCampaignAccessListAsync(int iAgentID, bool bActiveCampaign);
        Task<MessageResponse<string>> CancelRequestInBetweenAsync(int iRequestID, int iFormID);
        Task<MessageResponse<string>> CancelAccessRequestAsync(int iRequestID, int iFormID);
        Task<MessageResponse<int>> InsertUserMappingForApprovalAsync(InsertUserMappingForApprovalModel oUserMapping, int iFormID);
        Task<MessageResponse<List<UserRequestType>>> GetUserRequestTypeAsync(int userId, int requestId, int mappedOn);
        Task<MessageResponse<List<UserApproverDetail>>> GetUserApproverListAsync(int userId, int clientId, int processId, int flag, int iFormID);
        Task<MessageResponse<string>> InsertUserMappingApproversAndSendMailAsync(UserMappingApproverModel oUser, int iFormID);
        Task<MessageResponse<string>> ApproveAccessRequestAsync(int iRequestID, int iRequestTypeID, int iRequestType, int iApprovalLevel, int iFormID);
        Task<MessageResponse<string>> RejectAccessRequestAsync(int iRequestID, int iRequestTypeID, int iRequestType, int iApprovalLevel, int iFormID);
        Task<MessageResponse<List<AccessRequestApprovalList>>> GetUserApprovalListAsync();
        Task<MessageResponse<BEUserInfo>> GetUserDetailsWithRoleAsync(int iUserID);
        Task<MessageResponse<List<RoleApproverUserList>>> GetUserRoleApproverListAsync(int iRoleId);
        Task<MessageResponse<string>> InsertUserRoleDataAsync(PowerUserInfo oUser, int iMode, int iFormID);
    }
}
