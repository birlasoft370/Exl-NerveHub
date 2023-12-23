using BPA.Security.ServiceContract.FaultContracts;
using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using System;
using System.Data;
using System.ServiceModel;


namespace BPA.Security.ServiceContracts.Security
{
    [ServiceContract(Name = "UserAccessRequestServiceContract")]
    public interface IUserAccessRequestService:IDisposable
    {
        [OperationContract(Name = "GetUserDetails")]
        [FaultContract(typeof(ServiceFault))]
        DataSet GetUserDetails(int iUserID, BETenant oTenant);

        [OperationContract(Name = "GetClientList")]
        [FaultContract(typeof(ServiceFault))]
        DataSet GetClientList(int iLoggedinUserID, bool bActiveClient, BETenant oTenant);

        [OperationContract(Name = "GetProcessList")]
        [FaultContract(typeof(ServiceFault))]
        DataSet GetProcessList(int iLoggedinUserID, bool bActiveProcess, string sClientId, BETenant oTenant);

        [OperationContract(Name = "UpdateUserDetails")]
        [FaultContract(typeof(ServiceFault))]
        void UpdateUserDetails(BEUserInfo oUser, int iFormID, BETenant oTenant);

    }
}
