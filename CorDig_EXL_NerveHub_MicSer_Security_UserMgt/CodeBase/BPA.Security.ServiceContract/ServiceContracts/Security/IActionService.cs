
using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using System.Collections.Generic;
using System.ServiceModel;

namespace BPA.Security.ServiceContracts.Security
{
    [ServiceContract(Name = "ActionServiceContract")]
    public interface IActionService
    {

       //[OperationContract(Name = "GetActionList")]
       //[FaultContract(typeof(ServiceFault))]
        List<BEActionInfo> GetActionList( BETenant oTenant);

       //[OperationContract(Name = "GetActionListWithName")]
       //[FaultContract(typeof(ServiceFault))]
        List<BEActionInfo> GetActionList(string sActionName, BETenant oTenant);

       //[OperationContract(Name = "InsData")]
       //[FaultContract(typeof(ServiceFault))]
        void InsertData(BEActionInfo objAction, int iFormID, BETenant oTenant);

       //[OperationContract(Name = "UpdData")]
       //[FaultContract(typeof(ServiceFault))]
        void UpdateData(BEActionInfo objAction, int iFormID, BETenant oTenant);

       //[OperationContract(Name = "DelData")]
       //[FaultContract(typeof(ServiceFault))]
        void DeleteData(BEActionInfo objAction, int iFormID, BETenant oTenant);

    }
}
