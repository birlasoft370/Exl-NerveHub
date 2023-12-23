using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.ExternalRef.SharePointConfiguration;
using BPA.AppConfig.Datalayer.ExternalRef.SharePointConfiguration;
using BPA.AppConfig.ServiceContracts.ServiceContracts.ExternalRef.SharePointConfiguration;

namespace BPA.AppConfig.BusinessLayer.ExternalRef.SharePointConfiguration
{
   // [ExceptionShielding("WCF Exception Shielding")]
    public class BLSharePointConfiguration : IDisposable, ISharePointConfiguration
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        public bool CheckIfCampaignMappingExist(string campaignId, BETenant oTenant)
        {
            using (DLSharePointConfiguration oDLSharePointConfiguration = new DLSharePointConfiguration(oTenant))
            {
                return oDLSharePointConfiguration.CheckIfCampaignMappingExist(campaignId);
            }
        }
        public IList<BESharePointConfiguration> GetSharepointSchedulerList(int ShedulerID, BETenant oTenant)
        {
            using (DLSharePointConfiguration oDLSharePointConfiguration = new DLSharePointConfiguration(oTenant))
            {
                return oDLSharePointConfiguration.GetSharePointData(ShedulerID);
            }
        }

        public IList<BESharePointConfiguration> GetSearchSharepointList(int campaignId, BETenant oTenant)
        {
            using (DLSharePointConfiguration oDLSharePointConfiguration = new DLSharePointConfiguration(oTenant))
            {
                return oDLSharePointConfiguration.GetSearchSharePoint(campaignId);
            }
        }

        public void InsertData(BESharePointConfiguration oSharePointConfiguration, int iFormID, BETenant oTenant)
        {
            using (DLSharePointConfiguration oDLSharePointConfiguration = new DLSharePointConfiguration(oTenant))
            {
                oDLSharePointConfiguration.InsertData(oSharePointConfiguration);
            }
        }

        public void UpdateData(BESharePointConfiguration oBESharePointConfiguration, int iFormID, BETenant oTenant)
        {
            using (DLSharePointConfiguration oDLSharePointConfiguration = new DLSharePointConfiguration(oTenant))
            {
                oDLSharePointConfiguration.InsertData(oBESharePointConfiguration);
            }
        }
    }
}
