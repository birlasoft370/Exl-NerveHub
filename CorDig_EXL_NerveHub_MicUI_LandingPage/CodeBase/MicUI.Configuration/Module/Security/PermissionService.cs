using MicUI.Configuration.Helper;
using MicUI.Configuration.Services.Security;
using MicUI.Configuration.Services.ServiceModel;
using MicUI.Configuration.Services.Reports;


namespace MicUI.Configuration.Module.Security
{
    public class PermissionService : IPermissionService
    {
        private readonly ISecurityApiService _securityApiService;
        private readonly IReportsApiService _reportsService;
        public PermissionService(ISecurityApiService securityApiService, IReportsApiService reportsService)
        {
            _securityApiService = securityApiService;
            _reportsService = reportsService;
        }

        public List<BEUserInfo> GetUserRoleListUserRole()
        {
            var result = _securityApiService.GetUserRoleListUserRoleAsync().GetAwaiter().GetResult();
            return result.data ?? new List<BEUserInfo>();
        }
        public List<BEUserInfo> GetUserList(string sLoginName, bool bActiveUser, string SearchCondition)
        {
            var result = _securityApiService.GetUserListAsync(sLoginName, bActiveUser, SearchCondition).GetAwaiter().GetResult();
            return result.data ?? new List<BEUserInfo>();
        }
        public void InsertClientUserRecord(ClientUserInfoModel oUser, int iFormID)
        {
            var result = _securityApiService.InsertClientUserRecordAsync(oUser, iFormID).GetAwaiter().GetResult();

            if (result != null && result.message != null && result.status != true)
            {
                if (result.message.ToLower().Contains(GlobalConstant.ExNumber547NullInsert) || result.message.ToLower().Contains(GlobalConstant.ExNumber547ReferenceKeyconstraint))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else if (result.message.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                else if (result.message.ToLower().Contains(BPA.GlobalResources.DataLayer.Resources.msg_UserNameAlready.ToLower()))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_UserNameAlready);
                }
                throw new Exception(result.message);
            }
        }
        public BEUserInfo GetClientUserDetails(int iUserID)
        {
            var result = _securityApiService.GetClientUserDetailsAsync(iUserID).GetAwaiter().GetResult();
            return result.data ?? new BEUserInfo();
        }
        public void UpdateClientUserRecord(ClientUserInfoModel oUser, int iFormID)
        {
            var result = _securityApiService.UpdateClientUserRecordAsync(oUser, iFormID).GetAwaiter().GetResult();

            if (result != null && result.status != true)
            {
                if (result.message.ToLower().Contains(GlobalConstant.ExNumber547NullInsert) || result.message.ToLower().Contains(GlobalConstant.ExNumber547ReferenceKeyconstraint))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else if (result.message.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                else if (result.message.ToLower().Contains(BPA.GlobalResources.DataLayer.Resources.msg_UserNameAlready.ToLower()))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_UserNameAlready);
                }
                else if (result.message.ToLower().Contains(BPA.GlobalResources.DataLayer.Resources.msg_UpdateUserApprovalPending.ToLower()))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_UpdateUserApprovalPending);
                }
                throw new Exception(result.message);
            }
        }
        public List<UserRequestStatus> GetUserRequestStatus(DateTime dtFromDate, DateTime dtToDate)
        {
            var result = _securityApiService.GetUserRequestStatusAsync(dtFromDate, dtToDate).GetAwaiter().GetResult();
            return result.data ?? new List<UserRequestStatus>();
        }
        public List<BEUserSetting> GetUserSetting(int iUserID)
        {
            var result = _securityApiService.GetUserSettingAsync(iUserID).GetAwaiter().GetResult();
            return result.data ?? new List<BEUserSetting>();
        }
        public void CancelRequestInBetween(int iRequestID, int iFormID)
        {
            var result = _securityApiService.CancelRequestInBetweenAsync(iRequestID, iFormID).GetAwaiter().GetResult();
            if (result != null && result.message != null && result.status != true)
            {
                if (result.message.ToLower().Contains(GlobalConstant.ExNumber547NullInsert) || result.message.ToLower().Contains(GlobalConstant.ExNumber547ReferenceKeyconstraint))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else if (result.message.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }

                throw new Exception(result.message);
            }
        }
        public void CancelAccessRequest(int iRequestID, int iFormID)
        {
            var result = _securityApiService.CancelAccessRequestAsync(iRequestID, iFormID).GetAwaiter().GetResult();
            if (result != null && result.message != null && result.status != true)
            {
                if (result.message.ToLower().Contains(GlobalConstant.ExNumber547NullInsert) || result.message.ToLower().Contains(GlobalConstant.ExNumber547ReferenceKeyconstraint))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else if (result.message.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }

                throw new Exception(result.message);
            }
        }
        public int InsertUserMappingForApproval(InsertUserMappingForApprovalModel model, int iFormID)
        {
            var result = _securityApiService.InsertUserMappingForApprovalAsync(model, iFormID).GetAwaiter().GetResult();
            if (result != null && result.data == 0 && result.message != null)
            {
                if (result.message.ToLower().Contains(GlobalConstant.ExNumber547NullInsert) || result.message.ToLower().Contains(GlobalConstant.ExNumber547ReferenceKeyconstraint))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else if (result.message.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                else if (result.message.ToLower().Contains("Request for Approval is Pending for this User".ToLower()))
                {
                    throw new Exception("Request for Approval is Pending for this User");
                }
                throw new Exception(result.message);
            }
            else
            {
                return (result == null ? 0 : result.data);
            }
        }
        public List<UserRequestType> GetUserRequestType(int userId, int requestId, int mappedOn)
        {
            var result = _securityApiService.GetUserRequestTypeAsync(userId, requestId, mappedOn).GetAwaiter().GetResult();
            return result.data ?? new List<UserRequestType>();
        }
        public List<UserApproverDetail> GetUserApproverList(int UserId, int ClientId, int ProcessId, int Flag, int iFormID)
        {
            var result = _securityApiService.GetUserApproverListAsync(UserId, ClientId, ProcessId, Flag, iFormID).GetAwaiter().GetResult();
            return result.data ?? new List<UserApproverDetail>();
        }
        public void InsertUserMappingApprovers(UserMappingApproverModel approverList, int iFormID)
        {
            var result = _securityApiService.InsertUserMappingApproversAndSendMailAsync(approverList, iFormID).GetAwaiter().GetResult();
            if (result != null && result.message != null && result.status != true)
            {
                throw new Exception(result.message);
            }
        }
        public void ApproveAccessRequest(int iRequestID, int iRequestTypeID, int iRequestType, int iApprovalLevel, int iFormID)
        {
            var result = _securityApiService.ApproveAccessRequestAsync(iRequestID, iRequestTypeID, iRequestType, iApprovalLevel, iFormID).GetAwaiter().GetResult();
            if (result != null && result.message != null && result.status != true)
            {
                if (result.message.ToLower().Contains(GlobalConstant.ExNumber547NullInsert) || result.message.ToLower().Contains(GlobalConstant.ExNumber547ReferenceKeyconstraint))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else if (result.message.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                if (result.message.ToLower().Contains(BPA.GlobalResources.DataLayer.Resources.msg_Requestor_request_approver.ToLower()))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_Requestor_request_approver);
                }
                throw new Exception(result.message);
            }
        }
        public void RejectAccessRequest(int iRequestID, int iRequestTypeID, int iRequestType, int iApprovalLevel, int iFormID)
        {
            var result = _securityApiService.RejectAccessRequestAsync(iRequestID, iRequestTypeID, iRequestType, iApprovalLevel, iFormID).GetAwaiter().GetResult();
            if (result != null && result.message != null && result.status != true)
            {
                if (result.message.ToLower().Contains(GlobalConstant.ExNumber547NullInsert) || result.message.ToLower().Contains(GlobalConstant.ExNumber547ReferenceKeyconstraint))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else if (result.message.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw new Exception(result.message);
            }
        }
        public List<BEUserInfo> GetRequestUserList(string sLoginName, bool bActiveUser, int iLoggedinUserID, string SearchCondition)
        {
            var result = _reportsService.GetUserListAsync(sLoginName, bActiveUser, iLoggedinUserID, SearchCondition).GetAwaiter().GetResult();
            return result.data ?? new List<BEUserInfo>();

        }
        public List<AccessRequestApprovalList> GetUserApprovalList()
        {
            var result = _securityApiService.GetUserApprovalListAsync().GetAwaiter().GetResult();
            return result.data ?? new List<AccessRequestApprovalList>();
        }
        public BEUserInfo GetUserDetailsWithRole(int iUserID)
        {
            var result = _securityApiService.GetUserDetailsWithRoleAsync(iUserID).GetAwaiter().GetResult();
            return result.data ?? new BEUserInfo();
        }
        public List<RoleApproverUserList> GetUserRoleApproverList(int iRoleID)
        {
            var result = _securityApiService.GetUserRoleApproverListAsync(iRoleID).GetAwaiter().GetResult();
            return result.data ?? new List<RoleApproverUserList>();
        }
        public void InsertUserRoleData(PowerUserInfo oUserData, int iMode, int iFormID)
        {
            var result = _securityApiService.InsertUserRoleDataAsync(oUserData, iMode, iFormID).GetAwaiter().GetResult();
            if (result != null && result.message != null && result.status != true)
            {
                if (result.message.ToLower().Contains(GlobalConstant.ExNumber547NullInsert) || result.message.ToLower().Contains(GlobalConstant.ExNumber547ReferenceKeyconstraint))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else if (result.message.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                else if (result.message.ToLower().Contains(GlobalConstant.ApprovalPending))
                {
                    throw new Exception(BPA.GlobalResources.UI.AppConfiguration.Resources_PowerUser.display_Role_Request_Pending);
                }
                throw new Exception(result.message);
            }
        }
    }
}
