using BPA.Security.BusinessEntity.ExtrernalRefre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BPA.Security.ServiceContract.ExternalRef
{
    [ServiceContract(Name = "MasterTableServiceContract")]
    public interface IMasterTableService : IDisposable
    {
        //[OperationContract(Name = "GetMasterListWithFieldID")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEMasterTable> GetMasterList(int iFieldID, BETenant oTenant);

        //[OperationContract(Name = "GetMasterListWithField")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEMasterTable> GetMasterList(string sFieldID, BETenant oTenant);

        //[OperationContract(Name = "GetMasterListAll")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEMasterType> GetMasterList(bool bGetAll, BETenant oTenant);

        //[OperationContract(Name = "GetMasterListAllWithFieldDesc")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEMasterType> GetMasterList(string sFieldDesc, bool bGetAll, BETenant oTenant);

        //[OperationContract(Name = "DelData")]
        //[FaultContract(typeof(ServiceFault))]
        void DeleteData(BEMasterType oMasterType, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "InsData")]
        //[FaultContract(typeof(ServiceFault))]
        void InsertData(BEMasterType oMasterType, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "UpdData")]
        //[FaultContract(typeof(ServiceFault))]
        void UpdateData(BEMasterType oMasterType, int iFormID, BETenant oTenant);

        //[OperationContract(Name = "ManageMasterValues")]
        //[FaultContract(typeof(ServiceFault))]
        //void ManageMasterValues(Database db, DbTransaction trans, int iFieldID, List<BEMasterTable> lMasterTable);

        //[OperationContract(Name = "GetMasterDetails")]
        //[FaultContract(typeof(ServiceFault))]
        BEMasterType GetMasterDetails(int iFieldID, BETenant oTenant);

        IList<BEMasterTable> FillRoleLevel(BETenant oTenant);
    }
}
