using Microsoft.AspNetCore.Mvc.Rendering;
using MicUI.Configuration.Helper;
using MicUI.Configuration.Services.Security;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Security
{
    public class RolesService : IRolesService
    {
        private readonly ISecurityApiService _securityApiService;
        public RolesService(ISecurityApiService securityApiService)
        {
            _securityApiService = securityApiService;
        }
        public List<BEMasterTable> FillRoleLevel()
        {
            var roleLevel = _securityApiService.FillRoleLevelAsync().GetAwaiter().GetResult();
            return roleLevel.data;

        }
        public SelectList PopulateRoleApproverDropDownList(int roleId)
        {
            var roleLevel = _securityApiService.PopulateRoleApproverDropDownListAsync(roleId).GetAwaiter().GetResult();
            if (roleLevel != null && roleLevel.data != null && roleLevel.data.Count > 0)
            {
                return new SelectList(roleLevel.data, "UserID", "UserName");
            }
            else
            {
                return new SelectList(Enumerable.Empty<BEApproval>());
            }
        }
        public List<BEFormActionInfo> PopulateFormAction(int roleId)
        {
            var formAction = _securityApiService.PopulateFormActionAsync(roleId).GetAwaiter().GetResult();

            return formAction.data;

        }
        public List<RoleApprovalModel> GetRoleApprovalList(int iUserID)
        {
            var formAction = _securityApiService.GetRoleApprovalListAsync(iUserID).GetAwaiter().GetResult();

            return formAction.data;
        }
        public List<RoleApprovalModel> GetRoleRequestStatus(int iUser, DateTime dtFromDate, DateTime dtToDate)
        {
            var formAction = _securityApiService.GetRoleRequestStatusAsync(iUser, dtFromDate, dtToDate).GetAwaiter().GetResult();

            return formAction.data;
        }
        public List<BERoleInfo> GetRoleList()
        {
            var result = _securityApiService.GetRoleListAsync().GetAwaiter().GetResult();
            return result.data ?? new List<BERoleInfo>();
        }

        public void InsertData(RoleInfoModel oRoles, int iApproverID, int iFormID)
        {
            var result = _securityApiService.InsertDataRoleAsync(oRoles, iApproverID, iFormID).GetAwaiter().GetResult();
            if (result != null && result.status == false && result.data != null)
            {
                if (result.data.ToLower().Contains(GlobalConstant.ExNumber547NullInsert) || result.data.ToLower().Contains(GlobalConstant.ExNumber547ReferenceKeyconstraint))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else if (result.data.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                else if (result.data.ToLower().Contains(BPA.GlobalResources.DataLayer.Resources.msg_roleApprovalPending.ToLower()))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_roleApprovalPending);
                }
                else if (result.data.ToLower().Contains(BPA.GlobalResources.DataLayer.Resources.msg_RoleAlreadyExists.ToLower()))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_RoleAlreadyExists);
                }
                else if (result.data.ToLower().Contains(BPA.GlobalResources.DataLayer.Resources.msg_RoleAlreadySameName.ToLower()))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_RoleAlreadySameName);
                }
                else if (result.data.ToLower().Contains(BPA.GlobalResources.DataLayer.Resources.msg_RoleApprovalBeforeMaking.ToLower()))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_RoleApprovalBeforeMaking);
                }
                throw new Exception(result.data);
            }
        }

        public List<BERoleInfo> GetRoleList(string sRoleName, bool bActiveRoles)
        {
            var result = _securityApiService.GetRoleListByNameAsync(sRoleName, bActiveRoles).GetAwaiter().GetResult();
            return result.data ?? new List<BERoleInfo>();
        }
        public List<RoleApprovalRequestModel> GetRoleRequestStatus(DateTime dtFromDate, DateTime dtToDate)
        {
            var result = _securityApiService.GetRoleRequestStatusAsync(dtFromDate, dtToDate).GetAwaiter().GetResult();
            return result.data ?? new List<RoleApprovalRequestModel>();
        }
        public void CancelRoleRequest(int iRequestID, int iFormID)
        {
            var result = _securityApiService.CancelRoleRequestAsync(iRequestID, iFormID).GetAwaiter().GetResult();
            if (result != null && result.data != null && result.status != true)
            {
                if (result.data.ToLower().Contains(GlobalConstant.ExNumber547NullInsert) || result.data.ToLower().Contains(GlobalConstant.ExNumber547ReferenceKeyconstraint))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else if (result.data.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw new Exception(result.data);
            }
        }
        public void ApproveRoleRequest(int iRequestID, int iFormID)
        {
            var result = _securityApiService.ApproveRoleRequestAsync(iRequestID, iFormID).GetAwaiter().GetResult();
            if (result != null && result.data != null && result.status != true)
            {
                if (result.data.ToLower().Contains(GlobalConstant.ExNumber547NullInsert) || result.data.ToLower().Contains(GlobalConstant.ExNumber547ReferenceKeyconstraint))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else if (result.data.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw new Exception(result.data);
            }
        }
        public void RejectRoleRequest(int iRequestID, int iFormID)
        {
            var result = _securityApiService.RejectRoleRequestAsync(iRequestID, iFormID).GetAwaiter().GetResult();
            if (result != null && result.data != null && result.status != true)
            {
                if (result.data.ToLower().Contains(GlobalConstant.ExNumber547NullInsert) || result.data.ToLower().Contains(GlobalConstant.ExNumber547ReferenceKeyconstraint))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else if (result.data.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                throw new Exception(result.data);
            }
        }
        public List<RoleInfoModel> GetRoleList(int iRoleID)
        {
            var result = _securityApiService.GetRoleListAsync(iRoleID).GetAwaiter().GetResult();
            return result.data ?? new List<RoleInfoModel>();
        }
        public void UpdateData(RoleInfoModel oRoles, int iApproverID, int iFormID)
        {
            var result = _securityApiService.UpdateDataRoleAsync(oRoles, iApproverID, iFormID).GetAwaiter().GetResult();
            if (result != null && result.status == false && result.data != null)
            {
                if (result.data.ToLower().Contains(GlobalConstant.ExNumber547NullInsert) || result.data.ToLower().Contains(GlobalConstant.ExNumber547ReferenceKeyconstraint))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.ReferenceConstraint);
                }
                else if (result.data.ToLower().Contains(GlobalConstant.ExNumber2601Or2627Duplicate))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.UniqueConstraints);
                }
                else if (result.data.ToLower().Contains(BPA.GlobalResources.DataLayer.Resources.msg_roleApprovalPending.ToLower()))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_roleApprovalPending);
                }
                else if (result.data.ToLower().Contains(BPA.GlobalResources.DataLayer.Resources.msg_RoleAlreadyExists.ToLower()))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_RoleAlreadyExists);
                }
                else if (result.data.ToLower().Contains(BPA.GlobalResources.DataLayer.Resources.msg_RoleAlreadySameName.ToLower()))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_RoleAlreadySameName);
                }
                else if (result.data.ToLower().Contains(BPA.GlobalResources.DataLayer.Resources.msg_RoleApprovalBeforeMaking.ToLower()))
                {
                    throw new Exception(BPA.GlobalResources.DataLayer.Resources.msg_RoleApprovalBeforeMaking);
                }
                throw new Exception(result.data);
            }
        }
        public List<BEUserInfo> GetUserJobCode()
        {
            var result = _securityApiService.GetUserJobCodeAsync().GetAwaiter().GetResult();
            return result.data ?? new List<BEUserInfo>();
        }
        public List<BERoleInfo> GetClientRoleList()
        {
            var result = _securityApiService.GetClientRoleListAsync().GetAwaiter().GetResult();
            return result.data ?? new List<BERoleInfo>();
        }
    }
}
