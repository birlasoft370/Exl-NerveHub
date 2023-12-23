using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using System.Collections.Generic;
using System.ServiceModel;

namespace BPA.Security.ServiceContracts.Security
{
    [ServiceContract(Name = "UserLevelServiceContract")]
    public interface IUserLevelService
    {
       //[OperationContract(Name = "GetUserLevelListActive")]
       //[FaultContract(typeof(ServiceFault))]
        List<BEUserLevelInfo> GetUserLevelList(bool bActiveLevel, BETenant oTenant);

       //[OperationContract(Name = "GetUserLevelListWithName")]
       //[FaultContract(typeof(ServiceFault))]
        List<BEUserLevelInfo> GetUserLevelList(string sUserLevelName, bool bActiveLevel, BETenant oTenant);

       //[OperationContract(Name = "GetUserLevelListWithID")]
       //[FaultContract(typeof(ServiceFault))]
        List<BEUserLevelInfo> GetUserLevelList(int iUserLevelID, BETenant oTenant);

       //[OperationContract(Name = "InsData")]
       //[FaultContract(typeof(ServiceFault))]
        void InsertData(BEUserLevelInfo oUserLevel, int iFormID, BETenant oTenant);

       //[OperationContract(Name = "UpdData")]
       //[FaultContract(typeof(ServiceFault))]
        void UpdateData(BEUserLevelInfo oUserLevel, int iFormID, BETenant oTenant);

       //[OperationContract(Name = "DelData")]
       //[FaultContract(typeof(ServiceFault))]
        void DeleteData(BEUserLevelInfo oUserLevel, int iFormID, BETenant oTenant);

    }
}
