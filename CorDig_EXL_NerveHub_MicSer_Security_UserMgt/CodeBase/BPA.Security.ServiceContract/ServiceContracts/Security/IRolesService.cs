using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Text;
namespace BPA.Security.ServiceContracts.Security
{
    [ServiceContract(Name = "RolesServiceContract")]
    public interface IRolesService:IDisposable
    {
       //[OperationContract(Name = "GetRoleList")]
       //[FaultContract(typeof(ServiceFault))]
        List<BERoleInfo> GetRoleList(bool bActiveRoles, BETenant oTenant);

       //[OperationContract(Name = "GetRoleListWithName")]
       //[FaultContract(typeof(ServiceFault))]
        List<BERoleInfo> GetRoleList(string sRoleName, bool bActiveRoles, BETenant oTenant);

       //[OperationContract(Name = "GetRoleListWithRoleID")]
       //[FaultContract(typeof(ServiceFault))]
        List<BERoleInfo> GetRoleList(int iRoleID, BETenant oTenant);

    

       //[OperationContract(Name = "GetClientRoleList")]
       //[FaultContract(typeof(ServiceFault))]
        List<BERoleInfo> GetClientRoleList( BETenant oTenant);

        //Added 29-1-21
        List<BERoleInfo> GetUsertRoleList(bool isClient,BETenant oTenant);

        List<BERoleInfo> GetClientRoleListByUser(int userID, BETenant oTenant);

       //[OperationContract(Name = "InsertDataWithApproverID")]
       //[FaultContract(typeof(ServiceFault))]
        void InsertData(BERoleInfo oRoles, int iApproverID, int iFormID, StringBuilder sbMailBody, BETenant oTenant);

       //[OperationContract(Name = "InsData")]
       //[FaultContract(typeof(ServiceFault))]
        void InsertData(BERoleInfo oRoles, int iFormID, BETenant oTenant);

       //[OperationContract(Name = "UpdData")]
       //[FaultContract(typeof(ServiceFault))]
        void UpdateData(BERoleInfo oRoles, int iFormID, BETenant oTenant);

       //[OperationContract(Name = "UpdateDataWithApproverID")]
       //[FaultContract(typeof(ServiceFault))]
        void UpdateData(BERoleInfo oRoles, int iApproverID, int iFormID, StringBuilder sbMailBody, BETenant oTenant);

       //[OperationContract(Name = "DelData")]
       //[FaultContract(typeof(ServiceFault))]
        void DeleteData(BERoleInfo oRoles, int iFormID, BETenant oTenant);

       //[OperationContract(Name = "DeleteDataWithApproverId")]
       //[FaultContract(typeof(ServiceFault))]
        void DeleteData(BERoleInfo oRole, int iApproverId, int iFormID, StringBuilder sbMailBody, BETenant oTenant);

       //[OperationContract(Name = "GetRoleRequestStatus")]
       //[FaultContract(typeof(ServiceFault))]
        DataSet GetRoleRequestStatus(int iUser, DateTime dtFromDate, DateTime dtToDate, BETenant oTenant);

       //[OperationContract(Name = "GetRoleApprovalList")]
       //[FaultContract(typeof(ServiceFault))]
        DataSet GetRoleApprovalList(int iUser, BETenant oTenant);

       //[OperationContract(Name = "RejectRoleRequest")]
       //[FaultContract(typeof(ServiceFault))]
        void RejectRoleRequest(int iRequestID, int iUserID, int iFormID, BETenant oTenant);

       //[OperationContract(Name = "ApproveRoleRequest")]
       //[FaultContract(typeof(ServiceFault))]
        void ApproveRoleRequest(int iRequestID, int iUserID, int iFormID, BETenant oTenant);

       //[OperationContract(Name = "CancelRoleRequest")]
       //[FaultContract(typeof(ServiceFault))]
        void CancelRoleRequest(int iRequestID, int iUserID, int iFormID, BETenant oTenant);

       //[OperationContract(Name = "GetFormAction")]
       //[FaultContract(typeof(ServiceFault))]
        DataSet GetFormAction(int RoleID, BETenant oTenant);

        List<BEUserInfo> GetUserJobCode( BETenant oTenant);

    }
}
