using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Security
{
    public interface IPermissionService
    {
        List<BEUserInfo> GetUserRoleListUserRole();
        List<BEUserInfo> GetUserList(string sLoginName, bool bActiveUser, string SearchCondition);
        void InsertClientUserRecord(ClientUserInfoModel oUser, int iFormID);
        BEUserInfo GetClientUserDetails(int iUserID);
        void UpdateClientUserRecord(ClientUserInfoModel oUser, int iFormID);
        List<UserRequestStatus> GetUserRequestStatus(DateTime dtFromDate, DateTime dtToDate);
        List<BEUserSetting> GetUserSetting(int iUserID);
        void CancelRequestInBetween(int iRequestID, int iFormID);
        void CancelAccessRequest(int iRequestID, int iFormID);
        int InsertUserMappingForApproval(InsertUserMappingForApprovalModel model, int iFormID);
        List<UserRequestType> GetUserRequestType(int userId, int requestId, int mappedOn);
        List<UserApproverDetail> GetUserApproverList(int UserId, int ClientId, int ProcessId, int Flag, int iFormID);
        void InsertUserMappingApprovers(UserMappingApproverModel approverList, int iFormID);
        void ApproveAccessRequest(int iRequestID, int iRequestTypeID, int iRequestType, int iApprovalLevel, int iFormID);
        void RejectAccessRequest(int iRequestID, int iRequestTypeID, int iRequestType, int iApprovalLevel, int iFormID);
        List<BEUserInfo> GetRequestUserList(string sLoginName, bool bActiveUser, int iLoggedinUserID, string SearchCondition);
        List<AccessRequestApprovalList> GetUserApprovalList();
        BEUserInfo GetUserDetailsWithRole(int iUserID);
        List<RoleApproverUserList> GetUserRoleApproverList(int iRoleID);
        void InsertUserRoleData(PowerUserInfo oUserData, int iMode, int iFormID);
    }
}
