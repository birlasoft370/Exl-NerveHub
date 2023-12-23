using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using System.ServiceModel;

namespace BPA.Security.ServiceContracts.Security
{
    [ServiceContract(Name = "SessionServiceContract")]
    public interface ISessionService
    {
       //[OperationContract(Name = "InsertSessionStart")]
       //[FaultContract(typeof(ServiceFault))]
        int InsertSessionStart(BESession oBESession, BETenant oTenant);

       //[OperationContract(Name = "InsertSessionEnd")]
       //[FaultContract(typeof(ServiceFault))]
        void InsertSessionEnd(int SessionID, BETenant oTenant);

       //[OperationContract(Name = "FinalLogout")]
       //[FaultContract(typeof(ServiceFault))]
        void FinalLogout(int UserID, BETenant oTenant);

       //[OperationContract(Name = "GetUserID")]
       //[FaultContract(typeof(ServiceFault))]
        int GetUserID(int SessionID, BETenant oTenant);

       //[OperationContract(Name = "GetSessionID")]
       //[FaultContract(typeof(ServiceFault))]
        int GetSessionID(BESession oBESession, BETenant oTenant);
        //[OperationContract(Name = "InsertErorLog")]
       //[FaultContract(typeof(ServiceFault))]
        string InsertErorLog(string ErrorMessage, BETenant oTenant);
    }
}
