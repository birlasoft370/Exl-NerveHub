using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Config;
using BPA.EmailManagement.BusinessEntity.ExternalRef.WorkAllocation;
using BPA.EmailManagement.DataLayer.ExternalRef.WorkAllocation;
using BPA.EmailManagement.ServiceContract.ServiceContracts.WorkAllocation;
using System.Data;

namespace BPA.EmailManagement.BusinessLayer.ExternalRef.WorkAllocation
{
    // [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class BLAgentDashBoard : IAgentDashBoardService, IDisposable
    {
        public static int TempUserID = 0;
        /// <summary>
        /// Initializes a new instance of the <see cref="BLAgentDashBoard"/> class.
        /// </summary>
        public BLAgentDashBoard()
        {
            //ExceptionHelper.ExceptionfactoryInit();
        }

        public void Dispose()
        { }

        string _ServerHostName = System.Net.Dns.GetHostName().ToString() + " - Service";

        public int GetDStoreID(int CampaignID, out bool isMailCampaign, out bool isPGT, out bool IsTabConfiguration, out int iIncreaseSearch, out bool IsGridConfiguration, out bool isDistributionBot, BETenant oTenant)
        {

            using (DLAgentDashBoard oDLAgentDashBoard = new DLAgentDashBoard(oTenant))
            {
                return oDLAgentDashBoard.GetDStoreID(CampaignID, out isMailCampaign, out isPGT, out IsTabConfiguration, out iIncreaseSearch, out IsGridConfiguration, out isDistributionBot);
            }
        }
        public DataSet GetLinkCampaignData(int CampaignID, BETenant oTenant)
        {

            using (DLWorkObject oDLWorkObject = new DLWorkObject(oTenant))
            {
                return oDLWorkObject.GetAllLinkCampaignData(Convert.ToInt32(CampaignID));
            }
        }

        public List<BETerminationCodeInfo> GetTerminationCode(int CampaignID, BETenant oTenant)
        {
            using (DLAgentDashBoard oDLAgentDashBoard = new DLAgentDashBoard(oTenant))
            {
                return oDLAgentDashBoard.GetTerminationCode(CampaignID);
            }
        }

        public List<BEBreakInfo> GetBreakCode(int CampaignID, BETenant oTenant)
        {
            using (DLBreakCode oDLAgentDashBoard = new DLBreakCode(oTenant))
            {
                return oDLAgentDashBoard.GetBreakCode(CampaignID);
            }
        }

        public List<BEMasterTable> GetDelayCodes(BETenant oTenant)
        {
            using (DLAgentDashBoard oDLAgentDashBoard = new DLAgentDashBoard(oTenant))
            {
                return oDLAgentDashBoard.GetDelayCodes();
            }
        }

        public DataSet GetDStoreInfo(int DStoreID, int CampaignID, BETenant oTenant)
        {
            using (DLAgentDashBoard oDLAgentDashBoard = new DLAgentDashBoard(oTenant))
            {

                return oDLAgentDashBoard.GetDStoreInfo(DStoreID, CampaignID);
            }
        }
    }
}
