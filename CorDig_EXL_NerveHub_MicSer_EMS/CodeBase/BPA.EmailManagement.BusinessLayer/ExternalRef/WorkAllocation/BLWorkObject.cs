using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessEntity.ExternalRef.WorkAllocation;
using BPA.EmailManagement.DataLayer.ExternalRef.WorkAllocation;
using BPA.EmailManagement.ServiceContract.ServiceContracts.WorkAllocation;
using System.Data;

namespace BPA.EmailManagement.BusinessLayer.ExternalRef.WorkAllocation
{
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class BLWorkObject : IWorkObjectService, IDisposable//, IDataOperation<BEWorkObject>
    {
        public BLWorkObject()
        { }
        public void Dispose()
        { }

        public DataSet GetLinkCampaignColumn(int DGridId, int UserID, int WorkId, BETenant oTenant)
        {
            using (DLWorkObject oObject = new DLWorkObject(oTenant))
            {
                return oObject.GetLinkCampaignColumn(DGridId, UserID, WorkId);
            }
        }

        public List<BEWorkObject> GetObjectList(int iObjectID, BETenant oTenant, string culture)
        {
            using (DLWorkObject oObject = new DLWorkObject(oTenant))
            {
                return oObject.GetObjectListLang(iObjectID, culture);
            }
        }

        public List<BEWorkObject> GetObjectList(int StoreId, bool bGetAll, BETenant oTenant)
        {
            using (DLWorkObject oObject = new DLWorkObject(oTenant))
            {
                return oObject.GetObjectList(StoreId, bGetAll);
            }
        }

        public List<BEWorkObject> GetSearchableObject(int iStoreId, BETenant oTenant)
        {
            using (DLWorkObject objWork = new DLWorkObject(oTenant))
            {
                return objWork.GetSearchableObject(iStoreId);
            }
        }

        public DataSet GetAllGridData(int iStoreid, int iDObjectid, BETenant oTenant)
        {
            using (DLWorkObject oObject = new DLWorkObject(oTenant))
            {
                return oObject.GetAllGridData(iStoreid, iDObjectid);
            }
        }

        public List<BEWorkObjectChoice> GetObjectChoiceList(int iObjectID, BETenant oTenant)
        {
            using (DLWorkObject oObject = new DLWorkObject(oTenant))
            {
                return oObject.GetObjectChoiceList(iObjectID, BLAgentDashBoard.TempUserID);

            }
        }

        public DataTable GetWorkObjectFormulaEventValues(Int32 iCampId, BETenant oTenant)
        {
            using (DLWorkObject objWork = new DLWorkObject(oTenant))
            {
                return objWork.GetWorkObjectFormulaEvent(iCampId);
            }
        }
    }
}
