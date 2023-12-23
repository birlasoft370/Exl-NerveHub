using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessLayer.ExchangeData;
using BPA.EmailManagement.ServiceContract.ServiceContracts;
using System.Collections;

namespace BPA.EmailManagement.BusinessLayer
{
    //[ExceptionShielding("WCF Exception Shielding")]
    // [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class BLEmailFactoryGraph : IMailServiceGraph
    {
        IMailServiceGraph IMServiceGraph = null;
        public void Dispose()
        {
            IMServiceGraph = null;
        }
        private IMailServiceGraph getclassGraph(EmailServerType Exchangeserverversion)
        {

            switch (Exchangeserverversion)
            {

                case BusinessEntity.EmailServerType.MicrosoftGraph:
                    IMServiceGraph = new MicrosoftGraph();
                    break;

                default:
                    break;
            }
            return IMServiceGraph;
        }
        public Task<Hashtable> GetMailFolderListGraph(BEMailConfiguration oMailConfig, BETenant oTenant)
        {
            return getclassGraph(oMailConfig.iMailServerTypeID).GetMailFolderListGraph(oMailConfig, oTenant);
        }

        public Task<Hashtable> GetFolderListGraph(BEMailConfiguration oMailConfig, string folderID, string rootFolderName, BETenant oTenant)
        {
            return getclassGraph(oMailConfig.iMailServerTypeID).GetFolderListGraph(oMailConfig, folderID, rootFolderName, oTenant);
        }
    }
}
