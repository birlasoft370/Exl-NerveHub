using Microsoft.AspNetCore.Mvc.Rendering;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Security
{
    public interface IRolesService
    {
        List<BEMasterTable> FillRoleLevel();
        SelectList PopulateRoleApproverDropDownList(int roleId);
        List<BEFormActionInfo> PopulateFormAction(int roleId);
        List<RoleApprovalModel> GetRoleApprovalList(int iUserID);
        List<RoleApprovalModel> GetRoleRequestStatus(int iUser, DateTime dtFromDate, DateTime dtToDate);
        List<BERoleInfo> GetRoleList();
        void InsertData(RoleInfoModel oRoles, int iApproverID, int iFormID);
        List<BERoleInfo> GetRoleList(string sRoleName, bool bActiveRoles);
        List<RoleApprovalRequestModel> GetRoleRequestStatus(DateTime dtFromDate, DateTime dtToDate);
        void CancelRoleRequest(int iRequestID, int iFormID);
        void ApproveRoleRequest(int iRequestID, int iFormID);
        void RejectRoleRequest(int iRequestID, int iFormID);
        List<RoleInfoModel> GetRoleList(int iRoleID);
        void UpdateData(RoleInfoModel oRoles, int iApproverID, int iFormID);
        List<BEUserInfo> GetUserJobCode();
        List<BERoleInfo> GetClientRoleList();
    }
}
