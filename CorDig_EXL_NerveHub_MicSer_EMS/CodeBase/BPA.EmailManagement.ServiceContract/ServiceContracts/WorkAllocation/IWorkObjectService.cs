using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessEntity.ExternalRef.WorkAllocation;
using BPA.EmailManagement.ServiceContract.ExternalRef.FaultContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BPA.EmailManagement.ServiceContract.ServiceContracts.WorkAllocation
{
    [ServiceContract(Name = "WorkObjectServiceContract")]
    public interface IWorkObjectService : IDisposable
    {
        // DataSet GetLinkCampaignColumn(int DGridId, int UserID, int WorkId, BETenant oTenant);
        [OperationContract(Name = "GetObjectListLang")]
        [FaultContract(typeof(ServiceFault))]
        List<BEWorkObject> GetObjectList(int iObjectID, BETenant oTenant, string culture);
      
        [OperationContract(Name = "GetObjectListWithStore")]
        [FaultContract(typeof(ServiceFault))]
        List<BEWorkObject> GetObjectList(int StoreId, bool bGetAll, BETenant oTenant);
        [OperationContract(Name = "GetSearchableObject")]
        [FaultContract(typeof(ServiceFault))]
        List<BEWorkObject> GetSearchableObject(int iStoreId, BETenant oTenant);
        //DataSet GetAllGridData(int iStoreid, int iDObjectid, BETenant oTenant);
        [OperationContract(Name = "GetObjectChoiceListId")]
        [FaultContract(typeof(ServiceFault))]
        List<BEWorkObjectChoice> GetObjectChoiceList(int iObjectID, BETenant oTenant);
        [OperationContract(Name = "GetWorkObjectFormulaEventValues")]
        [FaultContract(typeof(ServiceFault))]
        DataTable GetWorkObjectFormulaEventValues(Int32 iCampId, BETenant oTenant);
    }
}
