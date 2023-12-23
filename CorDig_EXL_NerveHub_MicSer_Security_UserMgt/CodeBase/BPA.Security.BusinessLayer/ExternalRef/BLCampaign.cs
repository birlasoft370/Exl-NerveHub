using BPA.Security.BusinessEntity;
using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity.Security;
using BPA.Security.DataLayer.ExternalRef;
using BPA.Security.ServiceContract.ExternalRef;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.Security.BusinessLayer.ExternalRef
{
    /// <summary>
    /// BL Campaign
    /// </summary>   
    //[ExceptionShielding("WCF Exception Shielding")]
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
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

        #region Get List


        // <summary>
        /// Gets DStore ID.
        /// </summary>

        public string GetDStoreID(string Campaignid, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                return objCampaign.GetDStoreID(Campaignid);
            }

        }
        /// <summary>
        /// Gets the campaign list.
        /// </summary>
        /// <param name="iLoggedinUserID">The loggedin user ID.</param>
        /// <param name="bActiveCampaign">if set to <c>true</c> [active campaign].</param>
        /// <returns>All the Campaign assign to the userID</returns>
        public List<BECampaignInfo> GetCampaignList(int iLoggedinUserID, bool bActiveCampaign, BETenant oTenant)
        {
            return GetCampaignList(iLoggedinUserID, "", bActiveCampaign, oTenant);
        }

        /// <summary>
        /// Gets the campaign list.
        /// </summary>
        /// <param name="iLoggedinUserID">The loggedin user ID.</param>
        /// <param name="sCamapaignName">Name of the s camapaign.</param>
        /// <param name="bActiveCampaign">if set to <c>true</c> [active campaign].</param>
        /// <returns></returns>
        public List<BECampaignInfo> GetCampaignList(int iLoggedinUserID, string sCamapaignName, bool bActiveCampaign, BETenant oTenant)
        {
            return GetCampaignList(iLoggedinUserID, sCamapaignName, bActiveCampaign, false, oTenant);
        }

        /// <summary>
        /// Gets the campaign list.
        /// </summary>
        /// <param name="iLoggedinUserID">The loggedin user ID.</param>
        /// <param name="sCamapaignName">Name of the s camapaign.</param>
        /// <param name="bActiveCampaign">if set to <c>true</c> [active campaign].</param>
        /// <returns></returns>
        public List<BECampaignInfo> GetCampaignList(int iLoggedinUserID, string sCamapaignName, bool bActiveCampaign, bool DashboardRequest, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                return objCampaign.GetCampaignList(iLoggedinUserID, sCamapaignName, bActiveCampaign, DashboardRequest);
            }
        }

        /// <summary>
        /// To Get Campaign Access List
        /// </summary>
        /// <param name="iLoggedinUserID"></param>
        /// <param name="iAgentID"></param>
        /// <param name="bActiveCampaign"></param>
        /// <returns></returns>
        public List<BECampaignInfo> GetCampaignAccessList(int iLoggedinUserID, int iAgentID, bool bActiveCampaign, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                return objCampaign.GetCampaignAccessList(iLoggedinUserID, iAgentID, bActiveCampaign);
            }
        }

        /// <summary>
        /// Gets the campaign list.
        /// </summary>
        /// <param name="iLoggedinUserID">The i loggedin user ID.</param>
        /// <param name="iProcessID">The i process ID.</param>
        /// <param name="sCamapaignName">Name of the s camapaign.</param>
        /// <param name="bActiveCampaign">if set to <c>true</c> [b active campaign].</param>
        /// <returns></returns>
        public List<BECampaignInfo> GetCampaignList(int iLoggedinUserID, int iProcessID, string sCamapaignName, bool bActiveCampaign, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                return objCampaign.GetCampaignList(iLoggedinUserID, iProcessID, sCamapaignName, bActiveCampaign);
            }
        }

        /// <summary>
        /// Gets the campaign list.
        /// </summary>
        /// <param name="iCamapaignID">The camapaign ID.</param>
        /// <returns></returns>
        public List<BECampaignInfo> GetCampaignList(int iCamapaignID, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                return objCampaign.GetCampaignList(iCamapaignID);
            }
        }
        /// <summary>
        /// Gets the process wise campaign list.
        /// </summary>
        /// <param name="iFormID">The i form ID.</param>
        /// <param name="iLoggedinUserID">The i loggedin user ID.</param>
        /// <param name="iProcessID">The i process ID.</param>
        /// <returns></returns>
        public List<BECampaignInfo> GetProcessWiseCampaignList(int iFormID, int iLoggedinUserID, int iProcessID, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                return objCampaign.GetProcessWiseCampaignList(iFormID, iLoggedinUserID, iProcessID);
            }
        }
        #endregion

        #region IDataOperation
        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <param name="ocamp">The ocamp.</param>
        /// <param name="FormID">The form ID.</param>
        public void InsertData(BECampaignInfo ocamp, int FormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, ocamp.iCreatedBy, PermissionSet.ADD))
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            }

            if (ocamp.sCampaignName == string.Empty || ocamp.sCampaignName == "")
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Campaign Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            using (DLCampaign objCamp = new DLCampaign(oTenant))
            {
                objCamp.InsertData(ocamp);
            }
        }

        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <param name="ocamp">The ocamp.</param>
        /// <param name="FormID">The form ID.</param>
        public void UpdateData(BECampaignInfo ocamp, int FormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, ocamp.iCreatedBy, PermissionSet.UPDATE))
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            }

            if (ocamp.sCampaignName == string.Empty || ocamp.sCampaignName == "")
            {
              //  throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Campaign Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            }
            using (DLCampaign objCamp = new DLCampaign(oTenant))
            {
                objCamp.UpdateData(ocamp);
            }
        }

        /// <summary>
        /// Deletes the data.
        /// </summary>
        /// <param name="ocamp">The ocamp.</param>
        /// <param name="FormID">The form ID.</param>
        public void DeleteData(BECampaignInfo ocamp, int FormID, BETenant oTenant)
        {
            if (!CheckPermission.hasPermission(FormID, ocamp.iCreatedBy, PermissionSet.DELETE))
            {
                //throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.DeletePermission);
            }

            using (DLCampaign objCamp = new DLCampaign(oTenant))
            {
                objCamp.DeleteData(ocamp);
            }
        }
        #endregion


        /// <summary>
        /// Gets the campaign and role list.
        /// </summary>
        /// <param name="CampaignID">The campaign ID.</param>
        /// <returns></returns>
        public DataSet GetCampaignAndRoleList(int CampaignID, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                return objCampaign.GetCampaignAndRoleList(CampaignID);
            }
        }

        public DataSet GetCampaignDetails(int ProcessID, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                return objCampaign.GetCampaignDetails(ProcessID);
            }
        }

        /// <summary>
        /// Gets the campaign and role details.
        /// </summary>
        /// <param name="ClientID">The client ID.</param>
        /// <returns></returns>
        public DataSet GetCampaignAndRoleDetails(int ClientID, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                return objCampaign.GetCampaignAndRoleDetails(ClientID);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet GetCampaignDetailsByUserID(string LoginName, BETenant oTenant)
        {

            using (DLCampaign objCamp = new DLCampaign(oTenant))
            {
                return objCamp.GetCampaignDetailsByUserID(LoginName);
            }
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <param name="LoginName">Name of the login.</param>
        /// <returns></returns>
        public int GetUserId(string LoginName, BETenant oTenant)
        {

            using (DLCampaign objCamp = new DLCampaign(oTenant))
            {
                return objCamp.GetUserId(LoginName);
            }
        }

        /// <summary>
        /// Gets the role id.
        /// </summary>
        /// <returns></returns>
        public int GetRoleId(BETenant oTenant)
        {

            using (DLCampaign objCamp = new DLCampaign(oTenant))
            {
                return objCamp.GetRoleId();
            }
        }


        /// <summary>
        /// Gets the campaign list.
        /// </summary>
        /// <returns></returns>
        public List<BECampaignInfo> GetCampaignList(BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                return objCampaign.GetCampaignList();
            }
        }

        #region Campaign Approval
        /// <summary>
        ///  Insert Campaign Request change
        /// </summary>
        /// <param name="iApprovalId"></param>
        /// <param name="iUserId"></param>
        /// <param name="iUserLevel"></param>
        /// <param name="sChangeRequest"></param>
        public void InsertCampaignRequestChange(int iApprovalId, int iUserId, int iUserLevel, string sChangeRequest, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                objCampaign.InsertCampaignRequestChange(iApprovalId, iUserId, iUserLevel, sChangeRequest);
            }
        }
        /// <summary>
        /// update campaign request changes
        /// </summary>
        /// <param name="oCampaign"></param>
        public void UpdateCampaignRequestChange(BECampaignInfo oCampaign, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                objCampaign.UpdateCampaignRequestChange(oCampaign);
            }
        }
        /// <summary>
        /// Fetch Campaign Request Details
        /// </summary>
        /// <returns></returns>

        public DataTable FetchCampaignRequestDetails(int iApprovalId, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                return objCampaign.FetchCampaignRequestDetails(iApprovalId);
            }
        }
        /// <summary>
        /// To use for approval/reject/cancel
        /// </summary>
        /// <param name="oCampaign"></param>
        /// <param name="IsApproval"></param>
        public void InsertData(BECampaignInfo oCampaign, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                objCampaign.InsertData(oCampaign);
            }
        }
        /// <summary>
        /// To get list of pending campaign approval
        /// </summary>
        /// <param name="iUserId"></param>
        /// <param name="sFromDate"></param>
        /// <param name="sToDate"></param>
        /// <returns></returns>
        public DataTable GetPandingCampaignApproval(int iUserId, DateTime sFromDate, DateTime sToDate, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                return objCampaign.GetPandingCampaignApproval(iUserId, sFromDate, sToDate);
            }
        }
        #endregion

        public List<BEUserInfo> GetUserApproverListByProcess(int UserId, int iFormId, int ProcessId, BETenant oTenant)
        {
            using (DLCampaign objCampaign = new DLCampaign(oTenant))
            {
                return objCampaign.GetUserApproverListByProcess(UserId, iFormId, ProcessId);
            }
        }



    }
}