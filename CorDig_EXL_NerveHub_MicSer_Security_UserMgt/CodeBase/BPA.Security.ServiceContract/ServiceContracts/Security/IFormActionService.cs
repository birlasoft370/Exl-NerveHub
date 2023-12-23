using BPA.Security.ServiceContract.FaultContracts;
using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using System.Collections.Generic;
using System.ServiceModel;

namespace BPA.Security.ServiceContracts.Security
{
    [ServiceContract(Name = "FormActionServiceContract")]
    public interface IFormActionService
    {
        //[OperationContract(Name = "GetFormAction")]
        //[FaultContract(typeof(ServiceFault))]
        //DataSet GetFormAction(int RoleID);

       //[OperationContract(Name = "GetFormListWithName")]
       //[FaultContract(typeof(ServiceFault))]
        List<BEFormAction> GetFormList(string sFormName, BETenant oTenant);

       //[OperationContract(Name = "GetFormListWithID")]
       //[FaultContract(typeof(ServiceFault))]
        List<BEFormAction> GetFormList(int iFormID, BETenant oTenant);

       //[OperationContract(Name = "InsData")]
       //[FaultContract(typeof(ServiceFault))]
        void InsertData(BEFormAction objFormAction, int iFormID, BETenant oTenant);

       //[OperationContract(Name = "UpdData")]
       //[FaultContract(typeof(ServiceFault))]
        void UpdateData(BEFormAction objFormAction, int iFormID, BETenant oTenant);

       //[OperationContract(Name = "DelData")]
       //[FaultContract(typeof(ServiceFault))]
        void DeleteData(BEFormAction objFormAction, int iFormID, BETenant oTenant);

    }
}
