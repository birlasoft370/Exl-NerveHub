using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessLayer.CacheData;
using BPA.EmailManagement.DataLayer;
using BPA.EmailManagement.ServiceContract.ServiceContracts;

namespace BPA.EmailManagement.BusinessLayer
{
    //[ExceptionShielding("WCF Exception Shielding")]
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)] // Issue
    public class BLMailConfiguration : IMailConfigurationService, IDisposable
    {
        public void Dispose()
        { }

        #region  GetCampaignWiseList
        /// <summary>
        /// Gets the List.
        /// </summary>
        /// <param name="ClientID">The CamoaignID.</param>
        /// <returns></returns>
        public IList<BEMailConfiguration> GetCampaignWiseList(int iStoreID, BETenant oTenant)
        {
            using (DLMailConfiguration objMailConfiguration = new DLMailConfiguration(oTenant))
            {
                return objMailConfiguration.GetCampaignWiseList(iStoreID, false);
            }
        }
        public IList<BEMailConfiguration> GetCampaignWiseDatafromCache(int iStoreID, BETenant oTenant)
        {
            using (BLMailConfigCache objMailConfiguration = new BLMailConfigCache())
            {
                return objMailConfiguration.GetMailConfiguration(iStoreID, oTenant);
            }
        }

        public void InsertRecieveDateTime(int iCampaignID, int iMailConfigID, int iMailFolderDetailID, string sSubject,
            DateTime dMailRecievedTime, string sMailUniqueID, BETenant oTenant)
        {
            using (DLMailConfiguration objMailConfiguration = new DLMailConfiguration(oTenant))
            {
                objMailConfiguration.InsertRecieveDateTime(iCampaignID, iMailConfigID, iMailFolderDetailID, sSubject, dMailRecievedTime, sMailUniqueID);
            }
        }

        #endregion

        #region  GetMailConfigAllData
        /// <summary>
        /// Gets the List.
        /// </summary>
        /// <param name="ClientID">The CamoaignID.</param>
        /// <returns></returns>
        public IList<BEMailConfiguration> GetMailConfigAllData(int MailConfigID, BETenant oTenant)
        {
            using (DLMailConfiguration objMailConfiguration = new DLMailConfiguration(oTenant))
            {
                return objMailConfiguration.GetMailConfigAllData(MailConfigID);
            }
        }
        #endregion

        #region Get Mail ReceiveDatetime
        public IList<BEMailReceivedDateTime> getMailReceiveDateTime(int iCampaignID, int iMailConfigID, int iMailFolderDetailID, BETenant oTenant)
        {

            using (DLMailConfiguration oMail = new DLMailConfiguration(oTenant))
            {
                return oMail.getMailReceiveDateTime(iCampaignID, iMailConfigID, iMailFolderDetailID, oTenant);
            }
        }
        #endregion

        #region Insert Mail Configuration
        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <param name="MailConfiguration">client.</param>
        /// <param name="FormID">form ID.</param>
        public void InsertData(BEMailConfiguration MailConfiguration, int iFormID, BETenant oTenant)
        {
            //if (!CheckPermission.hasPermission(iFormID, MailConfiguration.iCreatedBy, PermissionSet.ADD))
            //{
            //    throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.InsertPermission);
            //}

            //if (string.IsNullOrEmpty(MailConfiguration.sMailBoxName))
            //{
            //    throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("Client Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            //}
            using (DLMailConfiguration objMailConfiguration = new DLMailConfiguration(oTenant))
            {
                objMailConfiguration.InsertData(MailConfiguration);
            }
            using (BLMailConfigCache oCacheData = new BLMailConfigCache())
            {
                oCacheData.RemoveCache(MailConfiguration.iStoreID, oTenant);
            }
        }

        // add by alankar
        public void InsertUpdateAdvancedConfiguration(BEMailConfiguration MailConfiguration, BETenant oTenant)
        {
            using (DLMailConfiguration objMailConfiguration = new DLMailConfiguration(oTenant))
            {
                objMailConfiguration.InsertUpdateAdvancedConfiguration(MailConfiguration);
            }
            using (BLMailConfigCache oCacheData = new BLMailConfigCache())
            {
                oCacheData.RemoveCache(MailConfiguration.iStoreID, oTenant);
            }
        }


        public IList<BEMailConfiguration> GetAdvancedConfiguration(int iCampaignID, BETenant oTenant)
        {

            using (DLMailConfiguration oMail = new DLMailConfiguration(oTenant))
            {
                return oMail.GetAdvancedConfiguration(iCampaignID);
            }
        }

        // end by alankar
        #endregion

        #region Update Mail Configuration
        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <param name="MailConfiguration">client.</param>
        /// <param name="FormID">form ID.</param>
        public void UpdateData(BEMailConfiguration MailConfiguration, int FormID, BETenant oTenant)
        {
            //if (!CheckPermission.hasPermission(FormID, MailConfiguration.iCreatedBy, PermissionSet.UPDATE))
            //{
            //    throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.UpdatePermission);
            //}

            //if (MailConfiguration.sMailBoxName == string.Empty || MailConfiguration.sMailBoxName == "")
            //{
            //    throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException("MailBox Name " + BPA.GlobalResources.BusinessLayer.Resources.RequiredField);
            //}
            using (DLMailConfiguration objMailConfiguration = new DLMailConfiguration(oTenant))
            {
                objMailConfiguration.UpdateData(MailConfiguration);
            }
            using (BLMailConfigCache omail = new BLMailConfigCache())
            {
                omail.RemoveCache(MailConfiguration.iStoreID, oTenant);
            }
            if (MailConfiguration.oMailfolderdetails != null)
            {
                using (MailfolderCache omailFolder = new MailfolderCache())
                {
                    foreach (BEMailfolderdetails ofolder in MailConfiguration.oMailfolderdetails)
                    {
                        omailFolder.RemoveCache(MailConfiguration, ofolder.MailFolderDetailID, oTenant);
                    }
                }
            }
        }
        #endregion

        #region Delete Mail Configuration
        /// <summary>
        /// Deletes the data.
        /// </summary>
        /// <param name="MailConfiguration">client.</param>
        /// <param name="FormID">form ID.</param>
        public void DeleteData(BEMailConfiguration MailConfiguration, int FormID, BETenant oTenant)
        {
            //if (!CheckPermission.hasPermission(FormID, MailConfiguration.iCreatedBy, PermissionSet.DELETE))
            //{
            //    throw new ExceptionHandler.ExceptionType.BusinessLogicCustomException(BPA.GlobalResources.BusinessLayer.Resources.DeletePermission);
            //}
            //using (DLMailConfiguration objClient = new DLMailConfiguration(oTenant))
            //{
            //    objClient.DeleteData(MailConfiguration);
            //}
        }
        #endregion

        #region Update PasswordExpire value
        /// <summary>
        /// Deletes the data.
        /// </summary>
        /// <param name="MailConfiguration">client.</param>
        /// <param name="FormID">form ID.</param>
        public void DisableMailConfig(BEMailConfiguration oMailConfig, int iExceptionType, BETenant oTenant)
        {

            using (DLMailConfiguration objClient = new DLMailConfiguration(oTenant))
            {
                objClient.DisableMailConfig(oMailConfig, iExceptionType);
            }
        }
        #endregion

        #region Get Work Addition Field
        public IList<BEMailCampaignField> GetMailCamapignFields(int iDStoreID, BETenant oTenant)
        {
            using (DLMailConfiguration objMailConfiguration = new DLMailConfiguration(oTenant))
            {
                return objMailConfiguration.GetMailCamapignFields(iDStoreID);
            }
        }
        //[EMS].[Usp_GetWorkAdditionField] 
        #endregion
    }
}