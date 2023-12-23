using BPA.AppConfig.BusinessEntity;
using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef.Configuration;
using BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation;
using BPA.AppConfig.Datalayer.ExternalRef.WorkAllocation;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.WorkAllocation;

namespace BPA.AppConfig.BusinessLayer.ExternalRef.WorkAllocation
{

    //[ExceptionShielding("WCF Exception Shielding")]
    public class BLStore : IStoreService, IDisposable, IDataOperation<BEStoreInfo>
    {
        public BLStore()
        { }


        public void Dispose()
        { }

        public List<BECampaignClientProcess> GetClientProcessList(int iCampID, BETenant oTenant)
        {
            using (DLStore objStore = new DLStore(oTenant))
            {
                return DataSetToList.ConvertTo<BECampaignClientProcess>(objStore.GetClientProcessList(iCampID).Tables[0]);
            }
        }

        public void InsertData(BEStoreInfo ClObject, int iFormID, BETenant oTenant)
        {
            throw new NotImplementedException();
        }

        public void UpdateData(BEStoreInfo ClObject, int iFormID, BETenant oTenant)
        {
            throw new NotImplementedException();
        }
        public void DeleteData(BEStoreInfo ClObject, int iFormID, BETenant oTenant)
        {
            throw new NotImplementedException();
        }
        public List<BEStoreInfo> GetStoreList(bool bActiveStore, int UserId, BETenant oTenant)
        {
            return GetStoreList("", bActiveStore, UserId, oTenant);
        }
        public List<BEStoreInfo> GetStoreList(string sStoreName, bool bActiveStore, int UserId, BETenant oTenant)
        {
            using (DLStore oStore = new DLStore(oTenant))
            {
                return oStore.GetStoreList(sStoreName, bActiveStore, UserId);
            }
        }
        public List<BEStoreInfo> GetStoreList(int iCampaignId, string sStoreName, bool bActiveStore, int UserId, BETenant oTenant)
        {
            using (DLStore oStore = new DLStore(oTenant))
            {
                return oStore.GetStoreList(iCampaignId, sStoreName, bActiveStore, UserId);
            }
        }
    }
}
