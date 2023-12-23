using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.BusinessEntity.Security;
using BPA.AppConfig.Datalayer.Config;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;
using System.Data;

namespace BPA.AppConfig.BusinessLayer.Config
{
    //[ExceptionShielding("WCF Exception Shielding")]
    // [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class BLCampaign : ICampaignService, IDisposable
    {
        #region construction
        /// <summary>
        /// Initializes a new instance of the <see cref="BLCampaign"/> class.
        /// </summary>
        public BLCampaign()
        {

        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }
        #endregion

        public List<BEUserInfo> GetUserApproverListByProcess(int UserId, int iFormId, int ProcessId, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                return objCampaign.GetUserApproverListByProcess(UserId, iFormId, ProcessId);
            }
        }

        public List<BECampaignInfo> GetCampaignList(int iLoggedinUserID, int iProcessID, string sCamapaignName, bool bActiveCampaign, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                return objCampaign.GetCampaignList(iLoggedinUserID, iProcessID, sCamapaignName, bActiveCampaign);
            }
        }

        public void InsertData(BECampaignInfo ocamp, int FormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(FormID, ocamp.iCreatedBy, PermissionSet.ADD))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }

            if (ocamp.sCampaignName == string.Empty || ocamp.sCampaignName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Campaign Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }*/
            using (DLCampaign objCamp = new DLCampaign(oTenant))
            {
                objCamp.InsertData(ocamp);
            }
        }

        public void UpdateData(BECampaignInfo ocamp, int FormID, BETenant oTenant)
        {
            /*
            if (!CheckPermission.hasPermission(FormID, ocamp.iCreatedBy, PermissionSet.UPDATE))
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }

            if (ocamp.sCampaignName == string.Empty || ocamp.sCampaignName == "")
            {
                throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Campaign Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }*/
            using (DLCampaign objCamp = new DLCampaign(oTenant))
            {
                objCamp.UpdateData(ocamp);
            }
        }

        public DataTable GetPandingCampaignApproval(int iUserId, DateTime sFromDate, DateTime sToDate, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                return objCampaign.GetPandingCampaignApproval(iUserId, sFromDate, sToDate);
            }
        }

        public DataTable FetchCampaignRequestDetails(int iApprovalId, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                return objCampaign.FetchCampaignRequestDetails(iApprovalId);
            }
        }

        public void InsertCampaignRequestChange(int iApprovalId, int iUserId, int iUserLevel, string sChangeRequest, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                objCampaign.InsertCampaignRequestChange(iApprovalId, iUserId, iUserLevel, sChangeRequest);
            }
        }
        public void UpdateCampaignRequestChange(BECampaignInfo oCampaign, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                objCampaign.UpdateCampaignRequestChange(oCampaign);
            }
        }
        public List<BECampaignInfo> GetCampaignList(int iCamapaignID, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                return objCampaign.GetCampaignList(iCamapaignID);
            }
        }
        public List<BECampaignInfo> GetProcessWiseCampaignList(int iFormID, int iLoggedinUserID, int iProcessID, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                return objCampaign.GetProcessWiseCampaignList(iFormID, iLoggedinUserID, iProcessID);
            }
        }
    }
}
