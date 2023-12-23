using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.ServiceContract.ExternalRef.FaultContracts;
using System.ServiceModel;

namespace BPA.EmailManagement.ServiceContract.ServiceContracts
{
    [ServiceContract(Name = "MailConfigurationContract")]
    public interface IMailConfigurationService : IEmailGenericService<BEMailConfiguration>, IDisposable
    {
        [OperationContract(Name = "getMailReceiveDateTime")]
        [FaultContract(typeof(ServiceFault))]
        IList<BEMailReceivedDateTime> getMailReceiveDateTime(int iCampaignID, int iMailConfigID, int iMailFolderDetailID, BETenant oTenant);

        [OperationContract(Name = "GetCampaignWiseList")]
        [FaultContract(typeof(ServiceFault))]
        IList<BEMailConfiguration> GetCampaignWiseList(int CampaignID, BETenant oTenant);

        [OperationContract(Name = "GetCampaignWiseDatafromCache")]
        [FaultContract(typeof(ServiceFault))]
        IList<BEMailConfiguration> GetCampaignWiseDatafromCache(int iStoreID, BETenant oTenant);

        [OperationContract(Name = "InsertRecieveDateTime")]
        [FaultContract(typeof(ServiceFault))]
        void InsertRecieveDateTime(int iCampaignID, int iMailConfigID, int iMailFolderDetailID, string sSubject,
            DateTime dMailRecievedTime, string sMailUniqueID, BETenant oTenant);

        [OperationContract(Name = "InsertData")]
        [FaultContract(typeof(ServiceFault))]
        void InsertData(BEMailConfiguration oBEMailConfiguration, int iFormID, BETenant oTenant);

        [OperationContract(Name = "UpdateData")]
        [FaultContract(typeof(ServiceFault))]
        void UpdateData(BEMailConfiguration oBEMailConfiguration, int iFormID, BETenant oTenant);


        [OperationContract(Name = "DeleteData")]
        [FaultContract(typeof(ServiceFault))]
        void DeleteData(BEMailConfiguration oBEMailConfiguration, int iFormID, BETenant oTenant);


        [OperationContract(Name = "GetMailConfigAllData")]
        [FaultContract(typeof(ServiceFault))]
        IList<BEMailConfiguration> GetMailConfigAllData(int MailConfigID, BETenant oTenant);


        [OperationContract(Name = "GetAdvancedConfiguration")]
        [FaultContract(typeof(ServiceFault))]
        IList<BEMailConfiguration> GetAdvancedConfiguration(int iCampaignID, BETenant oTenant);


        [OperationContract(Name = "InsertUpdateAdvancedConfiguration")]
        [FaultContract(typeof(ServiceFault))]
        void InsertUpdateAdvancedConfiguration(BEMailConfiguration MailConfiguration, BETenant oTenant);

        [OperationContract(Name = "DisableMailConfig")]
        [FaultContract(typeof(ServiceFault))]
        void DisableMailConfig(BEMailConfiguration oMailConfig, int iExceptionType, BETenant oTenant);

    }
}
