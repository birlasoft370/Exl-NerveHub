using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using BPA.Security.ServiceContract.FaultContracts;
using BPA.Security.BusinessEntity.Security;
using BPA.Security.BusinessEntity.ExtrernalRefre;

namespace BPA.Security.ServiceContracts.Security
{
    [ServiceContract(Name = "ERPJobRoleServiceContract")]
    public interface IERPJobRoleMapService
    {
       //[OperationContract(Name = "GetJob")]
       //[FaultContract(typeof(ServiceFault))]
        List<BEJobCodeInfo> GetJob( BETenant oTenant);


        //added by santosh 20200717
        List<BERoleInfo> GetRoleList(BETenant oTenant);

        List<BERoleInfo> GetUserRoleApproverList(int iRoleID,int iFormID, BETenant oTenant);

        //code ended by santosh

        //[OperationContract(Name = "GetJobRoleMapWithJobID")]
        //[FaultContract(typeof(ServiceFault))]
      //  List<BEErpJobRoleMap> GetJobRoleMap(int JobID, int RoleID, BETenant oTenant);

       //[OperationContract(Name = "GetJobRoleMapWithJobDesc")]
       //[FaultContract(typeof(ServiceFault))]
        List<BEErpJobRoleMap> GetJobRoleMap(string JobDesc, BETenant oTenant);

        List<BEErpJobRoleMap> GetMultiJobRoleMap(string RoleJobID, BETenant oTenant);
        List<RoleFormAccessModel> GetRoleFormMap(string RoleJobID, BETenant oTenant);

       //[OperationContract(Name = "GetJobRoleMapWithRoleJobID")]
       //[FaultContract(typeof(ServiceFault))]
        List<BEErpJobRoleMap> GetJobRoleMap(int RoleJobID, BETenant oTenant);

       //[OperationContract(Name = "InsData")]
       //[FaultContract(typeof(ServiceFault))]
     //   void InsertData(BEErpJobRoleMap oJobRole, int iFormID, BETenant oTenant);

       //[OperationContract(Name = "InsertDataApprover")]
       //[FaultContract(typeof(ServiceFault))]
        //void InsertData(BEErpJobRoleMap oJobRole, int iApprover, int iFormID, BETenant oTenant);

        void InsertData(BEErpJobRoleMap oJobRole, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "UpdData")]
        //[FaultContract(typeof(ServiceFault))]
        void UpdateData(BEErpJobRoleMap oJobRole, int iFormID, BETenant oTenant);

       //[OperationContract(Name = "DelData")]
       //[FaultContract(typeof(ServiceFault))]
        void DeleteData(BEErpJobRoleMap oJobRole, int iFormID, BETenant oTenant);

       //[OperationContract(Name = "GetERPJobRoleRequestStatus")]
       //[FaultContract(typeof(ServiceFault))]
       // DataSet GetERPJobRoleRequestStatus(int iUser, DateTime dtFromDate, DateTime dtToDate, BETenant oTenant);

        
        List<BEErpJobRoleMap>GetERPJobRoleRequestStatus(int iUser, DateTime dtFromDate, DateTime dtToDate, BETenant oTenant);

        //[OperationContract(Name = "GetERPJobRoleApprovalList")]
        //[FaultContract(typeof(ServiceFault))]
        //DataSet GetERPJobRoleApprovalList(int iUser, BETenant oTenant);

         //added by santosh
        List<BEErpJobRoleMap> GetERPJobRoleApprovalList(int iUser, BETenant oTenant);

        //[OperationContract(Name = "RejectERPJobRoleRequest")]
        //[FaultContract(typeof(ServiceFault))]
        void RejectERPJobRoleRequest(int iRequestID, int iUserID, int iFormID, BETenant oTenant);

       //[OperationContract(Name = "ApproveERPJobRoleRequest")]
       //[FaultContract(typeof(ServiceFault))]
        void ApproveERPJobRoleRequest(int iRequestID, int iUserID, int iFormID, BETenant oTenant);

       //[OperationContract(Name = "CancelERPJobRoleRequest")]
       //[FaultContract(typeof(ServiceFault))]
        void CancelERPJobRoleRequest(int iRequestID, int iUserID, int iFormID, BETenant oTenant);
    }
}
